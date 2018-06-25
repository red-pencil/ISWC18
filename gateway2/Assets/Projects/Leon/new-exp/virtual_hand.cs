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

	public Vector3 handPos;
	Vector3 handPosToHead;
	public Vector3 handPosNew;
	Vector3 handPosToHeadNew;
	Vector3 handPosToHeadZero;

	new Vector3 handRotAxis;
	new Quaternion handRot;
	new Vector3 debugRay;

	new public float offsetX;
	new public float offsetY;
	new public float offsetZ;
	new float handLength;

	public bool _isCompensate = false;
	public bool _isAugmented = false;
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

		if (_isAugmented) {

			handPosToHeadZero = Vector3.forward;
			handRot.SetFromToRotation (Vector3.forward, handPosToHead.normalized);
			handPosToHead = handRot * handPosToHead;  // twice

			//handLength = handPosToHead.magnitude;
			//handPosToHeadZero = new Vector3(0.0f, 0.0f, handLength);
			//handRotAxis = Vector3.Cross(handPosToHeadZero, handPosToHead);
			//handRot.w = Mathf.Sqrt (handPosToHeadZero.sqrMagnitude * handPosToHead.sqrMagnitude) + Vector3.Dot(handPosToHeadZero, handPosToHead);
			//handRot.x = handRotAxis.x;
			//handRot.y = handRotAxis.y;
			//handRot.z = handRotAxis.z;
			Vector3 debugR = handRot * handRot * Vector3.forward;
			Debug.DrawRay (transform.position, debugR, Color.red, 2f, false);
			//ssDebug.Log ("hand rotation" + handRot);
			//handPosToHead = handRot * handPosToHead;
		
		}

		//handPosToHead = vOrigin.transform.rotation * handPosToHead;//twice
		if (_isCompensate) {
			handPosToHeadNew = Quaternion.Inverse (vOrigin.transform.rotation) * handPosToHead;//twice
		} else {
			handPosToHeadNew = handPosToHead;
		}

		handPosToHeadNew.x += offsetX;
		handPosToHeadNew.y += offsetY;
		handPosToHeadNew.z += offsetZ;
		
		handPosNew = handPosToHeadNew + vOrigin.transform.position;

		transform.localPosition = handPosNew;

		//transform.localRotation = OVRInput.GetLocalControllerRotation(c);


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
