using System;
using TeppichsTools.Data;

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

        internal override Memento BuildMemento() => new SelectorMemento();
    }

    [Serializable]
    internal class SelectorMemento : Memento
    {
        public override Node BuildNode(Library library, Node[] children) => new Selector(children);
    }
}