using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Ears Input Node")]
	[NodeAttribute("TxKit/Transfer/Ears Input Node")]
	public class TxEarsInputNode : NodeBase {

		TxEarsOutput _ears;
		AudioSamples _leftSamples;
		AudioSamples _rightSamples;
		[Inlet]
		public AudioSamples LeftEar{
			set {
				if (!enabled) return;
				if (_ears != null && value != _leftSamples) {
					_ears.AddAudioSamples (0, value);
					_leftSamples = value;
				}
			}
		}

		[Inlet]
		public AudioSamples RightEar{
			set {
				if (!enabled) return;

				if (_ears != null && value != _rightSamples) {
					_ears.AddAudioSamples (1, value);
					_rightSamples = value;
				}
			}
		}

		[Serializable]
		public class EarsEvent : UnityEvent<TxEarsOutput> {}

		[SerializeField, Outlet]
		EarsEvent Ears;

		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
		}
		// Use this for initialization
		void Start () {
			_ears = new TxEarsOutput ();
			_ears.SetChannel (0, TxEarsOutput.SourceChannel.Left);
			_ears.SetChannel (1, TxEarsOutput.SourceChannel.Right);
		}
		
		// Update is called once per frame
		void Update () {
			Ears.Invoke (_ears);
		}
	}
}