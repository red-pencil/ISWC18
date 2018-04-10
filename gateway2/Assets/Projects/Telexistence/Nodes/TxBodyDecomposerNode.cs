using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Body Decomposer")]
	[NodeAttribute("TxKit/Transfer/Body Decomposer")]
	public class TxBodyDecomposerNode : NodeBase {
		

		Vector3 _headPos=Vector3.zero;
		Quaternion _headRotation= Quaternion.identity;

		[SerializeField,Outlet]
		Vector3Event HeadPos=new Vector3Event();

		[SerializeField,Outlet]
		Vector3Event HeadRot=new Vector3Event();

		TxKitBody _body;

		[Inlet]
		public TxKitBody Body {
			set {
				if (!enabled) return;
				_body = value;
			}
			get{
				return _body;
			}
		}

		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_Body" ) {
				Body = null;
			}
		}

		void Start()
		{
		}
		void Update()
		{
		
			if (Body != null) {
				HeadPos.Invoke (Body.RobotHeadPosition);
				HeadRot.Invoke (Body.RobotHeadRotation);
			}
		}
	}
}