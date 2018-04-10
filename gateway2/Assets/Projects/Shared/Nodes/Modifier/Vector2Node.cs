using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Modifier/Vector2")]
	[NodeAttribute("Modifier/Vector2")]
	public class Vector2Node : NodeBase
	{
		[SerializeField, Outlet]
		FloatEvent _xEvent = new FloatEvent ();
		[SerializeField, Outlet]
		FloatEvent _yEvent = new FloatEvent ();

		[SerializeField]Vector2 value;

		protected void _Invoke()
		{
			_xEvent.Invoke (value.x);
			_yEvent.Invoke (value.y);
		}


		[Inlet]
		public Vector2 Value {
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


	[AddComponentMenu("Klak/Wiring/Modifier/ToVector2")]
	[NodeAttribute("Modifier/ToVector2")]
	public class ToVector2Node : NodeBase
	{
		[SerializeField, Outlet]
		Vector2Event _valueEvent = new Vector2Event ();

		[SerializeField]Vector2 value=new Vector2();

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


		void Update()
		{
			_Invoke ();
		}
	}
}
