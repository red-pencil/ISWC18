using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtual_hand : MonoBehaviour {

	public Material myColor;
	public MeshRenderer targetObject;

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

	public bool _noEnhance = false;
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

		if (_noEnhance)
			handPosNew = handPos;


		handPosNew.x += offsetX;
		handPosNew.y += offsetY;
		handPosNew.z += offsetZ;


		transform.localPosition = handPosNew;

		transform.localRotation = OVRInput.GetLocalControllerRotation(c);



		///// change the color
		myColor = targetObject.material;

		if (GameObject.Find ("Sphere(Clone)") == null)
			myColor.color =	Color.white;
		
	}

	void OnTriggerEnter (Collider other) {

		//
		myColor.color =	Color.green;
		//myColor.albedo =
		//gameObject.transform.localScale = new Vector3 (0, 0, 0);
		Debug.Log ("enter");
	
	}

	void OnTriggerExit (Collider other) {

		//myColor = targetObject.material;
		myColor.color =	Color.white;
		//myColor.albedo =
		//gameObject.transform.localScale = new Vector3 (0.1f, 0.2f, 0.3f);
		Debug.Log ("Leave");

	}



}
