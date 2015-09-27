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


    // Use this for initialization
	void Start ()
    {
        // Get HitBehavior Script
        // Get Input Script
        // Get Input Script

    }

    // Update is called once per frame
    void Update ()
    {
        // If I'm hit, ignore input
        
        // If I hit, start Combo!
	}
}
