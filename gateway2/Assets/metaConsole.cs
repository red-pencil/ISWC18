using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class metaConsole : MonoBehaviour {

	public Text metaInfo;
	public bool _developerMode = true;

	public enum ControllerID
	{
		One,
		Two,
		Three,
		Four,
		Five,
		Six
	}

	public ControllerID conditionSwitch;

	private virtual_exp_3 compensateInfo;
	private Virtual_record targetInfo;
	private virtual_hand handInfoLeft;
	private virtual_hand handInfoRight;

	// Use this for initialization
	void Start () {

		compensateInfo = GameObject.Find ("Virtual Targets").GetComponent<virtual_exp_3> ();
		targetInfo = GameObject.Find ("Reproduce").GetComponent<Virtual_record> ();
		handInfoLeft = GameObject.Find ("LSphere").GetComponent<virtual_hand> ();
		handInfoRight = GameObject.Find ("RSphere").GetComponent<virtual_hand> ();



	}
	
	// Update is called once per frame
	void Update () {

		switch (conditionSwitch) {

		case ControllerID.One:  // neck x1 hand x0

			compensateInfo._compensateRI = false;
			targetInfo.vtDistance = 0.5f;
			targetInfo.vtNumber = 20;
			GameObject.Find ("LSphere").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("RSphere").transform.localScale = new Vector3 (0, 0, 0);
			metaInfo.text = "--- 1 ---\n";

			break;

		case ControllerID.Two:  // neck x2 hand x0

			compensateInfo._compensateRI = true;
			targetInfo.vtDistance = 0.5f;
			targetInfo.vtNumber = 20;
			GameObject.Find ("LSphere").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("RSphere").transform.localScale = new Vector3 (0, 0, 0);
			metaInfo.text = "--- 2 ---\n";

			break;

		case ControllerID.Three:  // neck x1 hand x1

			compensateInfo._compensateRI = false;
			targetInfo.vtDistance = 0.3f;
			targetInfo.vtNumber = 6;
			GameObject.Find ("LSphere").transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
			GameObject.Find ("RSphere").transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
			handInfoLeft._isCompensate = false;
			handInfoRight._isCompensate = false;
			handInfoLeft._isAugmented = false;
			handInfoRight._isAugmented = false;
			metaInfo.text = "--- 3 ---\n";

			break;

		case ControllerID.Four:  // neck x2 hand x1

			compensateInfo._compensateRI = true;
			targetInfo.vtDistance = 0.3f;
			targetInfo.vtNumber = 6;
			GameObject.Find ("LSphere").transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
			GameObject.Find ("RSphere").transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
			handInfoLeft._isCompensate = true;
			handInfoRight._isCompensate = true;
			handInfoLeft._isAugmented = false;
			handInfoRight._isAugmented = false;
			metaInfo.text = "--- 4 ---\n";

			break;

		case ControllerID.Five:  // neck x2 hand x2

			compensateInfo._compensateRI = true;
			targetInfo.vtDistance = 0.3f;
			targetInfo.vtNumber = 6;
			GameObject.Find ("LSphere").transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
			GameObject.Find ("RSphere").transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
			handInfoLeft._isCompensate = true;
			handInfoRight._isCompensate = true;
			handInfoLeft._isAugmented = true;
			handInfoRight._isAugmented = true;
			metaInfo.text = "--- 5 ---\n";

			break;
		}


		//metaInfo.text = "";

		if (Input.GetKeyDown (KeyCode.D)) {

			_developerMode = !_developerMode;

		}

		if (!_developerMode) {
			metaInfo.text += "OFF";
			return;
		}


		int targetNumber = targetInfo.vtNumber; // 20 (3 layers) for head-only task, 7 (1 layer) for neck-hand task
		float targetDistance =  targetInfo.vtDistance; // 0.5 for head-only task, 0.3 for neck-head task
		string testerName = targetInfo.user_name;
		int[] targetOrder = new int[targetNumber];
		targetOrder = targetInfo.vtOrder;

		// order to string
		string order = "";
		foreach (int item in targetOrder) {

			order = order + item.ToString() + ",";

		}

		metaInfo.text += "Tester: " + testerName.ToString () +
			"\n\n-----Target Info-----" +
			"\nnumber: " + targetNumber.ToString () +
			"\ndistance: " + targetDistance.ToString () +
			"\norder: " + order;

		if (GameObject.Find ("Sphere(Clone)")) {
			metaInfo.text += "\nlocation: " + GameObject.Find ("Sphere(Clone)").transform.localPosition.ToString ();
		}

		bool _compensatePos = compensateInfo._compensateP;
		bool _compensateRot = compensateInfo._compensateR;
		bool _compensateRotInverse = compensateInfo._compensateRI;



		metaInfo.text += "\npos compensate: " + toText (_compensatePos) +
			"\nrot inverse compensate: " + toText (_compensateRotInverse); // should be on when vision is agumented

		Vector3 realOrientation = compensateInfo.vOrigin.transform.rotation * Vector3.forward;
		Vector3 augmentedOrientation = compensateInfo.gameObject.transform.rotation * Vector3.forward;

		metaInfo.text += "\n\n-----Vision Info-----" +
			"\noritation(real): " + transLoc (realOrientation).ToString() +
			"\noritation(augmented): " + transLoc (augmentedOrientation).ToString();

		bool _handCompensateLeft = handInfoLeft._isCompensate;
		bool _handCompensateRight = handInfoRight._isCompensate;
		bool _handAugmentLeft = handInfoLeft._isAugmented;
		bool _handAugmentRight = handInfoRight._isAugmented;
		Vector3 handOffsetLeft = new Vector3 (handInfoLeft.offsetX, handInfoLeft.offsetY, handInfoLeft.offsetZ);
		Vector3 handOffserRight = new Vector3 (handInfoRight.offsetX, handInfoRight.offsetY, handInfoRight.offsetZ);
		Vector3 handPosLeft = GameObject.Find("LSphere").transform.position - GameObject.Find("CenterEyeAnchor").transform.position;
		Vector3 handPosRight = GameObject.Find ("RSphere").transform.position - GameObject.Find ("CenterEyeAnchor").transform.position;
		//Vector3 handPosLeft = handInfoLeft.gameObject.transform.position - handInfoLeft.vOrigin.transform.position;
		//Vector3 handPosRight = handInfoRight.gameObject.transform.position - handInfoRight.vOrigin.transform.position;

		Vector3 rawPosRight = handInfoRight.handPos;

		metaInfo.text += "\n\n-----Left Hand Info-----" +
		"\nhand location: " + handInfoLeft.transform.localPosition.ToString () +
			(transLoc (handPosLeft) + transLoc (handPosRight)).ToString() +
			"\ncompensate: " + toText(_handCompensateLeft) +
			"\naugment: " + toText(_handAugmentLeft);

		metaInfo.text += "\n\n-----Right Hand Info-----" +
		"\nhand location: " + handInfoRight.transform.localPosition.ToString () +
			(transLoc(rawPosRight)).ToString () + (transLoc(handPosRight) + transLoc(realOrientation)).ToString() +
			"\ncompensate: " + toText(_handCompensateRight) +
			"\naugment: " + toText(_handAugmentRight);

	}

	public string toText(bool _status)
	{
		if (_status)
			return "ON";
		else
			return "OFF";

	}

	public string transLoc0 (Vector3 loc)
	{
		Quaternion q;
		q = Quaternion.FromToRotation (loc.normalized, Vector3.forward);
		return q.eulerAngles.ToString ();
	}

	public Vector2 transLoc (Vector3 loc)
	{
		float lon, lat;
		Vector3.Normalize(loc);
		lon = Mathf.Atan2 (loc.x , loc.z) * Mathf.Rad2Deg;
		lat = Mathf.Asin (loc.y) * Mathf.Rad2Deg;

		return new Vector2 (lat, lon);
		//return "(" + lat.ToString("N1") + "," + lon.ToString("N1") + ")";

	}

}
