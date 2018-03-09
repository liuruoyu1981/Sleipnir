#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sleipnir;

namespace Sleipnir.Editor
{        
    public partial class GraphEditor : OdinEditorWindow
    {
        private Graph m_Graph;
        public List<GraphNode> Nodes;
        public List<GraphConnection> Connections;

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadNodeTypes();
            LoadStyles();
        }

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
            RemoveNodes();
            RemoveConnections();
            DrawGrid();
            BeginZoomed();
            DrawConnectionToMouse();
            base.DrawEditor(index);
            EndZoomed();
            GUI.matrix = m;
        }
    }
}
#endif