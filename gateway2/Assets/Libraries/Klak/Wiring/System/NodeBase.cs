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

// Suppress "unused variable" warning messages.
using System.Collections.Generic;


#pragma warning disable 0414

using UnityEngine;
using UnityEngine.Events;
using System;

namespace Klak.Wiring
{
    // Attribute for marking inlets
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class InletAttribute : Attribute
    {
        public InletAttribute() {}
    }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
	public class InletArrayAttribute : Attribute
	{
		public InletArrayAttribute() {}
	}

    // Attribute for marking outlets
    [AttributeUsage(AttributeTargets.Field)]
    public class OutletAttribute : Attribute
    {
        public OutletAttribute() {}
	}
	// Attribute for marking outlets
	[AttributeUsage(AttributeTargets.Field)]
	public class OutletArrayAttribute : Attribute
	{
		public OutletArrayAttribute() {}
	}

	public class NodeAttribute:Attribute
	{
		public string path;
		public NodeAttribute(string path)
		{
			this.path = path;
		}
	}

    // Base class of wiring node classes
    public class NodeBase : MonoBehaviour
    {

        [SerializeField]
        bool _active = true;
        [SerializeField, Inlet]
        public bool Active
        {
            set
            {
                if (value == _active)
                    return;
                _active = value;
                OnActiveChanged(_active);
            }
            get
            {
                return _active;
            }
        }

        [SerializeField, HideInInspector]
        Vector2 _wiringNodePosition = uninitializedNodePosition;

		[HideInInspector]
		protected string _title;

		public string title {
			get { return _title; }
			set{ _title = value; }
		}

        protected virtual void OnActiveChanged(bool a)
        { }



		[Serializable] 
		public class BaseNodeEvent:UnityEvent
		{

		}

		public virtual void OnInputConnected(NodeBase src,string srcSlotName,string targetSlotName){
		}

		public virtual void OnOutputConnected(string srcSlotName,NodeBase target,string targetSlotName){
		}

		public virtual void OnInputDisconnected(NodeBase src,string srcSlotName,string targetSlotName){
		}

		public virtual void OnOutputDisconnected(string srcSlotName,NodeBase target,string targetSlotName){
		}

        [Serializable]
        public class VoidEvent : UnityEvent {}

		[Serializable]
		public class GenericEvent<T> : UnityEvent<T> {}

        [Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        [Serializable]
		public class FloatEvent : UnityEvent<float> {}

		[Serializable]
		public class IntEvent : UnityEvent<int> {}

		[Serializable]
		public class StringEvent : UnityEvent<string> {}

        [Serializable]
        public class Vector3Event : UnityEvent<Vector3> {}

		[Serializable]
		public class Vector2Event : UnityEvent<Vector2> {}

        [Serializable]
        public class QuaternionEvent : UnityEvent<Quaternion> {}

		[Serializable]
		public class AudioGrabberEvent : UnityEvent<GstIAudioGrabber>{}

		[Serializable]
		public class TextureEvent : UnityEvent<Texture> {}

        [Serializable]
        public class ColorEvent : UnityEvent<Color> {}

		[Serializable]
		public class FloatArrayEvent : UnityEvent<List<float>>{}


        static public Vector2 uninitializedNodePosition {
            get { return new Vector2(-1000, -1000); }
        }
    }
}
