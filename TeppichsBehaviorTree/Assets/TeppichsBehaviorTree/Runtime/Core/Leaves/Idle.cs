using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using TeppichsTools.Time;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.Core.Leaves
{
    internal class Idle : Leaf
    {
        private Ticker ticker;

        public Idle(float duration) { InitializeTicker(duration); }

        private void InitializeTicker(float duration) => ticker = new Ticker(duration);

        protected override void Initialise(Blackboard blackboard) => ticker.Reset();

        protected override NodeState Continue(Blackboard blackboard)
        {
            if (ticker.Tick(Time.deltaTime))
                return NodeState.Success;

            return NodeState.Running;
        }

        protected override void Terminate(Blackboard blackboard) { }
    }
}