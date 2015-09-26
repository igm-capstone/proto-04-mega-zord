using UnityEngine;
using System.Collections;


// Reads every player input from the controller.
public class InputManager : MonoBehaviour
{
    // Class Variables
    // Player information
    [SerializeField]
    [Range(1, 8)]
    public int playerID = 1;

    //dPad Variables
    float lastX;
    float lastY;

    // Component Variables
    RobotSyncBehavior RobotSyncScrpt;
    void Start()
    {
        // Initialize  variables dPAd Buttons
        lastX = 0;
        lastY = 0;
        // Get necessary components.
        RobotSyncScrpt = GetComponent<RobotSyncBehavior>();
    }

    void Update()
    {
        ReadDpad();
        ReadButtons();
    }
    // Read a single Player ID
    void ReadButtons()
    {
        // Key variables.
        string keyRead;     // String cointaining the Read Key.
        bool keyState = false;      // Logic state of that key.

        // Buttons
        // LeftPunch_P axis
        keyRead = "LeftPunch";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // RightPunch_P axis
        keyRead = "RightPunch";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // LeftKick_P axis
        keyRead = "LeftKick";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // RightKick_P axis
        keyRead = "RightKick";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }

        // Block_P axis
        keyRead = "Block";
        if (Input.GetButtonUp(keyRead + "_P" + playerID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + playerID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, playerID, keyState);
        }
    }

    //REads the DPad as Button inputs instead of Axis
    void ReadDpad()
    {
        float LastDpadX = lastX;
        float LastDpadY = lastY;

        float CurDpadX = Input.GetAxisRaw("DPadX_P" + playerID.ToString());
        float CurDpadY = Input.GetAxisRaw("DPadY_P" + playerID.ToString());

        //Debug.Log("CurDpadX: " + CurDpadX.ToString());
        //Debug.Log("CurDpadY: " + CurDpadY.ToString());

        #region X-Axis
        // RightStrafe
        // GetButtonUp
        if (CurDpadX == 1.0f && LastDpadX != 1.0f)
            RobotSyncScrpt.ReceiveInput("Right", playerID, true);
        // GetButtonDown
        else if (CurDpadX != 1.0f && LastDpadX == 1.0f)
            RobotSyncScrpt.ReceiveInput("Right", playerID, false);

        // LeftStrafe
        // GetButtonUp
        if (CurDpadX == -1.0f && LastDpadX != -1.0f)
            RobotSyncScrpt.ReceiveInput("Left", playerID, true);
        // GetButtonDown
        else if (CurDpadX != -1.0f && LastDpadX == -1.0f)
            RobotSyncScrpt.ReceiveInput("Left", playerID, false);
        #endregion

        #region Y-Axis
        // Forward
        // GetButtonUp
        if (CurDpadY == 1.0f && LastDpadY != 1.0f)
            RobotSyncScrpt.ReceiveInput("Forward", playerID, true);
        // GetButtonDown
        else if (CurDpadY != 1.0f && LastDpadY == 1.0f)
            RobotSyncScrpt.ReceiveInput("Forward", playerID, false);

        // Backwards
        // GetButtonUp
        if (CurDpadY == -1.0f && LastDpadY != -1.0f)
            RobotSyncScrpt.ReceiveInput("Backward", playerID, true);
        // GetButtonDown
        else if (CurDpadY != -1.0f && LastDpadY == -1.0f)
            RobotSyncScrpt.ReceiveInput("Backward", playerID, false);
        #endregion
        
        // Update last X and Y
        lastX = CurDpadX;
        lastY = CurDpadY;
    }
}
