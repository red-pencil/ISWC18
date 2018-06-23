using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtual_exp : MonoBehaviour {

	// Use this for initialization
	new public GameObject vtarget;
	new public GameObject vorigin;
	new public int vcount = 6;
	new public int lon = 3;
	new public int lat = 3;
	new public bool exp = false;
	new public int targetid = 0;
	new float targetx = 0;
	new float targety = 0;
	new float targetz = 0;
	new float targetangle =0;
	new float targetdistance = 1;
	new GameObject clone;
	new float phi = 2 * Mathf.PI * ((Mathf.Sqrt(5)-1)/2);
	new float div = 0;
	public enum opt {x_axis, y_axis, z_axis, sphere_even, sphere_project, cube_space, random};
	public opt array = opt.y_axis;
	
	void Start () {
		transform.position = new Vector3 (0f, 0f, 0f);
		//transform.rotation = vorigin.transform.rotation;

		if (exp)
		{
			targetid = Random.Range(0, vcount-1);
		}


		if (array == opt.random)
		{
			for (int i = 0; i < vcount; i++)
			{
				if (exp) {
					if (i != targetid)
					{
						return;
					}
					clone = Instantiate(vtarget, Random.onUnitSphere, Quaternion.identity);
					clone.transform.parent = transform;
				}

				clone = Instantiate(vtarget, Random.onUnitSphere, Quaternion.identity);
				clone.transform.parent = transform;
			}
		}

		// rotate about Y axis
		if  (array == opt.y_axis)
		{
		for (int i = 0; i < vcount; i++) {

			targetangle = i * (2 * Mathf.PI / (vcount));
			targetx = Mathf.Cos (targetangle) * targetdistance;
			targety = 0;
			targetz = Mathf.Sin (targetangle) * targetdistance;

			//targety = i / vcount;
			//targetx = 1 * Mathf.Cos(( i + 1) * phi);
			//targetz = 1 * Mathf.Sin(( i + 1) * phi);

			clone = Instantiate(vtarget, new Vector3(targetx, targety, targetz), Quaternion.identity);
			clone.transform.parent = transform;

			}
		}

		// on sphere surface evenly
		if (array == opt.sphere_even) {
			for (int i = 0; i < vcount; i++) {
				div = (2 * (i + 1) - 1);
				targety = ( div / vcount - 1)* targetdistance;
				targetx = Mathf.Sqrt(1 - Mathf.Pow (targety, 2)) * Mathf.Cos(( i + 1) * phi)* targetdistance;
				targetz = Mathf.Sqrt(1 - Mathf.Pow (targety, 2)) * Mathf.Sin(( i + 1) * phi)* targetdistance;

				clone = Instantiate(vtarget, new Vector3(targetx, targety, targetz), Quaternion.identity);
				clone.transform.parent = transform;
			}
		}

		// on sphere surface longitude and latitutde
		if (array == opt.sphere_project) {
			for (int i = 0; i < lat; i++) {
				div = lat - 1;
				div = (i) / div;
				targety = Mathf.Cos (div * Mathf.PI) * targetdistance;
				//targety = (2 * div - 1) * targetdistance;
				for (int j = 0; j < lon; j++) {
					div = lon;
					div = j / div;
					targetx = Mathf.Sqrt(1 - Mathf.Pow (targety, 2)) * Mathf.Sin (2 * Mathf.PI * div) * targetdistance;
					targetz = Mathf.Sqrt(1 - Mathf.Pow (targety, 2)) * Mathf.Cos (2 * Mathf.PI * div) * targetdistance;

					clone = Instantiate(vtarget, new Vector3(targetx, targety, targetz), Quaternion.identity);
					clone.transform.parent = transform;
				}

			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.Inverse (vorigin.transform.rotation);

		transform.position = vorigin.transform.position;
		
	}
}
