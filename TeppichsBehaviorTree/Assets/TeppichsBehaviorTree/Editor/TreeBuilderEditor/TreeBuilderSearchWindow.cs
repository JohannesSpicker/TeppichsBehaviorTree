using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    public class TreeBuilderSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow         editorWindow;
        private TreeBuilderGraphView graphView;

        public void Initialize(TreeBuilderGraphView graph, EditorWindow window)
        {
            graphView    = graph;
            editorWindow = window;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            indentationIcon.Apply();
        }

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
            //TODO: make it possible to create all present behavior nodes

            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements")),
                new SearchTreeGroupEntry(new GUIContent("Behavior Tree"), 1),
                new SearchTreeEntry(new GUIContent("Node", indentationIcon))
                {
                    userData = new TreeBuilderNode(), level = 2
                }
            };

            return tree;
        }
    }
}