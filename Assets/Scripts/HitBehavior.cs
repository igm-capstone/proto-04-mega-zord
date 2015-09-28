using UnityEngine;
using System.Collections;

public class HitStats
{

    public string Key;
    public int RobotID;
    public float DamageDealt;
    public int NumActive;
    public float ComboTiming;

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
