using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class DialogueNode : Node
    {
        public string DialogueText;

        public bool   EntryPoint = false;
        public string guid;
    }
}