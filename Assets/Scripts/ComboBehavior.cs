using UnityEngine;
using System.Collections;

// Script responsible for the Combo System
public class ComboBehavior : MonoBehaviour
{

    // Get a Hit
    // If I hit: start combo timer
    // While on combo timer:
    //      Atacking robot: 
    //          if next hit happen restart timer less time
    //          Update Combo Counter UI
    //          Escalate Damage.
    //      Defensive Robot:
    //          Start Stun
    //              Tint Mesh Yellow
    //          Stay stun for duration of Combo Timer.

    // Component Variables
    InputManager[] inputArray;

    public GameObject meshObj;
    Material meshMaterial;

    // Use this for initialization
	void Start ()
    {
        // Get HitBehavior Script
        // Get Input Scripts
        inputArray = GetComponents<InputManager>();
        // Get Mesh Material
        meshMaterial = meshObj.GetComponent<Material>();
    }

    // Update is called once per frame
    void Update ()
    {
        // If I'm hit
            //ignore input
            // Terminate current Action
            // Wait for Timer
        
        // If I hit
            // Start My Combo Counter
            // Reset Timer counter
            // Timer counter needs to be smaler each time.
	}
}
