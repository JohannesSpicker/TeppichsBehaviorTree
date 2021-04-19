using UnityEngine;
using UnityEngine.AI;

namespace Pawns
{
    public class Navigation
    {
        private NavMeshAgent navMeshAgent;

        public Navigation(NavMeshAgent navMeshAgent) { this.navMeshAgent = navMeshAgent; }

        public bool SetDestination(Vector3 target) => navMeshAgent.SetDestination(target);
        public bool Warp(Vector3           target) => navMeshAgent.Warp(target);
        public void Stop()                         => navMeshAgent.isStopped = true;
    }
}