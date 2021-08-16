using System;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class LinkData
    {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;

        public LinkData(string baseNodeGuid, string portName, string targetNodeGuid)
        {
            this.baseNodeGuid   = baseNodeGuid;
            this.portName       = portName;
            this.targetNodeGuid = targetNodeGuid;
        }
    }
}