using UnityEngine;
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

    public float vectorMax = 5.0f;
    //Warrior types
    public enum Warrior{Karate, Ninja, Brute, Sorceress};

	public Warrior warrior;
	
    void Start()
    {
        rbs = GetComponent<RobotSyncBehavior>();
        rbs.ActionStarted += Rbs_ActionStarted;
        rbs.ActionChanged += Rbs_ActionChanged;
        rbs.ActionTerminated += Rbs_ActionTerminated;
    }

    private void Rbs_ActionTerminated(string obj)
    {
        if (obj == "RightPunch")         // Forward
        {
            forward = null;
        }
        else if (obj == "LeftKick")     // Backward
        {
            backward = null;
        }
        else if (obj == "LeftPunch")    // Left
        {
            left = null;
        }
        else if (obj == "RightKick")    // Right
        {
            right = null;
        }

        UpdateActions();
    }

    private void Rbs_ActionStarted(Action obj)
    {
        if (obj.Key == "RightPunch")         // Forward
        {
            forward = obj;
        }
        else if (obj.Key == "LeftKick")     // Backward
        {
            backward = obj;
        }
        else if (obj.Key == "LeftPunch")    // Left
        {
            left = obj;
        }
        else if (obj.Key == "RightKick")    // Right
        {
            right = obj;
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
    }

   
    void Update()
	{
        //Get input from controls
        float x = inputVec.x;  //Input.GetAxisRaw("Forward_P1");
        float z = inputVec.z;  //-(Input.GetAxisRaw("Sideways_P1"));
                               //	inputVec = new Vector3(-x, 0, -z);

        UpdateMovement();
		//Apply inputs to animator
		animator.SetFloat("Input X", z);
		animator.SetFloat("Input Z", -(x));

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

		if (Input.GetButtonDown("Fire1"))
		{
			animator.SetTrigger("Attack1Trigger");
			if (warrior == Warrior.Brute)
				StartCoroutine (COStunPause(1.2f));
			else if (warrior == Warrior.Sorceress)
				StartCoroutine (COStunPause(1.2f));
			else
				StartCoroutine (COStunPause(.6f));
		}

    //	UpdateMovement();  //update character position and facing
	}

	public IEnumerator COStunPause(float pauseTime)
	{
		isStunned = true;
		yield return new WaitForSeconds(pauseTime);
		isStunned = false;
	}
	
	void RotateTowardsMovementDir()  //face character along input direction
	{
		if (inputVec != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	float UpdateMovement()
	{
		Vector3 motion = inputVec;  //get movement input from controls

		//reduce input for diagonal movement
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
		
		RotateTowardsMovementDir();  //if not strafing, face character along input direction

		return inputVec.magnitude;  //return a movement value for the animator, not currently used
	}

	void OnGUI () 
	{
		if (GUI.Button (new Rect (25, 85, 100, 30), "Attack1")) 
		{
			animator.SetTrigger("Attack1Trigger");

			if (warrior == Warrior.Brute || warrior == Warrior.Sorceress)  //if character is Brute or Sorceress
				StartCoroutine (COStunPause(1.2f));
			else
				StartCoroutine (COStunPause(.6f));
		}
	}
}