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

        UpdateMovementActions();
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
        UpdateMovementActions();
    }
    
    private void rb_ActionChanged(Action evalAction)
    {
        UpdateAttackActions();
        UpdateMovementActions();    // Updates the movement actions.
    }

    void UpdateAttackActions()
    {
        if (rightPunch != null)
        {
            if (rightPunch.IsSynchronized())
            {
                animator.SetBool("MirrorPunch", false);
                animator.SetTrigger("PunchTrigger");
                rbtSyncBhvr.TerminateAction("RightPunch");
            }
            else if (!rightPunch.IsActive()) {
                rbtSyncBhvr.TerminateAction("RightPunch");
            }
        }
        else if (leftPunch != null)
        {
            if (leftPunch.IsSynchronized())
            {
                animator.SetBool("MirrorPunch", true);
                animator.SetTrigger("PunchTrigger");
                rbtSyncBhvr.TerminateAction("LeftPunch");
            }
            else if (!leftPunch.IsActive())
            {
                rbtSyncBhvr.TerminateAction("LeftPunch");
            }
        }
        else if (leftKick != null)
        {
            if (leftKick.IsSynchronized())
            {
                animator.SetBool("MirrorKick", false);
                animator.SetTrigger("KickTrigger");
                rbtSyncBhvr.TerminateAction("LeftKick");
            }
            else if (!leftKick.IsActive())
            {
                rbtSyncBhvr.TerminateAction("LeftKick");
            }
        }
        else if (rightKick != null)
        {
            if (rightKick.IsSynchronized())
            {
                animator.SetBool("MirrorKick", true);
                animator.SetTrigger("KickTrigger");
                rbtSyncBhvr.TerminateAction("RightKick");
            }
            else if (!rightKick.IsActive())
            {
                rbtSyncBhvr.TerminateAction("RightKick");
            }
        }
        else if (block != null)
        {
            if (block.IsSynchronized())
            {
                animator.SetBool("Block", true);
                rbtSyncBhvr.TerminateAction("Block");
            }
            else if (!block.IsActive())
            {
                rbtSyncBhvr.TerminateAction("Block");
            }
        }
    }


    void UpdateMovementActions()
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

        rbtSyncBhvr.TerminateAction(readKey);
    }
}
