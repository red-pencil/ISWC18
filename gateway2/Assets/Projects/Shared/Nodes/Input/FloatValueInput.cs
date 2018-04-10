using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Input/Float Value")]
	[NodeAttribute("Input/Float Value")]
	public class FloatValueInput : GenericValueInput<float>
	{
		[SerializeField, Outlet]
		FloatEvent _valueEvent = new FloatEvent();

		protected override void _Invoke(float value)
		{
			_valueEvent.Invoke (value);
		}




	}
}