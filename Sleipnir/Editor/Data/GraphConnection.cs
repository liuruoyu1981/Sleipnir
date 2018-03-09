#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sleipnir;

namespace Sleipnir.Editor
{
    [Serializable]
    public class GraphConnection {
        public bool IsDeleted = false;
        public Connection Connection;
        public GraphNode A;
        public GraphNode B;

        private GraphEditor m_Editor;
        
        public GraphConnection(GraphEditor edtior, Connection connection, GraphNode a, GraphNode b)
        {
            m_Editor = edtior;
            Connection = connection;
            A = a;
            B = b;
        }

        public override bool Equals(System.Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) 
                return false;

            GraphConnection other = (GraphConnection)obj;
            return (A == other.A && B == other.B);
        }
        
        public override int GetHashCode()
        {
            return A.GetHashCode() + B.GetHashCode();
        }
        
        public override string ToString()
        {
            return string.Format("{0} => {1}", A, B);
        }
        
        public void Draw()
        {
            Handles.DrawBezier(
                m_Editor.GridToGUIDrawRect(A.OutputKnob()).center,
                m_Editor.GridToGUIDrawRect(B.InputKnob()).center,
                m_Editor.GridToGUIDrawRect(A.OutputKnob()).center + Vector2.right * 50f,
                m_Editor.GridToGUIDrawRect(B.InputKnob()).center + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
        }
    }
}
#endif