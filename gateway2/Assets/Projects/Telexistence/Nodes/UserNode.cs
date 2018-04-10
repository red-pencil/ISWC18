using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Perceptual/User Connector")]
	[NodeAttribute("TxKit/Perceptual/User Connector")]
	public class UserNode : NodeBase {

		[SerializeField]
		TxEyesRenderer _eyesRenderer;

		[SerializeField]
		TxEarsPlayer _earsPlayer;

		TxEyesOutput _eyes;
		TxEarsOutput _ears;
		TxKitBody _body;

		[Inlet]
		public TxEyesOutput Eyes {
			set {
				if (!enabled) return;
				_eyes = value;
				_eyesRenderer.Output = _eyes;
            }
            get { return _eyes; }
		}

		[Inlet]
		public TxEarsOutput Ears {
			set {
				if (!enabled) return;
				if (_ears != value) {
					_ears = value;
					_earsPlayer.Output = _ears;
				}
            }
            get { return _ears; }
		}
		[Inlet]
		public TxKitBody Body {
			set {
				if (!enabled) return;
				if (_body != value) {
					_body = value;
					_eyesRenderer.BodyController = _body;
				}
            }
            get { return _body; }
		}


		public override void OnInputDisconnected (NodeBase src, string srcSlotName, string targetSlotName)
		{
			base.OnInputDisconnected (src, srcSlotName, targetSlotName);
			if (targetSlotName == "set_Eyes" ) {
				Eyes = null;
			}
			if (targetSlotName == "set_Ears" ) {
				Ears = null;
			}
			if (targetSlotName == "set_Body" ) {
				Body = null;
			}
		}

        void Start()
        {
            if (_eyesRenderer == null)
            {
                _eyesRenderer = gameObject.AddComponent<TxEyesRenderer>();
            }
            if (_earsPlayer == null)
            {
                _earsPlayer = gameObject.AddComponent<TxEarsPlayer>();
            }
        }
		void Update()
		{
		}
	}
}