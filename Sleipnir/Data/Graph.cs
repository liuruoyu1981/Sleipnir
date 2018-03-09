using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Sleipnir
{
    [CreateAssetMenu(menuName = "Sleipnir/Graph")]
    public class Graph : SerializedScriptableObject
    {
        public int GridSnapSize = 16;
        [HideInInlineEditors]
        public List<INode> Nodes;
        [HideInInlineEditors]
        public List<Connection> Connections;
        
        void Awake() // OnCreate
        {
            Nodes = new List<INode>();
            Connections = new List<Connection>(); 
        }

        public bool HasConnection(INode a, INode b)
        {
            for (int i = 0; i < Connections.Count; i++) {
                if (Connections[i].A == a && Connections[i].B == b)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveNode(INode node)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
               if (Nodes[i] == node)
               {
                   for (int j = Connections.Count - 1; j >= 0 ; j--)
                   {
                       if (Connections[j].A == node || Connections[j].B == node)
                       {
                           Connections.RemoveAt(j);
                       }
                   }
                   Nodes.RemoveAt(i);
                   return;
               } 
            }
        }

        public void RemoveConnection(Connection connection)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i] == connection)
                {
                    Connections.RemoveAt(i);
                    return;
                }
            }
        }

        [Button]
        public void Evaluate()
        {
            ResetEvaluations();
            PerformEvaluations();
            ResetEvaluations();
        }

        private void ResetEvaluations()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].HasEvaluated = false;
            }
        }

        private bool IsReadyToEvaluate(INode node)
        {
            bool output = true;
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].B == Nodes[i])
                {
                    if (!Connections[i].A.HasEvaluated)
                    {
                        output = false;
                    }
                }
            }
            return output;
        }

        private void EvaluateNode(INode node)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].B == node)
                {
                    Debug.LogFormat("Node: {0} needs input from Node: {1}", node, Connections[i].A);
                }
            }
            node.Evaluate();
        }

        private void PerformEvaluations()
        {
            bool NodesStillNeedEvaluation = true;
            while (NodesStillNeedEvaluation)
            {
                NodesStillNeedEvaluation = false;
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (!Nodes[i].HasEvaluated)
                    {
                        if (IsReadyToEvaluate(Nodes[i]))
                        {
                            try
                            {
                                EvaluateNode(Nodes[i]);
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogErrorFormat("Error evaluating node: {0}\n{1}", Nodes[i].Name, ex);
                            }
                            Nodes[i].HasEvaluated = true;
                        } else {
                            NodesStillNeedEvaluation = true;
                        }
                    }
                }
            }
        }
    }
}