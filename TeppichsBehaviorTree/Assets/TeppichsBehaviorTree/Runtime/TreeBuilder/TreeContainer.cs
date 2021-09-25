using System;
using System.Collections.Generic;
using TeppichsBehaviorTree.Runtime.TreeBuilder;
using UnityEngine;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class TreeContainer : ScriptableObject
    {
        public List<LinkData> links    = new List<LinkData>();
        public List<NodeData> nodeData = new List<NodeData>();

        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();

        public NodeData EntryPoint => nodeData[0];
    }
}