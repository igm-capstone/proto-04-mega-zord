using UnityEngine;
using System.Collections;

public class ComboBonus : MonoBehaviour
{

    // Class Variables

    // Component Variables
    //public GameObject meshObj;
    //Material meshMaterial;
    //Color origMeshColor;
    //Color stunMeshColor;

    ActorBehavior actBhvr;

    RobotSyncBehavior RbtSync;

    // Use this for initialization
    void Start()
    {
        // Get ActorBehavior Script and gebing listening for events
        actBhvr = GetComponentInChildren<ActorBehavior>();
        actBhvr.DidGetHit += ActBhvr_DidGetHit; ;
        actBhvr.DidHit += ActBhvr_DidHit;

        // Get Robot Sunc Behavior
        RbtSync = GetComponent<RobotSyncBehavior>();

        // Get Mesh Material
        //meshMaterial = meshObj.GetComponent<Renderer>().material;

        // Set Up diferent colors
        //origMeshColor = meshMaterial.color;
        //stunMeshColor = Color.yellow;
    }

    private void ActBhvr_DidGetHit(ActorBehavior dfnsActBhvr, HitStats dfnsHitStats)
    {
        Debug.Log(dfnsActBhvr.gameObject.transform.parent.name + " got Hit");
    }

    private void ActBhvr_DidHit(ActorBehavior atckActBhvr, HitStats atckHitStats)
    {
        Debug.Log(atckActBhvr.gameObject.transform.parent.name + " Hit");
        // Call Hit Function here!
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
