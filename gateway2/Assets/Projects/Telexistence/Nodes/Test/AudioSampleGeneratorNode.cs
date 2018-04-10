using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Klak.Wiring
{
	
	[AddComponentMenu("Klak/Wiring/TxKit/Test/AudioSampleGeneratorcNode")]
	[NodeAttribute("TxKit/Test/AudioSampleGeneratorcNode")]
	public class AudioSampleGeneratorcNode : NodeBase {

		[SerializeField,Outlet]
		FloatArrayEvent Samples=new FloatArrayEvent();



		List<float> _samplesBuffer=new List<float>();

		public int BufferLength=1024;
		// Use this for initialization
		void Start () {
			Oscillator _osc;
			_osc = new Oscillator ();
			_osc.SetNote (70);
			int samples = (int)Mathf.Ceil((float)BufferLength /(float) _osc.SamplesCount ())*_osc.SamplesCount ();
			for(int i=0;i<BufferLength;++i){
				_samplesBuffer.Add(_osc.Sample());
			}
		}

		void OnDestroy()
		{
		}
		
		// Update is called once per frame
		void Update () {
			Samples.Invoke (_samplesBuffer);

		}
	}
}