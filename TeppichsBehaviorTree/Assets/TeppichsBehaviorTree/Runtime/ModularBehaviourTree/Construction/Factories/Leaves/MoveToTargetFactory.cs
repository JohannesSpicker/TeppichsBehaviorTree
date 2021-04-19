using ModularBehaviourTree.Leaves;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Leaves
{
    [CreateAssetMenu(fileName = "MoveToTarget", menuName = "ModularBehaviourTree/Leaves/MoveToTarget")]
    internal class MoveToTargetFactory : NodeFactory
    {
        [SerializeField] private float range;

        public override Node CreateNode() => new MoveToTarget(range);
    }
}