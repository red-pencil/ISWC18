using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TxEarsOutput {


	public enum SourceChannel
	{
		Both,
		Left,
		Right
	}

	public class AudioChannel:List<AudioSamples>,IGstAudioPlayerHandler
	{
		public TxEarsOutput Owner;
		public SourceChannel channel= SourceChannel.Both;
		public Vector3 AudioLocation;

		public AudioChannel(TxEarsOutput o)
		{
			Owner=o;
		}
		public void AddAudioPacket (AudioSamples samples)
		{
		/*	AudioSamples samples = Owner.CreatePacket();
			samples.samples = data;
			samples.startIndex = startIndex;
			samples.channels = channels;*/
			lock (this) {
				Add (samples);
				if (Count > Owner.MaxBuffersCount)
					RemoveAt (0);
			}
		}
		AudioSamples GetExistingPacket()
		{
			if (Count > 0) {

				AudioSamples p = this [0];
				RemoveAt (0);
				return p;
			}
			return null;
		}

		public void ReadAudio(float[] data, int channels,bool block)
		{
			uint length = 0;
			int timeout = 0;
			float average = 0;
			uint DataLength = (uint)data.Length;
			uint srcChannelsCount = 2;
			uint targetLength = DataLength;

			uint channelIndex = 0;
			uint stepSize = 1;

			while (length < DataLength) {
				AudioSamples p;

				lock (this) {
					p = GetExistingPacket ();
				}
				if (p == null) {
					if (block && timeout < 20) {
						++timeout;
						continue;
					} else
						break;
				}

				srcChannelsCount = p.channels;
				if (srcChannelsCount == 2 && this.channel != SourceChannel.Both) {
					srcChannelsCount = 1;
					stepSize = 2;
					channelIndex = (uint)(this.channel == SourceChannel.Left ? 0 : 1);
				}

				//calculate the left amount of data in this packet
				uint sz = (uint)Mathf.Max (0, p.samples.Length - p.startIndex);
				//determine the amount of data we going to use of this packet
				uint count = (uint)Mathf.Min (sz, 
					             Mathf.Max (0, data.Length - length)/*Remaining data to be filled*/);

				GstNetworkAudioPlayer.ProcessAudioPackets (p.samples, (int)p.startIndex, (int)channelIndex, (int)count, (int)stepSize, (int)srcChannelsCount, data, (int)length,(int) channels);

				lock (this) {
					if (count + p.startIndex < p.samples.Length) {
						p.startIndex = count + p.startIndex;
						Insert (0, p);
					} else
						Owner.RemovePacket (p);
				}
				length += count;
			}
		}
	}

	public delegate void Deleg_OnEarsOutputChanged(TxEarsOutput output);
	public event Deleg_OnEarsOutputChanged OnEarsOutputChanged;
	//List<AudioSamples> _graveyard = new List<AudioSamples> ();

	Dictionary<int,AudioChannel> _channels=new Dictionary<int,AudioChannel>();
	public Dictionary<int,AudioChannel> Channels
	{
		get{
			return _channels;
		}
	}

	AudioSamples CreatePacket()
	{
		return new AudioSamples ();
	}
	protected void RemovePacket(AudioSamples p)
	{
		/*lock (_graveyard) {
			_graveyard.Add (p);
		}*/
	}


	public int MaxBuffersCount = 100;

	bool _supportSpatialAudio=false;
	public bool SupportSpatialAudio
	{
		get{ return _supportSpatialAudio; }
		set{
			if (value == _supportSpatialAudio)
				return;
			_supportSpatialAudio = value;
			if (OnEarsOutputChanged != null)
				OnEarsOutputChanged (this);
		}
	}

	public void SetChannel(int channel, SourceChannel ch)
	{
		lock (_channels) {
			var c=GetChannel(channel,true);
			if(c!=null)
				c.channel = ch;
		}
	}

	public void AddAudioSamples(int channel,AudioSamples samples)
	{
		var c = GetChannel (channel, true);
		c.AddAudioPacket (samples);
	}

	public AudioSamples GetSamples(int channel,bool remove=true)
	{
		AudioSamples s=null;
		lock (_channels) {
			var c = GetChannel (channel, false);
			if (c == null)
				return null;
			if (c.Count > 0) {
				s = c [0];
				if (remove) {
					c.RemoveAt (0);
				}
			}
		}
		return s;
	}

	public AudioChannel GetChannel(int channel,bool create=false)
	{
		lock (_channels) {
			if (!_channels.ContainsKey (channel)) {
				if (!create)
					return null;
				else {
					_channels.Add (channel, new AudioChannel (this));
					if (OnEarsOutputChanged != null)
						OnEarsOutputChanged (this);
				}
			}
			return _channels[channel];
		}
	}


	public void Clear()
	{
		lock (_channels) {
			_channels.Clear ();
		}
		if (OnEarsOutputChanged != null)
			OnEarsOutputChanged (this);
	}
}

