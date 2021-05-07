using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView dialogueGraphView;
        private EditorWindow      editorWindow;
        private Texture2D         indentationIcon;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
                new SearchTreeEntry(new GUIContent("Dialogue Node", indentationIcon))
                {
                    userData = new DialogueNode(), level = 2
                }
            };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 worldMousePosition =
                editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent,
                                                                   context.screenMousePosition
                                                                   - editorWindow.position.position);

            Vector2 localMousePosition = dialogueGraphView.contentViewContainer.WorldToLocal(worldMousePosition);

            switch (searchTreeEntry.userData)
            {
                case DialogueNode dialogueNode:
                    dialogueGraphView.CreateNode("Dialogue Node", localMousePosition);

                    return true;
                default: return false;
            }
        }

        public void Init(DialogueGraphView graphView, EditorWindow window)
        {
            dialogueGraphView = graphView;
            editorWindow      = window;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            indentationIcon.Apply();
        }
    }
}