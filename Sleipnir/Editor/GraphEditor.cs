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
    public partial class GraphEditor : OdinEditorWindow
    {
        private Graph m_Graph;
        public List<GraphNode> Nodes;
        public List<GraphConnection> Connections;

        public void LoadGraph(Graph graph)
        {
            m_Graph = graph;
            Nodes = new List<GraphNode>();
            Connections = new List<GraphConnection>();
            var NodeMapping = new Dictionary<string, GraphNode>();
            
            for (int i = 0; i < graph.Nodes.Count; i++) {
                GraphNode newNode = new GraphNode(this, graph.Nodes[i]);
                Nodes.Add(newNode);
                NodeMapping.Add(newNode.ID, newNode);
            }
            for (int i = 0; i < graph.Connections.Count; i++) {
                Connections.Add(new GraphConnection(this, graph.Connections[i], NodeMapping[graph.Connections[i].A.ID], NodeMapping[graph.Connections[i].B.ID]));
            }
        }
        protected override void DrawEditor(int index)
        {
            Matrix4x4 m = GUI.matrix;
            ProcessInput();
            Cleanup();
            DrawGrid();
            BeginZoomed();
            DrawConnectionToMouse();
            base.DrawEditor(index);
            EndZoomed();
            GUI.matrix = m;
        }
        
        private void Cleanup()
        {
            for (int i = Nodes.Count - 1; i >= 0; i--) {
                if (Nodes[i].IsDeleted)
                {
                    if (m_Graph.Nodes[i].ID == Nodes[i].ID)
                    {
                        for (int j = Connections.Count - 1; j >= 0; j--) {
                            if (Connections[j].A.ID == Nodes[i].ID || Connections[j].B.ID == Nodes[i].ID)
                            {
                                Connections[j].IsDeleted = true;
                            }
                        }
                        m_Graph.Nodes.RemoveAt(i);
                        Nodes.RemoveAt(i);
                    }
                    else
                    {
                        Debug.LogErrorFormat("Tried to delete node {0} but the index order didn't match!?! {1}", i, Nodes[i].Name);    
                    }
                }
            }
            for (int i = Connections.Count - 1; i >= 0; i--) {
                if (Connections[i].IsDeleted) 
                {
                    if (m_Graph.Connections[i].ID == Connections[i].ID)
                    {
                        m_Graph.Connections.RemoveAt(i);
                        Connections.RemoveAt(i);
                    }
                    else
                    {
                        Debug.LogErrorFormat("Tried to delete connection {0} but the index order didn't match!?! {1}", i, Connections[i].ID);    
                    }
                }
            }
        }
    }
}
#endif