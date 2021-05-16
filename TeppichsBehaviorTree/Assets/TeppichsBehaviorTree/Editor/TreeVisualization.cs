using UnityEditor;

public class TreeVisualization : EditorWindow
{
    private void OnEnable()  => Selection.selectionChanged += Repaint;
    private void OnDisable() => Selection.selectionChanged -= Repaint;

    private void OnGUI() { }

    [MenuItem("Tools/TreeVisualization")]
    public static void OpenWindow() => GetWindow<TreeVisualization>();
}