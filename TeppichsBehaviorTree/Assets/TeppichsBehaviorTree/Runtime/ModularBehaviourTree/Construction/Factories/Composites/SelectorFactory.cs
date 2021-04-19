using ModularBehaviourTree.Composites;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Composites
{   
    [CreateAssetMenu(fileName = "Selector", menuName = "ModularBehaviourTree/Composites/Selector")]
    internal class SelectorFactory : CompositeFactory
    {
        public override Node CreateNode() => new Selector(BuildNodes());
    }
}