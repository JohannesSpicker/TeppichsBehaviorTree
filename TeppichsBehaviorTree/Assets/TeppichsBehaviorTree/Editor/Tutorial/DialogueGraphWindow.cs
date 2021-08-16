using System.Linq;
using TeppichsBehaviorTree.Runtime.DialogueGraphRuntime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class DialogueGraphWindow : EditorWindow
    {
        private DialogueGraphView dialogueGraphView;

        private string fileName = "New Narrative";

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void OnDisable() => rootVisualElement.Remove(dialogueGraphView);

        private void GenerateBlackBoard()
        {
            Blackboard blackBoard = new Blackboard(dialogueGraphView);
            blackBoard.Add(new BlackboardSection {title = "Exposed Properties"});

            blackBoard.addItemRequested = _blackBoard =>
            {
                dialogueGraphView.AddPropertyToBlackBoard(new ExposedProperty());
            };

            blackBoard.editTextRequested = (blackboard, element, newValue) =>
            {
                string oldPropertyName = ((BlackboardField) element).text;

                if (dialogueGraphView.exposedProperties.Any(x => x.propertyName == newValue))
                    EditorUtility.DisplayDialog("Error",
                                                "This property name already exists, please choose another one!", "OK");

                int propertyIndex =
                    dialogueGraphView.exposedProperties.FindIndex(x => x.propertyName == oldPropertyName);

                dialogueGraphView.exposedProperties[propertyIndex].propertyName = newValue;

                ((BlackboardField) element).text = newValue;
            };

            blackBoard.SetPosition(new Rect(10, 30, 200, 140));

            dialogueGraphView.Add(blackBoard);
            dialogueGraphView.blackboard = blackBoard;
        }

        private void GenerateMiniMap()
        {
            MiniMap miniMap = new MiniMap {anchored = true};

            // var coords  = dialogueGraphView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
            Vector2 coords = dialogueGraphView.contentViewContainer.WorldToLocal(new Vector2(30, 30));

            miniMap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
            dialogueGraphView.Add(miniMap);
        }

        [MenuItem("Graph/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            DialogueGraphWindow window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void ConstructGraphView()
        {
            dialogueGraphView = new DialogueGraphView(this) {name = "Dialogue Graph"};
            dialogueGraphView.StretchToParentSize();
            rootVisualElement.Add(dialogueGraphView);
        }

        private void GenerateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            TextField fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) {text  = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});

            rootVisualElement.Add(toolbar);
        }

        public void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");

                return;
            }

            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(dialogueGraphView);

            if (save)
                saveUtility.SaveGraph(fileName);
            else
                saveUtility.LoadGraph(fileName);
        }
    }
}