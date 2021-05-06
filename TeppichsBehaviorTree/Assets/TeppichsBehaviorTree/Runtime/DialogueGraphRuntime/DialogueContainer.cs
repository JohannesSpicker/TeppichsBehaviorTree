using System;
using System.Collections.Generic;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.DialogueGraphRuntime
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData>     nodeLinks         = new List<NodeLinkData>();
        public List<DialogueNodeData> dialogueNodeData  = new List<DialogueNodeData>();
        public List<ExposedProperty>  exposedProperties = new List<ExposedProperty>();
    }
}