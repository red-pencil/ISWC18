using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Representation/TxRobot Connector")]
	[NodeAttribute("TxKit/Representation/TxRobot Connector")]
	public class TxRobotNode : NodeBase {

		[SerializeField]
		public RobotConnectionComponent _robotConnector;

	//	[SerializeField]
		TxKitBody _txbody;

	//	[SerializeField]
		TxKitEyes _txeyes;

	//	[SerializeField]
		TxKitEars _txears;

	//	[SerializeField]
		TxKitMouth _txmouth;



		[Serializable]
		class TxEyesOutputEvent:UnityEvent<TxEyesOutput>
		{
		}

		[Serializable]
		class TxKitBodyEvent:UnityEvent<TxKitBody>
		{
		}

		[Serializable]
		class TxEarsOutputEvent:UnityEvent<TxEarsOutput>
		{
		}


		[Serializable]
		public class TxRobotEvent:UnityEvent<RobotConnectionComponent>
		{
		}


		//outlets
		[SerializeField, Outlet]
		TxEyesOutputEvent Eyes = new TxEyesOutputEvent();
		[SerializeField, Outlet]
		TxEarsOutputEvent Ears = new TxEarsOutputEvent();
		[SerializeField, Outlet]
		TxKitBodyEvent Body = new TxKitBodyEvent();


		[SerializeField, Outlet]
		TxRobotEvent Robot = new TxRobotEvent();



		[Inlet]
		public RobotConnectionComponent RobotConnector
		{
			set {
				if (!enabled) return;

				if (_robotConnector == value)
					return;
				_robotConnector = value;
				_robotLinked ();

			}
			get{
				return _robotConnector;
			}
		}

		//inlets
		[Inlet]
		public Vector3 HeadPosition {
			set {
				if (!enabled) return;

				if (_txbody != null) {
					_txbody.HeadPosition = value;
				}
			}
		}



		[Inlet]
		public Quaternion HeadRotation{
			set {
				if (!enabled) return;

				if (_txbody != null) {
					_txbody.HeadOrientation = value;
				}
			}
		}


		//inlets
		[Inlet]
		public GstIAudioGrabber Mouth {
			set {
				if (!enabled) return;
				_txmouth.Grabber=value;

			}
		}



		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_Mouth" ) {
				_txmouth.Grabber=null;
			}
		}

		public override void OnOutputConnected (string srcSlotName, NodeBase target, string targetSlotName)
		{
			base.OnOutputConnected (srcSlotName, target, targetSlotName);
			if (targetSlotName == "RobotConnector" ) {
				Robot.Invoke (RobotConnector);
			}
		}

		void _robotLinked()
		{
			if (RobotConnector != null) {
				_txeyes = RobotConnector.GetComponent<TxKitEyes> ();
				_txbody = RobotConnector.GetComponent<TxKitBody> ();
				_txmouth = RobotConnector.GetComponent<TxKitMouth> ();
				_txears = RobotConnector.GetComponent<TxKitEars> ();
			}
		}

		void Start()
		{
			_robotLinked ();
		}

		void Update()
		{
			if(_txeyes!=null)
				Eyes.Invoke (_txeyes.Output);
			if(_txears!=null)
				Ears.Invoke (_txears.Output);
			Body.Invoke (_txbody);

			Robot.Invoke (RobotConnector);
		}
	}
}