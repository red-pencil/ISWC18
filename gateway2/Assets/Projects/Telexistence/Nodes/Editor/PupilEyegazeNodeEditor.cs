using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Klak.Wiring.Patcher;

namespace Klak.Wiring
{
	[CustomEditor(typeof(PupilEyegazeNode))]
	public class PupilEyegazeNodeEditor : Editor {


		void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();


			serializedObject.ApplyModifiedProperties();
		}
	}

	[NodeRendererAttribute(typeof(PupilEyegazeNode))]
	public class PupilEyegazeNodeRenderer : Node {
		public PupilEyegazeNodeRenderer()
		{
			//	this.color = UnityEditor.Graphs.Styles.Color.Red;

		}


		public override void OnNodeDraw(GraphGUI host)
		{
			base.OnNodeDraw (host);

		}

		GUIStyle pinStyle;
		public override void OnNodeUI (GraphGUI host)
		{ 
			base.OnNodeUI (host);

			var e=this.runtimeInstance as RobotConnectorNode;

			GUILayout.BeginHorizontal ();
			GUILayout.EndHorizontal ();


		}
	}
	[CustomEditor(typeof(PupilEyegazeNodeRenderer))]
	class PupilEyegazeNodeRendererEditor : NodeEditor
	{
	}

}