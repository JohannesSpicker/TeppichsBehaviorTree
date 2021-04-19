using Pawns;
using UnityEngine;
using UnityEngine.AI;

namespace ModularBehaviourTree
{
    public class Blackboard
    {
        public readonly NavMeshAgent navMeshAgent;
        public readonly Navigation   navigation;
        
        public readonly MonoBehaviour treeTicker;

        public Transform target;

        private Transform transform;

        public Blackboard(MonoBehaviour treeTicker, NavMeshAgent navMeshAgent, Transform target)
        {
            this.treeTicker   = treeTicker;
            this.transform    = treeTicker.transform;
            this.navMeshAgent = navMeshAgent;
            this.navigation   = new Navigation(navMeshAgent);
            this.target       = target;
        }

        public Vector3 Position() => transform.position;
    }
}