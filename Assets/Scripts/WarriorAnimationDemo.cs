﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorAnimationDemo : MonoBehaviour {
    public RobotSyncBehavior rbs;
	public Animator animator;

	float rotationSpeed = 30;
	Vector3 inputVec;
	bool isMoving;
	bool isStunned;

    Action forward;
    Action backward;
    Action left;
    Action right;
    Action rightPunch;
    Action leftPunch;
    Action rightKick;
    Action leftKick;
    Action block;


    //Warrior types
    public enum Warrior{Karate, Ninja, Brute, Sorceress};

	public Warrior warrior;
	
    void Start()
    {
        rbs = GetComponent<RobotSyncBehavior>();
        rbs.ActionStarted += Rbs_ActionStarted;
        rbs.ActionChanged += Rbs_ActionChanged;
        rbs.ActionTerminated += Rbs_ActionTerminated;

        //animator = GetComponent<Animator>();
    }

    private void Rbs_ActionTerminated(string obj)
    {
        
        // Movement Axis
        if (obj == "Forward")
        {
            forward = null;
        }
        else if (obj == "Backward")
        {
            backward = null;
        }
        else if (obj == "Left")
        {
            left = null;
        }
        else if (obj == "Right")        {
            right = null;
        }
        
        // Button inputs
        if (obj == "RightPunch")
        {
            //forward = null;
            rightPunch = null;
        }
        else if (obj == "LeftKick")
        {
            //backward = null;
            leftKick = null;
        }
        else if (obj == "LeftPunch")
        {
            //left = null;
            leftPunch = null;
        }
        else if (obj == "RightKick")
        {
            //right = null;
            rightKick = null;
        }
        else if (obj == "Block")
        {
            block = null;
        }
        UpdateActions();
    }

    private void Rbs_ActionStarted(Action obj)
    {
        
        // Movement Axis
        if (obj.Key == "Forward")          
        {
            forward = obj;
        }
        else if (obj.Key == "Backward")      
        {
            backward = obj;
        }
        else if (obj.Key == "Left")     
        {
            left = obj;
        }
        else if (obj.Key == "Right")    
        {
            right = obj;
        }
        
        // Attack Inputs
        if (obj.Key == "RightPunch")        
        {
            rightPunch = obj;
            //forward = obj;
        }
        else if (obj.Key == "LeftKick")     
        {
            leftKick = obj;
            //backward = obj;
        }
        else if (obj.Key == "LeftPunch")    
        {
            leftPunch = obj;
            //left = obj;
        }
        else if (obj.Key == "RightKick")    
        {
            rightKick = obj;
            //right = obj;
        }
        else if (obj.Key == "Block")
        {
            block = obj;
        }

        Debug.Log("Action Started");
        UpdateActions();
    }

    private void Rbs_ActionChanged(Action obj)
    {
        Debug.Log("Action Changed");
        UpdateActions();
    }


    void UpdateActions()
    {
        inputVec = new Vector3();

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

        inputVec += (f + b + l + r);

        Debug.Log(inputVec);
    }

   
    void Update()
	{
        //Get input from controls
        float x = inputVec.x;  //Input.GetAxisRaw("Forward_P1");
        float z = inputVec.z;  //-(Input.GetAxisRaw("Sideways_P1"));
                               //	inputVec = new Vector3(-x, 0, -z);

       // UpdateMovement();
		//Apply inputs to animator
		//animator.SetFloat("Input X", z);
		//animator.SetFloat("Input Z", -(x));

		if (x != 0 || z != 0 )  //if there is some input
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

        RotateTowardsMovementDir();
        UpdateMovement();  //update character position and facing


    }

    //public IEnumerator COStunPause(float pauseTime)
    //{
    //	isStunned = true;
    //	yield return new WaitForSeconds(pauseTime);
    //	isStunned = false;
    //}

    void RotateTowardsMovementDir()  //face character along input direction
    {
        if (inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
        }
    }

   void UpdateMovement()
   {
   	Vector3 motion = inputVec;  //get movement input from controls

    	//reduce input for diagonal movement
    	//motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
        GetComponent<Rigidbody>().velocity = motion *10;
        Debug.Log(motion);

    //	RotateTowardsMovementDir();  //if not strafing, face character along input direction

   	//return inputVec.magnitude;  //return a movement value for the animator, not currently used
   }
}