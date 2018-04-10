using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Klak.Wiring
{
	public class GenericValueInput<T> : NodeBase 
	{

		[SerializeField]
		T _value=default(T);

		[SerializeField]
		T _minValue=default(T);

		[SerializeField]
		T _maxValue=default(T);

		protected virtual void _validate(){
		}

		public T MinValue
		{
			set{
				_minValue = Value;
				_validate ();
				_Invoke (_value);
			}
			get{
				return _minValue;
			}
		}
		public T MaxValue
		{
			set{
				_maxValue = Value;
				_validate ();
				_Invoke (_value);
			}
			get{
				return _maxValue;
			}
		}

		public T Value
		{
			set{
				_value = value;
				_validate ();
				_Invoke (_value);
			}
			get{
				return _value;
			}
		}

		#region Node I/O

		protected virtual void _Invoke(T value)
		{
		}

		[Inlet]
		public void Bang()
		{
			_Invoke (_value);
		}
		#endregion

		#region MonoBehaviour functions


		void Start()
		{
		}

		void Update()
		{
		}

		#endregion
	}
}