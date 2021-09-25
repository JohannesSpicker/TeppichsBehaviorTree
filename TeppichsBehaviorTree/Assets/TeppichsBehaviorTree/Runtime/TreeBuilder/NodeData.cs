using System;
using System.Collections.Generic;
using System.Linq;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsTools.Data;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.TreeBuilder
{
    /// <summary>
    ///     Used and wrapped in TreeBuilderGraph.
    /// </summary>
    [Serializable]
    public class NodeData
    {
        public string  guid;
        public Vector2 position;

        public Library library;
        public Type    nodeType;

        public NodeData(Type nodeType, string guid, Vector2 position, Library library)
        {
            this.nodeType = nodeType;
            this.guid     = guid;
            this.position = position;
            this.library  = new Library(library);
        }

        public static NodeData TypeToNodeData(Type nodeType)
        {
            if (nodeType.IsSubclassOf(typeof(Node)) && !nodeType.Name.Contains("Mock")
                                                    && !nodeType.Name
                                                                .Contains("Test")) //&& HasDefaultConstructor(nodeType))
                return new NodeData(nodeType, Guid.NewGuid().ToString(), Vector2.zero, new Library());

            return null;
        }

        //private static bool HasDefaultConstructor(Type type) =>
        //    type.GetConstructors().Any(t => !t.GetParameters().Any());

        public virtual Node BuildNode() => Node.BuildNode(nodeType, library, null);
    }

    /// <summary>
    ///     Used for saving.
    ///     Used for loading into runtime.
    /// </summary>
    [Serializable]
    public class NodeDataWithChildren : NodeData
    {
        public List<NodeDataWithChildren> children;

        public NodeDataWithChildren(Type                       nodeType, string guid, Vector2 position, Library library,
                                    List<NodeDataWithChildren> children) : base(nodeType, guid, position, library)
        {
            this.children = children;
        }

        public override Node BuildNode() => Node.BuildNode(nodeType, library, BuildChildren());

        private List<Node> BuildChildren() => children.Select(child => child.BuildNode()).ToList();
    }
}