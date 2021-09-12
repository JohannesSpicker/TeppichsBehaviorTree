using System;
using System.Collections.Generic;
using System.Linq;
using ModularBehaviourTree;
using TeppichsBehaviorTree.TreeBuilder;
using TeppichsTools.Data;
using TeppichsTools.Runtime.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = ModularBehaviourTree.Node;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    public class TreeBuilderSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow         editorWindow;
        private TreeBuilderGraphView graphView;

        private Texture2D indentationIcon;

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            //Vector2 worldMousePosition =
            //    editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent,
            //                                                       context.screenMousePosition
            //                                                       - editorWindow.position.position);

            //Vector2 localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);

            Vector2 localMousePosition = graphView.contentViewContainer.WorldToLocal(context.screenMousePosition);

            switch (searchTreeEntry.userData)
            {
                case TreeBuilderNode node:
                    graphView.CreateNode("Node", localMousePosition);

                    return true;
                default: return false;
            }
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            Type[] nodeTypes = TypeReflectionUtility.GetSubtypesFromAllAssemblies(typeof(Node));

            List<SearchTreeEntry> myTree = new List<SearchTreeEntry>();
            myTree.Add(new SearchTreeGroupEntry(new GUIContent("Create Elements")));

            List<NodeData> preparedData = new List<NodeData>();

            foreach (Type nodeType in nodeTypes)
            {
                NodeData nodeData = NodeData.TypeToNoteData(nodeType);

                if (nodeData != null)
                    preparedData.Add(nodeData);
            }

            string lastNamespace = "";

            foreach (NodeData nodeData in preparedData.OrderBy(x => x.GetType().Namespace).ThenBy(x => GetType().Name))
            {
                string currentNamespace = nodeData.GetType().Namespace;

                if (currentNamespace != lastNamespace)
                {
                    myTree.Add(new SearchTreeGroupEntry(new GUIContent(currentNamespace), 1));
                    lastNamespace = currentNamespace;
                }

                SearchTreeEntry candidate = NodeDataToSearchTreeEntry(nodeData);

                if (null != candidate)
                    myTree.Add(candidate);
            }

            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements")),
                new SearchTreeGroupEntry(new GUIContent("Behavior Tree"),  1),
                new SearchTreeGroupEntry(new GUIContent("Behavior Tree2"), 1),
                new SearchTreeEntry(new GUIContent("Node", indentationIcon))
                {
                    userData =
                        new TreeBuilderNode(false,
                                            new NodeData(new MockMemento(), Guid.NewGuid().ToString(),
                                                         Vector2.down, new Library())),
                    level = 2
                },
                new SearchTreeEntry(new GUIContent("Node2", indentationIcon))
                {
                    userData =
                        new TreeBuilderNode(false,
                                            new NodeData(new MockMemento(), Guid.NewGuid().ToString(),
                                                         Vector2.down, new Library())),
                    level = 2
                }
            };

            return myTree;
        }

        private static SearchTreeEntry NodeDataToSearchTreeEntry(NodeData nodeData)
        {
            return new SearchTreeEntry(new GUIContent(nodeData.memento.GetType().Name, new Texture2D(1, 1)))
            {
                userData =
                    new TreeBuilderNode(false,
                                        new NodeData(new MockMemento(), Guid.NewGuid().ToString(), Vector2.down,
                                                     new Library())),
                level = 2
            };

            return null;
        }

        public void Initialize(TreeBuilderGraphView graphView, EditorWindow editorWindow)
        {
            this.graphView    = graphView;
            this.editorWindow = editorWindow;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            indentationIcon.Apply();
        }
    }
}