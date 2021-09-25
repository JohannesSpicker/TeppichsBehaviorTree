using System.Collections.Generic;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;

namespace TeppichsBehaviorTree.Runtime.Core.Composites
{
    internal class Selector : Composite
    {
        internal Selector(List<Node> nodes) : base(nodes) { }

        protected override NodeState Continue(Blackboard blackboard)
        {
            for (; cursor < nodes.Count; cursor++)
            {
                NodeState childState = nodes[cursor].Tick(blackboard);

                if (childState != NodeState.Failure)
                    return childState;
            }

            return NodeState.Failure;
        }
    }
}