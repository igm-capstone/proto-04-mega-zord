using UnityEngine;
using System.Collections;

public class ComboBonus : MonoBehaviour
{
    // Combo Varialbes
    ActorBehavior actBhvr;
    RobotSyncBehavior rbtSync;
    bool isComboing;
    float comboTimer = 0;
    public int cmbCount;
    public float cmbThreshold;
    public float cmbDmgMult;
    public float cmbTimeout = 4;
    public float chipDamage = 1;
    string prvsHit;


    InputPanelHUD otherHUD;
    // FeedBack variables
    // Color Flash
    public GameObject meshObj;
    Material meshMaterial;
    Color origMeshColor;
    Color dmgMeshColor;
    Color blckMeshColor;
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
        cmbCount = 0;
        cmbPitch = 0.8f;
        cmbDmgMult = 1.0f;
        cmbThreshold = 1.0f;

        // Get ActorBehavior Script and gebing listening for events
        actBhvr = GetComponentInChildren<ActorBehavior>();
        actBhvr.DidGetHit += ActBhvr_DidGetHit; ;
        actBhvr.DidHit += ActBhvr_DidHit;

        // Get Robot Sunc Behavior
        rbtSync = GetComponent<RobotSyncBehavior>();

        //Get the other robots HUD
        foreach (RobotSyncBehavior robot in GameObject.FindObjectsOfType<RobotSyncBehavior>())
        {
            if (robot != this.rbtSync)
            {
                otherHUD = robot.GetHUD();
                break;
            }
        }

        // Get Mesh Material
        meshMaterial = meshObj.GetComponent<Renderer>().material;
        // Set Up diferent colors
        origMeshColor = meshMaterial.color;
        dmgMeshColor = Color.red;
        blckMeshColor = Color.cyan;

        // Get Sound Player and sounds
        punchSndPlyr = GetComponent<AudioSource>();
        punchSndArray = Resources.LoadAll<AudioClip>("Sounds/");
    }

    private void ActBhvr_DidGetHit(ActorBehavior dfnsActBhvr, HitStats atckHitStats)
    {
        if (atckHitStats.WasBlocked)
        {
            // Flashes the mesh when hit blocked
            StartCoroutine(colorFlash(blckMeshColor));
        }
        else
        {
            // Flashes the mesh when damaged
            StartCoroutine(colorFlash(dmgMeshColor));
        }
    }

    private void ActBhvr_DidHit(ActorBehavior atckActBhvr, HitStats atckHitStats)
    {
        if(atckHitStats.WasBlocked)
        {
            // Adjust damage
            atckHitStats.DamageDealt = chipDamage;
            // Set pitch to very low and play Hit sound.
            cmbPitch = 0.5f;
            punchSndPlyr.pitch = cmbPitch;
            punchSndPlyr.PlayOneShot(punchSndArray[0]);

            // Apply block knockback;

            // Drop Combo
            cmbCount = 0;
            isComboing = false;
            cmbDmgMult = 1.0f;
            prvsHit = null;
            return;
        }
        // Get current hit key
        string curHit = atckHitStats.Key;
        // Checks for Sync-hit
        if (atckHitStats.ComboTiming < cmbThreshold)
        {
            // New hit must be different from precious
            if (curHit != prvsHit)
            {
                // Increase combo count
                cmbCount++;
                // Reset Combo Timer
                comboTimer = cmbTimeout;
                // First Combo Hit
                if (!isComboing)
                { // Combo Start
                    isComboing = true;
                    StartCoroutine(ComboTimer());

                    // Reset pitch and play first Sync-Hit sound.
                    cmbPitch = 1.0f;
                    punchSndPlyr.pitch = cmbPitch;
                    punchSndPlyr.PlayOneShot(punchSndArray[0]);
                }
                else
                {
                    // Increase pitch linearly
                    cmbPitch += 0.2f;
                    // Apply current pitch and play second+ hit sound;
                    punchSndPlyr.pitch = cmbPitch;
                    punchSndPlyr.PlayOneShot(punchSndArray[0]);
                    // Cap Combo sound Pitch
                    if (cmbPitch > 2.4f) cmbPitch = 2.4f;
                }

                // Combo Multiplyer progression
                cmbDmgMult = 1 + (Mathf.Pow(cmbCount, 2)) / 20;
                // Cap combo Multiplier
                if (cmbDmgMult > 2.0f) cmbDmgMult = 2.0f;

                // Updates Damage
                atckHitStats.DamageDealt *= cmbDmgMult;
            }
            else
            { //Sync hit, but same hit - Drop combo.
                cmbCount = 0;
                isComboing = false;
                cmbDmgMult = 1.0f;

                // Reset pitch and play first Sync-Hit sound.
                cmbPitch = 1.0f;
                punchSndPlyr.pitch = cmbPitch;
                punchSndPlyr.PlayOneShot(punchSndArray[0]);
            }
        }
        else // Failed sync hit or blocked hit - Drop combo
        {
            cmbCount = 0;
            isComboing = false;
            cmbDmgMult = 1.0f;
        }
        // Update Combo HUD
        otherHUD.ShowCombo(cmbCount);
        otherHUD.SetComboAlpha(1);

        // Updates previous Hit
        prvsHit = curHit;
    }

    // Makes Mesh Flash during flashTime
    IEnumerator colorFlash(Color flashColor)
    {
        meshMaterial.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        meshMaterial.color = origMeshColor;
    }

    // Counts time player has to continue a combo before ti is automatically dropped.
    IEnumerator ComboTimer()
    {
        // Timming loop (100 miliseconds)
        while (comboTimer > 0.0f)
        {
            yield return new WaitForSeconds(0.1f);
            comboTimer -= 0.1f;
            // Fade combo UI over time
            otherHUD.SetComboAlpha(comboTimer / cmbTimeout);
        }
        // Drop combo
        cmbCount = 0;
        isComboing = false;
        cmbDmgMult = 1.0f;
        prvsHit = null;
        // Update combo UI
        otherHUD.ShowCombo(cmbCount);
    }
}
