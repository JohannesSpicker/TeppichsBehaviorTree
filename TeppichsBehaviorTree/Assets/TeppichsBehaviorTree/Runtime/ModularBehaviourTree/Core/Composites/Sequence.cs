namespace ModularBehaviourTree.Composites
{
    internal class Sequence : Composite
    {
        internal Sequence(Node[] nodes) : base(nodes) { }
        
        protected override NodeState Continue(Blackboard blackboard)
        {
            for (; cursor < nodes.Length; cursor++)
            {
                NodeState childState = nodes[cursor].Tick(blackboard);

                if (childState != NodeState.Success)
                    return childState;
            }

            return NodeState.Success;
        }
    }
}