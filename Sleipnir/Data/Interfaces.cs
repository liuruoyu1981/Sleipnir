using System;
using UnityEngine;

namespace Sleipnir
{
    public interface INode
    {
        string ID { get; }
        string Name { get; set; }
        Rect Rect { get; set; }

        bool HasEvaluated { get; set; }
        void Evaluate();
    }
}