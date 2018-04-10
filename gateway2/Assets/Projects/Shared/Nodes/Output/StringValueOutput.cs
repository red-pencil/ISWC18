using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Output/String Value")]
	[NodeAttribute("Output/String Value")]
	public class StringValueOutput : NodeBase {

		public string _inputValue;

		public string Value
		{
			get{ return _inputValue; }
		}
		[Inlet]
		public string input {
			set {
				if (!enabled) return;
				_inputValue = value;
			}
		}

	}
}
