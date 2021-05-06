using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class DialogueNode : Node
    {
        public string guid;

        public string DialogueText;

        public bool EntryPoint = false;
    }
}