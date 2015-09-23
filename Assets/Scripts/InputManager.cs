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

    void Start ()
    {
        RobotSyncScrpt = GetComponent<RobotSyncBehavior>();
    }

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
        keyRead = "Forward";
        if (Input.GetAxis(keyRead + "_P" + playerID.ToString()) >0)
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetAxis(keyRead + "_P" + playerID.ToString()) < 0)
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // Sideways_P axis
        keyRead = "Sideways";
        if (Input.GetAxis(keyRead + "_P" + playerID.ToString()) > 0)
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetAxis(keyRead + "_P" + playerID.ToString()) < 0)
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // LeftPunch_P axis
        keyRead = "LeftPunch";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // RightPunch_P axis
        keyRead = "RightPunch";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // LeftKick_P axis
        keyRead = "LeftKick";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // RightKick_P axis
        keyRead = "RightKick";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // Block_P axis
        keyRead = "Block";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
    }
}
