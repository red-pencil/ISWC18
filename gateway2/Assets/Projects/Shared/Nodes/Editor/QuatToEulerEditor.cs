using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Wiring.Patcher;
using UnityEditor;


namespace Klak.Wiring
{/*
	[CustomEditor(typeof(QuatToEulerNode))]
	public class QuatToEulerNodeEditor : Editor
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

	[NodeRendererAttribute(typeof(QuatToEulerNode))]
	public class QuatToEulerNodeNodeRenderer : Node {
		public QuatToEulerNodeNodeRenderer()
		{
			//	this.color = UnityEditor.Graphs.Styles.Color.Red;

		}
		protected override void Initialize (NodeBase runtimeInstance)
		{
			base.Initialize (runtimeInstance);
		}
		public override void OnNodeUI (GraphGUI host)
		{ 
			base.OnNodeUI (host);
			var e=this.runtimeInstance as QuatToEulerNode;

			GUILayout.BeginHorizontal ();
			//GUILayout.Label ("Value");
			//UnityEditor.EditorGUILayout.LabelField(e.Euler.ToString(),EditorStyles.boldLabel);
			GUILayout.EndHorizontal ();

		}
	}
	[CustomEditor(typeof(QuatToEulerNodeNodeRenderer))]
	class QuatToEulerNodeNodeRendererEditor : NodeEditor
	{
	}*/

}