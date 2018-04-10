using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Klak.Wiring
{


	[AddComponentMenu("Klak/Wiring/Modifier/Float Linear Shifter")]
	[NodeAttribute("Modifier/Float Linear Shifter")]
	public class FloatLinearShifterNode:GenericLinearShifterNode<float>
    {
        [SerializeField, Outlet]
        public FloatEvent value = new FloatEvent();

		bool _enabled=true;
		[SerializeField, Inlet]
		public bool Enabled
		{
			get{
				return _enabled;
			}
			set{
				_enabled = value;
			}
		}


        protected override float _Invoke(float input)
        {
            var v = base._Invoke(input);
            value.Invoke(v);
            return v;
        }
		public FloatLinearShifterNode()
		{
			scaler=1;
		}
		public override float linearShift(float v)
		{
			if (!_enabled)
				return v;
			return v*scaler+shift;
		}
	}
}
