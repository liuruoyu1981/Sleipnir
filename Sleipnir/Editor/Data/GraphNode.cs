#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace Sleipnir
{
    [Serializable]
    public class GraphNode {
        private GraphEditor m_Editor;
        private Node m_Node;
        public Node Node {
            get { return m_Node; }
        }
        //[HideInInspector]
        public bool IsDeleted = false;
        
        public string ID {
            get { return m_Node.ID; }
        }
        public Rect Rect {
            get { return m_Node.Rect; }
            set { m_Node.Rect = value; }
        }

        [ShowInInspector, ReadOnly]
        public string Name {
            get { return m_Node.Name; }
        }
        
        [ShowInInspector, HideReferenceObjectPicker]
        public INode Data {
            get { return m_Node.Data; }
            set { m_Node.Data = value; }
        }

        public GraphNode(GraphEditor editor, Node node)
        {
            m_Editor = editor;
            m_Node = node;
        }

        public override bool Equals(System.Object obj)  {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) 
                return false;

            GraphNode other = (GraphNode)obj;
            return (ID == other.ID);
        }
        
        public override int GetHashCode() {
            return ID.GetHashCode();
        }
        
        public override string ToString()
        {
            return Name;
        }
        
        public Rect InputKnob ()
        {
            if (GraphEditor.LayoutDirection == GraphEditor.GraphDirection.Horizontal)
            {
                return new Rect(Rect.xMin - GraphEditor.nodeKnobSize, Rect.yMin + (Rect.height / 2), GraphEditor.nodeKnobSize, GraphEditor.nodeKnobSize);
            } else {
                return new Rect(Rect.xMin + (Rect.width / 2) - (GraphEditor.nodeKnobSize / 2), Rect.yMin - GraphEditor.nodeKnobSize, GraphEditor.nodeKnobSize, GraphEditor.nodeKnobSize);
            }
        }
        public Rect OutputKnob ()
        {
            if (GraphEditor.LayoutDirection == GraphEditor.GraphDirection.Horizontal)
            {
                return new Rect(Rect.xMax, Rect.yMin + (Rect.height / 2), GraphEditor.nodeKnobSize, GraphEditor.nodeKnobSize);
            } else {
                return new Rect(Rect.xMin + (Rect.width / 2) - (GraphEditor.nodeKnobSize / 2), Rect.yMax, GraphEditor.nodeKnobSize, GraphEditor.nodeKnobSize);
            }
        }
        // Edges
        public Rect TopEdge ()
        {
            return new Rect(Rect.xMin, Rect.yMin - GraphEditor.mouseEdgeWidth, Rect.width, GraphEditor.mouseEdgeWidth);
        }
        public Rect RightEdge ()
        {
            return new Rect(Rect.xMax, Rect.yMin, GraphEditor.mouseEdgeWidth, Rect.height);
        }
        public Rect BottomEdge ()
        {
            return new Rect(Rect.xMin, Rect.yMax, Rect.width, GraphEditor.mouseEdgeWidth);
        }
        public Rect LeftEdge ()
        {
            return new Rect(Rect.xMin - GraphEditor.mouseEdgeWidth, Rect.yMin, GraphEditor.mouseEdgeWidth, Rect.height);
        }
        // Corners
        public Rect TopRightCorner ()
        {
            return new Rect(Rect.xMax, Rect.yMin - GraphEditor.mouseEdgeWidth, GraphEditor.mouseEdgeWidth, GraphEditor.mouseEdgeWidth);
        }
        public Rect BottomRightCorner ()
        {
            return new Rect(Rect.xMax, Rect.yMax, GraphEditor.mouseEdgeWidth, GraphEditor.mouseEdgeWidth);
        }
        public Rect BottomLeftCorner ()
        {
            return new Rect(Rect.xMin - GraphEditor.mouseEdgeWidth, Rect.yMax, GraphEditor.mouseEdgeWidth, GraphEditor.mouseEdgeWidth);
        }
        public Rect TopLeftCorner ()
        {
            return new Rect(Rect.xMin - GraphEditor.mouseEdgeWidth, Rect.yMin - GraphEditor.mouseEdgeWidth, GraphEditor.mouseEdgeWidth, GraphEditor.mouseEdgeWidth);
        }
        public bool IsTouchingResizeZone(Vector2 test)
        {
            if (InputKnob().Contains(test) || OutputKnob().Contains(test))
            {
                return false;
            }
            if (
                BottomEdge().Contains(test) ||
                RightEdge().Contains(test) ||
                BottomRightCorner().Contains(test) ||
                TopEdge().Contains(test) ||
                LeftEdge().Contains(test) ||
                TopRightCorner().Contains(test) ||
                BottomLeftCorner().Contains(test) ||
                TopLeftCorner().Contains(test)
            )
            {
                return true;
            }
            return false;
        }
        public bool IsTouchingCorner(Vector2 test)
        {
            if(TopRightCorner().Contains(test))
            {
                return true;
            }
            else if (TopLeftCorner().Contains(test))
            {
                return true;
            }
            else if (BottomRightCorner().Contains(test))
            {
                return true;
            }
            else if (BottomLeftCorner().Contains(test))
            {
                return true;
            }
            return false;
        }
        public bool IsTouchingCornersTLBR(Vector2 test)
        {
            if (TopLeftCorner().Contains(test))
            {
                return true;
            }
            else if (BottomRightCorner().Contains(test))
            {
                return true;
            }
            return false;
        }
        public bool IsTouchingCornersTRBL(Vector2 test)
        {
            if(TopRightCorner().Contains(test))
            {
                return true;
            }
            else if (BottomLeftCorner().Contains(test))
            {
                return true;
            }
            return false;
        }
        public bool IsTouchingTopBottom(Vector2 test)
        {
            if(TopEdge().Contains(test))
            {
                return true;
            }
            else if (BottomEdge().Contains(test))
            {
                return true;
            }
            return false;
        }
        public bool IsTouchingSides(Vector2 test)
        {
            if(RightEdge().Contains(test))
            {
                return true;
            }
            else if (LeftEdge().Contains(test))
            {
                return true;
            }
            return false;
        }
        
        public void BeginDraw()
        {
            GUI.Box(m_Editor.GridToGUIDrawRect(Rect), "");
            // Knobs should be a texture?
            if (GUI.Button(m_Editor.GridToGUIDrawRect(InputKnob()), ""))
            {
                m_Editor.OnClickInPoint(this);
            }
            if (GUI.Button(m_Editor.GridToGUIDrawRect(OutputKnob()), ""))
            {
                m_Editor.OnClickOutPoint(this);
            }
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(InputKnob()), Color.black);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(OutputKnob()), Color.black);
            
            // Debug
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(InputKnob()), Color.black);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(OutputKnob()), Color.red);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(TopEdge()), Color.red);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(BottomEdge()), Color.blue);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(RightEdge()), Color.green);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(LeftEdge()), Color.yellow);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(TopRightCorner()), Color.black);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(TopLeftCorner()), Color.black);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(BottomRightCorner()), Color.black);
            //EditorGUI.DrawRect(m_Editor.GridToGUIDrawRect(BottomLeftCorner()), Color.black);
            GUILayout.BeginArea(m_Editor.GridToGUIDrawRect(Rect));
        }
        
        public void EndDraw()
        {
            GUILayout.EndArea();
        }
    }
}
#endif