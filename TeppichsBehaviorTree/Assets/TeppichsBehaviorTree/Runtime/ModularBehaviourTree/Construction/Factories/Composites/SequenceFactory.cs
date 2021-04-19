using ModularBehaviourTree.Composites;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Composites
{
    [CreateAssetMenu(fileName = "Sequence", menuName = "ModularBehaviourTree/Composites/Sequence")]
    internal class SequenceFactory : CompositeFactory
    {
        public override Node CreateNode() => new Sequence(BuildNodes());
    }
}