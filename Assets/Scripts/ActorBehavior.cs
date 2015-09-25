using UnityEngine;
using System.Collections;

public class ActorBehavior : MonoBehaviour 
{
    RobotSyncBehavior rb;
    Animator animator;
    string key;

    void Start()
    {
        rb = GetComponent<RobotSyncBehavior>();
        animator = GetComponent<Animator>();
        rb.ActionStarted += rb_ActionMove;
        
    }

    void rb_ActionTerminated(string obj)
    {
        //animation still goes on. but the key can be pressed again to repeat the action
    }
    
    void rb_ActionMove(Action obj)
    {
        key = obj.Key;
        if (key == "LeftPunch")
        {
            animator.SetBool("MirrorPunch", true);
            animator.SetTrigger("PunchTrigger");
        }
        if (key == "RightPunch")
        {
            animator.SetBool("MirrorPunch", false);
            animator.SetTrigger("PunchTrigger");
        }

        if (key == "LeftKick")
        {
            animator.SetBool("MirrorKick", false);
            animator.SetTrigger("KickTrigger");
        }
        if (key == "RightKick")
        {
            animator.SetBool("MirrorKick", true);
            animator.SetTrigger("KickTrigger");
        }

        if (key == "Block")
            animator.SetTrigger("Attack1Trigger");
    }

    public void Hit()
    {        
        //Debug.Log("hit" + key);
        rb.ActionTerminated += rb_ActionTerminated;
        rb.TerminateAction(key);
    }
}
