using UnityEngine;
using System.Collections;

public class HitStats
{

    public string Key;
    public int RobotID;
    public float SyncScore;
    public int NumActive;

    public HitStats(string key, int robotID, float syncScore, int numActive)
    {
        Key = key;
        SyncScore = syncScore;
        NumActive = numActive;
        RobotID = robotID;
    }
}


public class HitBehavior : MonoBehaviour
{
    public ActorBehavior ab;
    public HitStats hitStats = null;
}
