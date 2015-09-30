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

    ComboBonus cmbBnus;

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
    public float DamageScale = 10; // Divides the max health to determine max damage
    private bool tempBool = false;
    #endregion

    void Start()
    {
        // Get components
        rbtSyncBhvr = transform.parent.GetComponent<RobotSyncBehavior>();
        //animator = GetComponent<Animator>();
        health = transform.parent.GetComponent<Health>();
        maxDamage = health.MaxHealth / DamageScale;

        cmbBnus = transform.parent.GetComponent<ComboBonus>();


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

    void Update()
    {
        if (moveVec.magnitude != 0)  //if there is some input
        {
            //set that character is moving
            animator.SetBool("Moving", true);
        }
        else
        {
            //character is not moving
            animator.SetFloat("AnimSpeed", 0.6f);
            animator.SetBool("Moving", false);
        }

        //Start animations if it is not already playing
        //if (leftKick != null && animator.GetBool("EndLeftKick") == true)
        //{
        //    Debug.Log("1");
        //    animator.SetBool("LeftKickTrigger", true);
        //    animator.SetBool("EndLeftKick", false);
        //}        
        //else if (rightKick != null && animator.GetBool("EndRightKick") == true)
        //{
        //    animator.SetBool("RightKickTrigger", true);
        //    animator.SetBool("EndRightKick", false);
        //}
        //else if (leftPunch != null && animator.GetBool("EndLeftPunch") == true)
        //{
        //    animator.SetBool("LeftPunchTrigger", true);
        //    animator.SetBool("EndLeftPunch", false);
        //}
        //else if (rightPunch != null && animator.GetBool("EndRightPunch") == true)
        //{
        //    animator.SetBool("RightPunchTrigger", true);
        //    animator.SetBool("EndRightPunch", false);
        //}
        //Debug.Log("leftKick" + leftKick);
        //Debug.Log("rightKick" + rightKick);
        //Debug.Log("leftPunch" + leftPunch);
        //Debug.Log("rightPunch" + rightPunch);
        Debug.Log(tempBool);
        UpdateMovement();  //update character Velocity   
        TerminateAnimation();
    }

    //Transition to Idle Animation if the actions weren't synchronised
    private void TerminateAnimation()
    {
        if (leftKick == null && animator.GetCurrentAnimatorStateInfo(0).IsName("LeftKick") && animator.GetFloat("LeftKickSpeed") < 0.95f)
        {
            animator.SetFloat("LeftKickSpeed", 0.2f);
            animator.SetBool("LeftKickTrigger", false);
            animator.SetBool("EndLeftKick", true);
        }

        if (rightKick == null && animator.GetCurrentAnimatorStateInfo(0).IsName("RightKick") && animator.GetFloat("RightKickSpeed") < 0.95f)
        {
            animator.SetFloat("RightKickSpeed", 0.2f);
            animator.SetBool("RightKickTrigger", false);
            animator.SetBool("EndRightKick", true);
        }

        if (leftPunch == null && animator.GetCurrentAnimatorStateInfo(0).IsName("LeftPunch") && animator.GetFloat("LeftPunchSpeed") < 0.95f)
        {
            animator.SetFloat("LeftPunchSpeed", 0.2f);
            animator.SetBool("LeftPunchTrigger", false);
            animator.SetBool("EndLeftPunch", true);
        }

        if (rightKick == null && animator.GetCurrentAnimatorStateInfo(0).IsName("RightPunch") && animator.GetFloat("RightPunchSpeed") < 0.95f)
        {
            animator.SetFloat("RightPunchSpeed", 0.2f);
            animator.SetBool("RightPunchTrigger", false);
            animator.SetBool("EndRightPunch", true);
        }
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
            GetAnimSpeed();
            if (rightPunch.IsSynchronized() && rightPunchAnim == false)
            {
                float cmbTime = Time.time - rightPunch.Time;
                float invTime = Mathf.Exp(-1.622f * (cmbTime + Mathf.Epsilon));
                rightHand.GetComponent<HitBehavior>().hitStats = new HitStats("RightPunch", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers, cmbTime);
                rbtSyncBhvr.TerminateAction("RightPunch");
            }
            else if (!rightPunch.IsActive())
                rbtSyncBhvr.TerminateAction("RightPunch");
        }
        else if (leftPunch != null)
        {
            GetAnimSpeed();
            if (leftPunch.IsSynchronized() && leftPunchAnim == false)
            {
                float cmbTime = Time.time - leftPunch.Time;
                float invTime = Mathf.Exp(-1.622f * (cmbTime+ Mathf.Epsilon));
                leftHand.GetComponent<HitBehavior>().hitStats = new HitStats("LeftPunch", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers, cmbTime);
                rbtSyncBhvr.TerminateAction("LeftPunch");
            }
            else if (!leftPunch.IsActive())
                rbtSyncBhvr.TerminateAction("LeftPunch");
        }
        else if (leftKick != null)
        {
            GetAnimSpeed();
            if (tempBool == false && animator.GetBool("EndLeftKick") == true)
            {
                Debug.Log("1");
                animator.SetBool("LeftKickTrigger", true);
                animator.SetBool("EndLeftKick", false);
            }   
            if (leftKick.IsSynchronized() && leftKickAnim == false)
            {
                float cmbTime = Time.time - leftKick.Time;
                float invTime = Mathf.Exp(-1.622f * (cmbTime + Mathf.Epsilon));
                leftFoot.GetComponent<HitBehavior>().hitStats = new HitStats("LeftPunch", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers, cmbTime);
                rbtSyncBhvr.TerminateAction("LeftKick");
                tempBool = true;
            }
            else if (!leftKick.IsActive())                            
                rbtSyncBhvr.TerminateAction("LeftKick");
        }
        else if (rightKick != null)
        {
            GetAnimSpeed();
            if (rightKick.IsSynchronized() && rightKickAnim == false)
            {
                float cmbTime = Time.time - rightKick.Time;
                float invTime = Mathf.Exp(-1.622f * (cmbTime + Mathf.Epsilon));
                rightFoot.GetComponent<HitBehavior>().hitStats = new HitStats("LeftPunch", rbtSyncBhvr.RobotID, maxDamage * invTime, rbtSyncBhvr.NumberOfPlayers, cmbTime);
                rbtSyncBhvr.TerminateAction("RightKick");
            }
            else if (!rightKick.IsActive())
                rbtSyncBhvr.TerminateAction("RightKick");
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

    void GetAnimSpeed()
    {
        float animSpeed = 0;        

        if (rightPunch != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in rightPunch.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            float temp = animator.GetFloat("RightPunchSpeed");
            animSpeed = 0.0025f * Mathf.Exp(1.4914f * count * 4 / rbtSyncBhvr.NumberOfPlayers);
            if (temp < 0.95f || animSpeed >= 0.95f)
                animator.SetFloat("RightPunchSpeed", animSpeed);
        }
        else if (leftPunch != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in leftPunch.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            float temp = animator.GetFloat("LeftPunchSpeed");
            animSpeed = 0.0025f * Mathf.Exp(1.4914f * count * 4 / rbtSyncBhvr.NumberOfPlayers);
            if (temp < 0.95f || animSpeed >= 0.95f)
                animator.SetFloat("LeftPunchSpeed", animSpeed);
        }
        else if (rightKick != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in rightKick.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            float temp = animator.GetFloat("RightKickSpeed");
            animSpeed = 0.0025f * Mathf.Exp(1.4914f * count * 4 / rbtSyncBhvr.NumberOfPlayers);
            if (temp < 0.95f || animSpeed >= 0.95f)
                animator.SetFloat("RightKickSpeed", animSpeed);
        }

        else if (leftKick != null)
        {
            int count = 0;
            foreach (PlayerSyncInfo psi in leftKick.PlayerSyncInfos)
            {
                if (psi.IsPressing()) { count++; }
            }
            if (count == 0)
                tempBool = false;
            float temp = animator.GetFloat("LeftKickSpeed");
            animSpeed = 0.0025f * Mathf.Exp(1.4914f * count * 4 / rbtSyncBhvr.NumberOfPlayers);
            if (temp < 0.95f || animSpeed >= 0.95f)
                animator.SetFloat("LeftKickSpeed", animSpeed);
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
       }

        moveVec += (f + b + l + r);

        // Calculate animation speed
        float animSp = (moveVec / speedScale).magnitude/3.7f;
        if (b.magnitude > 0) animSp *= -1;
        animator.SetFloat("AnimSpeed", animSp);
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
                DidGetHit(this, hs);
                DidHit(other.gameObject.GetComponent<HitBehavior>().ab, hs);
                other.gameObject.GetComponent<HitBehavior>().hitStats = null;
                
                // This needs to Happen after the Hit events are sent.
                health.TakeDamage(hs.DamageDealt);
            }
        }
    }

    //function from the animation
    public void Hit(float value)
    {
    }

    public void EndAnimation(float value)
    {
        if (value == 1)   //LeftKick
        {
            animator.SetBool("EndLeftKick", true);
            animator.SetFloat("LeftKickSpeed", 0.2f);
            leftKickAnim = false;
        }
        if (value == 2)   //RightKick
        {
            animator.SetBool("EndRightKick", true);
            animator.SetFloat("RightKickSpeed", 0.2f);
            rightKickAnim = false;
        }
        if (value == 3)   //LeftPunch
        {
            animator.SetBool("EndLeftPunch", true);
            animator.SetFloat("LeftPunchSpeed", 0.2f);
            leftPunchAnim = false;
        }
        if (value == 4)   //RightPunch
        {
            animator.SetBool("EndRightPunch", true);
            animator.SetFloat("RightPunchSpeed", 0.2f);
            rightPunchAnim = false;
        }
        if (value == 5)   //Block
            blockAnim = false;
    }

    public void StartAnimation(float value)
    {
        if (value == 1)   //LeftKick
            leftKickAnim = true;
        if (value == 2)   //RightKick
            rightKickAnim = true;
        if (value == 3)   //LeftPunch
            leftPunchAnim = true;
        if (value == 4)   //RightPunch
            rightPunchAnim = true;
        if (value == 5)   //Block
            blockAnim = true;
    }
}
