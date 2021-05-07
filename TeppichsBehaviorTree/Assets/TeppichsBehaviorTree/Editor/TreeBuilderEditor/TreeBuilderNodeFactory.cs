using System;
using UnityEngine;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class TreeBuilderNodeFactory
    {
        private class MyClass
        {
            public string someString;

            public MyClass(string someString, int someInt, float someFloat, Vector3 herrVector) { this.someString = someString; }
        }

        //eine dataNode hat
        //"blackboard" mit 
        //type to name to value dictionaries

        public static void SomeFunc()
        {
            var ctors = typeof(MyClass).GetConstructors();

            var ctor = ctors[0];

            foreach (var param in ctor.GetParameters())
            {
                Debug.Log(param.Position.ToString());
                Debug.Log(param.Name);
                Debug.Log(param.ParameterType);
            }
        }
    }
}