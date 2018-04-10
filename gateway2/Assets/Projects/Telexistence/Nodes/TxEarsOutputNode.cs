using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Ears Input Node")]
	[NodeAttribute("TxKit/Transfer/Ears Output Node")]
	public class TxEarsOutputNode : NodeBase {

		TxEarsOutput _ears;


		[Serializable]
		public class AudioSamplesEvent:UnityEvent<AudioSamples>
		{
		}

		[Inlet]
		public TxEarsOutput Ears{
			set {
				if (!enabled) return;
				_ears = value;
			}
		}


		[SerializeField, Outlet]
		AudioSamplesEvent Left;

		[SerializeField, Outlet]
		AudioSamplesEvent Right;



		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_Ears" ) {
				Ears = null;
			}
		}
		// Use this for initialization
		void Start () {
			_ears = new TxEarsOutput ();
			_ears.SetChannel (0, TxEarsOutput.SourceChannel.Left);
			_ears.SetChannel (1, TxEarsOutput.SourceChannel.Right);
		}
		
		// Update is called once per frame
		void Update () {
			if (_ears != null) {
				var c = _ears.GetSamples (0, false);
				if (c != null)
					Left.Invoke (c);

				c = _ears.GetSamples (1, false);
				if (c != null)
					Right.Invoke (c);
			}
		}
	}
}