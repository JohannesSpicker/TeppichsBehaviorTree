using ModularBehaviourTree.Core.Decorators;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Decorators
{
    [CreateAssetMenu(fileName = "Inverter", menuName = "ModularBehaviourTree/Decorators/Inverter")]
    internal class InverterFactory: DecoratorFactory
    {
        public override Node CreateNode() => new Inverter(nodeFactory.CreateNode());
    }
}