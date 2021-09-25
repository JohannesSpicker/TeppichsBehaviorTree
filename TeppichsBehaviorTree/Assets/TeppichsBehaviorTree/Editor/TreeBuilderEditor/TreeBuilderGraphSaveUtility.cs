using System.Collections.Generic;
using System.Linq;
using ModularBehaviourTree;
using TeppichsBehaviorTree.Editor.TreeBuilderEditor;
using TeppichsBehaviorTree.TreeBuilder;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeppichsBehaviorTree.Editor.TreeRunnerEditor
{
    public class TreeBuilderGraphSaveUtility
    {
        private TreeContainer        containerCache;
        private TreeBuilderGraphView targetGraphView;

        private List<Edge>            Edges => targetGraphView.edges.ToList();
        private List<TreeBuilderNode> Nodes => targetGraphView.nodes.ToList().Cast<TreeBuilderNode>().ToList();

        public static TreeBuilderGraphSaveUtility GetInstance(TreeBuilderGraphView targetGraphView) =>
            new TreeBuilderGraphSaveUtility { targetGraphView = targetGraphView };

        #region Clear

        private void ClearGraph()
        {
            Nodes.Find(x => x.entryPoint).Guid = containerCache.links[0].baseNodeGuid;

            foreach (TreeBuilderNode node in Nodes.Where(node => !node.entryPoint))
            {
                //remove edges connected to this node
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => targetGraphView.RemoveElement(edge));

                //then remove the node
                targetGraphView.RemoveElement(node);
            }
        }

        #endregion

        #region Save

        public void SaveGraph(string fileName)
        {
            TreeContainer dialogueContainer = ScriptableObject.CreateInstance<TreeContainer>();

            if (!SaveNodes(dialogueContainer))
                return;

            SaveExposedProperties(dialogueContainer);

            if (!AssetDatabase.IsValidFolder(StringCollection.savePath))
                AssetDatabase.CreateFolder("Assets", StringCollection.folderName);

            if (!AssetDatabase.IsValidFolder(StringCollection.resourcePath))
                AssetDatabase.CreateFolder(StringCollection.savePath, "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, StringCollection.resourcePath + $"/{fileName}.asset");
        }

        private void SaveExposedProperties(TreeContainer dialogueContainer) =>
            dialogueContainer.exposedProperties.AddRange(targetGraphView.exposedProperties);

        private bool SaveNodes(TreeContainer treeContainer)
        {
            if (!Edges.Any())
                return false;

            Edge[] connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            foreach (Edge t in connectedPorts)
            {
                TreeBuilderNode outputNode = t.output.node as TreeBuilderNode;
                TreeBuilderNode inputNode  = t.input.node as TreeBuilderNode;

                treeContainer.links.Add(new LinkData(outputNode.Guid, t.output.portName, inputNode.Guid));
            }

            foreach (TreeBuilderNode dialogueNode in Nodes.Where(node => !node.entryPoint))
                treeContainer.nodeData.Add(dialogueNode.ToNodeData());

            return true;
        }

        #endregion

        #region Load

        public void LoadGraph(string fileName)
        {
            containerCache = Resources.Load<TreeContainer>(fileName);

            if (containerCache == null)
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
            targetGraphView.ClearBlackBoardAndExposedProperties();

            foreach (ExposedProperty exposedProperty in containerCache.exposedProperties)
                targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                List<LinkData> connections = containerCache.links.Where(x => x.baseNodeGuid == Nodes[i].Guid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string          targetNodeGuid = connections[j].targetNodeGuid;
                    TreeBuilderNode targetNode     = Nodes.First(x => x.Guid == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new
                                               Rect(containerCache.nodeData.First(x => x.guid == targetNodeGuid).position,
                                                    targetGraphView.defaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port input, Port output)
        {
            Edge tempEdge = new Edge { input = input, output = output };

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (NodeData nodeData in containerCache.nodeData)
            {
                TreeBuilderNode tempNode =
                    TreeBuilderNodeFactory.CreateTreeBuilderNode(targetGraphView,
                                                                 nodeData.memento.BuildNode(null, null).GetType(),
                                                                 Vector2.zero);

                tempNode.Guid = nodeData.guid;
                targetGraphView.AddElement(tempNode);

                List<LinkData> nodePorts = containerCache.links.Where(x => x.baseNodeGuid == nodeData.guid).ToList();

                nodePorts.ForEach(x => TreeBuilderNodeFactory.AddChoicePort(targetGraphView, tempNode, x.portName));
            }
        }

        #endregion
    }
}