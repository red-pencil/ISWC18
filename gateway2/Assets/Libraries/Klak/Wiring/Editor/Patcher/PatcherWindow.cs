//
// Klak - Utilities for creative coding with Unity
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;

namespace Klak.Wiring.Patcher
{
    // Patcher window class
    public class PatcherWindow : EditorWindow
    {
        #region Class methods

        // Open the patcher window with a given patch.
		public static PatcherWindow OpenPatch(Wiring.Patch patch)
        {
            var window = EditorWindow.GetWindow<PatcherWindow>("EDD");
			window.Initialize(patch);
			window.wantsMouseMove = true;
            window.Show();
			//EditorWindow.FocusWindowIfItsOpen ();
			return window;
        }

        // Open from the main menu (only open the empty window).
        [MenuItem("Window/TxKit/EDD")]
        static void OpenEmpty()
        {
            OpenPatch(null);
        }

        #endregion

        #region EditorWindow functions

		GUIStyle _labelStyle;

        void OnEnable()
        {
            // Initialize if it hasn't been initialized.
            // (this could be happened when a window layout is loaded)
            if (_graph == null) Initialize(null);

            Undo.undoRedoPerformed += OnUndo;
        }

        void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndo;
        }

        void OnUndo()
        {
            // Invalidate the graph and force repaint.
            _graph.Invalidate();
            Repaint();
        }

        void OnFocus()
        {
            // Invalidate the graph if the hierarchy was touched while unfocused.
            if (_hierarchyChanged) _graph.Invalidate();
        }

        void OnLostFocus()
        {
            _hierarchyChanged = false;
        }

        void OnHierarchyChange()
        {
            _hierarchyChanged = true;
        }

		bool _autoUpdate=false;

		float kZoomMin=0.1f;
		float kZoomMax=2.0f;

		private void DrawNonZoomArea()
		{
			_zoom = EditorGUI.Slider(new Rect(50.0f, 30.0f, 200.0f, 25.0f), _zoom, kZoomMin, kZoomMax);
		}
		float _zoom=1.75f;
		private  Rect _zoomArea = new Rect (0, 75, 600, 300);
		private Vector2 _zoomCoordsOrigin = Vector2.zero;

		private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
		{
			return (screenCoords - _zoomArea.min) / _zoom + _zoomCoordsOrigin;
		}

		private void HandleEvents()
		{
			// Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
			// as the zoom center instead of the top left corner of the zoom area. This is achieved by
			// maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.
			if (Event.current.type == EventType.ScrollWheel)
			{
				Vector2 screenCoordsMousePos = Event.current.mousePosition;
				Vector2 delta = Event.current.delta;
				Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
				float zoomDelta = -delta.y / 150.0f;
				float oldZoom = _zoom;
				_zoom += zoomDelta;
				_zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
				_zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

				Event.current.Use();
			}

			// Allow moving the zoom area's origin by dragging with the middle mouse button or dragging
			// with the left mouse button with Alt pressed.
			if (Event.current.type == EventType.MouseDrag &&
				(Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) ||
				Event.current.button == 2)
			{
				Vector2 delta = Event.current.delta;
				delta /= _zoom;
				_zoomCoordsOrigin += delta;

				Event.current.Use();
			}
		}
        void OnGUI()
		{
            const float kBarHeight = 17;
            var width = position.width;
            var height = position.height;

            // Synchronize the graph with the patch at this point.
            if (!_graph.isValid)
            {
                _graphGUI.PushSelection();
                _graph.SyncWithPatch();
                _graphGUI.PopSelection();
            }

            // Show the placeholder if the patch is not available.
            if (!_graph.isValid)
            {
                DrawPlaceholderGUI();
                return;
            }

			/*HandleEvents ();
			_zoomArea = new Rect (0-_zoomCoordsOrigin.x, 0-_zoomCoordsOrigin.y, width, height - kBarHeight);
            // Main graph area
			EditorZoomArea.Begin (_zoom,_zoomArea);*/
			_graphGUI.BeginGraphGUI(this, new Rect(0, 0, width, height - kBarHeight));
            _graphGUI.OnGraphGUI();
			_graphGUI.EndGraphGUI();
			//EditorZoomArea.End ();

            // Clear selection on background click
            var e = Event.current;
            if (e.type == EventType.MouseDown && e.clickCount == 1)
                _graphGUI.ClearSelection();


			_autoUpdate=GUILayout.Toggle(_autoUpdate,"Auto Refresh",GUILayout.Width(100));

		//	if (_labelStyle == null) 
			{
				_labelStyle = new GUIStyle (GUI.skin.GetStyle ("Label"));
				_labelStyle.normal.textColor = Color.white;
				_labelStyle.alignment = TextAnchor.MiddleCenter;
				_labelStyle.fontSize = 14;
			}

			GUI.Label (new Rect (width-320, height-80, 300, 40), "Embodied-Driven Design Framework"/*"\nDeveloped by: MHD Yamen Saraiji"*/,_labelStyle);

            // Status bar
            GUILayout.BeginArea(new Rect(0, height - kBarHeight, width, kBarHeight));
            GUILayout.Label(_graph.patch.name);
            GUILayout.EndArea();
			//DrawNonZoomArea ();

        }

        #endregion

        #region Private members

        Graph _graph;
        GraphGUI _graphGUI;
        bool _hierarchyChanged;

        // Initializer (called from OpenPatch)
        void Initialize(Wiring.Patch patch)
        {
            hideFlags = HideFlags.HideAndDontSave;
            _graph = Graph.Create(patch);
            _graphGUI = _graph.GetEditor();
        }

        // Draw the placeholder GUI.
        void DrawPlaceholderGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("No patch is selected for editing", EditorStyles.largeLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("You must select a patch in Hierarchy, then press 'Open Patcher' from Inspector.", EditorStyles.miniLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

		void Update()
		{
			if(_autoUpdate)
				Repaint ();
		}

        #endregion
    }
}
