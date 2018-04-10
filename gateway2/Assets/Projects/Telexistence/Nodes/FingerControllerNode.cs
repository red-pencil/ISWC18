using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Perceptual/Finger Controller")]
	[NodeAttribute("TxKit/Perceptual/Finger Controller")]
	public class FingerControllerNode : NodeBase {

		static string[] _FingersNames = new string[]{
			"Thumb",
			"Index",
			"Middle",
			"Ring",
			"Pinky",
			"Other"
		};

		[SerializeField]
		Leap.Finger _finger;

		[Inlet]
		public Leap.Finger Finger {
			set {
				if (!enabled) return;
				_finger = value;

				if (_finger != null) {
					Tip.Invoke (_finger.TipPosition.ToVector3 ());
					Direction.Invoke (_finger.Direction.ToQuaternion ());
					this._title = _FingersNames[(int)_finger.Type] + " Finger";
				} else {
					this._title = "Unassigned Finger";
				}
			}
		}
		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_Finger" ) {
				Finger = null;
			}
		}

		[SerializeField,Outlet]
		Vector3Event Tip = new Vector3Event ();

		[SerializeField,Outlet]
		QuaternionEvent Direction = new QuaternionEvent ();

		void Update()
		{
		}
	}
}
