using UnityEngine;
using System.Collections;


// Reads every player input from the controller.
public class InputManager : MonoBehaviour {

    // Class Variables
    [SerializeField]
    [Range(1,2)]
    public int playerID = 1;

    // Component Variables
    RobotSyncBehavior RobotSyncScrpt;

    //Test Variables
    public string AxisName;
    public bool AxisState;

    void Start ()
    {
        RobotSyncScrpt = GetComponent<RobotSyncBehavior>();
    }
    
   	// Update is called once per frame
	void Update ()
    {
        ReadPlayerInput();  // Reads the player input
        
	}

    // Read a single Player ID
    void ReadPlayerInput()
    {
        // Key variables.
        string keyRead;     // String cointaining the Read Key.
        bool keyState = false;      // Logic state of that key.

        // Forward_P axis
        keyRead = "Forward_P" + playerID.ToString();
        if (Input.GetAxis(keyRead) >0)
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetAxis(keyRead) < 0)
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // Sideways_P axis
        keyRead = "Sideways_P" + playerID.ToString();
        if (Input.GetAxis(keyRead) > 0)
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetAxis(keyRead) < 0)
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // LeftPunch_P axis
        keyRead = "LeftPunch_P" + playerID.ToString();
        if (Input.GetButtonUp(keyRead))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // RightPunch_P axis
        keyRead = "RightPunch_P" + playerID.ToString();
        if (Input.GetButtonUp(keyRead))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // LeftKick_P axis
        keyRead = "LeftKick_P" + playerID.ToString();
        if (Input.GetButtonUp(keyRead))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // RightKick_P axis
        keyRead = "RightKick_P" + playerID.ToString();
        if (Input.GetButtonUp(keyRead))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // Block_P axis
        keyRead = "Block_P" + playerID.ToString();
        if (Input.GetButtonUp(keyRead))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        AxisName = keyRead;
        AxisState = keyState;

    }
}
