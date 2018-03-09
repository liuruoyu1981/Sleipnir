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
    public class NodeDrawer : OdinValueDrawer<GraphNode>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<GraphNode> entry, GUIContent label)
        {
            entry.SmartValue.BeginDraw();
            GUIHelper.PushHierarchyMode(false);
            GUIHelper.PushLabelWidth(entry.SmartValue.Rect.width * 0.3f);
            this.CallNextDrawer(entry, null);
            GUIHelper.PopLabelWidth();
            GUIHelper.PopHierarchyMode();
            entry.SmartValue.EndDraw();
        }
    }
    
    [OdinDrawer]
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public class GraphNodeListDrawer<ListType> : OdinValueDrawer<ListType> where ListType : List<GraphNode>
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