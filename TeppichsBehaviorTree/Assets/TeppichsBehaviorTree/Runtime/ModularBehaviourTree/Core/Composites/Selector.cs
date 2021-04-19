namespace ModularBehaviourTree.Composites
{
    internal class Selector : Composite
    {
        internal Selector(Node[] nodes) : base(nodes) { }

        protected override NodeState Continue(Blackboard blackboard)
        {
            for (; cursor < nodes.Length; cursor++)
            {
                NodeState childState = nodes[cursor].Tick(blackboard);

                if (childState != NodeState.Failure)
                    return childState;
            }

            return NodeState.Failure;
        }
    }
}