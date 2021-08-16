using System;
using TeppichsBehaviorTree.TreeBuilder;
using TeppichsTools.Data;
using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    /// <summary>
    ///     Visual node seen in the graphView
    /// </summary>
    public class TreeBuilderNode : Node
    {
        public bool    entryPoint;
        public string  guid;
        public Library library = new Library();
        public Type    type;

        public TreeBuilderNode(bool entryPoint, string guid, Type type, string title)
        {
            this.entryPoint = entryPoint;
            this.guid       = guid;
            this.type       = type;
            this.title      = title;
        }

        public sealed override string title { get => base.title; set => base.title = value; }

        public NodeData ToNodeData() => new NodeData(type, guid, GetPosition().position, library);
    }
}