using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Output/Int Value")]
	[NodeAttribute("Output/Int Value")]
	public class IntValueOutput : NodeBase {

		public int _inputValue;

		public int Value
		{
			get{ return _inputValue; }
		}
		[Inlet]
		public int input {
			set {
				if (!enabled) return;
				_inputValue = value;
			}
		}

	}
}
