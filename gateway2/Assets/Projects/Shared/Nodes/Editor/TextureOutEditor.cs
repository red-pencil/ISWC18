using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Klak.Wiring.Patcher;
using Graphs = UnityEditor.Graphs;

namespace Klak.Wiring
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(TextureOutput))]
	public class TextureOutEditor : Editor
	{
		SerializedProperty _texture;

		void OnEnable()
		{
			_texture = serializedObject.FindProperty("tex");

		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(_texture);

			serializedObject.ApplyModifiedProperties();
		}
	}
}

[NodeRendererAttribute(typeof(TextureOutput))]
public class TextureOutputNodeRenderer : Node {
	public TextureOutputNodeRenderer()
	{
	//	this.color = UnityEditor.Graphs.Styles.Color.Red;
	}

	void OnEnable()
	{
		
	}

	bool slots_updated=false;

	UnityEditor.Graphs.Slot texSlot;
	public override void OnNodeUI (GraphGUI host)
	{ 
		if (false && nodeStyle == null) {

			nodeStyle = new GUIStyle (Graphs.Styles.GetNodeStyle (this.style, this.color, true));
			nodeStyle.normal.background = Resources.Load<Texture2D> ("Character");
			nodeStyle.fontStyle = FontStyle.Bold;
		}
		
		base.OnNodeUI (host);
		var e=this.runtimeInstance as TextureOutput;
		var tex = e.tex;
		if (texSlot==null) {
			texSlot=AddInputSlot("Test",typeof(Texture));

		}

		// TODO: Check if texture is readable

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Texture");
		GUILayout.Box (tex, new GUILayoutOption[] { GUILayout.Width (64), GUILayout.Height (64) });
		GUILayout.EndHorizontal ();

		if (texSlot != null) {
			
			var instance=(texSlot.node as Node).runtimeInstance;
			var v = TryGetSlotPropertyValue (texSlot);
			if (v != null) {
			
				GUILayout.Box (v as Texture, new GUILayoutOption[] { GUILayout.Width (64), GUILayout.Height (64) });
			}
		}


	}
}

[CustomEditor(typeof(TextureOutputNodeRenderer))]
class TextureOutputNodeRendererEditor : NodeEditor
{
}

/*
*/