using UnityEngine;
using System.Collections;

public class ComboBonus : MonoBehaviour
{
    // Combo Varialbes
    ActorBehavior actBhvr;
    RobotSyncBehavior RbtSync;
    bool isComboing;
    float cmbCount;
    public float cmbThreshold;
    public float cmbDmgMult;

    // FeedBack variables
    // Color Flash
    public GameObject meshObj;
    Material meshMaterial;
    Color origMeshColor;
    Color dmgMeshColor;
    float flashTime;
    // Combo sound
    AudioSource punchSndPlyr;   // Player
    AudioClip[] punchSndArray;
    float cmbPitch;

    // Use this for initialization
    void Start()
    {
        // Initialize Class Variables
        flashTime = 0.1f;
        isComboing = false;
        cmbCount = 0.0f;
        cmbPitch = 0.8f;
        cmbDmgMult = 1.0f;
        cmbThreshold = 1.0f;

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

        // Get Sound Player and sounds
        punchSndPlyr = GetComponent<AudioSource>();
        punchSndArray = Resources.LoadAll<AudioClip>("Sounds/");
    }

    private void ActBhvr_DidGetHit(ActorBehavior dfnsActBhvr, HitStats dfnsHitStats)
    {
        // Flashes the mesh when damaged
        StartCoroutine(DmgFlash());
    }

    private void ActBhvr_DidHit(ActorBehavior atckActBhvr, HitStats atckHitStats)
    {
        // Checks for Combo hit
        if (atckHitStats.ComboTiming < cmbThreshold)
        {
            // First Combo Hit
            if (!isComboing)
            {
                // Increase combo count and set to Start Combo
                cmbCount++;
                isComboing = true;

                // Reset Combo sound pitch adn play First Hit sound.
                cmbPitch = 1.0f;
                punchSndPlyr.PlayOneShot(punchSndArray[0]);
            }
            else
            {
                // Second+ Combo Hits
                cmbCount++;

                // Apply current pitch and play second+ hit sound;
                punchSndPlyr.pitch = cmbPitch;
                punchSndPlyr.PlayOneShot(punchSndArray[1]);

                // Increase pitch linearly
                cmbPitch += 0.2f;
                // Cap Combo sound Pitch
                if (cmbPitch > 2.4f) cmbPitch = 2.4f;
            }

            // Combo Multiplyer progression
            cmbDmgMult= 1+(Mathf.Pow(cmbCount,2))/20;
            // Cap combo Multiplier
            if (cmbDmgMult > 2.0f) cmbDmgMult = 2.0f;
            // Updates Damage
            atckHitStats.DamageDealt *= cmbDmgMult;

        }
        // Failed sync hit - Reset Combo variables
        // (Needs testing because I could not test 1 vs Myself)
        else
        {
            cmbCount = 0;
            isComboing = false;
            cmbDmgMult = 1.0f;
        }
    }

    // Makes Mesh Flash during flashTime
    IEnumerator DmgFlash()
    {
        meshMaterial.color = dmgMeshColor;
        yield return new WaitForSeconds(flashTime);
        meshMaterial.color = origMeshColor;
    }
}
