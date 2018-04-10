using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Klak.Wiring
{
	
	[AddComponentMenu("Klak/Wiring/TxKit/Perceptual/Microphone Audio Source")]
	[NodeAttribute("TxKit/Perceptual/Microphone Audio Source")]
	public class MicAudioSrcNode : NodeBase {

		GstLocalAudioGrabber _grabber;


		[SerializeField,Outlet]
		AudioGrabberEvent Grabber=new AudioGrabberEvent();

		public int InterfaceID=0;
		public int Channels=1;
		public int SamplingRate=44100;

		float _volume=-1;
		[Inlet]
		public float Volume{
			set{
				if (!enabled) return;
				if (_grabber != null && _volume != value) {
					_volume = value;
					_grabber.SetVolume (_volume);
				}
			}
		}
		// Use this for initialization
		void Start () {
			_grabber = new GstLocalAudioGrabber ();
			_grabber.Init (GstLocalAudioGrabber.GetAudioInputInterfaceGUID(InterfaceID),Channels, SamplingRate);

			int count = GstLocalAudioGrabber.GetAudioInputInterfacesCount ();
			for (int i = 0; i < count; ++i) {
				string name = GstLocalAudioGrabber.GetAudioInputInterfaceName (i);
				string guid = GstLocalAudioGrabber.GetAudioInputInterfaceGUID (i);
				Debug.Log (i.ToString()+">> "+name + ":" + guid);
			}
		}


		public override void OnOutputConnected (string srcSlotName, NodeBase target, string targetSlotName)
		{
			base.OnOutputConnected (srcSlotName, target, targetSlotName);
			if (srcSlotName == "Grabber" && _grabber!=null) {
				_grabber.Restart ();
				_grabber.Start ();
				_grabber.SetVolume (_volume);
			}
		}
		public override void OnOutputDisconnected (string srcSlotName, NodeBase target, string targetSlotName)
		{
			base.OnOutputDisconnected (srcSlotName, target, targetSlotName);
			if (srcSlotName == "Grabber"&& _grabber!=null) {
				_grabber.Close();
			}
		}

		void OnDestroy()
		{
			_grabber.Destroy();
		}
		
		// Update is called once per frame
		void Update () {
			if(_grabber!=null)
				Grabber.Invoke (_grabber as GstIAudioGrabber);
		}
	}
}