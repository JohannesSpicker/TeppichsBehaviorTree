using System;
using System.Collections.Generic;
using TeppichsTools.Data;
using UnityEngine;

namespace ModularBehaviourTree.Conditions
{
    public class IsInRange : Condition
    {
        private readonly float range;

        public IsInRange(float range) { this.range = range; }

        protected override void    Initialise(Blackboard blackboard) { }
        protected override void    Terminate(Blackboard  blackboard) { }
        internal override  Memento BuildMemento()                    => throw new NotImplementedException();

        protected override bool Check(Blackboard blackboard) => blackboard.target != null
                                                                && Vector3.Distance(blackboard.Position(),
                                                                    blackboard.target.position) < range;
    }

    [Serializable]
    internal class IsInRangeMemento : Memento
    {
        public override Node BuildNode(Library library, List<Node> children) => new IsInRange(library.Read<float>("range"));
    }
}