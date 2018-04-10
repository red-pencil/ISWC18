using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Eyes Input Node")]
	[NodeAttribute("TxKit/Transfer/Eyes Input Node")]
	public class TxEyesInputNode : NodeBase {

        [SerializeField]
		TxEyesOutput _eyes;
        CameraConfigurations _tmpConfig;

		[Inlet]
		public Texture LeftEye{
			set {
				if (!enabled) return;
				if(_eyes!=null)
					_eyes.LeftEye = value;
			}
		}

		[Inlet]
		public Texture RightEye{
			set {
				if (!enabled) return;

				if(_eyes!=null)
					_eyes.RightEye = value;
			}
		}
		[Inlet]
		public CameraConfigurations Config{
			set {
				if (!enabled) return;

				if(_eyes!=null)
					_eyes.Configuration = value;
			}
		}

		[Serializable]
		public class EyesEvent : UnityEvent<TxEyesOutput> {}

		[SerializeField, Outlet]
		EyesEvent Eyes;

		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_LeftEye" ) {
				LeftEye = null;
			}
			if (targetSlotName == "set_RightEye" ) {
				RightEye = null;
            }
            if (targetSlotName == "set_Config")
            {
                Config = _tmpConfig;
            }
		}
		// Use this for initialization
		void Start () {
            //_eyes = new TxEyesOutput ();
            _tmpConfig = _eyes.Configuration;// new CameraConfigurations ();
		}
		
		// Update is called once per frame
		void Update () {
			Eyes.Invoke (_eyes);
		}
	}
}