﻿using System;
using System.Collections.Generic;
using System.Linq;
using ModularBehaviourTree;
using TeppichsBehaviorTree.TreeBuilder;
using TeppichsTools.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Blackboard = UnityEditor.Experimental.GraphView.Blackboard;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    public class TreeBuilderGraphView : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(150f, 200f);

        public  Blackboard              blackboard;
        public  List<ExposedProperty>   exposedProperties = new List<ExposedProperty>();
        private TreeBuilderSearchWindow searchWindow;

        public TreeBuilderGraphView(EditorWindow editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("TreeBuilderGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GenerateEntryPoint());
            AddSearchWindow(editorWindow);
        }

        private TreeBuilderNode GenerateEntryPoint()
        {
            TreeBuilderNode node =
                new TreeBuilderNode(true,
                                    new NodeData(new MockMemento(), Guid.NewGuid().ToString(), Vector2.up, new Library()));

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

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            searchWindow = ScriptableObject.CreateInstance<TreeBuilderSearchWindow>();
            searchWindow.Initialize(this, editorWindow);

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        private Port GeneratePort(TreeBuilderNode node, Direction portDirection,
                                  Port.Capacity   capacity = Port.Capacity.Single) =>
            node.InstantiatePort(Orientation.Horizontal, portDirection, capacity,
                                 typeof(float)); //type float is arbitrary, can be used to pass data

        public TreeBuilderNode CreateTreeBuilderNode(string nodeName, Vector2 localMousePosition)
        {
            TreeBuilderNode dialogueNode =
                new TreeBuilderNode(false,
                                    new NodeData(new MockMemento(), Guid.NewGuid().ToString(), Vector2.up, new Library()));

            Port inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            dialogueNode.inputContainer.Add(inputPort);

            dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            Button button = new Button(() => { AddChoicePort(dialogueNode); });
            dialogueNode.titleContainer.Add(button);
            button.text = "New Choice";

            TextField textField = new TextField(string.Empty);

            textField.RegisterValueChangedCallback(evt => { dialogueNode.title = evt.newValue; });

            textField.SetValueWithoutNotify(dialogueNode.title);
            dialogueNode.mainContainer.Add(textField);

            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
            dialogueNode.SetPosition(new Rect(localMousePosition, defaultNodeSize));

            return dialogueNode;
        }

        public void AddChoicePort(TreeBuilderNode treeBuilderNode, string overriddenPortName = "")
        {
            Port generatedPort = GeneratePort(treeBuilderNode, Direction.Output);

            Label oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            int outputPortCount = treeBuilderNode.outputContainer.Query("connector").ToList().Count;
            generatedPort.portName = $"Choice {outputPortCount}";

            string choicePortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Choice {outputPortCount + 1}"
                : overriddenPortName;

            TextField textField = new TextField {name = string.Empty, value = choicePortName};
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);
            Button deleteButton = new Button(() => RemovePort(treeBuilderNode, generatedPort)) {text = "X"};
            generatedPort.contentContainer.Add(deleteButton);

            generatedPort.portName = choicePortName;

            treeBuilderNode.outputContainer.Add(generatedPort);
            treeBuilderNode.RefreshPorts();
            treeBuilderNode.RefreshExpandedState();
        }

        private void RemovePort(TreeBuilderNode treeBuilderNode, Port generatedPort)
        {
            IEnumerable<Edge> targetEdge = edges.ToList()
                                                .Where(x => x.output.portName == generatedPort.portName
                                                            && x.output.node  == generatedPort.node);

            if (!targetEdge.Any())
                return;

            Edge edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());

            treeBuilderNode.outputContainer.Remove(generatedPort);
            treeBuilderNode.RefreshPorts();
            treeBuilderNode.RefreshExpandedState();
        }

        public void CreateNode(string nodeName, Vector2 mousePosition) =>
            AddElement(CreateTreeBuilderNode(nodeName, mousePosition));

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void ClearBlackBoardAndExposedProperties()
        {
            exposedProperties.Clear();
            blackboard.Clear();
        }

        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
        {
            string localPropertyName  = exposedProperty.propertyName;
            string localPropertyValue = exposedProperty.propertyValue;

            while (exposedProperties.Any(x => x.propertyName == localPropertyName))
                localPropertyName = $"{localPropertyName}(1)";

            ExposedProperty property = new ExposedProperty();
            property.propertyName  = localPropertyName;
            property.propertyValue = localPropertyValue;

            exposedProperties.Add(property);

            VisualElement container = new VisualElement();

            BlackboardField blackboardField =
                new BlackboardField {text = property.propertyName, typeText = "string property"};

            container.Add(blackboardField);

            TextField propertyValueTextField = new TextField("Value:") {value = localPropertyValue};

            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                int changingPropertyIndex = exposedProperties.FindIndex(x => x.propertyName == property.propertyName);
                exposedProperties[changingPropertyIndex].propertyValue = evt.newValue;
            });

            BlackboardRow blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
            container.Add(blackBoardValueRow);

            blackboardField.Add(container);
        }
    }
}