using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Perceptual/HMD Controller")]
	[NodeAttribute("TxKit/Perceptual/HMD Controller")]
	public class HMDNode : NodeBase {
		

		Vector3 _headPos=Vector3.zero;
		Quaternion _headRotation= Quaternion.identity;

		[SerializeField,Outlet]
		Vector3Event Position=new Vector3Event();

		[SerializeField,Outlet]
		QuaternionEvent Rotation=new QuaternionEvent();

		public Vector3 GetPosition {
			get {
				return _headPos;
			}
		}

		public Quaternion GetOrientation {
			get {
				return _headRotation;
			}
		}

		void Start()
		{
			if (UnityEngine.XR.XRDevice.isPresent && OVRManager.instance!=null)
			{
				OVRManager.display.RecenterPose();
			}
		}



		[Inlet]
		public void Calibrate()
		{
			UnityEngine.XR.InputTracking.Recenter ();
		}


		void Update()
        {
            if (!Active)
                return;
			_headPos=UnityEngine.XR.InputTracking.GetLocalPosition (UnityEngine.XR.XRNode.CenterEye);
			var q=UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.CenterEye);
			q.x = -q.x;
			q.y = -q.y;
			_headRotation = q;

			Position.Invoke (_headPos);
			Rotation.Invoke (_headRotation);
		}
	}
}