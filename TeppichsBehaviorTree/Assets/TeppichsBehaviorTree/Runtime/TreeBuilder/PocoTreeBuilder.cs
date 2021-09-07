using ModularBehaviourTree;

namespace TeppichsBehaviorTree.TreeBuilder
{
    public static class PocoTreeBuilder
    {
        public static Node UnpackNode(NodeDataWithChildren data) => data.BuildNode();
    }
}