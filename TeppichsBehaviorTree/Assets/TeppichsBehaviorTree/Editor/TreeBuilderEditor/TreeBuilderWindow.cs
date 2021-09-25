using System.Linq;
using TeppichsBehaviorTree.Editor.TreeRunnerEditor;
using TeppichsBehaviorTree.TreeBuilder;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.TreeBuilderEditor
{
    public class TreeBuilderWindow : EditorWindow
    {
        private string fileName = "New Behavior Tree";

        private TreeBuilderGraphView graphView;

        private void OnEnable()
        {
            GenerateGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void OnDisable() => rootVisualElement.Remove(graphView);

        
        private void GenerateBlackBoard()
        {
            Blackboard blackBoard = new Blackboard(graphView);
            blackBoard.Add(new BlackboardSection {title = "Exposed Properties"});

            blackBoard.addItemRequested = _blackBoard =>
            {
                graphView.AddPropertyToBlackBoard(new ExposedProperty());
            };

            blackBoard.editTextRequested = (blackboard, element, newValue) =>
            {
                string oldPropertyName = ((BlackboardField) element).text;

                if (graphView.exposedProperties.Any(x => x.propertyName == newValue))
                    EditorUtility.DisplayDialog("Error",
                                                "This property name already exists, please choose another one!", "OK");

                int propertyIndex =
                    graphView.exposedProperties.FindIndex(x => x.propertyName == oldPropertyName);

                graphView.exposedProperties[propertyIndex].propertyName = newValue;

                ((BlackboardField) element).text = newValue;
            };

            blackBoard.SetPosition(new Rect(10, 30, 200, 140));

            graphView.Add(blackBoard);
            graphView.blackboard = blackBoard;
        }
        
        [MenuItem("Behavior Tree/TreeBuilder")]
        public static void Open()
        {
            TreeBuilderWindow window = GetWindow<TreeBuilderWindow>();
            window.titleContent = new GUIContent("TreeBuilder");
        }

        private void GenerateGraphView()
        {
            graphView = new TreeBuilderGraphView(this) {name = "Tree Builder Graph View"};
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void GenerateToolbar()
        {
            Toolbar   toolbar           = new Toolbar();
            TextField fileNameTextField = new TextField("File Name:");

            fileNameTextField.SetValueWithoutNotify(fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) {text  = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});

            rootVisualElement.Add(toolbar);
        }

        private void GenerateMiniMap()
        {
            MiniMap miniMap = new MiniMap();

            // var coords  = dialogueGraphView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
            Vector2 coords = graphView.contentViewContainer.WorldToLocal(new Vector2(30, 30));

            miniMap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
            graphView.Add(miniMap);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");

                return;
            }

            TreeBuilderGraphSaveUtility saveUtility = TreeBuilderGraphSaveUtility.GetInstance(graphView);

            if (save)
                saveUtility.SaveGraph(fileName);
            else
                saveUtility.LoadGraph(fileName);
        }
    }
}