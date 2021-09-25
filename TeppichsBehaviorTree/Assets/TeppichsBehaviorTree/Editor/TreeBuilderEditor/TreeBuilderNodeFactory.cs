using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModularBehaviourTree;
using TeppichsBehaviorTree.TreeBuilder;
using TeppichsTools.Data;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace TeppichsBehaviorTree.Editor.TreeBuilderEditor
{
    /// <summary>
    ///     Shall build nodes for GraphView and SaveUtility
    /// </summary>
    public static class TreeBuilderNodeFactory
    {
        public static TreeBuilderNode GenerateEntryPoint()
        {
            TreeBuilderNode node =
                new TreeBuilderNode(true,
                                    new NodeData(new MockMemento(), Guid.NewGuid().ToString(), Vector2.up,
                                                 new Library()), "NoTitle");

            Port generatedPort = GeneratePort(node, Direction.Output);
            generatedPort.portName = "Next";
            node.outputContainer.Add(generatedPort);

            node.capabilities &= ~Capabilities.Copiable;
            node.capabilities &= ~Capabilities.Deletable;
            node.capabilities &= ~Capabilities.Renamable;

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(100, 200, 100, 150));

            return node;
        }

        private static Port GeneratePort(Node          node, Direction portDirection,
                                         Port.Capacity capacity = Port.Capacity.Single) =>
            node.InstantiatePort(Orientation.Horizontal, portDirection, capacity,
                                 typeof(float)); //type float is arbitrary, can be used to pass data

        public static TreeBuilderNode CreateTreeBuilderNode(TreeBuilderGraphView treeBuilderGraphView, Type nodeType,
                                                            Vector2              localMousePosition)
        {
            {
                //get constructors of type

                //make fields for parameters of constructor

                //add an input port

                //add output ports and/or ability to add more output ports (for composites)

                //return node;
            }

            //construct node
            TreeBuilderNode treeBuilderNode = new TreeBuilderNode(false, null, nodeType.Name); //dont pass null
            treeBuilderNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            //add parameter fields
            foreach (ParameterInfo parameter in GetParameters())
                AddFieldToTreeBuilderNode(parameter);

            //link fields with nodeData library

            //create input port
            Port inputPort = GeneratePort(treeBuilderNode, Direction.Input);
            inputPort.portName = "Input";
            treeBuilderNode.inputContainer.Add(inputPort);

            //decorators need output
            if (nodeType.IsSubclassOf(typeof(Decorator))) { }

            //composites need add port button
            if (nodeType.IsSubclassOf(typeof(Composite)))
            {
                Button button = new Button(() => { AddChoicePort(treeBuilderGraphView, treeBuilderNode); });
                treeBuilderNode.titleContainer.Add(button);
                button.text = "New Choice";

                AddChoicePort(treeBuilderGraphView, treeBuilderNode);
            }

            treeBuilderNode.RefreshPorts();
            treeBuilderNode.RefreshExpandedState();
            treeBuilderNode.SetPosition(new Rect(localMousePosition, treeBuilderGraphView.defaultNodeSize));

            return treeBuilderNode;

            IEnumerable<ParameterInfo> GetParameters()
            {
                ConstructorInfo longestConstructorInfo = null;

                //return nodeType.GetConstructors().OrderByDescending(info => info.GetParameters().Length).First()
                //               .GetParameters();

                foreach (ConstructorInfo conInfo in nodeType.GetConstructors())
                    if (longestConstructorInfo is null
                        || longestConstructorInfo.GetParameters().Length < conInfo.GetParameters().Length)
                        longestConstructorInfo = conInfo;

                return longestConstructorInfo?.GetParameters();
            }

            void AddFieldToTreeBuilderNode(ParameterInfo parameter)
            {
                Type parameterType = parameter.ParameterType;

                dynamic field = null;

                if (parameterType == typeof(string))
                {
                    field       = new TextField(parameter.Name);
                    field.value = "";
                }
                else if (parameterType == typeof(float))
                {
                    field       = new FloatField(parameter.Name);
                    field.value = 0f;
                }

                treeBuilderNode.mainContainer.Add(field);
            }
        }

        public static void AddChoicePort(TreeBuilderGraphView graphView, TreeBuilderNode treeBuilderNode,
                                         string               overriddenPortName = "")
        {
            Port generatedPort = GeneratePort(treeBuilderNode, Direction.Output);

            Label oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            int outputPortCount = treeBuilderNode.outputContainer.Query("connector").ToList().Count;
            generatedPort.portName = $"Choice {outputPortCount}";

            string choicePortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Choice {outputPortCount + 1}"
                : overriddenPortName;

            Button deleteButton =
                new Button(() => RemovePort(graphView, treeBuilderNode, generatedPort)) { text = "X" };

            generatedPort.contentContainer.Add(deleteButton);

            generatedPort.portName = choicePortName;

            treeBuilderNode.outputContainer.Add(generatedPort);
            treeBuilderNode.RefreshPorts();
            treeBuilderNode.RefreshExpandedState();
        }

        private static void RemovePort(GraphView treeBuilderGraphView, Node treeBuilderNode, Port generatedPort)
        {
            IEnumerable<Edge> targetEdge = treeBuilderGraphView.edges.ToList()
                                                               .Where(x => x.output.portName == generatedPort.portName
                                                                           && x.output.node  == generatedPort.node);

            if (!targetEdge.Any())
                return;

            Edge edge = targetEdge.First();
            edge.input.Disconnect(edge);
            treeBuilderGraphView.RemoveElement(targetEdge.First());

            treeBuilderNode.outputContainer.Remove(generatedPort);
            treeBuilderNode.RefreshPorts();
            treeBuilderNode.RefreshExpandedState();
        }

        #region Old Attempts

        public static TreeBuilderNode CreateTreeBuilderNode<T>(Memento memento) where T : ModularBehaviourTree.Node =>
            null;

        public static TreeBuilderNode CreateNode(ModularBehaviourTree.Node baseNode)
        {
            //var node = new TreeBuilderNode 
            //    {type = baseNode.GetType(), Guid = Guid.NewGuid().ToString()};

            if (baseNode is Leaf) { }
            else if (baseNode is Decorator) { }
            else if (baseNode is Composite) { }
            else if (baseNode is Condition) { }

            Debug.LogError("TreeBuilderNodeFactory \t Can't create node because of invalid Node type.");

            //get constructors of type

            //make fields for parameters of constructor

            //add an input port

            //add output ports and/or ability to add more output ports (for composites)

            //return node;
            return null;
        }

        #endregion
    }
}