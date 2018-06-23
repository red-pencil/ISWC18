using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtual_exp_3 : MonoBehaviour {

	new public GameObject vOrigin;

	new public bool _compensateP;
	new public bool _compensateR;
	new public bool _compensateRI;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (_compensateP)
			transform.position = vOrigin.transform.position;

			
		if (_compensateR)
			transform.rotation = vOrigin.transform.rotation;

		if (Input.GetKeyDown (KeyCode.S))
			_compensateRI = !_compensateRI;


		if (_compensateRI)
			transform.rotation = Quaternion.Inverse (vOrigin.transform.rotation);

	}
}
