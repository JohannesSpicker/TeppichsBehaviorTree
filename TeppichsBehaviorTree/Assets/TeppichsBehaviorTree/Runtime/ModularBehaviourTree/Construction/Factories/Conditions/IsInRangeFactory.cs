using ModularBehaviourTree.Conditions;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Conditions
{
    [CreateAssetMenu(fileName = "IsInRange", menuName = "ModularBehaviourTree/Conditions/IsInRange")]
    internal class IsInRangeFactory : NodeFactory
    {
        [SerializeField] private float range = 2f;

        public override Node CreateNode() => new IsInRange(range);
    }
}