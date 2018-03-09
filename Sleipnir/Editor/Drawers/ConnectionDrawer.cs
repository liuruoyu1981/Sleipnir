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
    
    [OdinDrawer]
    public class ConnectionDrawer : OdinValueDrawer<GraphConnection>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<GraphConnection> entry, GUIContent label)
        {
            entry.SmartValue.Draw();
        }
    }
    
    
    [OdinDrawer]
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public class ConnectionListDrawer<ListType> : OdinValueDrawer<ListType> where ListType : List<GraphConnection>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<ListType> entry, GUIContent label)
        {
            for (int i = 0; i < entry.Property.Children.Count; i++)
            {
                entry.Property.Children[i].Draw();
            }
        }
    }
    
}
#endif