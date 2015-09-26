using UnityEngine;
using System.Collections;

// This Script is jut o stop the movement of the body bug.
public class AnimationBugFix : MonoBehaviour
{
    Transform RobotMesh;

	// Use this for initialization
	void Start ()
    {
        RobotMesh = transform.FindChild("RobotMesh");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 offsetVec = new Vector3(0.5f, 0.5f, 0.5f);
        if(RobotMesh.localPosition.x > offsetVec.x ||
            RobotMesh.localPosition.y > offsetVec.y ||
            RobotMesh.localPosition.z > offsetVec.z)
        {
            RobotMesh.localPosition = Vector3.zero;
        }
	}
}
