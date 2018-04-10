using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Output/Float Value")]
	[NodeAttribute("Output/Float Value")]
	public class FloatValueOutput : NodeBase {

		[SerializeField]
		float _inputValue;

		public float Value
		{
			get{ return _inputValue; }
		}

		[Inlet]
		public float input {
			set {
				if (!enabled) return;
				_inputValue = value;
			}
		}

	}
}
