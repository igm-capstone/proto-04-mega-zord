using UnityEngine;
using System.Collections;


// Reads every player input from the controller.
public class InputManager : MonoBehaviour
{
    // Class Variables
    // Player information
    [SerializeField]
    [Range(1, 8)]
    public int joystickID = 1;

    //dPad Variables
    float lastX;
    float lastY;

    // Ignores Input
    public bool isIgnoringInput;

    // Component Variables
    RobotSyncBehavior RobotSyncScrpt;
    void Start()
    {
        // Initialize  variables dPAd Buttons
        lastX = 0;
        lastY = 0;
        // Get necessary components.
        RobotSyncScrpt = GetComponent<RobotSyncBehavior>();

        // Starts state is reading input;
        isIgnoringInput = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isIgnoringInput = true;
        }
        else
        {
            isIgnoringInput = false;
        }
        // Read input if it is not being ignored
        if (!isIgnoringInput)
        {
            ReadDpad();
            ReadButtons();
        }
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
        if (Input.GetButtonUp(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }

        // RightPunch_P axis
        keyRead = "RightPunch";
        if (Input.GetButtonUp(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }

        // LeftKick_P axis
        keyRead = "LeftKick";
        if (Input.GetButtonUp(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }

        // RightKick_P axis
        keyRead = "RightKick";
        if (Input.GetButtonUp(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }

        // Block_P axis
        keyRead = "Block";
        if (Input.GetButtonUp(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = false;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }
        else if (Input.GetButtonDown(keyRead + "_P" + joystickID.ToString()))
        {
            keyState = true;
            RobotSyncScrpt.ReceiveInput(keyRead, joystickID, keyState);
        }
    }

    //REads the DPad as Button inputs instead of Axis
    void ReadDpad()
    {
        float LastDpadX = lastX;
        float LastDpadY = lastY;

        float CurDpadX = Input.GetAxisRaw("DPadX_P" + joystickID.ToString());
        float CurDpadY = Input.GetAxisRaw("DPadY_P" + joystickID.ToString());

        //Debug.Log("CurDpadX: " + CurDpadX.ToString());
        //Debug.Log("CurDpadY: " + CurDpadY.ToString());

        #region X-Axis
        // RightStrafe
        // GetButtonUp
        if (CurDpadX == 1.0f && LastDpadX != 1.0f)
            RobotSyncScrpt.ReceiveInput("Right", joystickID, true);
        // GetButtonDown
        else if (CurDpadX != 1.0f && LastDpadX == 1.0f)
            RobotSyncScrpt.ReceiveInput("Right", joystickID, false);

        // LeftStrafe
        // GetButtonUp
        if (CurDpadX == -1.0f && LastDpadX != -1.0f)
            RobotSyncScrpt.ReceiveInput("Left", joystickID, true);
        // GetButtonDown
        else if (CurDpadX != -1.0f && LastDpadX == -1.0f)
            RobotSyncScrpt.ReceiveInput("Left", joystickID, false);
        #endregion

        #region Y-Axis
        // Forward
        // GetButtonUp
        if (CurDpadY == 1.0f && LastDpadY != 1.0f)
            RobotSyncScrpt.ReceiveInput("Forward", joystickID, true);
        // GetButtonDown
        else if (CurDpadY != 1.0f && LastDpadY == 1.0f)
            RobotSyncScrpt.ReceiveInput("Forward", joystickID, false);

        // Backwards
        // GetButtonUp
        if (CurDpadY == -1.0f && LastDpadY != -1.0f)
            RobotSyncScrpt.ReceiveInput("Backward", joystickID, true);
        // GetButtonDown
        else if (CurDpadY != -1.0f && LastDpadY == -1.0f)
            RobotSyncScrpt.ReceiveInput("Backward", joystickID, false);
        #endregion
        
        // Update last X and Y
        lastX = CurDpadX;
        lastY = CurDpadY;
    }
}
