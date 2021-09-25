using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.Core.Leaves
{
    internal class MoveToTarget : Leaf
    {
        private readonly float range;
        public MoveToTarget(float range = 6) { this.range = range; }

        protected override void Initialise(Blackboard blackboard)
        {
            if (HasTarget(blackboard))
                blackboard.navigation.SetDestination(blackboard.target.position);
        }

        protected override NodeState Continue(Blackboard blackboard) => !HasTarget(blackboard) ? NodeState.Failure :
            IsInRange(blackboard, range) ? NodeState.Success : NodeState.Running;

        protected override void Terminate(Blackboard blackboard) =>
            blackboard.navMeshAgent.SetDestination(blackboard.navMeshAgent.transform.position);

        private static bool HasTarget(Blackboard blackboard) => blackboard.target;

        private static bool IsInRange(Blackboard blackboard, float range) =>
            Vector3.Distance(blackboard.navMeshAgent.transform.position, blackboard.target.position) < range;
    }
}