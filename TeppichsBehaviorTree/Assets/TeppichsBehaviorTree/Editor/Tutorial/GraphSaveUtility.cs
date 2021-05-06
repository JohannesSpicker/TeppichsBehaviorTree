using System.Collections.Generic;
using System.Linq;
using TeppichsBehaviorTree.Runtime.DialogueGraphRuntime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace TeppichsBehaviorTree.Editor.Tutorial
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _containerCache;

        private List<Edge>         Edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility() {_targetGraphView = targetGraphView};
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            if (!SaveNodes(dialogueContainer))
                return;

            SaveExposedProperties(dialogueContainer);

            if (!AssetDatabase.IsValidFolder("Assets/DialogueSaves"))
                AssetDatabase.CreateFolder("Assets", "DialogueSaves");

            if (!AssetDatabase.IsValidFolder("Assets/DialogueSaves/Resources"))
                AssetDatabase.CreateFolder("Assets/DialogueSaves", "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/DialogueSaves/Resources/{fileName}.asset");
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer)
        {
            dialogueContainer.exposedProperties.AddRange(_targetGraphView.exposedProperties);
        }

        private bool SaveNodes(DialogueContainer dialogueContainer)
        {
            if (!Edges.Any())
                return false;

            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as DialogueNode;
                var inputNode  = connectedPorts[i].input.node as DialogueNode;

                dialogueContainer.nodeLinks.Add(new NodeLinkData()
                {
                    BaseNodeGuid   = outputNode.guid,
                    PortName       = connectedPorts[i].output.portName,
                    TargetNodeGuid = inputNode.guid
                });
            }

            foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
            {
                dialogueContainer.dialogueNodeData.Add(new DialogueNodeData()
                {
                    guid         = dialogueNode.guid,
                    dialogueText = dialogueNode.DialogueText,
                    position     = dialogueNode.GetPosition().position
                });
            }

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
            
            foreach (var exposedProperty in _containerCache.exposedProperties)
                _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var connections = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].guid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode     = Nodes.First(x => x.guid == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new
                                               Rect(_containerCache.dialogueNodeData.First(x => x.guid == targetNodeGuid).position,
                                                    _targetGraphView.defaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port input, Port output)
        {
            var tempEdge = new Edge() {input = input, output = output};

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _containerCache.dialogueNodeData)
            {
                var tempNode = _targetGraphView.CreateDialogueNode(nodeData.dialogueText, Vector2.zero);
                tempNode.guid = nodeData.guid;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodeData.guid).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(x => x.EntryPoint).guid = _containerCache.nodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint)
                    continue;

                //remove edges connected to this node
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                //then remove the node
                _targetGraphView.RemoveElement(node);
            }
        }
    }
}