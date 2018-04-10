//
// Klak - Utilities for creative coding with Unity
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;
using Klak.Math;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/Switching/Toggle Float")]
	[NodeAttribute("Switching/Toggle Float")]
	public class ToggleFloat : NodeBase
    {

        #region Node I/O

		int _activeValue=0;
		float[] values=new float[3];

        [Inlet]
        public float V1 {
            set {
				if (!enabled)
					return;
				values [0] = value;
			}
		}
		[Inlet]
		public float V2 {
			set {
				if (!enabled)
					return;
				values [1] = value;
			}
		}
		[Inlet]
		public float V3 {
			set {
				if (!enabled)
					return;
				values [2] = value;
			}
		}
		[Inlet]
		public void trigger() {
			if (!enabled) return;

			_activeValue = (_activeValue + 1) % values.Length;
		}

        [SerializeField, Outlet]
        FloatEvent _valueEvent = new FloatEvent();

		[SerializeField, Outlet]
		IntEvent _modeEvent = new IntEvent();


        #endregion
		#region MonoBehaviour functions

        void Start()
        {
        }

        void Update()
        {
			_valueEvent.Invoke (values [_activeValue]);
			_modeEvent.Invoke (_activeValue);
        }

        #endregion
    }
}
