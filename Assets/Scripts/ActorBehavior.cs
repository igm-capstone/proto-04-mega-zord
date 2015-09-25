using UnityEngine;
using System.Collections;

public class ActorBehavior : MonoBehaviour 
{
    RobotSyncBehavior rbtSyncBhvr;
    Animator animator;
    string readKey;

    // Movement Variables
    Vector3 movmentVec;

    // Input Actions
    Action forward;
    Action backward;
    Action left;
    Action right;
    Action rightPunch;
    Action leftPunch;
    Action rightKick;
    Action leftKick;
    Action block;

    void Start()
    {
        rbtSyncBhvr = GetComponent<RobotSyncBehavior>();
        animator = GetComponent<Animator>();
        rbtSyncBhvr.ActionStarted += rb_ActionMove;
        
    }

    void rb_ActionTerminated(string obj)
    {
        //animation still goes on. but the key can be pressed again to repeat the action
    }
    
    void rb_ActionMove(Action obj)
    {
        readKey = obj.Key;
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
            animator.SetTrigger("Attack1Trigger");
    }

    public void Hit()
    {        
        //Debug.Log("hit" + key);
        rbtSyncBhvr.ActionTerminated += rb_ActionTerminated;
        rbtSyncBhvr.TerminateAction(readKey);
    }
}
