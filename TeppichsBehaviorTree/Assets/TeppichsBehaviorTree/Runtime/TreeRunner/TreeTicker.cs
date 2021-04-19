using ModularBehaviourTree;
using ModularBehaviourTree.Construction.Factories;
using UnityEngine;
using UnityEngine.AI;

namespace TreeTickerSpace
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class TreeTicker : MonoBehaviour
    {
        [SerializeField] private NodeFactory nodeFactory;
        private                  Node        behaviourTree;

        private Blackboard blackboard;

        private void Awake()
        {
            blackboard = new Blackboard(GetComponent<NavMeshAgent>());

            behaviourTree = nodeFactory.CreateNode();
        }

        private void Update() => behaviourTree.Tick(blackboard);
    }
}