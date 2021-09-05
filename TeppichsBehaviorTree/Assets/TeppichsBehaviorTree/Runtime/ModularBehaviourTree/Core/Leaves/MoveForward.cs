using System;
using TeppichsTools.Data;
using UnityEngine;

namespace ModularBehaviourTree.Leaves
{
    internal class MoveForward : Leaf
    {
        private const float failureMargin = 0.2f;

        protected override void Initialise(Blackboard blackboard) =>
            blackboard.navMeshAgent.SetDestination(blackboard.navMeshAgent.transform.position + Vector3.forward);

        protected override NodeState Continue(Blackboard blackboard) =>
            Vector3.Distance(blackboard.navMeshAgent.destination, blackboard.navMeshAgent.transform.position)
            < failureMargin
                ? NodeState.Success
                : NodeState.Running;

        protected override void Terminate(Blackboard blackboard) =>
            blackboard.navMeshAgent.SetDestination(blackboard.navMeshAgent.transform.position);

        internal override Memento BuildMemento() => throw new NotImplementedException();
    }

    [Serializable]
    internal class MoveForwardMemento : Memento
    {
        public override Node BuildNode(Library library, Node[] children) => new MoveForward();
    }
}