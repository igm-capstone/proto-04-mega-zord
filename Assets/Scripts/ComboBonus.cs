using UnityEngine;
using System.Collections;

public class ComboBonus : MonoBehaviour
{

    // Class Variables
    float flashTime;
    bool isComboing;
    float cmbCount;

    //Component Variables
    public GameObject meshObj;
    Material meshMaterial;
    Color origMeshColor;
    Color dmgMeshColor;

    ActorBehavior actBhvr;

    RobotSyncBehavior RbtSync;

    // Use this for initialization
    void Start()
    {
        // Initialize Class Variables
        flashTime = 0.1f;
        isComboing = false;
        cmbCount = 0.0f;

        // Get ActorBehavior Script and gebing listening for events
        actBhvr = GetComponentInChildren<ActorBehavior>();
        actBhvr.DidGetHit += ActBhvr_DidGetHit; ;
        actBhvr.DidHit += ActBhvr_DidHit;

        // Get Robot Sunc Behavior
        RbtSync = GetComponent<RobotSyncBehavior>();

        // Get Mesh Material
        meshMaterial = meshObj.GetComponent<Renderer>().material;
        // Set Up diferent colors
        origMeshColor = meshMaterial.color;
        dmgMeshColor = Color.red;
    }

    private void ActBhvr_DidGetHit(ActorBehavior dfnsActBhvr, HitStats dfnsHitStats)
    {
        // Flashes the mesh when damaged
        StartCoroutine(DmgFlash());
        Debug.Log(dfnsActBhvr.gameObject.transform.parent.name + " got Hit");
    }

    // 
    private void ActBhvr_DidHit(ActorBehavior atckActBhvr, HitStats atckHitStats)
    {
        // Combo hit
        if (atckHitStats.ComboTiming < 1.0f)
        {
            // First Combo Hit
            if (!isComboing)
            {
                cmbCount++;
                isComboing = true;
                Debug.Log("First Combo Hit: " + cmbCount.ToString());
            }
            else
            {
                cmbCount++;
                // Second + Combo Hit
                Debug.Log("Combo Hit: " + cmbCount.ToString());
            }
        }
        // Failed hit - Reset Combo
        else
        {
            cmbCount = 0;
            isComboing = false;

            Debug.Log("Restart combo: " + cmbCount.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DmgFlash()
    {
        meshMaterial.color = dmgMeshColor;
        yield return new WaitForSeconds(flashTime);
        meshMaterial.color = origMeshColor;
    }
}
