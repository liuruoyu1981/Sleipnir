using System;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sleipnir
{
    public abstract class BaseNode : ScriptableObject, INode
    {
        private string m_ID;
        public string ID { get { return m_ID; } }
        [ShowInInspector, PropertyOrder(-100000), HideLabel, InlineButton("RenameAsset", "Rename")]
        public string Name { get; set; }
        #if UNITY_EDITOR
        public void RenameAsset()
        {
            name = Name;
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), Name);
        }
        #endif

        [HideInInspector]
        public Rect Rect { get; set; }

        [HideInInspector]
        public bool HasEvaluated { get; set; }
        
        void Awake()  // Create
        {
            m_ID = Guid.NewGuid().ToString();
            Name = GetType().Name;
            Rect = new Rect(Vector2.zero, new Vector2(250, 125));
        }

        public virtual void Evaluate() {}

        public override bool Equals(System.Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) 
                return false;

            BaseNode other = (BaseNode)obj;
            return (ID == other.ID);
        }
        
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
        
        public override string ToString()
        {
            return Name;
        }
    }
}