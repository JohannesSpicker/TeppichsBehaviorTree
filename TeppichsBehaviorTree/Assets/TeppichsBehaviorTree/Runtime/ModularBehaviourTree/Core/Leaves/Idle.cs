using System;
using Tools;
using UnityEngine;

namespace ModularBehaviourTree.Leaves
{
    internal class Idle : Leaf
    {
        private Ticker ticker;

        public Idle(float                               duration) { ticker = new Ticker(duration); }
        protected override void      Initialise(Blackboard blackboard) => ticker.Reset();

        protected override NodeState Continue(Blackboard blackboard)
        {
            if (ticker.Tick(Time.deltaTime))
                return NodeState.Success;

            return NodeState.Running;
        }
        protected override void      Terminate(Blackboard  blackboard) {}
    }
}