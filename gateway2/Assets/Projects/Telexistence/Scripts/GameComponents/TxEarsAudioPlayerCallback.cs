using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxEarsAudioPlayerCallback : MonoBehaviour {


	TxEarsPlayer _player;
	public TxEarsPlayer Player {
		set{ _player = value; }
		get{ return _player; }
	}

	public int Channel=0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnAudioFilterRead(float[] data, int channels)
	{
		//if(!SupportSpatialAudio)
		Player.ReadAudio (Channel,data, channels);
	}


	void OnAudioRead(float[] data) {
		Player.ReadAudio (Channel,data, 1);
	}
}
