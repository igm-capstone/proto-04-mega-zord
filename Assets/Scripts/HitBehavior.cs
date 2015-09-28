using UnityEngine;
using System.Collections;

public class HitStats
{

    public string Key;          // Attacking Robot Key
    public int RobotID;         // Attacking Robot ID
    public float DamageDealt;   // Damage Dealt to Defending Robot
    public int NumActive;       // Attacking robot currently pressing the button
    public float ComboTiming;   // Elapsed time since first player pressed the attack button to last player pressed button

    public HitStats(string key, int robotID, float damageDealt, int numActive, float comboTimming)
    {
        Key = key;
        DamageDealt = damageDealt;
        NumActive = numActive;
        RobotID = robotID;
        ComboTiming = comboTimming;
    }
}


public class HitBehavior : MonoBehaviour
{
    public ActorBehavior ab;
    public HitStats hitStats = null;
}
