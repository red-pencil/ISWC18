using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Klak.Wiring.Patcher;

namespace Klak.Wiring
{
/*	[CustomEditor(typeof(UserNode))]
	public class UserNodeEditor : Editor {


		void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();


			serializedObject.ApplyModifiedProperties();
		}
	}*/

	[NodeRendererAttribute(typeof(UserNode))]
	public class UserNodeRenderer : Node {
		public UserNodeRenderer()
		{
			//	this.color = UnityEditor.Graphs.Styles.Color.Red;

		}


		public override void OnNodeDraw(GraphGUI host)
		{
			// Recapture the variable for the delegate.
			var node2 = this;

			// Subwindow style (active/nonactive)
			var isActive = host.selection.Contains(this);

			if (this.nodeStyle == null) {
				nodeStyle = new GUIStyle( UnityEditor.Graphs.Styles.GetNodeStyle (this.style, this.color, true));
				nodeStyle.normal.background = Resources.Load<Texture2D> ("TxUser"); 
			}


			// Show the subwindow of this node.
			this.position = GUILayout.Window(
				this.GetInstanceID(), this.position,
				delegate { host.NodeGUI(node2); },
				"",nodeStyle, GUILayout.Width(128),GUILayout.Height(128)
			);

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
	[CustomEditor(typeof(UserNodeRenderer))]
	class UserNodeRendererEditor : NodeEditor
	{
	}

}