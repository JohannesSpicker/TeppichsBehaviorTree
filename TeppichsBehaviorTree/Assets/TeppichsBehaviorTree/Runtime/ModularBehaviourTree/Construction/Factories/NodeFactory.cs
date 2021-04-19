using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories
{
    public abstract class NodeFactory : ScriptableObject
    {
        public abstract Node CreateNode();
    }
}