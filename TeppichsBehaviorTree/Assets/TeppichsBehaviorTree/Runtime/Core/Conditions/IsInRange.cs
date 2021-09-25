using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using UnityEngine;

namespace TeppichsBehaviorTree.Runtime.Core.Conditions
{
    public class IsInRange : Condition
    {
        private readonly float range;

        public IsInRange(float range) { this.range = range; }

        protected override void Initialise(Blackboard blackboard) { }
        protected override void Terminate(Blackboard  blackboard) { }

        protected override bool Check(Blackboard blackboard) => blackboard.target != null
                                                                && Vector3.Distance(blackboard.Position(),
                                                                    blackboard.target.position) < range;
    }
}