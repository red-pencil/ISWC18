using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class RTPAudioSource:IAudioSource
{


	public RobotConnectionComponent RobotConnector;
	public TxEarsOutput Output;

	List<GstIAudioGrabber> _audioGrabber=new List<GstIAudioGrabber>();
	List<GstAudioPacketProcessor> _audioProcessor=new List<GstAudioPacketProcessor>();
	bool _configReceived=false;

	public bool AudioStream = false;
	RobotInfo _ifo;
	int _audioSourceCount = 1; 
	bool _isSpatialAudio=true;
	bool _audioCreated=false;


	public List<GstIAudioGrabber> AudioGrabber
	{
		get{
			return _audioGrabber;
		}
	}

	void OnAudioConfig(string config)
	{
		if (_audioCreated)
			return;

		Output.Clear();
		//XmlReader reader = XmlReader.Create (new StringReader (config));
		XmlDocument d = new XmlDocument ();
		d.Load(new StringReader (config));
		int.TryParse (d.DocumentElement.GetAttribute ("StreamsCount"), out _audioSourceCount);
		if (d.DocumentElement.GetAttribute ("SpatialAudio") == "1" || 
			d.DocumentElement.GetAttribute ("SpatialAudio") == "True")
			_isSpatialAudio = true;
		else
			_isSpatialAudio = false;

		Output.SupportSpatialAudio = _isSpatialAudio;

		int channel = 0;
		XmlNodeList elems= d.DocumentElement.GetElementsByTagName ("Pos");
		foreach (XmlNode e in elems) {
			Vector3 v = new Vector3 ();
			string[] comps= e.Attributes.GetNamedItem ("Val").Value.Split(",".ToCharArray());
			v.x = float.Parse (comps [0]);
			v.y = float.Parse (comps [1]);
			v.z = float.Parse (comps [2]);
			var c=Output.GetChannel (channel, true);
			c.AudioLocation=v;
		}
		_configReceived = true;
	}

	void _initAudioPlayers()
	{
		_audioCreated = true;
		_configReceived = false;
		//Create audio playback
		if(AudioStream)
		{
			string audioPorts = "";
			TxEarsOutput.SourceChannel[] channels;
			if (!_isSpatialAudio) {
				channels = new TxEarsOutput.SourceChannel[1]{ TxEarsOutput.SourceChannel.Both };
				for (int i = 0; i < _audioSourceCount; ++i) {
					var c=Output.GetChannel (i, true);
					c.AudioLocation=Vector3.zero;
				}

			} else {
				//check number of audio locations
				for (int i = 0; i < 2*_audioSourceCount; ++i) {
					var c=Output.GetChannel (i, true);
					c.AudioLocation=Vector3.zero;
				}

				channels = new TxEarsOutput.SourceChannel[2]{ TxEarsOutput.SourceChannel.Right,TxEarsOutput.SourceChannel.Left};
			
			}

			int idx = 0;
			int channelIndex = 0;
			for (int i = 0; i < _audioSourceCount; ++i) {
				GstNetworkAudioGrabber grabber;
				GstAudioPacketProcessor processor;

				grabber = new GstNetworkAudioGrabber ();

				uint audioPort = (uint)Settings.Instance.GetPortValue ("AudioPort", 0);
				string ip = Settings.Instance.GetValue("Ports","ReceiveHost",_ifo.IP);
				grabber.Init (audioPort, 2, AudioSettings.outputSampleRate);
				grabber.Start ();
				
				audioPort = grabber.GetAudioPort ();
				Debug.Log ("Playing audio from port:" + audioPort.ToString ());
				audioPorts += audioPort.ToString ();
				if (i != _audioSourceCount - 1)
					audioPorts += ",";

				// next create the audio grabber to encapsulate the audio player
				processor = new GstAudioPacketProcessor();
				processor.Grabber = grabber;
				_audioProcessor.Add (processor);

				//link processor with the channels
				for (int j = 0; j < channels.Length; ++j,++channelIndex) {
					var c=Output.GetChannel (channelIndex, true);
					c.channel = channels [j];
					processor.AttachedPlayers.Add (c);
				}

				_audioGrabber.Add (grabber);

			}
			RobotConnector.Connector.SendData (TxKitEars.ServiceName,"AudioPort", audioPorts, true);
		}
	}
	public void Init(RobotInfo ifo)
	{
		_ifo = ifo;
		_audioCreated = false;
		RobotConnector.Connector.DataCommunicator.OnAudioConfig += OnAudioConfig;

		RobotConnector.Connector.SendData(TxKitEars.ServiceName,"AudioParameters","",false,true);

	}

	public void Close()
	{
		if (_audioGrabber != null) {
			for(int i=0;i<_audioGrabber.Count;++i)
				_audioGrabber[i].Close ();
			_audioGrabber.Clear ();
		}
		foreach (var g in _audioProcessor) {
			g.Close ();
		}
		_audioProcessor.Clear ();
		Output.Clear ();
		_configReceived = false;
	}

	public void Pause()
	{
		if (_audioGrabber != null) {

			foreach(var g in _audioGrabber){
				g.Pause ();
			}

		}
			
	}


	public void Resume()
	{
		if (_audioGrabber != null) {
			foreach(var g in _audioGrabber){
				g.Resume ();
			}
		}
	}

	public void Update()
	{
		if (_configReceived) {
			_initAudioPlayers ();
			_configReceived = false;
		}
	}

	public float GetAverageAudioLevel ()
	{
		return 0;
	}
	public void SetAudioVolume (float vol)
	{
		foreach (var g in _audioGrabber) {
			g.SetVolume (vol);
		}
	}
}
