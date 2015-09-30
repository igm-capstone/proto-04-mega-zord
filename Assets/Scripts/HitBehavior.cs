using UnityEngine;
using System.Collections;

public class HitStats
{

    public string Key;          // Attacking Robot Key
    public int RobotID;         // Attacking Robot ID
    public float DamageDealt;   // Damage Dealt to Defending Robot
    public int NumActive;       // Attacking robot currently pressing the button
    public float ComboTiming;   // Elapsed time since first player pressed the attack button to last player pressed button
    public bool WasBlocked;     // Was the hit Blocked?

    public HitStats(string key, int robotID, float damageDealt, int numActive, float comboTimming, bool wasBlocked)
    {
        Key = key;
        DamageDealt = damageDealt;
        NumActive = numActive;
        RobotID = robotID;
        ComboTiming = comboTimming;
        WasBlocked = wasBlocked;
    }
}


public class HitBehavior : MonoBehaviour
{
    public ActorBehavior ab;
    public HitStats hitStats = null;
}
