using System;
using TeppichsTools.Data;
using TeppichsTools.Time;
using UnityEngine;

namespace ModularBehaviourTree.Leaves
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

        protected override void    Terminate(Blackboard blackboard) { }
        internal override  Memento BuildMemento()                   => new IdleMemento();
    }

    [Serializable]
    internal class IdleMemento : Memento
    {
        public override Node BuildNode(Library library, Node[] children) => new Idle(library.Read<float>("duration"));
    }
}