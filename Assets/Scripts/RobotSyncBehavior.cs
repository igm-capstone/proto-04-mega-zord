using UnityEngine;
using System.Collections.Generic;

public class Action
{
    public readonly string Key;
    public readonly float Time;
    public float SyncScore;     // Synchronization Score...????

    public PlayerSyncInfo[] PlayerSyncInfos { get; private set; }
    
     
    public Action(string key, int numberOfPlayers, float timeStamp)
    {
        Key = key;
        Time = timeStamp;
        PlayerSyncInfos = new PlayerSyncInfo[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerSyncInfos[i] = new PlayerSyncInfo(i);
        }
    }

    public bool IsSynchronized()       // All players synchronized in this action
    {
        foreach (PlayerSyncInfo psi in PlayerSyncInfos)
        {
            if (!psi.IsPressing())
            {
                return false;
            }
        }

        return true;
    }

    public bool IsActive()    // At Least One Player Participating in action
    {
        foreach (PlayerSyncInfo psi in PlayerSyncInfos)
        {
            if (psi.IsPressing())
            {
                return true;
            }
        }

        return false;
    }

}

public class PlayerSyncInfo
{
    public readonly int PlayerID;
    public float Delay;         // First time pressing a button during an action
    public float Release;       // Any button releases during an action
    public float Break;         // Cumulative time button was not presses during an action
    public float Total;         // Total time button was pressed during the action

    public PlayerSyncInfo(int playerID)
    {
        PlayerID = playerID;
        Delay = float.NaN;
        Release = float.NaN;
        Break = 0.0f;
    }

    public bool IsPressing()
    {
        return (float.IsNaN(Release) && !float.IsNaN(Delay));
    }
}

public class RobotSyncBehavior : MonoBehaviour
{
    public event System.Action<Action> ActionStarted;
    public event System.Action<Action> ActionChanged;
    public event System.Action<string> ActionTerminated;
    
    [Range(1, 2)]
    public int RobotID = 1;
    [Range(1, 4)]
    public int NumberOfPlayers = 4;

    public GameObject robotHUDPrefab;


    Dictionary<string, Action> actionDictionary;

    int[] joystickIDs;
    private InputPanelHUD hud;

    void Awake()
    {
        Camera c = gameObject.GetComponentInChildren<Camera>();
        c.rect = new Rect((RobotID == 1) ? 0 : 0.5f, 0, 0.5f, 1);

        GameObject canvas = GameObject.Instantiate(robotHUDPrefab);
        canvas.transform.SetParent(transform);
        canvas.GetComponent<Canvas>().worldCamera = c;


        actionDictionary = new Dictionary<string, Action>();
        hud = GetComponentInChildren<InputPanelHUD>();

        joystickIDs = new int[NumberOfPlayers+1];
        int controllersBeingUsed = FindObjectsOfType<InputManager>().Length;
        for (int pID = 1; pID <= NumberOfPlayers; pID++)
        {
            InputManager input = gameObject.AddComponent<InputManager>();
            input.joystickID = controllersBeingUsed + pID;
            joystickIDs[pID] = controllersBeingUsed + pID;
        }
    }

    public void ReceiveInput(string key, int joystickID, bool state)
    {
        bool isNew = false;
        Action action;
        if (!actionDictionary.TryGetValue(key, out action))
        {
            action = new Action(key, NumberOfPlayers, Time.time);
            actionDictionary.Add(key, action);
            isNew = true;
        }

        ProcessAction(action, JoystickID2PlayerID(joystickID), state, isNew);
    }

    void ProcessAction(Action action, int playerID, bool state, bool isNew)
    {
        if (hud) hud.SetPressed(action.Key, playerID, state);

        // Updates individual PlayerSyncInfo
        playerID = playerID % NumberOfPlayers;
        var playerSyncInfo = action.PlayerSyncInfos[playerID];
        if (state)
        {
            if (float.IsNaN(playerSyncInfo.Delay))
            {
                // Player has begun pressing a button.
                playerSyncInfo.Delay = Time.time - action.Time;
            }
            else
            {
                // Player has repeated a button press during an action.
                playerSyncInfo.Break += (Time.time - action.Time) - playerSyncInfo.Release;
                playerSyncInfo.Release = float.NaN;
            }
        }
        else
        {
            // Player has release a button during an action.
            playerSyncInfo.Release = Time.time - action.Time;
        }

        // Update Player Total Score
        playerSyncInfo.Total = (Time.time - action.Time) - playerSyncInfo.Break;

        // Update Action Sync
        float score = 0.0f;
        foreach (PlayerSyncInfo psi in action.PlayerSyncInfos)
        {
            score += psi.Total;
        }
        action.SyncScore = score / NumberOfPlayers;

        if (isNew && ActionStarted != null)
        {
            ActionStarted(action);
        }
        else if (ActionChanged != null)
        {
            ActionChanged(action);
        }
    }

    public void TerminateAction(string key)
    {
        actionDictionary.Remove(key);
        if (ActionTerminated != null)
        {
            ActionTerminated(key);
        }
    }

    public int PlayerID2JoystickID(int playerID)
    {
        return joystickIDs[playerID];
    }

    public int JoystickID2PlayerID(int playerID)
    {
        for (int i = 0; i < joystickIDs.Length; i++)
            if (playerID == joystickIDs[i]) return i;
        return 1;
    }
}
