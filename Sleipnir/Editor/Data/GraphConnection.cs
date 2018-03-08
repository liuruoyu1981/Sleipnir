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
    public class GraphConnection {
        private GraphEditor m_Editor;
        private Connection m_Connection;
        public bool IsDeleted = false;
        
        public string ID {
            get { return m_Connection.ID; }
        }
        private GraphNode m_A;
        public Node A {
            get { return m_Connection.A; }
            set { m_Connection.A = value; }
        }
        private GraphNode m_B;
        public Node B {
            get { return m_Connection.B; }
            set { m_Connection.B = value; }
        }
        
        public GraphConnection(GraphEditor edtior, Connection connection, GraphNode a, GraphNode b)
        {
            m_Editor = edtior;
            m_Connection = connection;
            m_A = a;
            m_B = b;
        }
        
        public void Draw()
        {
            Handles.DrawBezier(
                m_Editor.GridToGUIDrawRect(m_A.OutputKnob()).center,
                m_Editor.GridToGUIDrawRect(m_B.InputKnob()).center,
                m_Editor.GridToGUIDrawRect(m_A.OutputKnob()).center + Vector2.right * 50f,
                m_Editor.GridToGUIDrawRect(m_B.InputKnob()).center + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
        }
    }
}
#endif