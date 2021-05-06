using System;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.DialogueGraphRuntime
{
    [Serializable]
    public class DialogueNodeData
    {
        public string  guid;
        public string  dialogueText;
        public Vector2 position;
    }
}