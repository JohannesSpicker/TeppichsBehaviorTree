using System;
using System.Collections.Generic;
using TeppichsTools.Data;
using UnityEngine;

namespace ModularBehaviourTree.Leaves
{
    internal class MoveToTarget : Leaf
    {
        private readonly float range;
        public MoveToTarget() { }
        public MoveToTarget(float range) { this.range = range; }

        protected override void Initialise(Blackboard blackboard)
        {
            if (HasTarget(blackboard))
                blackboard.navigation.SetDestination(blackboard.target.position);
        }

        protected override NodeState Continue(Blackboard blackboard)
        {
            if (!HasTarget(blackboard))
                return NodeState.Failure;

            if (Vector3.Distance(blackboard.navMeshAgent.transform.position, blackboard.target.position) < range)
                return NodeState.Success;

            return NodeState.Running;
        }

        protected override void Terminate(Blackboard blackboard) =>
            blackboard.navMeshAgent.SetDestination(blackboard.navMeshAgent.transform.position);

        internal override Memento BuildMemento() => new MoveToTargetMemento();

        private bool HasTarget(Blackboard blackboard) => blackboard.target;
    }

    [Serializable]
    internal class MoveToTargetMemento : Memento
    {
        public override Node BuildNode(Library library, List<Node> children) =>
            new MoveToTarget(library.Read<float>("range"));
    }
}