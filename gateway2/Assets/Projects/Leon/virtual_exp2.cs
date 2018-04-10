using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class virtual_exp2 : MonoBehaviour {


	new public GameObject vtarget;
	new public GameObject vorigin;
	public bool show_all = false;
	new public float[] lon = new float[10];
	new public float[] lat = new float[10];
	new float[] lonpi;
	new float[] latpi;

	new int targetid;

	float targetx = 0.0f;
	float targety = 0.0f;
	float targetz = 0.0f;
	public new float vtdistance = 1;
	new GameObject clone;
	public bool _compensate = true;
	public bool _mirror = true;

	new public int[] vtorder;

	public Material mycolor;

	public MeshRenderer TargetObject;

	public Text countDown;


	Data.DBWriter writer=new Data.DBWriter();
	bool _started = false;
	bool _pause = false;
	new float time;
	public string name = "your name";

	// Use this for initialization
	void Start () {

		writer.AddKey ("KeyTime");
		writer.AddKey ("Target ID");
		writer.AddKey ("Target_lon");
		writer.AddKey ("Target_lat");
		writer.AddKey ("Target_y");
		writer.AddKey ("Target_z");
		writer.AddKey ("Target_y");
		writer.AddKey ("Target_z");
		writer.AddKey ("Target_x");
		writer.AddKey ("Target_y");
		writer.AddKey ("Target_z");
		writer.AddKey ("HMD_rx");
		writer.AddKey ("HMD_ry");
		writer.AddKey ("HMD_rz");
		//writer.AddKey ("Hand_x");
		//writer.AddKey ("Hand_y");
		//writer.AddKey ("Hand_z");

		//lon = new float[] { 90, 120, 150, 180, 90, 90, 90, 180, 180, 135};
		//lat = new float[] {  0,   0,   0,   0, 30, 60, 90,  30, 60 , 45};
		lon = new float[] { 90, 120, 150, 180, 90, 120, 150, 180, 180, 135};
		lat = new float[] {  0,   0,   0,   0,-30, -30, -30, -30, -60 ,-45};
		lonpi = new float[(int) lon.LongLength];
		latpi = new float[(int) lat.LongLength];

		for (int i = 0; i < lon.Length; i++) {
			lonpi [i] = lon [i] / 360 * 2 * Mathf.PI;
		}
		for (int i = 0; i < lat.Length; i++) {
			latpi [i] = lat [i] / 360 * 2 * Mathf.PI;
		}

		if (show_all) {

			for (int i = 0; i < Mathf.Min(lon.Length, lat.Length); i++) {

				targetx = Mathf.Sin (lonpi [i]) * Mathf.Cos (latpi [i]) * vtdistance;
				targety = Mathf.Sin (latpi [i]) * vtdistance;
				targetz = Mathf.Cos (lonpi [i]) * Mathf.Cos (latpi [i]) * vtdistance;



				clone = Instantiate(vtarget, new Vector3(targetx, targety, targetz), Quaternion.identity);
				clone.transform.parent = transform;

		} 

	}
	}
	
	// Update is called once per frame


	void Update ()
	{

		if (_compensate) {
			
			transform.rotation = Quaternion.Inverse (vorigin.transform.rotation);

		}
		transform.position = vorigin.transform.position;

		// randomize
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			int vtpos = -1;

			vtorder = new int[10];
			Debug.Log("Freashing");

			for (int i = 1; i < 10; i = i + 0) { // i = 1, 2, 3, 4, 5
				targetid = Random.Range (1, 10); // exclusive
				vtpos = System.Array.IndexOf (vtorder, targetid); // vtpos = 0, 1, 2, 3, 4, -1
//				Debug.Log("ID" + targetid + "_pos" + vtpos);
				if (vtpos < 0) {
					vtorder [i] = targetid;
					i++;
				}

			}
			targetid = 0;
				
		}


		if (Input.GetKeyDown(KeyCode.A))
//		for (int i =1; Input.GetKeyDown (KeyCode.A); i++)
		{

			if (targetid > 9) {
				targetid -= 10;
			}

			int i = 1;
			i = vtorder [targetid];
			//targetid = 
			Debug.Log ("ID = " + targetid);
			Debug.Log ("I = " + i);

				targetx = Mathf.Sin (lonpi [i]) * Mathf.Cos (latpi [i]) * vtdistance;
				targety = Mathf.Sin (latpi [i]) * vtdistance;
				targetz = Mathf.Cos (lonpi [i]) * Mathf.Cos (latpi [i]) * vtdistance;

			if (_mirror && Random.Range(0,1) < 0.5) {
				targetx = -targetx;
			}

				

			clone = Instantiate (vtarget, new Vector3 (targetx, targety, targetz), Quaternion.identity);
			clone.transform.parent = transform;

			targetid++;
		}


		if (Input.GetKeyDown(KeyCode.S)) {

			//StartCoroutine(Wait());

			Destroy (GameObject.Find("Sphere(Clone)"));
			Debug.Log("Destroy");
		}

		if (Input.GetKeyDown (KeyCode.Q)) {


			_started = !_started;
			if(_started)
			{
				Debug.Log("Started Recording");
				writer.ClearData();
			}
			else
			{
				Debug.Log("Stop Recording");
				writer.WriteValues(Application.dataPath + "/vtdata_" + name +".txt");
			}

		}

		if (Input.GetKeyDown (KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.One)) {
			
			StartCoroutine (Exp ());
		}



		if (Input.GetKeyDown (KeyCode.Tab)) {

			_pause = !_pause;
			Debug.Log("Pause Recording");
		}



		if (_started && !_pause)
			Record ();




	}


	void Record(){

//		if (!_started)
//			return;

		time += Time.deltaTime;
		writer.AddData ("KeyTime", time.ToString ());
		writer.AddData ("Target ID", (targetid - 1).ToString ());
		writer.AddData ("HMD_rx", vorigin.transform.position.x.ToString ());
		writer.AddData ("HMD_ry", vorigin.transform.position.y.ToString ());
		writer.AddData ("HMD_rz", vorigin.transform.position.z.ToString ());
		writer.AddData ("Target_x", targetx.ToString ());
		writer.AddData ("Target_y", targety.ToString ());
		writer.AddData ("Target_z", targetz.ToString ());
		writer.AddData ("Target_lon", lon[vtorder[targetid-1]].ToString ());
		writer.AddData ("Target_lat", lat[vtorder[targetid-1]].ToString ());


		//writer.AddData ("HeadY", headpos.y.NormalizeAngle().ToString ());
		//writer.AddData ("CameraX", camerapos.x.NormalizeAngle().ToString ());
		//writer.AddData ("HeadX", headpos.x.NormalizeAngle ().ToString ());
		//writer.AddData ("CameraZ", camerapos.z.NormalizeAngle().ToString ());
		//writer.AddData ("HeadZ", headpos.z.NormalizeAngle ().ToString ());
		writer.PushData ();

		//System.IO.File.AppendAllText("C:\\Leon\\ExperimentHead\\Assets\\data.txt", "###" + headpos + "###");
	}

	IEnumerator Wait()
	{
		Debug.Log (Time.time);
		yield return new WaitForSeconds (3);
		Debug.Log (Time.time);
	}

	IEnumerator Exp()
	{
		Destroy (GameObject.Find ("Sphere(Clone)"));
		Debug.Log ("Destroy");

		//if(GameObject.Find ("Sphere(Clone)") != null) 
		//{
		//	Destroy (GameObject.Find ("Sphere(Clone)"));
		//	Debug.Log ("One More");
		//}

		mycolor = TargetObject.material;
		mycolor.color =	Color.green;
		Debug.Log (Time.time);

		countDown.text = "3";
		yield return new WaitForSeconds (1);
		countDown.text = "2";
		yield return new WaitForSeconds (1);
		countDown.text = "1";
		yield return new WaitForSeconds (1);
		countDown.text = "GO";

		if (targetid > 9) {
			targetid -= 10;
		}

		int i = 1;
		i = vtorder [targetid];
		//targetid = 
		Debug.Log ("ID = " + targetid + ", i = " + i);

		targetx = Mathf.Sin (lonpi [i]) * Mathf.Cos (latpi [i]) * vtdistance;
		targety = Mathf.Sin (latpi [i]) * vtdistance;
		targetz = Mathf.Cos (lonpi [i]) * Mathf.Cos (latpi [i]) * vtdistance;

		if (_mirror && targetx > 0 && Random.Range(-0.1f,1.0f) < 0.5)
		{
			targetx = -targetx;
		}

		targetx += vorigin.transform.position.x;
		targety += vorigin.transform.position.y;
		targetz += vorigin.transform.position.z;


		clone = Instantiate (vtarget, new Vector3 (targetx, targety, targetz), Quaternion.identity, transform);

		//clone.transform.parent = transform;

		targetid++;
		Debug.Log (Time.time);
	}

	void OnGUI () {
		// Make a background box
		//GUI.Box(new Rect((float)Screen.width * 0.5f,Screen.height-200,100,100), "Loader Menu");


	}

}

	