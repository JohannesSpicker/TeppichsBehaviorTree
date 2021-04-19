using ModularBehaviourTree.Conditions;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Conditions
{
    [CreateAssetMenu(fileName = "FindTarget", menuName = "ModularBehaviourTree/Conditions/FindTarget")]
    internal class FindTargetFactory : NodeFactory
    {
        [SerializeField] private float range = 2f;

        public override Node CreateNode() => new FindTarget(range);
    }
}