namespace ModularBehaviourTree.Core.Decorators
{
    internal class Repeater : Decorator
    {
        private readonly uint repetitions;
        private          uint counter;
        public Repeater(Node node, uint repetitions) : base(node) { this.repetitions = repetitions; }

        protected override void Initialise(Blackboard blackboard) => counter = 0;

        protected override NodeState Continue(Blackboard blackboard)
        {
            for (; counter < repetitions; counter++)
            {
                NodeState childState = node.Tick(blackboard);

                if (childState != NodeState.Success)
                    return childState;
            }

            return NodeState.Success;
        }

        protected override void Terminate(Blackboard blackboard) { }
    }
}