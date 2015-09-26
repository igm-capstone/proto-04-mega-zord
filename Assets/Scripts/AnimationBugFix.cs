using UnityEngine;
using System.Collections;

// This Script is jut o stop the movement of the body bug.
public class AnimationBugFix : MonoBehaviour
{
    Transform RobotMesh;
    public Vector3 offsetVec = new Vector3(0.5f, 0.5f, 0.5f);

	// Use this for initialization
	void Start ()
    {
        RobotMesh = transform.FindChild("RobotMesh");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Mathf.Abs(RobotMesh.localPosition.x) > offsetVec.x ||
            Mathf.Abs(RobotMesh.localPosition.y) > offsetVec.y ||
            Mathf.Abs(RobotMesh.localPosition.z) > offsetVec.z)
        {
            RobotMesh.localPosition = Vector3.zero;
        }
	}
}
