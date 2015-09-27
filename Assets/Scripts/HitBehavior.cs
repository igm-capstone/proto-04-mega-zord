using UnityEngine;
using System.Collections;

public class HitStats
{
    public string Key;
    public float SyncScore;
    public int NumActive;

    public HitStats(string key, float syncScore, int numActive)
    {
        Key = key;
        SyncScore = syncScore;
        NumActive = numActive;
    }
}


public class HitBehavior : MonoBehaviour
{
    public HitStats hitStats = null;
}
