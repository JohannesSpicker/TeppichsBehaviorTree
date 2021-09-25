using System;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;

namespace TeppichsBehaviorTree.Runtime.Core.Decorators
{
    internal class Inverter : Decorator
    {
        public Inverter(Node                          node) : base(node) { }
        protected override void Initialise(Blackboard blackboard) { }

        protected override NodeState Continue(Blackboard blackboard) => base.Continue(blackboard) switch
        {
            NodeState.Failure => NodeState.Success,
            NodeState.Running => NodeState.Running,
            NodeState.Success => NodeState.Failure,
            _                 => throw new ArgumentOutOfRangeException()
        };

        protected override void Terminate(Blackboard blackboard) { }
    }
}