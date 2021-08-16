using System;
using TeppichsTools.Data;
using UnityEngine;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class NodeData
    {
        public        Type     type; 
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
}