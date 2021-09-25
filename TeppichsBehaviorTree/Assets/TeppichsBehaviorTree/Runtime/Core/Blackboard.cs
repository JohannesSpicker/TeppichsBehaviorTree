using Pawns;
using UnityEngine;
using UnityEngine.AI;

namespace TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core
{
    public class Blackboard
    {
        public readonly Navigation   navigation;
        public readonly NavMeshAgent navMeshAgent;

        public readonly Transform transform;

        public Transform target;

        public Blackboard(NavMeshAgent navMeshAgent)
        {
            transform         = navMeshAgent.transform;
            this.navMeshAgent = navMeshAgent;
            navigation        = new Navigation(navMeshAgent);
        }

        public Vector3 Position() => transform.position;
    }
}