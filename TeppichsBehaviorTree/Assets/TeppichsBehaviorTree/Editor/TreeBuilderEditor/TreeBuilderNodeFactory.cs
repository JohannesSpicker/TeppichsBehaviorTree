using System;
using System.Reflection;
using ModularBehaviourTree;
using TeppichsBehaviorTree.Editor.TreeRunnerEditor;
using TeppichsBehaviorTree.TreeBuilder;
using TeppichsTools.Data;
using UnityEngine;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class TreeBuilderNodeFactory
    {
        public static void SomeFunc()
        {
            ConstructorInfo[] constructors = typeof(MyClass).GetConstructors();

            ConstructorInfo ctor = constructors[0];

            foreach (ParameterInfo param in ctor.GetParameters())
            {
                //Debug.Log(param.Position.ToString());
                //Debug.Log(param.Name);
                //Debug.Log(param.ParameterType);
            }
        }

        public TreeBuilderNode CreateNode(ModularBehaviourTree.Node baseNode)
        {
            var node = new TreeBuilderNode {type = baseNode.GetType(), guid = Guid.NewGuid().ToString()};

            if (baseNode is Leaf) { }
            else if (baseNode is Decorator) { }
            else if (baseNode is Composite) { }
            else if (baseNode is Condition) { }

            var type = GetType(); //delete this line

            Debug.LogError("TreeBuilderNodeFactory \t Can't create node because of invalid Node type.");

            ConstructorInfo constructor = type.GetConstructors()[0];

            //make fields for parameters of constructor

            //add an input port

            //add output ports and/or ability to add more output ports (for composites)

            return node;
        }

        public TreeBuilderNode LoadNode(NodeData nodeData)
        {
            TreeBuilderNode node = new TreeBuilderNode
            {
                type = nodeData.type, guid = nodeData.guid, library = new Library(nodeData.library)
            };

            return new TreeBuilderNode();
        }

        private class MyClass
        {
            public string someString;

            public MyClass(string someString, int someInt, float someFloat, Vector3 herrVector)
            {
                this.someString = someString;
            }
        }
    }
}