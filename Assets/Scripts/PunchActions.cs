using UnityEngine;
using System.Collections;

public class PunchActions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hit()
    {
        transform.parent.GetComponent<ActorBehavior>().PunchHit();
    }

}
