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
        [HideInInlineEditors]
        public List<Node> Nodes;
        [HideInInlineEditors]
        public List<Connection> Connections;
        
        void Awake() // OnCreate
        {
            Nodes = new List<Node>();
            Connections = new List<Connection>(); 
        }
    }
}