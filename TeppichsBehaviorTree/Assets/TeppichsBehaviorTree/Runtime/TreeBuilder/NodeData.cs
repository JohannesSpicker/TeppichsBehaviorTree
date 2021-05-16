using System;
using TeppichsTools.Data;
using UnityEngine;

namespace TeppichsBehaviorTree.TreeBuilder
{
    [Serializable]
    public class NodeData
    {
        public Type    type; 
        public string  guid;
        public Vector2 position;

        public Library library = new Library();
    }
}