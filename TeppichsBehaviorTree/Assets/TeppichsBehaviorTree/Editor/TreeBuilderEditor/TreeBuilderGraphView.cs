using System;
using System.Collections.Generic;
using System.Linq;
using TeppichsBehaviorTree.TreeBuilder;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.TreeBuilderEditor
{
    public class TreeBuilderGraphView : GraphView
    {
        public readonly Vector2               defaultNodeSize   = new Vector2(150f, 200f);
        public readonly List<ExposedProperty> exposedProperties = new List<ExposedProperty>();

        public  Blackboard              blackboard;
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

            AddElement(TreeBuilderNodeFactory.GenerateEntryPoint());
            AddSearchWindow(editorWindow);
        }

        #region GraphView stuff

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            searchWindow = ScriptableObject.CreateInstance<TreeBuilderSearchWindow>();
            searchWindow.Initialize(this, editorWindow);

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public void CreateNode(Type nodeType, Vector2 mousePosition) =>
            AddElement(TreeBuilderNodeFactory.CreateTreeBuilderNode(this, nodeType, mousePosition));

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

        #endregion

        #region BlackBoard

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

            ExposedProperty property =
                new ExposedProperty { propertyName = localPropertyName, propertyValue = localPropertyValue };

            exposedProperties.Add(property);

            VisualElement container = new VisualElement();

            BlackboardField blackboardField =
                new BlackboardField { text = property.propertyName, typeText = "string property" };

            container.Add(blackboardField);

            TextField propertyValueTextField = new TextField("Value:") { value = localPropertyValue };

            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                int changingPropertyIndex = exposedProperties.FindIndex(x => x.propertyName == property.propertyName);
                exposedProperties[changingPropertyIndex].propertyValue = evt.newValue;
            });

            BlackboardRow blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
            container.Add(blackBoardValueRow);

            blackboardField.Add(container);
        }

        #endregion
    }
}