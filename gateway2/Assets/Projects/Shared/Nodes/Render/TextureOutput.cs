using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Wiring;
using System.Reflection;


[AddComponentMenu("Klak/Wiring/Output/Texture Output")]
[NodeAttribute("Output/Texture Output")]
public class TextureOutput : NodeBase {

	public Texture tex;


	[SerializeField, Inlet]
	public Texture Tex
	{
		set {
			
			tex = value;
		}
	}


}
