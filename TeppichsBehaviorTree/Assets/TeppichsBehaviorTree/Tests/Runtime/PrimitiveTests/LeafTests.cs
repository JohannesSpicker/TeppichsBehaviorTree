using ModularBehaviourTree;
using NUnit.Framework;
using TreeTickerSpace;
using UnityEngine;
using UnityEngine.AI;

namespace Tests.PrimitiveTests
{
    public class LeafTests
    {
        [Test]
        public void FirstTickIsRunning()
        {
            Setup(out Blackboard context, out MockPrimitives.MockLeaf leaf);

            Node.NodeState state = leaf.Tick(context);

            Assert.True(state is Node.NodeState.Running);

            CleanUp(context, leaf);
        }

        [Test]
        public void SecondTickIsSuccess()
        {
            Setup(out Blackboard context, out MockPrimitives.MockLeaf leaf);

            leaf.Tick(context);
            Node.NodeState state = leaf.Tick(context);

            Assert.True(state is Node.NodeState.Success);

            CleanUp(context, leaf);
        }

        private static void Setup(out Blackboard blackboard, out MockPrimitives.MockLeaf leaf)
        {
            leaf = new MockPrimitives.MockLeaf();

            GameObject gameObject = new GameObject();
            blackboard = new Blackboard(gameObject.AddComponent<TreeTicker>(), gameObject.GetComponent<NavMeshAgent>(), gameObject.transform);
        }

        private static void CleanUp(Blackboard blackboard, MockPrimitives.MockLeaf leaf) =>
            Object.DestroyImmediate(blackboard.treeTicker.gameObject);
    }
}