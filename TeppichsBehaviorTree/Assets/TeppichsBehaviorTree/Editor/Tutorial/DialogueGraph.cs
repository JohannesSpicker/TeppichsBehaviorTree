using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class DialogueGraph : EditorWindow
    {
        private DialogueGraphView dialogueGraphView;

        private void OnEnable()
        {
            ConstructGraph();
            GenerateToolbar();
            GenerateMiniMap();
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap {anchored = true};
            miniMap.SetPosition(new Rect(10, 30, 200, 140));
            dialogueGraphView.Add(miniMap);
        }

        private void OnDisable() => rootVisualElement.Remove(dialogueGraphView);

        [MenuItem("Graph/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            DialogueGraph window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void ConstructGraph()
        {
            dialogueGraphView = new DialogueGraphView {name = "Dialogue Graph"};
            dialogueGraphView.StretchToParentSize();
            rootVisualElement.Add(dialogueGraphView);
        }

        private string fileName = "New Narrative";

        private void GenerateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) {text  = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});

            Button nodeCreateButton = new Button(() => { dialogueGraphView.CreateNode("Dialogue Node"); });
            nodeCreateButton.text = "Create Node";

            toolbar.Add(nodeCreateButton);
            rootVisualElement.Add(toolbar);
        }

        public void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");

                return;
            }

            var saveUtility = GraphSaveUtility.GetInstance(dialogueGraphView);

            if (save)
                saveUtility.SaveGraph(fileName);
            else
                saveUtility.LoadGraph(fileName);
        }
    }
}