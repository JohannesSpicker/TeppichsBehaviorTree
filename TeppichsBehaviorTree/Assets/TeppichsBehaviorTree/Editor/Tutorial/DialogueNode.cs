using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class DialogueNode : Node
    {
        public string dialogueText;

        public bool   entryPoint = false;
        public string guid;
    }
}