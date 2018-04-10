using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Wiring.Patcher;
using UnityEditor;


namespace Klak.Wiring
{
	[CustomEditor(typeof(FloatValueOutput))]
	public class FloatValueOutputEditor : Editor
	{
		SerializedProperty _value;

		void OnEnable()
		{
			_value = serializedObject.FindProperty("_inputValue");

		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(_value);
			EditorGUI.EndDisabledGroup ();

			serializedObject.ApplyModifiedProperties();
		}
	}

	[NodeRendererAttribute(typeof(FloatValueOutput))]
	public class FloatValueOutputNodeRenderer : Node {
		public FloatValueOutputNodeRenderer()
		{
			//	this.color = UnityEditor.Graphs.Styles.Color.Red;

		}
		public override void OnNodeUI (GraphGUI host)
		{ 
			base.OnNodeUI (host);
			var e=this.runtimeInstance as FloatValueOutput;

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Value");
			UnityEditor.EditorGUILayout.FloatField(e.Value,EditorStyles.boldLabel);
			GUILayout.EndHorizontal ();

		}
	}
	[CustomEditor(typeof(FloatValueOutputNodeRenderer))]
	class FloatValueOutputNodeRendererEditor : NodeEditor
	{
	}

}