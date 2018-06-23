using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEntry (Collider other) {

		//myColor = targetObject.material;
		//myColor.color =	Color.green;
		//myColor.albedo =
		transform.localScale = new Vector3 (0, 0, 0);
		Debug.Log ("!!!");

	}

}
