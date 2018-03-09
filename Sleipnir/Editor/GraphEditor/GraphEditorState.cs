#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sleipnir;

namespace Sleipnir.Editor
{
    public partial class GraphEditor
    {
        public enum GraphDirection
        {
            Horizontal, // Left -> Right
            Vertical // Top -> Down
        }
        public static GraphDirection LayoutDirection = GraphDirection.Horizontal;
        
        public static float nodeKnobSize = 20f;
        
        public static Rect m_LayoutRect;
        private static Vector2 m_Pan = Vector2.zero;
        public static Vector2 Pan { get { return m_Pan; } set { m_Pan = value; GUIHelper.RequestRepaint(); } }
        private static float m_Zoom = 1f;
        public static float Zoom { get { return m_Zoom; } set { m_Zoom = Mathf.Clamp(value, 1f, 7f); GUIHelper.RequestRepaint(); } }        
        public static float ZoomSpeed = 100.0f;
        public static GraphNode CurrentSelection;
        public static GraphNode selectedIn;
        public static GraphNode selectedOut;
        public static float edgeWidth = 5f;
        public static float mouseEdgeWidth {
            get { return edgeWidth * Zoom; }
        }
        public static bool IsResizing = false;
        public static bool ResizeTop = false;
        public static bool ResizeRight = false;
        public static bool ResizeBottom = false;
        public static bool ResizeLeft = false;
        public static bool IsPanning = false;
        public static bool IsDragging = false;
        
        public Vector2 GUIToGridPosition(Vector2 guiPosition) {
            return (guiPosition - (position.size * 0.5f) - (Pan / Zoom)) * Zoom;
        }

        public Vector2 GridToGUIPosition(Vector2 gridPosition) {
            return (position.size * 0.5f) + (Pan / Zoom) + (gridPosition / Zoom);
        }

        public Rect GridToGUIDrawRect(Rect gridRect) {
            gridRect.position = GridToGUIPositionNoClip(gridRect.position);
            return gridRect;
        }

        public Vector2 GridToGUIPositionNoClip(Vector2 gridPosition) {
            Vector2 center = position.size * 0.5f;
            float xOffset = (center.x * Zoom + (Pan.x + gridPosition.x));
            float yOffset = (center.y * Zoom + (Pan.y + gridPosition.y));
            return new Vector2(xOffset, yOffset);
        }

        private List<Type> INodeTypes;
        private void LoadNodeTypes() {
            INodeTypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach( var assembly in assemblies ) {
                var types = assembly.GetTypes();
                foreach( var t in types ) {
                    if( typeof( INode ).IsAssignableFrom( t ) && !t.IsAbstract ) {
                        INodeTypes.Add( t );
                    }
                }
            }
        }
    }
}
#endif

