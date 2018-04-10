using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Eyes Output Node")]
	[NodeAttribute("TxKit/Transfer/Eyes Output Node")]
	public class TxEyesOutputNode : NodeBase {

		TxEyesOutput _eyes;

		[Inlet]
		public TxEyesOutput Eyes {
			set {
				if (!enabled) return;
				_eyes = value;
			}
		}


		[Serializable]
		public class CameraConfigurationsEvent : UnityEvent<CameraConfigurations> {}


		[SerializeField, Outlet]
		TextureEvent _leftEye;

		[SerializeField, Outlet]
		TextureEvent _rightEye;

		[SerializeField, Outlet]
		CameraConfigurationsEvent _config;

		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_Eyes" ) {
				Eyes = null;
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
            if (!Active)
                return;
			if (_eyes != null) {
				_leftEye.Invoke (_eyes.LeftEye);
				_rightEye.Invoke (_eyes.RightEye);
				_config.Invoke (_eyes.Configuration);
			} else {
				_leftEye.Invoke (null);
				_rightEye.Invoke (null);
				_config.Invoke (null);
			}
		}
	}
}