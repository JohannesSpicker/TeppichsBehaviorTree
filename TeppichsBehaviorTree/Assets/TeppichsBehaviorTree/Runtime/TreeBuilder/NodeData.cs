using System;
using System.Collections.Generic;
using System.Linq;
using ModularBehaviourTree;
using TeppichsTools.Data;
using UnityEngine;

namespace TeppichsBehaviorTree.TreeBuilder
{
    /// <summary>
    ///     Base NodeData
    ///     Memento holds the type.
    ///     Used and wrapped in TreeBuilderGraph.
    /// </summary>
    [Serializable]
    public class NodeData
    {
        public string  guid;
        public Vector2 position;

        public Library library;

        public Memento memento;

        public NodeData(Memento memento, string guid, Vector2 position, Library library)
        {
            this.memento  = memento;
            this.guid     = guid;
            this.position = position;
            this.library  = new Library(library);
        }

        public static NodeData TypeToNodeData(Type nodeType)
        {
            if (nodeType.IsSubclassOf(typeof(Node)) && !nodeType.Name.Contains("Mock")
                                                    && !nodeType.Name.Contains("Test")
                                                    && HasDefaultConstructor(nodeType))
            {
                Memento memento = (Activator.CreateInstance(nodeType) as Node)?.BuildMemento();

                if (memento != null)
                    return new NodeData(memento, Guid.NewGuid().ToString(), Vector2.zero, new Library());
            }

            return null;
        }

        private static bool HasDefaultConstructor(Type type) =>
            type.GetConstructors().Any(t => t.GetParameters().Count() == 0);

        public static Type NodeDataToType(NodeData nodeData) =>
            nodeData.memento.BuildNode(new Library(), null).GetType();
    }

    /// <summary>
    ///     Used for saving.
    ///     Used for loading into runtime.
    /// </summary>
    [Serializable]
    public class NodeDataWithChildren : NodeData
    {
        public List<NodeDataWithChildren> children;

        public NodeDataWithChildren(Memento                    memento, string guid, Vector2 position, Library library,
                                    List<NodeDataWithChildren> children) : base(memento, guid, position, library)
        {
            this.children = children;
        }

        public Node BuildNode() => memento.BuildNode(library, BuildChildren());

        private List<Node> BuildChildren()
        {
            List<Node> nodes = new List<Node>();

            foreach (NodeDataWithChildren child in children)
                nodes.Add(child.BuildNode());

            return nodes;
        }
    }
}