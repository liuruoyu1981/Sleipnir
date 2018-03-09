#if UNITY_EDITOR
using System;
using System.IO;
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
        private void CreateNode(Type type, Vector2 gridPosition)
        {
            BaseNode newNode = ScriptableObject.CreateInstance(type) as BaseNode;
            newNode.Rect = new Rect(gridPosition, newNode.Rect.size);
            string graphPath = Directory.GetParent(AssetDatabase.GetAssetPath(m_Graph)).ToString();
            string assetPath = string.Format("{0}/{1}_{2}.asset", graphPath, newNode.Name, newNode.ID);
			AssetDatabase.CreateAsset(newNode, assetPath);
			EditorUtility.SetDirty(newNode);
            BuildNode(newNode);
        }
        
        private void BuildNode(INode node)
        {
            m_Graph.Nodes.Add(node);
            Nodes.Add(new GraphNode(this, node));
        }
        
        
        private void CreateConnection()
        {
            if (m_Graph.HasConnection(selectedOut.Node, selectedIn.Node)) return;
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

        private void RemoveNodes()
        {
            for (int i = Nodes.Count - 1; i >= 0; i--) {
                if (Nodes[i].IsDeleted)
                {
                    for (int j = Connections.Count - 1; j >= 0; j--) {
                        if (Connections[j].A == Nodes[i] || Connections[j].B == Nodes[i])
                        {
                            Connections[j].IsDeleted = true;
                        }
                    }
                    m_Graph.RemoveNode(Nodes[i].Node);
                    Nodes.RemoveAt(i);
                }
            }
        }
        
        public void SwapNode(GraphNode oldNode, INode newNode)
        {
            oldNode.IsDeleted = true;
            BuildNode(newNode);
        }

        private void RemoveConnections()
        {
            for (int i = Connections.Count - 1; i >= 0; i--) {
                if (Connections[i].IsDeleted) 
                {
                    m_Graph.RemoveConnection(Connections[i].Connection);
                    Connections.RemoveAt(i);
                }
            }
        }
    }
}
#endif