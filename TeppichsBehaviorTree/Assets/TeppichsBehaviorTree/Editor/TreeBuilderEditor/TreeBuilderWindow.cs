using TeppichsBehaviorTree.Editor.TreeRunnerEditor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
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
        }

        private void OnDisable() => rootVisualElement.Remove(graphView);

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

            //toolbar.Add(new Button(() => RequestDataOperation(true)) {text  = "Save Data"});
            //toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});

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
    }
}