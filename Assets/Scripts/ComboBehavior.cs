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

    // Use this for initialization
	void Start ()
    {
        // Get HitBehavior Script
        // Get Input Scripts
        inputArray = GetComponents<InputManager>();
        // Get Mesh Material
        meshMaterial = meshObj.GetComponent<Renderer>().material;
        origMeshColor = meshMaterial.color;
        stunMeshColor = Color.yellow;
    }

    // Update is called once per frame
    void Update ()
    {
        comboTimer = 3.0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Start Stun");
            StartCoroutine(StartComboStun(comboTimer));
        }

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
            inputArray[i].isIgnoringInput = true;
        }
        // Retint material
        meshMaterial.color = stunMeshColor;

        // Terminate Action!

        // Stun animation Start goes here!

        yield return new WaitForSeconds(timer);
            // Dumb Wait for timer Time!
        // Get out of stun
        // Reset input
        for (int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i].isIgnoringInput = false;
        }
        // Retint material
        meshMaterial.color = origMeshColor;
    }
}
