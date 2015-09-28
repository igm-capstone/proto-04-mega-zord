using UnityEngine;
using System.Collections;

public class ActorBehavior : MonoBehaviour 
{
    public event System.Action<ActorBehavior, HitStats> DidGetHit;
    public event System.Action<ActorBehavior, HitStats> DidHit;

    #region Class Variables
    RobotSyncBehavior rbtSyncBhvr;
    Health health;
    public Animator animator;

    // Movement Variables
    Vector3 moveVec;
    private Transform targetRobot;
    //public float maxRadius = 100.0f;
    //public float minRadius = 1.0f; 

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

    GameObject leftHand;
    GameObject rightHand;
    GameObject leftFoot;
    GameObject rightFoot;

    // Animation Variables
    bool isMoving;
    bool isStunned;
    float AnimSpeed;

    private bool leftKickAnim = false, rightKickAnim = false, leftPunchAnim = false, rightPunchAnim = false, blockAnim = false;
    private float maxDamage;
    public float DamageScale = 5; // Divides the max health to determine max damage
    #endregion

    void Start()
    {
        // Get components
        rbtSyncBhvr = transform.parent.GetComponent<RobotSyncBehavior>();
        //animator = GetComponent<Animator>();
        health = transform.parent.GetComponent<Health>();
        maxDamage = health.MaxHealth / DamageScale;

        // Robot Sync Initialization
        rbtSyncBhvr.ActionStarted += rb_ActionStarted;
        rbtSyncBhvr.ActionChanged += rb_ActionChanged;
        rbtSyncBhvr.ActionTerminated += rb_ActionTerminated;

        leftHand = transform.FindChild("Motion/B_Pelvis/B_Spine/B_Spine1/B_L_Clavicle/B_L_UpperArm/B_L_Forearm/B_L_Hand").gameObject;
        rightHand = transform.FindChild("Motion/B_Pelvis/B_Spine/B_Spine1/B_R_Clavicle/B_R_UpperArm/B_R_Forearm/B_R_Hand").gameObject;
        leftFoot = transform.FindChild("Motion/B_Pelvis/B_L_Thigh/B_L_Calf/B_L_Foot").gameObject;
        rightFoot = transform.FindChild("Motion/B_Pelvis/B_R_Thigh/B_R_Calf/B_R_Foot").gameObject;

        leftHand.GetComponent<HitBehavior>().ab = this;
        rightHand.GetComponent<HitBehavior>().ab = this;
        leftFoot.GetComponent<HitBehavior>().ab = this;
        rightFoot.GetComponent<HitBehavior>().ab = this;

        foreach (ActorBehavior actor in FindObjectsOfType<ActorBehavior>())
        {
            if (actor != this)
            {
                targetRobot = actor.transform;
                break;
            }
        }

        // set Running to always true because we don't use it.
        //animator.SetBool("Running", true);
    }

    void Awake()
    {
        
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
        UpdateAttackActions();
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
            //float count = 0;
            //foreach (PlayerSyncInfo psi in rightPunch.PlayerSyncInfos)
            //{
            //    if (psi.IsPressing()) { count++; }
            //}

            //float moveSpeed = ((Mathf.Exp(5 / rbtSyncBhvr.NumberOfPlayers * count) - 1) / 20);
            //moveSpeed /= ((Mathf.Exp(5) - 1) / 40);
            //animator.SetFloat("PunchAnimSpeed", moveSpeed);
            //animator.SetBool("MirrorPunch", false);
            //animator.SetTrigger("PunchTrigger");

            if (rightPunch.IsSynchronized() && rightPunchAnim == false)
            {
                float invTime = Mathf.Exp(-1.622f * (Time.time - rightPunch.Time + Mathf.Epsilon));
                rightHand.GetComponent<HitBehavior>().hitStats = new HitStats("RightPunch", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers);
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
            if (leftPunch.IsSynchronized() && leftPunchAnim == false)
            {
                float invTime = Mathf.Exp(-1.622f * (Time.time - rightPunch.Time + Mathf.Epsilon));
                leftHand.GetComponent<HitBehavior>().hitStats = new HitStats("LeftPunch", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers);
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
            if (leftKick.IsSynchronized() && leftKickAnim == false)
            {
                float invTime = Mathf.Exp(-1.622f * (Time.time - rightPunch.Time + Mathf.Epsilon));
                leftFoot.GetComponent<HitBehavior>().hitStats = new HitStats("LeftKick", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers);
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
            if (rightKick.IsSynchronized() && rightKickAnim == false)
            {
                float invTime = Mathf.Exp(-1.622f * (Time.time - rightPunch.Time + Mathf.Epsilon));
                rightFoot.GetComponent<HitBehavior>().hitStats = new HitStats("RightKick", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers);
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
            if (block.IsSynchronized() && blockAnim == false)
            {
                // Not sure what to do here yet.
               // rightHand.GetComponent<HitBehavior>().hitStats = new HitStats("RightPunch", rightPunch.SyncScore, 4);
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

        // Movement Rate adjustment Variables
        float adjstConst = 5;
        float speedScale = 2;

        if (forward != null)
        {
            int count = 0;
            float moveSpeed = 0;
            foreach (PlayerSyncInfo psi in forward.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }

            // Calculate movement speed
            moveSpeed = ((Mathf.Exp(adjstConst / rbtSyncBhvr.NumberOfPlayers* count) - 1) / 10);
            moveSpeed /= speedScale;
            f = Vector3.forward * moveSpeed;

            //// Calculate animaiton speed
            //moveSpeed /=((Mathf.Exp(adjstConst) - 1) / 20); // Normalization
            //animator.SetFloat("AnimSpeed", moveSpeed);
        }

        Vector3 b = new Vector3();
        if (backward != null)
        {
            int count = 0;
            float moveSpeed = 0;
            foreach (PlayerSyncInfo psi in backward.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }

            // Calculate movement speed
            moveSpeed = -((Mathf.Exp(adjstConst / rbtSyncBhvr.NumberOfPlayers * count) - 1) / 10);
            moveSpeed /= speedScale;
            b = Vector3.forward * moveSpeed;

            //// Calculate animaiton speed
            //moveSpeed = Mathf.Abs(moveSpeed);
            //moveSpeed /= ((Mathf.Exp(adjstConst) - 1) / 20); // Normalization
            //animator.SetFloat("AnimSpeed", moveSpeed);
        }

        Vector3 r = new Vector3();
        if (right != null)
        {
            int count = 0;
            float moveSpeed = 0;
            foreach (PlayerSyncInfo psi in right.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            
            // Calculate movement speed
            moveSpeed = ((Mathf.Exp(adjstConst / rbtSyncBhvr.NumberOfPlayers * count) - 1) / 10);
            moveSpeed /= speedScale;
            r = Vector3.right * moveSpeed;

            //// Calculate animaiton speed
            //moveSpeed /= ((Mathf.Exp(adjstConst) - 1) / 20); // Normalization
            //animator.SetFloat("AnimSpeed", moveSpeed);
        }

        Vector3 l = new Vector3();
        if (left != null)
        {
            int count = 0;
            float moveSpeed = 0;
            foreach (PlayerSyncInfo psi in left.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            
            // Calculate movement speed
            moveSpeed = -((Mathf.Exp(adjstConst / rbtSyncBhvr.NumberOfPlayers * count) - 1) / 10);
            moveSpeed /= speedScale;
            l = Vector3.right * moveSpeed;

            //// Calculate animaiton speed
            //moveSpeed = Mathf.Abs(moveSpeed);
            //moveSpeed /= ((Mathf.Exp(adjstConst) - 1) / 20); // Normalization
            //animator.SetFloat("AnimSpeed", moveSpeed);
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
        transform.parent.GetComponent<Rigidbody>().velocity = motion;
    }

    public void OnTriggerEnter(Collider other)
    {
        HitStats hs = null;
        if (other.gameObject.GetComponent<HitBehavior>() && other.gameObject.GetComponent<HitBehavior>().hitStats != null)
        {
            hs = other.gameObject.GetComponent<HitBehavior>().hitStats;
            if (hs.RobotID != rbtSyncBhvr.RobotID)
            {

                health.TakeDamage(hs.SyncScore);
                DidGetHit(this, hs);
                DidHit(other.gameObject.GetComponent<HitBehavior>().ab, hs);
                other.gameObject.GetComponent<HitBehavior>().hitStats = null;
            }
        }
    }

    //function from the animation
    public void Hit()
    {        
        // Nothing for now.
    }

    public void endAnimation(float value)
    {
        if (value == 1)   //Punch
        {
            if(animator.GetBool("MirrorPunch") == true)
                leftPunchAnim = false;
            if(animator.GetBool("MirrorPunch") == false)
                rightPunchAnim = false;
        }
        if(value == 2)   //Kick
        {
            if(animator.GetBool("MirrorKick") == true)
                rightKickAnim = false;
            if (animator.GetBool("MirrorKick") == false)
                leftKickAnim = false;
        }
        if (value == 3)   //Block
            blockAnim = false;
    }

    public void StartAnimation(float value)
    {
        if (value == 1)   //Punch
        {
            if (animator.GetBool("MirrorPunch") == true)
                leftPunchAnim = true;
            if (animator.GetBool("MirrorPunch") == false)
                rightPunchAnim = true;
        }
        if (value == 2)   //Kick
        {
            if (animator.GetBool("MirrorKick") == true)
                rightKickAnim = true;
            if (animator.GetBool("MirrorKick") == false)
                leftKickAnim = true;
        }
        if (value == 3)   //Block
            blockAnim = true;
    }
}
