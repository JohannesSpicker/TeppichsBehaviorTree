using System;
using TeppichsTools.Data;
using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    /// <summary>
    /// Visual node seen in the graphView
    /// </summary>
    public class TreeBuilderNode : Node
    {
        public Type type;

        public bool   entryPoint = false;
        public string guid;
        
        public Library library = new Library();
    }
}