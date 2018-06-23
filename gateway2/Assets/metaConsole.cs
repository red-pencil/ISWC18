using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class metaConsole : MonoBehaviour {

	public Text metaInfo;

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

		bool _compensatePos = compensateInfo._compensateP;
		bool _compensateRot = compensateInfo._compensateR;
		bool _compensateRotInverse = compensateInfo._compensateRI;

		int targetNumber = targetInfo.vtNumber; // 20 (3 layers) for head-only task, 7 (1 layer) for neck-hand task
		float targetDistance =  targetInfo.vtDistance; // 0.5 for head-only task, 0.3 for neck-head task
		string testerName = targetInfo.user_name;
		int[] targetOrder = new int[targetNumber];
		targetOrder = targetInfo.vtOrder;

		bool _handCompensateLeft = handInfoLeft._isCompensate;
		bool _handCompensateRight = handInfoRight._isCompensate;
		bool _handAugmentLeft = handInfoLeft._isAugmented;
		bool _handAugmentRight = handInfoRight._isAugmented;
		Vector3 handOffsetLeft = new Vector3 (handInfoLeft.offsetX, handInfoLeft.offsetY, handInfoLeft.offsetZ);
		Vector3 handOffserRight = new Vector3 (handInfoRight.offsetX, handInfoRight.offsetY, handInfoRight.offsetZ);

		string order;

		metaInfo.text = "Tester:" + testerName.ToString() +
		"\n\nTarget Info:\n" +
			"\nnumber:" + targetNumber.ToString() +
			"\ndistance:" + targetDistance.ToString() +
			"\norder:" + targetOrder.ToString();


		
	}

	public string toText(bool _status)
	{
		if (_status)
			return "ON";
		else
			return "OFF";

	}

}
