using UnityEngine;
using System.Collections;

public class ActorBehavior : MonoBehaviour 
{
    #region Class Variables
    RobotSyncBehavior rbtSyncBhvr;
    public Animator animator;

    // Movement Variables
    Vector3 moveVec;
    private Transform targetRobot;
    public float moveSpeed = 10.0f;
    public float maxRadius = 100.0f;
    public float minRadius = 1.0f; 

    // Input Variables
    string readKey;
    Action forward;
    Action backward;
    Action left;
    Action right;
    Action rightPunch;
    Action leftPunch;
    Action rightKick;
    Action leftKick;
    Action block;

    // Animation Variables
    bool isMoving;
    bool isStunned;

    #endregion

    void Start()
    {
        // Get components
        rbtSyncBhvr = GetComponent<RobotSyncBehavior>();
        //animator = GetComponent<Animator>();

        // Robot Sync Initialization
        rbtSyncBhvr.ActionStarted += rb_ActionStarted;
        rbtSyncBhvr.ActionChanged += rb_ActionChanged;
        rbtSyncBhvr.ActionTerminated += rb_ActionTerminated;

        foreach (ActorBehavior actor in FindObjectsOfType<ActorBehavior>())
        {
            if (actor != this)
            {
                targetRobot = actor.transform;
                break;
            }
        }

    }

    void Update()
    {

        if (moveVec.magnitude != 0)  //if there is some input
        {
            //set that character is moving
            animator.SetBool("Moving", true);
            isMoving = true;
            animator.SetBool("Running", true);
        }
        else
        {
            //character is not moving
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
            isMoving = false;
        }

        UpdateMovement();  //update character Velocity 

    }

    private void rb_ActionTerminated(string obj)
    {
        // Movement Axis
        if (obj == "Forward")
            forward = null;
        else if (obj == "Backward")
            backward = null;

        else if (obj == "Left")
            left = null;
        else if (obj == "Right")
            right = null;

        // Button inputs
        if (obj == "RightPunch")
            rightPunch = null;
        else if (obj == "LeftPunch")
            leftPunch = null;

        else if (obj == "LeftKick")
            leftKick = null;
        else if (obj == "RightKick")
            rightKick = null;

        else if (obj == "Block")
            block = null;

        UpdateActions();
    }

    private void rb_ActionStarted(Action obj)
    {
        // Movement Axis
        if (obj.Key == "Forward")
            forward = obj;
        else if (obj.Key == "Backward")
            backward = obj;

        else if (obj.Key == "Left")
            left = obj;
        else if (obj.Key == "Right")
            right = obj;

        // Attack Inputs
        if (obj.Key == "RightPunch")
            rightPunch = obj;
        else if (obj.Key == "LeftPunch")
            leftPunch = obj;

        else if (obj.Key == "RightKick")
            rightKick = obj;
        else if (obj.Key == "LeftKick")
            leftKick = obj;

        else if (obj.Key == "Block")
            block = obj;

        //Debug.Log("Action Started");
        UpdateActions();
    }
    
    private void rb_ActionChanged(Action evalAction)
    {
        readKey = evalAction.Key;
        if (readKey == "LeftPunch")
        {
            animator.SetBool("MirrorPunch", true);
            animator.SetTrigger("PunchTrigger");
        }
        if (readKey == "RightPunch")
        {
            animator.SetBool("MirrorPunch", false);
            animator.SetTrigger("PunchTrigger");
        }

        if (readKey == "LeftKick")
        {
            animator.SetBool("MirrorKick", false);
            animator.SetTrigger("KickTrigger");
        }
        if (readKey == "RightKick")
        {
            animator.SetBool("MirrorKick", true);
            animator.SetTrigger("KickTrigger");
        }

        if (readKey == "Block")
            animator.SetBool("Block",true);

        //Debug.Log("Action Changed");
        UpdateActions();    // Updates the movement actions.
    }

    void UpdateActions()
    {
        moveVec = new Vector3();
        Vector3 f = new Vector3();
        if (forward != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in forward.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            f += Vector3.forward * count;
        }

        Vector3 b = new Vector3();
        if (backward != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in backward.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            b -= Vector3.forward * count;
        }

        Vector3 r = new Vector3();
        if (right != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in right.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            r += Vector3.right * count;
        }

        Vector3 l = new Vector3();
        if (left != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in left.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            l -= Vector3.right * count;
        }

        moveVec += (f + b + l + r);
    }

    void UpdateMovement()
    {
        // Get movement from Input puts it in Local space
        Vector3 motion = this.transform.localToWorldMatrix.MultiplyVector(moveVec);

        // Lock camera to Always look at target
        transform.LookAt(targetRobot);

        // Aplies Movement
        GetComponent<Rigidbody>().velocity = motion* moveSpeed;
    }

    public void Hit()
    {        
        Debug.Log("hittttttttttttttttttttt");
        rbtSyncBhvr.ActionTerminated += rb_ActionTerminated;
        rbtSyncBhvr.TerminateAction(readKey);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, targetRobot.position);
    }
}
