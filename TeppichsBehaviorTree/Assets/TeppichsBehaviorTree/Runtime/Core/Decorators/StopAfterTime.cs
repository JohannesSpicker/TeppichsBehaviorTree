using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using TeppichsTools.Time;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.Core.Decorators
{
    internal class StopAfterTime : Decorator
    {
        private readonly Ticker ticker;
        public StopAfterTime(Node node, float duration) : base(node) { ticker = new Ticker(duration); }

        protected override void Initialise(Blackboard blackboard) => ticker.Reset();

        protected override NodeState Continue(Blackboard blackboard) =>
            ticker.Tick(Time.deltaTime) ? NodeState.Failure : node.Tick(blackboard);

        protected override void Terminate(Blackboard blackboard) =>
            blackboard.navMeshAgent.SetDestination(blackboard.navMeshAgent.transform.position);
    }
}