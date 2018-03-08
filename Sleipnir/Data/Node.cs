using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Sleipnir
{
    [Serializable]
    public class Node {
        public string ID;
        public Rect Rect; 
        [ShowInInspector, HideReferenceObjectPicker]
        public INode Data;

        public string Name{
            get {
                if (Data == null)
                {
                    return ID;
                } else {
                    return Data.Name;
                }
            }
        }

        public Node(Vector2 gridPosition)
        {
            ID = Guid.NewGuid().ToString();
            Rect = new Rect(gridPosition, new Vector2(200, 100));
        }

        public override bool Equals(System.Object obj)  {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) 
                return false;

            Node other = (Node)obj;
            return (ID == other.ID);
        }
        
        public override int GetHashCode() {
            return ID.GetHashCode();
        }
        
        public override string ToString()
        {
            return Name;
        }
    }
}