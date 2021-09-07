using TeppichsBehaviorTree.TreeBuilder;
using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    /// <summary>
    ///     Visual node seen in the graphView
    /// </summary>
    public class TreeBuilderNode : Node
    {
        public  bool     entryPoint;
        private NodeData nodeData;

        public TreeBuilderNode(bool entryPoint, NodeData nodeData)
        {
            this.entryPoint = entryPoint;
            this.nodeData   = nodeData;
        }

        public string Guid { get => nodeData.guid; set => nodeData.guid = value; }

        public sealed override string title { get => base.title; set => base.title = value; }

        public NodeData ToNodeData()
        {
            nodeData.position = GetPosition().position;

            return nodeData;
        }
    }
}