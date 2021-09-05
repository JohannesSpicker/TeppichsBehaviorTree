using System;
using System.Collections.Generic;
using System.Linq;
using TeppichsTools.Data;
using UnityEngine;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class NodeData
    {
        //runtime
        public        Type     type;
        
        //graphView
        public        string   guid;
        public        Vector2  position;

        public Library library;

        public NodeData(Type type, string guid, Vector2 position, Library library)
        {
            this.type     = type;
            this.guid     = guid;
            this.position = position;
            this.library  = new Library(library);
        }
    }

    [Serializable]
    public class SavedNode
    {
        public Type    type;
        public string  guid;
        public Vector2 position;
        
        public Library library;

        public List<SavedNode> children;
        public SavedNode(Type type, string guid, Vector2 position, Library library, List<SavedNode> children)
        {
            this.type     = type;
            this.guid     = guid;
            this.position = position;
            this.library  = new Library(library);
            this.children = children.ToList();
        }
    }
}