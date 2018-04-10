using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Leap;
using Leap.Unity;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Perceptual/Hand Controller")]
	[NodeAttribute("TxKit/Perceptual/Hand Controller")]
	public class HandControllerNode : NodeBase {

		[SerializeField]
		LeapServiceProvider _handController;

		public enum HandType
		{
			Left,
			Right
		}

		[SerializeField]
		public HandType Type=HandType.Left;

		[Serializable]
		public class FingerEvent:UnityEvent<Leap.Finger>
		{
		}


		[SerializeField,Outlet]
		FingerEvent Thumb=new FingerEvent();

		[SerializeField,Outlet]
		FingerEvent Index=new FingerEvent();

		[SerializeField,Outlet]
		FingerEvent Middle=new FingerEvent();

		[SerializeField,Outlet]
		FingerEvent Ring=new FingerEvent();

		[SerializeField,Outlet]
		FingerEvent Pinky=new FingerEvent();

		[SerializeField,Outlet]
		QuaternionEvent Direction = new QuaternionEvent ();

		[SerializeField,Outlet]
		Vector3Event Position = new Vector3Event ();


		[SerializeField,Outlet]
		FloatEvent Fist = new FloatEvent ();


		FingerEvent[] _fingers;

		[Inlet]
		public void Calibrate()
		{
			Debug.Log ("Calibrate");
		}

		void Start()
		{
			
			_fingers = new FingerEvent[5] {
				Thumb,
				Index,
				Middle,
				Ring,
				Pinky
			};
		}
		void Update()
		{
			if (_handController == null || !_handController.GetLeapController().IsConnected)
				return;

			var h =(Type == HandType.Left) ? Hands.Left : Hands.Right;
			if (h == null)
				return;
			if (h.Confidence<0.7f)
				return;

			Direction.Invoke (h.Direction.ToQuaternion());
			Position.Invoke(h.PalmPosition.ToVector3());
			Fist.Invoke (Hands.GetFistStrength (h));

			foreach (var f in h.Fingers) {
				_fingers [(int)f.Type].Invoke (f);
		
			}
		}
	}
}
