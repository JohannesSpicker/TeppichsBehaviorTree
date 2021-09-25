using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.TreeBuilder;

namespace TeppichsBehaviorTree.TreeBuilder
{
    public static class PocoTreeBuilder
    {
        public static Node UnpackNode(NodeDataWithChildren data) => data.BuildNode();
    }
}