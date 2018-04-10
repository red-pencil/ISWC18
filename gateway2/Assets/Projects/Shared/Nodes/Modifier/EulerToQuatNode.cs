using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Modifier/Euler To Quat")]
	[NodeAttribute("Modifier/Euler To Quat")]
	public class EulerToQuatNode : NodeBase
	{
		[SerializeField, Outlet]
		QuaternionEvent _quatEvent = new QuaternionEvent();

		[SerializeField]Quaternion outVal;

		[SerializeField] Vector3  _inputValue=new Vector3();

		protected void _Invoke(Vector3 value)
        {
            if (!Active)
                return;
			outVal=Quaternion.Euler(value);
			_quatEvent.Invoke (outVal);
		}

		[Inlet]
		public float Pitch
		{
			set {
				if (!enabled) return;
				_inputValue.x = value;
				_Invoke (_inputValue);
			}
		}
		[Inlet]
		public float Yaw
		{
			set {
				if (!enabled) return;
				_inputValue.y = value;
				_Invoke (_inputValue);
			}
		}
		[Inlet]
		public float Roll
		{
			set {
				if (!enabled) return;
				_inputValue.z = value;
				_Invoke (_inputValue);
			}
		}

		public Vector3 Euler {
			get {
				return _inputValue;
			}
		}

		public Quaternion Value {
			get {
				return outVal;
			}
		}
	}
}
