using System.Collections.Generic;
using System.Linq;
using ModularBehaviourTree;
using TeppichsBehaviorTree.TreeBuilder;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    public class TreeBuilderGraphSaveUtility
    {
        private TreeContainer    _containerCache;
        private TreeBuilderGraphView _targetGraphView;

        private List<Edge>         Edges => _targetGraphView.edges.ToList();
        private List<TreeBuilderNode> Nodes => _targetGraphView.nodes.ToList().Cast<TreeBuilderNode>().ToList();

        public static TreeBuilderGraphSaveUtility GetInstance(TreeBuilderGraphView targetGraphView) =>
            new TreeBuilderGraphSaveUtility {_targetGraphView = targetGraphView};

        public void SaveGraph(string fileName)
        {
            TreeContainer dialogueContainer = ScriptableObject.CreateInstance<TreeContainer>();

            if (!SaveNodes(dialogueContainer))
                return;

            SaveExposedProperties(dialogueContainer);

            if (!AssetDatabase.IsValidFolder(StringCollection.savePath))
                AssetDatabase.CreateFolder("Assets", "DialogueSaves");

            if (!AssetDatabase.IsValidFolder(StringCollection.resourcePath))
                AssetDatabase.CreateFolder("StringCollection.savePath", "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, StringCollection.savePath + $"/{fileName}.asset");
        }

        private void SaveExposedProperties(TreeContainer dialogueContainer) =>
            dialogueContainer.exposedProperties.AddRange(_targetGraphView.exposedProperties);

        private bool SaveNodes(TreeContainer treeContainer)
        {
            if (!Edges.Any())
                return false;

            Edge[] connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                TreeBuilderNode outputNode = connectedPorts[i].output.node as TreeBuilderNode;
                TreeBuilderNode inputNode  = connectedPorts[i].input.node as TreeBuilderNode;

                treeContainer.links.Add(new NodeLinkData
                {
                    baseNodeGuid   = outputNode.guid,
                    portName       = connectedPorts[i].output.portName,
                    targetNodeGuid = inputNode.guid
                });
            }

            foreach (TreeBuilderNode dialogueNode in Nodes.Where(node => !node.entryPoint))
                treeContainer.nodeData.Add(dialogueNode.ToNodeData());

            return true;
        }

        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<TreeContainer>(fileName);

            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exists!", "OK");

                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
            CreateExposedProperties();
        }

        private void CreateExposedProperties()
        {
            _targetGraphView.ClearBlackBoardAndExposedProperties();

            foreach (ExposedProperty exposedProperty in _containerCache.exposedProperties)
                _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                List<LinkData> connections =
                    _containerCache.links.Where(x => x.baseNodeGuid == Nodes[i].guid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string          targetNodeGuid = connections[j].targetNodeGuid;
                    TreeBuilderNode targetNode     = Nodes.First(x => x.guid == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new
                                               Rect(_containerCache.nodeData.First(x => x.guid == targetNodeGuid).position,
                                                    _targetGraphView.defaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port input, Port output)
        {
            Edge tempEdge = new Edge {input = input, output = output};

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (NodeData nodeData in _containerCache.nodeData)
            {
                TreeBuilderNode tempNode = _targetGraphView.CreateDialogueNode(nodeData.dialogueText, Vector2.zero);
                tempNode.guid = nodeData.guid;
                _targetGraphView.AddElement(tempNode);

                List<LinkData> nodePorts =
                    _containerCache.links.Where(x => x.baseNodeGuid == nodeData.guid).ToList();

                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.portName));
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(x => x.entryPoint).guid = _containerCache.links[0].baseNodeGuid;

            foreach (TreeBuilderNode node in Nodes)
            {
                if (node.entryPoint)
                    continue;

                //remove edges connected to this node
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                //then remove the node
                _targetGraphView.RemoveElement(node);
            }
        }
     }
}