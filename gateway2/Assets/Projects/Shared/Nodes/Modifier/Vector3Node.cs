using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Modifier/Vector3")]
	[NodeAttribute("Modifier/Vector3")]
	public class Vector3Node : NodeBase
	{
		[SerializeField, Outlet]
		FloatEvent _xEvent = new FloatEvent ();
		[SerializeField, Outlet]
		FloatEvent _yEvent = new FloatEvent ();
		[SerializeField, Outlet]
		FloatEvent _zEvent = new FloatEvent ();

		[SerializeField]Vector3 value;

		protected void _Invoke()
		{
			_xEvent.Invoke (value.x);
			_yEvent.Invoke (value.y);
			_zEvent.Invoke (value.z);
		}


		[Inlet]
		public Vector3 Value {
			set {
				if (!enabled) return;
				this.value = value;
				_Invoke ();
			}
			get{
				return this.value;
			}
		}


		void Update()
		{
			_Invoke ();
		}
	}
	[AddComponentMenu("Klak/Wiring/Modifier/ToVector3")]
	[NodeAttribute("Modifier/ToVector3")]
	public class ToVector3Node : NodeBase
	{
		[SerializeField, Outlet]
		Vector3Event _valueEvent = new Vector3Event ();

		[SerializeField]Vector3 value=new Vector3();

		protected void _Invoke()
		{
			_valueEvent.Invoke (value);
		}


		[Inlet]
		public float X {
			set {
				if (!enabled) return;
				this.value.x = value;
				_Invoke ();
			}
			get{
				return this.value.x;
			}
		}

		[Inlet]
		public float Y {
			set {
				if (!enabled) return;
				this.value.y = value;
				_Invoke ();
			}
			get{
				return this.value.y;
			}
		}

		[Inlet]
		public float Z {
			set {
				if (!enabled) return;
				this.value.z = value;
				_Invoke ();
			}
			get{
				return this.value.z;
			}
		}


		void Update()
		{
			_Invoke ();
		}
	}
}
