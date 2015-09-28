using UnityEngine;
using System.Collections;

// Script responsible for the Combo System
public class ComboBehavior : MonoBehaviour
{

    // Get a Hit
    // If I hit: start combo timer
    // While on combo timer:
    //      Atacking robot: 
    //          Update Combo Counter UI
    //          Escalate Damage.
    //      Defensive Robot:
    //          Start Stun
    //              Tint Mesh Yellow
    //              Ignore Input
    //              If hit again restart timer

    // Class Variables
    float comboTimer;

    // Component Variables
    InputManager[] inputArray;

    public GameObject meshObj;
    Material meshMaterial;
    Color origMeshColor;
    Color stunMeshColor;

    ActorBehavior actBhvr;

    RobotSyncBehavior RbtSync;

    // Use this for initialization
	void Start ()
    {
        // Get ActorBehavior Script and gebing listening for events
        actBhvr = GetComponentInChildren<ActorBehavior>();
        actBhvr.DidGetHit += ActBhvr_DidGetHit; ;
        actBhvr.DidHit += ActBhvr_DidHit;

        // Get Robot Sunc Behavior
        RbtSync = GetComponent<RobotSyncBehavior>();

        // Get Input Scripts
        inputArray = GetComponents<InputManager>();
        
        // Get Mesh Material
        meshMaterial = meshObj.GetComponent<Renderer>().material;
        
        // Set Up diferent colors
        origMeshColor = meshMaterial.color;
        stunMeshColor = Color.yellow;
    }

    private void ActBhvr_DidGetHit(ActorBehavior arg1, HitStats arg2)
    {
        // Reset stun counter

        Debug.Log(this.gameObject.name + "got Hit");
        // Call Stun Function Here
    }

    private void ActBhvr_DidHit(ActorBehavior arg1, HitStats arg2)
    {
        Debug.Log(this.gameObject.name + "Hit");
        // Call Hit Function here!
    }

    // Update is called once per frame
    void Update ()
    {
        // Test Code
        //  Stun start
        comboTimer = 3.0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Start Stun");
            StartCoroutine(StartComboStun(comboTimer));
        }
        // Stun Reset
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Pressed Z");
            StopCoroutine(StartComboStun(comboTimer));
            comboTimer = 1.0f;
            StartCoroutine(StartComboStun(comboTimer));
        }
        
        // If I hit
            // Start My Combo Counter
            // Reset Timer counter
            // Timer counter needs to be smaler each time.
	}

    // Start stun cycle
    IEnumerator StartComboStun(float timer)
    {
        //Set up combo stun
        // Ignores input
        for (int i = 0; i < inputArray.Length; i++)
        {
            //inputArray[i].isIgnoringInput = true;
        }
        // Retint material
        meshMaterial.color = stunMeshColor;

        // Terminate Action!
        //RbtSync.TerminateAction(curAction);
        

        // Stun animation Start goes here!

        yield return new WaitForSeconds(timer);
            // Dumb Wait for timer Time!
        // Get out of stun
        // Reset input
        for (int i = 0; i < inputArray.Length; i++)
        {
            //inputArray[i].isIgnoringInput = false;
        }
        // Retint material
        meshMaterial.color = origMeshColor;
    }

    // Gets called each time a hit happens
    void Hitcombo()
    {
        // precious hit counter = hit counter +1
        // nextHitDamage gets escalated down
    }
}
