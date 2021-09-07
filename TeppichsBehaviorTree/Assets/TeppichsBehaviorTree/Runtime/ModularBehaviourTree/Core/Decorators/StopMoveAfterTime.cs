using System;
using System.Collections.Generic;
using TeppichsTools.Data;
using TeppichsTools.Time;
using UnityEngine;

namespace ModularBehaviourTree.Core.Decorators
{
    internal class StopMoveAfterTime : Decorator
    {
        private readonly Ticker ticker;

        public StopMoveAfterTime(Node node, float duration) : base(node) { ticker = new Ticker(duration); }

        protected override void Initialise(Blackboard blackboard) => ticker.Reset();

        protected override NodeState Continue(Blackboard blackboard)
        {
            if (ticker.Tick(Time.deltaTime))
                return NodeState.Failure;

            return node.Tick(blackboard);
        }

        protected override void Terminate(Blackboard blackboard) =>
            blackboard.navMeshAgent.SetDestination(blackboard.navMeshAgent.transform.position);

        internal override Memento BuildMemento() => new StopMoveAfterTimeMemento();
    }

    [Serializable]
    internal class StopMoveAfterTimeMemento : Memento
    {
        public override Node BuildNode(Library library, List<Node> children) =>
            new StopMoveAfterTime(children[0], library.Read<float>("duration"));
    }
}