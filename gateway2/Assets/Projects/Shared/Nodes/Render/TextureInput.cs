using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Wiring;
using System.Reflection;

[AddComponentMenu("Klak/Wiring/Input/Texture Input")]
[NodeAttribute("Input/Texture Input")]
public class TextureInput : NodeBase {

	public Texture tex;
	Texture _oldTex;


	[SerializeField, Outlet]
	TextureEvent _tex;

	void Update()
	{
		if (tex != _oldTex) {
			_tex.Invoke (tex);
			_oldTex = tex;
		}
	}

}