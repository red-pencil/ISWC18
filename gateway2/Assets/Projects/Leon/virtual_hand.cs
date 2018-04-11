using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtual_hand : MonoBehaviour {

	public enum ControllerID
	{
		None,
		Left,
		Right
	}
		
	public GameObject vOrigin;

	new Vector3 handPos;
	new Vector3 handPosToHead;
	new Vector3 handPosNew;
	new Vector3 handPosToHeadNew;

	new public float offsetX;
	new public float offsetY;
	new public float offsetZ;

	public ControllerID Controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		var c = OVRInput.Controller.LTouch;
		if (Controller == ControllerID.Left)
			c = OVRInput.Controller.LTouch;
		else if (Controller == ControllerID.Right)
			c = OVRInput.Controller.RTouch;
		else return;

		handPos = OVRInput.GetLocalControllerPosition (c);
		handPosToHead = handPos - vOrigin.transform.position;
		//handPosToHead = vOrigin.transform.rotation * handPosToHead;//twice
		handPosToHeadNew = Quaternion.Inverse(vOrigin.transform.rotation) * handPosToHead;//twice
		handPosNew = handPosToHeadNew + vOrigin.transform.position;


		handPosNew.x += offsetX;
		handPosNew.y += offsetY;
		handPosNew.z += offsetZ;


		transform.localPosition = handPosNew;

		transform.localRotation = OVRInput.GetLocalControllerRotation(c);
		
	}
}
