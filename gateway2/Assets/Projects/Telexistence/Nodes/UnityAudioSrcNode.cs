using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Klak.Wiring
{
	
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Unity Audio Source")]
	[NodeAttribute("TxKit/Transfer/Unity Audio Source")]
	public class UnityAudioSrcNode : NodeBase {

		GstUnityAudioGrabber _grabber;


		[SerializeField,Outlet]
		AudioGrabberEvent Grabber=new AudioGrabberEvent();

		public int BufferLength=1024;
		public int Channels=1;
		public int SamplingRate=44100;


		List<float> _samplesBuffer=new List<float>();
		[Inlet]
		public List<float> Samples {
			set {
				if (!enabled) return;
				_samplesBuffer.AddRange(value);
			}
		}

		// Use this for initialization
		void Start () {
			_grabber = new GstUnityAudioGrabber ();
			_grabber.Init (BufferLength, Channels, SamplingRate);
			_grabber.Start ();
		}

		void OnDestroy()
		{
			_grabber.Destroy();
		}
		
		// Update is called once per frame
		void Update () {
			if(_grabber!=null)
				Grabber.Invoke (_grabber as GstIAudioGrabber);

			while (_samplesBuffer.Count > BufferLength) {
//				Debug.Log ("Samples");
				var buffer=_samplesBuffer.GetRange (0, BufferLength);
				_grabber.AddFrame (buffer.ToArray());
				_samplesBuffer.RemoveRange (0, BufferLength);
			}
		}
	}
}