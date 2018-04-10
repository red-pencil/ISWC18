using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/OVRVision Input Node")]
	[NodeAttribute("TxKit/Transfer/OVRVision Input Node")]

	public class OVRVisionInputNode : NodeBase {

		OvrvisionSource _camSource;

		[SerializeField, Outlet]
		TextureEvent left;

		[SerializeField, Outlet]
		TextureEvent right;

		public OvrvisionSource.CamSettings Settings=new OvrvisionSource.CamSettings();

		// Use this for initialization
		void Start () {
			_camSource = new OvrvisionSource ();
			_camSource.settings = Settings;
			_camSource.Init ();
		}

		void OnDestroy()
		{
			_camSource.Close ();
		}
		
		// Update is called once per frame
		void Update () {
			if (_camSource.Update () ) {
				left.Invoke (_camSource.LeftEye());
				right.Invoke (_camSource.RightEye());
			}

			if (Input.GetKeyDown (KeyCode.PageUp)) {
				_camSource.settings.conf_exposure += 500;
				_camSource.UpdateOvrvisionSetting ();
			}
			if (Input.GetKeyDown (KeyCode.PageDown)) {
				_camSource.settings.conf_exposure -= 500;
				_camSource.UpdateOvrvisionSetting ();
			}

		}
	}
}