using System;
using System.Collections.Generic;
using UnityEngine;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class TreeContainer : ScriptableObject
    {
        public List<LinkData>        links             = new List<LinkData>();
        public List<NodeData>        nodeData          = new List<NodeData>();
    }
}