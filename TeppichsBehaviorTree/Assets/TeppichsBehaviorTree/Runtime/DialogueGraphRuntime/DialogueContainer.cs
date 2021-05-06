using System;
using System.Collections.Generic;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.DialogueGraphRuntime
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData>     NodeLinks        = new List<NodeLinkData>();
        public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
    }
}