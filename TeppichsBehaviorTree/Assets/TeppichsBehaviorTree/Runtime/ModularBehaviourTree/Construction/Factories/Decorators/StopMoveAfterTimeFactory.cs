using ModularBehaviourTree.Core.Decorators;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Decorators
{
    [CreateAssetMenu(fileName = "StopMoveAfterTime", menuName = "ModularBehaviourTree/Decorators/StopMoveAfterTime")]
    internal class StopMoveAfterTimeFactory : DecoratorFactory
    {
        [SerializeField] private float duration = 2f;

        public override Node CreateNode() => new StopMoveAfterTime(nodeFactory.CreateNode(), duration);
    }
}