using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Representation/Robot Connector")]
	[NodeAttribute("TxKit/Representation/Robot Connector")]
	public class RobotConnectorNode : NodeBase {

		[SerializeField]
		public RobotConnectionComponent RobotConnector;

        [SerializeField]
		int _robotID=-1;

		[SerializeField, Outlet]
		TxRobotNode.TxRobotEvent Robot = new TxRobotNode.TxRobotEvent();

		[Inlet]
		public int Index
		{
			set{
				if (value == _robotID)
					return;
				_robotID = value;
			}
		}
		[Inlet]
		public void Connect()
		{
			if (RobotConnector == null)
                return;
            if (!Active)
                return;
			_Connect ();
		}
		[Inlet]
		public void Disconnect()
		{
			if (RobotConnector == null)
                return;
            if (!Active)
                return;
			RobotConnector.DisconnectRobot ();
		}
		[Inlet]
		public void StartUpdate()
		{
			if (RobotConnector == null)
                return;
            if (!Active)
                return;
			RobotConnector.StartUpdate ();
		}
		[Inlet]
		public void StopUpdate()
		{
			if (RobotConnector == null)
                return;
            if (!Active)
                return;
			RobotConnector.EndUpdate ();
		}


		void _Connect()
		{
			if (RobotConnector == null)
				return;
            RobotConnector.RobotIndex = _robotID;
            if (!Active)
                return;
			RobotConnector.ConnectRobot ();
		}

		void Update()
        {
            if (!Active)
                return;
			Robot.Invoke (RobotConnector);
		}
	}
}