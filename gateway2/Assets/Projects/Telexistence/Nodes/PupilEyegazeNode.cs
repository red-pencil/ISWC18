using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace Klak.Wiring
{
	[AddComponentMenu("Klak/Wiring/TxKit/Perceptual/Pupil Eyegaze")]
	[NodeAttribute("TxKit/Perceptual/Pupil Eyegaze")]
	public class PupilEyegazeNode : NodeBase {

		Vector2 _eyePos=Vector2.zero;
		Vector2 _leftEyePos=Vector2.zero;
		Vector2 _rightEyePos=Vector2.zero;

		[SerializeField,Outlet]
		Vector2Event Position=new Vector2Event();
		[SerializeField,Outlet]
		Vector2Event LeftPosition=new Vector2Event();
		[SerializeField,Outlet]
		Vector2Event RightPosition=new Vector2Event();

		public Vector2 GetPosition {
			get {
				return _eyePos;
			}
		}

		public Vector2 GetLeftPosition {
			get {
				return _leftEyePos;
			}
		}

		public Vector2 GetRightPosition {
			get {
				return _rightEyePos;
			}
		}

		void Start()
		{
		}
		void Update()
		{
			if (PupilGazeTracker.Exists) {
				_eyePos = PupilGazeTracker.Instance.EyePos;
				Position.Invoke (_eyePos);

				_leftEyePos = PupilGazeTracker.Instance.LeftEyePos;
				LeftPosition.Invoke (_leftEyePos);

				_rightEyePos = PupilGazeTracker.Instance.RightEyePos;
				RightPosition.Invoke (_rightEyePos);
			}
		}
	}
}