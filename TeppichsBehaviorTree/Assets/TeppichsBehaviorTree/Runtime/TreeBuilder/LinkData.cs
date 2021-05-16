using System;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class LinkData
    {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;
    }
}