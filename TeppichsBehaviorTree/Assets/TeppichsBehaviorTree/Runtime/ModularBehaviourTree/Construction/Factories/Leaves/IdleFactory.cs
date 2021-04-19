using ModularBehaviourTree.Leaves;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Leaves
{
    [CreateAssetMenu(fileName = "Idle", menuName = "ModularBehaviourTree/Leaves/Idle")]
    internal class IdleFactory : NodeFactory
    {
        [SerializeField] private float duration;

        public override Node CreateNode() => new Idle(duration);
    }
}