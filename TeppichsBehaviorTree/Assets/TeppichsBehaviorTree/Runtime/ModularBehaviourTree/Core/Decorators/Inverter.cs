using System;
using TeppichsTools.Data;

namespace ModularBehaviourTree.Core.Decorators
{
    internal class Inverter : Decorator
    {
        public Inverter(Node node) : base(node) { }

        protected override void Initialise(Blackboard blackboard) { }

        protected override NodeState Continue(Blackboard blackboard)
        {
            NodeState childState = base.Continue(blackboard);

            switch (childState)
            {
                case NodeState.Failure: return NodeState.Success;
                case NodeState.Running: return NodeState.Running;
                case NodeState.Success: return NodeState.Failure;
                default:                throw new ArgumentOutOfRangeException();
            }
        }

        protected override void Terminate(Blackboard blackboard) { }

        internal override Memento BuildMemento() => new InverterMemento();
    }

    [Serializable]
    internal class InverterMemento : Memento
    {
        public override Node BuildNode(Library library, Node[] children) => new Inverter(children[0]);
    }
}