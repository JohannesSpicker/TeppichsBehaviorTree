using System;

namespace TeppichsBehaviorTree.Runtime.DialogueGraphRuntime
{
    [Serializable]
    public class NodeLinkData
    {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;
    }
}