using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Decorators
{
    internal abstract class DecoratorFactory: NodeFactory
    {
        public NodeFactory nodeFactory;
    }
}