using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Virtual_record : MonoBehaviour {

	new public float[] lon = new float[10];
	new public float[] lat = new float[10];
	new float[] lonpi;
	new float[] latpi;

	new public GameObject vTarget;
	new public GameObject vOrigin;


	new GameObject clone;
	float targetx = 0.0f;
	float targety = 0.0f;
	float targetz = 0.0f;
	public new float vtDistance = 1;

	public bool _mirror =false;
	new public int[] vtOrder;
	new int targetID;
	new int round;

	public Material myColor;
	public MeshRenderer targetObject;

	public Text countDown;
	public Text roundCount;
	public int targetNumber = 10;
	public bool _left = false;
	public bool _countDown = false;

	new bool _target;

	Data.DBWriter writer=new Data.DBWriter();
	bool _start = false;
	bool _pause = false;
	new float time;
	public string name = "your name";

	public float rtdistance=0;

	// Use this for initialization
	void Start () {

		//lon = new float[] { 90, 120, 150, 180, 90, 90, 90, 180, 180, 135};
		//lat = new float[] {  0,   0,   0,   0, 30, 60, 90,  30, 60 , 45};

		//lon = new float[] { 90, 120, 150, 180, 90, 120, 150, 180, 180,  90};
		//lat = new float[] {  0,   0,   0,   0,-30, -30, -30, -30, -60, -60};

		//lon = new float[] { 90, 120, 150, 180,  90, 120, 150, 180, 30, 60,   0,  30,  60,  0, 30, 60, 90, 120, 150, 180 };
		//lat = new float[] {  0,   0,   0,   0, -30, -30, -30, -30,  0,  0, -30, -30, -30, 30, 30, 30, 30,  30,  30,  30 };

		//lonpi = new float[(int) lon.LongLength];
		//latpi = new float[(int) lat.LongLength];

		lonpi = new float[targetNumber];
		latpi = new float[targetNumber];

		for (int i = 0; i < targetNumber; i++) {
			lonpi [i] = lon [i] / 360 * 2 * Mathf.PI;
		}
		for (int i = 0; i < targetNumber; i++) {
			latpi [i] = lat [i] / 360 * 2 * Mathf.PI;
		}



		// record

		writer.AddKey ("KeyTime");
		writer.AddKey ("Time-Time");
		writer.AddKey ("KeyPress");
		writer.AddKey ("Target ID");
		writer.AddKey ("Target_lon");
		writer.AddKey ("Target_lat");
		writer.AddKey ("Target_x");
		writer.AddKey ("Target_y");
		writer.AddKey ("Target_z");
		writer.AddKey ("HMD_rx");
		writer.AddKey ("HMD_ry");
		writer.AddKey ("HMD_rz");
		
	} //start()
	
	// Update is called once per frame
	void Update () {

		if (clone != null) {
			rtdistance = Vector3.Distance (clone.transform.position, vOrigin.transform.position);
		}

		if (GameObject.Find ("Sphere(Clone)") != null)
			_target = true;
		else
			_target = false;


		if (Input.GetKeyDown (KeyCode.Alpha5))
			RandomTarget ();


		/// generate all targets once
		/// 
		if (Input.GetKeyDown (KeyCode.KeypadEnter)) {

//			while (_target)
//			{

//				Destroy (GameObject.Find ("Sphere(Clone)"));
//			if (GameObject.Find ("Sphere(Clone)") != null)
//				_target = true;
//			else
//				_target = false;

//			}

			if (_target) {

				Destroy (GameObject.Find ("Sphere(Clone)"));
				
			} else {

				//for (int i = 0; i < Mathf.Min (lon.Length, lat.Length); i++) {
				for (int i = 0; i < targetNumber; i++) {

					targetx = Mathf.Sin (lonpi [i]) * Mathf.Cos (latpi [i]) * vtDistance;
					targety = Mathf.Sin (latpi [i]) * vtDistance;
					targetz = Mathf.Cos (lonpi [i]) * Mathf.Cos (latpi [i]) * vtDistance;

					clone = Instantiate (vTarget, new Vector3 (targetx, targety, targetz), Quaternion.identity);

					if (targetx > 0) {

						clone = Instantiate (vTarget, new Vector3 (-targetx, targety, targetz), Quaternion.identity);

					}

				}
					
			}

		}


		/// exp with 5 seconds waiting time



		/// exp without cooling down time

		if (Input.GetKeyDown (KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.One)) {

			if (_countDown) {
				
				StartCoroutine (Exp ());

			} else {
				

			if (_target) {
				Destroy (GameObject.Find ("Sphere(Clone)"));
				Debug.Log ("Destroy");
				countDown.text = "Back";

			} else {
				
					GenerateTarget ();
				}

			}

			RecordKey ();
		}


		/// record data
		/// 
		if (Input.GetKeyDown (KeyCode.Tab)) {
			_start = !_start;

			if (_start)
			{
				Debug.Log("Started Recording");
				writer.ClearData();
			}
			else
			{
				Debug.Log("Stop Recording");
				writer.WriteValues(Application.dataPath + "\\vtdata\\" + name +".txt");
			}

		}

		if (Input.GetKeyDown (KeyCode.Q)) {

			_pause = !_pause;
			Debug.Log("Pause Recording");
		}

		if (_start && !_pause)
			Record ();


		}

	/// Generates the target. 5 seconds waiting time
	/// 
	/// 
	IEnumerator Exp()
	{
		RecordKey (); //end KEY

		Destroy (GameObject.Find ("Sphere(Clone)"));
		Debug.Log ("Destroy");

		//if(GameObject.Find ("Sphere(Clone)") != null) 
		//{
		//	Destroy (GameObject.Find ("Sphere(Clone)"));
		//	Debug.Log ("One More");
		//}

		myColor = targetObject.material;
		myColor.color =	Color.green;
		Debug.Log (Time.time);

		countDown.text = "5";
		yield return new WaitForSeconds (1);
		countDown.text = "4";
		yield return new WaitForSeconds (1);
		countDown.text = "3";
		yield return new WaitForSeconds (1);
		countDown.text = "2";
		yield return new WaitForSeconds (1);
		countDown.text = "1";
		yield return new WaitForSeconds (1);
		countDown.text = "GO";

		if (targetID > 9) {

			RandomTarget ();
			//targetID -= 10;
			targetID = 0;
			round++;

		}

		roundCount.text = "Round " + round + "-" + targetID;

		int i = 1;
		i = vtOrder [targetID] - 1;

		//targetid = 
		Debug.Log ("ID = " + targetID + ", i = " + i);

		targetx = Mathf.Sin (lonpi [i]) * Mathf.Cos (latpi [i]) * vtDistance;
		targety = Mathf.Sin (latpi [i]) * vtDistance;
		targetz = Mathf.Cos (lonpi [i]) * Mathf.Cos (latpi [i]) * vtDistance;

		if (_mirror && targetx > 0 && Random.Range(-0.1f,1.0f) < 0.5)
		{
		//targetx = -targetx;
		}

		clone = Instantiate (vTarget, new Vector3 (targetx, targety, targetz), Quaternion.identity, transform);

		//clone.transform.parent = transform;

		targetID++;

		Debug.Log (Time.time);

		RecordKey (); // begin KEY
	}


	/// Generates the target. without cooling-down time

		void GenerateTarget ()
	{
		targetID++;

			myColor = targetObject.material;
			myColor.color =	Color.green;
			Debug.Log (Time.time);
	
		if (_left)
			countDown.text = "<<<Find";
		else
			countDown.text = "Find>>>";

		if (targetID > (targetNumber-1)) {

				RandomTarget ();
				//targetID -= 10;
				targetID = 0;
				round++;

			}

			roundCount.text = "Round " + round + "-" + targetID;

			int i = 1;
			i = vtOrder [targetID] - 1;

			//targetid = 
			Debug.Log ("ID = " + targetID + ", i = " + i);

			targetx = Mathf.Sin (lonpi [i]) * Mathf.Cos (latpi [i]) * vtDistance;
			targety = Mathf.Sin (latpi [i]) * vtDistance;
			targetz = Mathf.Cos (lonpi [i]) * Mathf.Cos (latpi [i]) * vtDistance;

			if (_mirror && targetx > 0 && Random.Range(-0.1f,1.1f) < 0.5)
			{
				targetx = -targetx;
			}

			if (_left)
			targetx = -targetx;

			clone = Instantiate (vTarget, new Vector3 (targetx, targety, targetz), Quaternion.identity, transform);
			clone.transform.localPosition = new Vector3 (targetx, targety, targetz);
			//clone.transform.parent = transform;

			

			Debug.Log (Time.time);


		}

	void RecordKey (){

		int targetIDreal = vtOrder [targetID] - 1;

		writer.AddData ("KeyTime", time.ToString ());
		writer.AddData ("KeyPress", "1");
		writer.AddData ("Time-Time", Time.time.ToString ());
		writer.AddData ("Target ID", targetIDreal.ToString ());
		writer.AddData ("HMD_rx", vOrigin.transform.position.x.ToString ());
		writer.AddData ("HMD_ry", vOrigin.transform.position.y.ToString ());
		writer.AddData ("HMD_rz", vOrigin.transform.position.z.ToString ());
		writer.AddData ("Target_x", targetx.ToString ());
		writer.AddData ("Target_y", targety.ToString ());
		writer.AddData ("Target_z", targetz.ToString ());
		writer.AddData ("Target_lon", lon [targetIDreal].ToString ());
		writer.AddData ("Target_lat", lat [targetIDreal].ToString ());

		writer.PushData ();

		//System.IO.File.AppendAllText(Application.dataPath +  "\\vtdata\\" + name + "_keydata.txt", Time.time + "\t"+ (targetID - 1).ToString () + "\t" +);

	}

	void Record(){
		
		int targetIDreal = vtOrder [targetID] - 1;

		time += Time.deltaTime;
		writer.AddData ("KeyTime", time.ToString ());
		writer.AddData ("KeyPress", "0");
		writer.AddData ("Time-Time", Time.time.ToString ());
		writer.AddData ("Target ID", targetIDreal.ToString ());
		writer.AddData ("HMD_rx", vOrigin.transform.position.x.ToString ());
		writer.AddData ("HMD_ry", vOrigin.transform.position.y.ToString ());
		writer.AddData ("HMD_rz", vOrigin.transform.position.z.ToString ());
//		writer.AddData ("Target_x", targetx.ToString ());
//		writer.AddData ("Target_y", targety.ToString ());
//		writer.AddData ("Target_z", targetz.ToString ());
//		writer.AddData ("Target_lon", lon[vtOrder[targetID-1] -1].ToString ());
//		writer.AddData ("Target_lat", lat[vtOrder[targetID-1] -1].ToString ());

		writer.PushData ();

	}


	void RandomTarget(){

		int vtPosInOrder = -1; // the position of certain targetID in the new order
		int targetIDtemp = 0;

		vtOrder = new int[targetNumber];
		Debug.Log("Freashing");

		//for (int i = 1; i < 11; i = i + 0)
		for (int i = 1; i < (targetNumber + 1); i = i + 0) { // i = 1~10
			targetIDtemp = Random.Range (1, (targetNumber + 1)); // exclusive = 1~10 don't use 0, because initial value is 0
			vtPosInOrder = System.Array.IndexOf (vtOrder, targetIDtemp); // vtPosInOrder = 0~9, -1
			//				Debug.Log("ID" + targetid + "_pos" + vtpos);
			if (vtPosInOrder < 0) {
				vtOrder [i-1] = targetIDtemp; // vtOrder[0~9] = 1~10
				i++;
			}

		}

		targetIDtemp = 0;

	}

}
