using System;
using System.Reflection;
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
            ConstructorInfo[] ctors = typeof(MyClass).GetConstructors();

            ConstructorInfo ctor = ctors[0];

            foreach (ParameterInfo param in ctor.GetParameters())
            {
                //Debug.Log(param.Position.ToString());
                //Debug.Log(param.Name);
                //Debug.Log(param.ParameterType);
            }
        }

        public TreeBuilderNode CreateNode(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors()[0];

            //make fields for parameters of constructor

            //add an input port

            //add output ports and/or ability to add more output ports (for composites)

            return new TreeBuilderNode {type = type, guid = Guid.NewGuid().ToString()};
        }

        public TreeBuilderNode LoadNode(NodeData nodeData)
        {
            TreeBuilderNode node = new TreeBuilderNode
            {
                type = nodeData.type, guid = nodeData.guid, library = new Library() nodeData.library
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