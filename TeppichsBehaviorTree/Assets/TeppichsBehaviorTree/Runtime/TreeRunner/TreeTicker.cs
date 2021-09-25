using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using UnityEngine;
using UnityEngine.AI;

namespace TreeTickerSpace
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class TreeTicker : MonoBehaviour
    {
        private Node behaviourTree;

        private Blackboard blackboard;

        private void Awake() => blackboard = new Blackboard(GetComponent<NavMeshAgent>());

        private void Update() => behaviourTree?.Tick(blackboard);

        private void InsertNode(Node node) => behaviourTree = node;
    }
}