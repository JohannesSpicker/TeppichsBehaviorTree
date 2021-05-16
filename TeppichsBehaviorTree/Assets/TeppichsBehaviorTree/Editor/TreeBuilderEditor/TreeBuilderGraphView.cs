using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    public class TreeBuilderGraphView : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(150f, 200f);

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
            TreeBuilderNode node = new TreeBuilderNode
            {
                title = "START", guid = Guid.NewGuid().ToString(), entryPoint = true
            };

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
        
        private Port GeneratePort(TreeBuilderNode  node, Direction portDirection,
                                  Port.Capacity capacity = Port.Capacity.Single) =>
            node.InstantiatePort(Orientation.Horizontal, portDirection, capacity,
                                 typeof(float));

        public void CreateNode(string node, Vector2 localMousePosition) { throw new NotImplementedException(); }
    }
}