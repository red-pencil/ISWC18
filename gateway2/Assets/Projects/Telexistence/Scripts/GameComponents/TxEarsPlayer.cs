using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxEarsPlayer : MonoBehaviour {
	

	TxEarsOutput _output;
	public TxEarsOutput Output
	{
		get{ return _output; }
		set{
			if (_output == value)
				return;
			if (_output != null)
				_output.OnEarsOutputChanged -= OnEarsOutputChanged;
			_output = value;
			if (_output != null)
				_output.OnEarsOutputChanged += OnEarsOutputChanged;

			_changed = true;
		}
	}



	bool _audioInited=false;
	List<AudioSource> _audioSource=new List<AudioSource>();
	bool _changed=false;

	void OnEarsOutputChanged(TxEarsOutput output)
	{
		_changed = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_changed) {
			Destroy ();
			Init ();
			_changed = false;
		}
	}

	void OnDestroy()
	{
		Destroy();
	}

	void Destroy()
	{
		if (!_audioInited)
			return;
		for (int i=0;i< _audioSource.Count;++i) {
			Destroy (_audioSource[i].gameObject);
		}
		_audioSource.Clear ();
		_audioInited = false;
	}

	void Init()
	{
		if (_audioInited || Output==null)
			return;
		var channels = Output.Channels;

		AudioClip clip = AudioClip.Create("", 1, 1, AudioSettings.outputSampleRate, false);
		clip.SetData(new float[] { 1 }, 0);

		foreach (var c in channels) {

			var audioObj = new GameObject ("AudioObject");
			audioObj.transform.parent = this.transform;
			audioObj.transform.localPosition = Vector3.zero;
			AudioSource asrc = audioObj.AddComponent<AudioSource> ();
			asrc.loop = true;
			asrc.clip = clip;
			asrc.spatialBlend=Output.SupportSpatialAudio?1.0f:0.0f;
			asrc.Play ();
			var player = audioObj.AddComponent<TxEarsAudioPlayerCallback> ();
			player.Player = this;
			player.Channel = c.Key;
			_audioSource.Add (asrc);
		}

		_audioInited = true;
	}



	public void ReadAudio(int channel,float[] data, int channels)
	{
		if (_output==null )
			return;
		var c=_output.GetChannel (channel,false);
		if (c == null)
			return;
		c.ReadAudio (data, channels, true);
	}
}
