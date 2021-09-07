using System;
using System.Collections.Generic;
using TeppichsTools.Data;

namespace ModularBehaviourTree.Composites
{
    internal class Sequence : Composite
    {
        internal Sequence(List<Node> nodes) : base(nodes) { }

        protected override NodeState Continue(Blackboard blackboard)
        {
            for (; cursor < nodes.Count; cursor++)
            {
                NodeState childState = nodes[cursor].Tick(blackboard);

                if (childState != NodeState.Success)
                    return childState;
            }

            return NodeState.Success;
        }

        internal override Memento BuildMemento() => new SequenceMemento();
    }

    [Serializable]
    internal class SequenceMemento : Memento
    {
        public override Node BuildNode(Library library, List<Node> children) => new Sequence(children);
    }
}