using System;
using System.Collections.Generic;
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