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
}

public class PlayerSyncInfo
{
    public readonly int PlayerID;
    public float Delay;
    public float Release;
    public float Break;
    public PlayerSyncInfo(int playerID)
    {
        PlayerID = playerID;
        Delay = float.NaN;
        Release = float.NaN;
        Break = 0.0f;
    }
}

public class RobotSyncBehavior : MonoBehaviour
{
    public event System.Action<Action> ActionStarted;
    public event System.Action<Action> ActionChanged;
    public event System.Action<string> ActionTerminated;

    public int NumberOfPlayers = 4;
    Dictionary<string, Action> actionDictionary;

    void Awake()
    {
        actionDictionary = new Dictionary<string, Action>();
    }

    public void ReceiveInput(string key, int playerID, bool state)
    {
        bool isNew = false;
        Action action;
        if (!actionDictionary.TryGetValue(key, out action))
        {
            action = new Action(key, NumberOfPlayers, Time.time);
            actionDictionary.Add(key, action);
            isNew = true;
        }
        
        // Print buttom pressed on the screen
        //Debug.Log("Key received: " + key);

        ProcessAction(action, playerID, state, isNew);
    }

    void ProcessAction(Action action, int playerID, bool state, bool isNew)
    {
        // Updates individual PlayerSyncInfo
        var playerSyncInfo = action.PlayerSyncInfos[playerID];
        if (state)
        {
            if (float.IsNaN(playerSyncInfo.Delay))
            {
                playerSyncInfo.Delay = Time.time - action.Time;
            }
            else
            {
                playerSyncInfo.Break += (Time.time - action.Time) - playerSyncInfo.Release;
                playerSyncInfo.Release = float.NaN;
            }
        }
        else
        {
            playerSyncInfo.Release = Time.time - action.Time;
        }

        // Update Action Sync Score


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
}
