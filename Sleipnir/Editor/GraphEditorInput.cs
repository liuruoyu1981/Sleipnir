#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

namespace Sleipnir
{        
    public partial class GraphEditor
    {
        private void ProcessInput() {
            if (m_Graph == null) return;
            OnHover();
            if (Event.current.OnMouseDown(0, false))
            {
                OnLeftMouseDown();
            }
            if (Event.current.OnMouseDown(1, false))
            {
                OnRightMouseDown();
            }
            if (Event.current.OnMouseMoveDrag(false))
            {
                OnDrag();
            }
            if (Event.current.OnMouseUp(0, false))
            {
                OnLeftMouseUp();
            }
            if (Event.current.OnEventType(EventType.ScrollWheel))
            {
                OnScrollWheel();
            }
        }
        
        public void OnHover()
        {
            Vector2 currentPosition = GUIToGridPosition(Event.current.mousePosition);
            Rect currentRect = new Rect(Event.current.mousePosition, Vector2.one);
            for (int i = 0; i < Nodes.Count; i++) {
                if(Nodes[i].InputKnob().Contains(currentPosition) || Nodes[i].OutputKnob().Contains(currentPosition))
                {
                    continue;
                }
                if(Nodes[i].IsTouchingCorner(currentPosition))
                {
                    if(Nodes[i].IsTouchingCornersTLBR(currentPosition))
                    {
                        EditorGUIUtility.AddCursorRect(currentRect, MouseCursor.ResizeUpLeft);
                    }
                    else if (Nodes[i].IsTouchingCornersTRBL(currentPosition))
                    {
                        EditorGUIUtility.AddCursorRect(currentRect, MouseCursor.ResizeUpRight);
                    }
                }
                else
                {
                    if (Nodes[i].IsTouchingTopBottom(currentPosition))
                    {
                        EditorGUIUtility.AddCursorRect(currentRect, MouseCursor.ResizeVertical);
                    }
                    else if (Nodes[i].IsTouchingSides(currentPosition))
                    {
                        EditorGUIUtility.AddCursorRect(currentRect, MouseCursor.ResizeHorizontal);
                    }
                }  
            }
        }
        
        public void OnDrag()
        {
            Vector2 delta = Event.current.delta * Zoom;
            if (CurrentSelection != null)
            {
                Rect currentRect = CurrentSelection.Rect;
                if (IsResizing)
                {
                    if (ResizeTop)
                    {
                        currentRect.yMin += delta.y;
                    }
                    if (ResizeRight)
                    {
                        currentRect.xMax += delta.x;
                    }
                    if (ResizeBottom)
                    {
                        currentRect.yMax += delta.y;
                    }
                    if (ResizeLeft)
                    {
                        currentRect.xMin += delta.x;
                    }
                }
                if (IsDragging)
                {
                    currentRect.position += delta;
                }
                CurrentSelection.Rect = currentRect;
            }
            if (IsPanning)
            {
                Pan += delta;
            }
        }
        
        public void OnLeftMouseDown()
        {
            IsPanning = true;
            CurrentSelection = null;
            Vector2 currentPosition = GUIToGridPosition(Event.current.mousePosition);
            for (int i = 0; i < Nodes.Count; i++) {
                if (Nodes[i].Rect.Contains(currentPosition))
                {
                    CurrentSelection = Nodes[i];
                    IsDragging = true;
                    IsPanning = false;
                }
                if(Nodes[i].IsTouchingResizeZone(currentPosition)){
                    CurrentSelection = Nodes[i];
                    IsResizing = true;
                    IsDragging = false;
                    IsPanning = false;
                    
                    bool top = CurrentSelection.TopEdge().Contains(currentPosition);
                    bool right = CurrentSelection.RightEdge().Contains(currentPosition);
                    bool bottom = CurrentSelection.BottomEdge().Contains(currentPosition);
                    bool left = CurrentSelection.LeftEdge().Contains(currentPosition);
                    bool tr = CurrentSelection.TopRightCorner().Contains(currentPosition);
                    bool tl = CurrentSelection.TopLeftCorner().Contains(currentPosition);
                    bool br = CurrentSelection.BottomRightCorner().Contains(currentPosition);
                    bool bl = CurrentSelection.BottomLeftCorner().Contains(currentPosition);
                    
                    if (top || tr || tl)
                    {
                        ResizeTop = true;
                    }
                    if (right || tr || br)
                    {
                        ResizeRight = true;
                    }
                    if (bottom || br || bl)
                    {
                        ResizeBottom = true;
                    }
                    if (left || tl || bl)
                    {
                        ResizeLeft = true;
                    }
                }
            }
        }
        
        private void OnRightMouseDown()
        {
            Vector2 currentPosition = GUIToGridPosition(Event.current.mousePosition);
            GenericMenu menu = new GenericMenu();
            bool onNode = false;
            for (int i = 0; i < Nodes.Count; i++) {
                GraphNode currentNode = Nodes[i];
                if (Nodes[i].Rect.Contains(currentPosition))
                {
                    onNode = true;
                    menu.AddItem(new GUIContent("Delete Node"), false, () => { currentNode.IsDeleted = true; });
                }
            }
            if (!onNode)
            {
                menu.AddItem(new GUIContent("Create Node"), false, () => { CreateNode(currentPosition); }); 
            }
            menu.ShowAsContext();
        }
        
        public void OnLeftMouseUp()
        {
            IsResizing = false;
            ResizeTop = false;
            ResizeRight = false;
            ResizeBottom = false;
            ResizeLeft = false;
            IsPanning = false;
            IsDragging = false;
        }
        
        public void OnScrollWheel()
        {
            Zoom += -Event.current.delta.y / ZoomSpeed;
        }
        
        private void CreateNode(Vector2 gridPosition)
        {
            Node newNode = new Node(gridPosition);
            m_Graph.Nodes.Add(newNode);
            Nodes.Add(new GraphNode(this, newNode));
        }
        
        private void CreateConnection()
        {
            for (int i = 0; i < m_Graph.Connections.Count; i++) {
                if (m_Graph.Connections[i].A.ID == selectedOut.ID && m_Graph.Connections[i].B.ID == selectedIn.ID)
                {
                    return;
                }
            }
            Connection newConn = new Connection(selectedOut.Node, selectedIn.Node);
            m_Graph.Connections.Add(newConn);
            Connections.Add(new GraphConnection(this, newConn, selectedOut, selectedIn));
        }
        
        public void OnClickInPoint(GraphNode node)
        {
            if (selectedOut == null) return;
            selectedIn = node;
            if (selectedIn != selectedOut)
            {
                CreateConnection();
            }
            selectedIn = null;
            selectedOut = null;
        }

        public void OnClickOutPoint(GraphNode node)
        {
            selectedIn = null;
            selectedOut = node;
        }
    }
}
#endif