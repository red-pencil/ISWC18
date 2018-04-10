using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Transfer/Webcamera Input Node")]
	[NodeAttribute("TxKit/Transfer/Webcamera Input Node")]

	public class WebCameraInputNode : NodeBase {

		public int Index=0;

		LocalWebcameraSource _camSource;

		[SerializeField, Outlet]
		TextureEvent texture;


		// Use this for initialization
		void Start () {
			_camSource = new LocalWebcameraSource ();
			var idx = new List<int> ();
			idx.Add (Index);
			_camSource.Init (idx);
		}

		void OnDestroy()
		{
			_camSource.Close ();
		}
		
		// Update is called once per frame
		void Update () {
			if (Index != _camSource.GetCameraIndex () [0]) {
				var idx = new List<int> ();
				idx.Add (Index);
				_camSource.Close ();
				_camSource.Init (idx);
			}

			if (texture.GetPersistentEventCount () > 0) {
				var t = _camSource.GetEyeTexture (0);
				texture.Invoke (t);
			}

		}
	}
}