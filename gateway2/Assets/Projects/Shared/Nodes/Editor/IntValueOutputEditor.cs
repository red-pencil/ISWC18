using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Wiring.Patcher;
using UnityEditor;


namespace Klak.Wiring
{
	[CustomEditor(typeof(IntValueOutput))]
	public class IntValueOutputEditor : Editor
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

	[NodeRendererAttribute(typeof(IntValueOutput))]
	public class IntValueOutputNodeRenderer : Node {
		public IntValueOutputNodeRenderer()
		{
			//	this.color = UnityEditor.Graphs.Styles.Color.Red;

		}
		public override void OnNodeUI (GraphGUI host)
		{ 
			base.OnNodeUI (host);
			var e=this.runtimeInstance as IntValueOutput;

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Value");
			UnityEditor.EditorGUILayout.IntField(e.Value,EditorStyles.boldLabel);
			GUILayout.EndHorizontal ();

		}
	}
	[CustomEditor(typeof(IntValueOutputNodeRenderer))]
	class IntValueOutputNodeRendererEditor : NodeEditor
	{
	}

}