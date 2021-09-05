using System;
using TeppichsTools.Data;
using UnityEngine;

namespace ModularBehaviourTree.Conditions
{
    internal class FindTarget : Condition
    {
        private readonly float range;
        public FindTarget(float range) { this.range = range; }

        protected override void    Initialise(Blackboard blackboard) { }
        protected override void    Terminate(Blackboard  blackboard) { }
        internal override  Memento BuildMemento()                    => throw new NotImplementedException();

        protected override bool Check(Blackboard blackboard) => blackboard.target = GetFirstBestTarget(blackboard);

        private Transform GetFirstBestTarget(Blackboard blackboard)
        {
            Vector3 position = blackboard.Position();

            Collider[] candidates = Physics.OverlapSphere(position, range);

            for (int i = 0; i < candidates.Length; i++)
                if (candidates[i].transform != blackboard.transform)
                    return candidates[i].transform;

            return candidates[0]?.transform;
        }
    }

    [Serializable]
    internal class FindTargetMemento : Memento
    {
        public override Node BuildNode(Library library, Node[] children) =>
            new FindTarget(library.Read<float>("range"));
    }
}