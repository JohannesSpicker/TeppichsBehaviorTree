using ModularBehaviourTree.Leaves;
using UnityEngine;

namespace ModularBehaviourTree.Construction.Factories.Leaves
{
    [CreateAssetMenu(fileName = "MoveForward", menuName = "ModularBehaviourTree/Leaves/MoveForward")]
    public class MoveForwardFactory : NodeFactory
    {
        public override Node CreateNode() => new MoveForward();
    }
}