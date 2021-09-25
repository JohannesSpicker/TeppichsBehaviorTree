using System;
using System.Collections.Generic;
using System.Linq;
using TeppichsTools.Runtime.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = TeppichsBehaviorTree.Runtime.Core.Primitives.Node;

namespace TeppichsBehaviorTree.Editor.TreeBuilderEditor
{
    public class TreeBuilderSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow         editorWindow;
        private TreeBuilderGraphView graphView;

        private Texture2D indentationIcon;

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (!(searchTreeEntry.userData is Type nodeType) || !nodeType.IsSubclassOf(typeof(Node)))
                return false;

            Vector2 worldMousePosition =
                editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent,
                                                                   context.screenMousePosition
                                                                   - editorWindow.position.position);

            Vector2 localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);

            graphView.CreateNode(nodeType, localMousePosition);

            return true;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTree =
                new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("Create Elements")) };

            string[] lastNamespace = new string[0];

            foreach (Type type in TypeReflectionUtility.GetSubtypesFromAllAssemblies(typeof(Node))
                                                       .OrderBy(t => t.Namespace).ThenBy(t => t.Name))
            {
                int level = 1;

                if (type.Namespace != null)
                {
                    string[] currentNamespace = type.Namespace.Split('.');

                    for (int i = 0; i < currentNamespace.Length; i++)
                        if (lastNamespace.Length <= i || lastNamespace[i] != currentNamespace[i])
                            AddGroupToTree(currentNamespace[i], i + 1);

                    lastNamespace = currentNamespace;
                    level         = currentNamespace.Length + 1;
                }

                AddNodeToTree(type, level);
            }

            return searchTree;

            void AddGroupToTree(string title, int level) =>
                searchTree.Add(new SearchTreeGroupEntry(new GUIContent(title), level));

            void AddNodeToTree(Type type, int level)
            {
                SearchTreeEntry candidate = TypeToSearchTreeEntry(type, level);

                if (null != candidate)
                    searchTree.Add(candidate);
            }

            static SearchTreeEntry TypeToSearchTreeEntry(Type type, int level) =>
                new SearchTreeEntry(new GUIContent(type.Name)) { userData = type, level = level };
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