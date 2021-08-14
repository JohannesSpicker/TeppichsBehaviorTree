using System.Collections.Generic;
using System.Linq;
using TeppichsBehaviorTree.Runtime.DialogueGraphRuntime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class GraphSaveUtility
    {
        private DialogueContainer _containerCache;
        private DialogueGraphView _targetGraphView;

        private List<Edge>         Edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView) =>
            new GraphSaveUtility {_targetGraphView = targetGraphView};

        public void SaveGraph(string fileName)
        {
            DialogueContainer dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            if (!SaveNodes(dialogueContainer))
                return;

            SaveExposedProperties(dialogueContainer);

            if (!AssetDatabase.IsValidFolder("Assets/DialogueSaves"))
                AssetDatabase.CreateFolder("Assets", "DialogueSaves");

            if (!AssetDatabase.IsValidFolder("Assets/DialogueSaves/Resources"))
                AssetDatabase.CreateFolder("Assets/DialogueSaves", "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/DialogueSaves/Resources/{fileName}.asset");
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer) =>
            dialogueContainer.exposedProperties.AddRange(_targetGraphView.exposedProperties);

        private bool SaveNodes(DialogueContainer dialogueContainer)
        {
            if (!Edges.Any())
                return false;

            Edge[] connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                DialogueNode outputNode = connectedPorts[i].output.node as DialogueNode;
                DialogueNode inputNode  = connectedPorts[i].input.node as DialogueNode;

                dialogueContainer.nodeLinks.Add(new NodeLinkData
                {
                    baseNodeGuid   = outputNode.guid,
                    portName       = connectedPorts[i].output.portName,
                    targetNodeGuid = inputNode.guid
                });
            }

            foreach (DialogueNode dialogueNode in Nodes.Where(node => !node.entryPoint))
                dialogueContainer.dialogueNodeData.Add(new DialogueNodeData
                {
                    guid         = dialogueNode.guid,
                    dialogueText = dialogueNode.dialogueText,
                    position     = dialogueNode.GetPosition().position
                });

            return true;
        }

        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<DialogueContainer>(fileName);

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
                List<NodeLinkData> connections =
                    _containerCache.nodeLinks.Where(x => x.baseNodeGuid == Nodes[i].guid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string       targetNodeGuid = connections[j].targetNodeGuid;
                    DialogueNode targetNode     = Nodes.First(x => x.guid == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new
                                               Rect(_containerCache.dialogueNodeData.First(x => x.guid == targetNodeGuid).position,
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
            foreach (DialogueNodeData nodeData in _containerCache.dialogueNodeData)
            {
                DialogueNode tempNode = _targetGraphView.CreateDialogueNode(nodeData.dialogueText, Vector2.zero);
                tempNode.guid = nodeData.guid;
                _targetGraphView.AddElement(tempNode);

                List<NodeLinkData> nodePorts =
                    _containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodeData.guid).ToList();

                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.portName));
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(x => x.entryPoint).guid = _containerCache.nodeLinks[0].baseNodeGuid;

            foreach (DialogueNode node in Nodes)
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