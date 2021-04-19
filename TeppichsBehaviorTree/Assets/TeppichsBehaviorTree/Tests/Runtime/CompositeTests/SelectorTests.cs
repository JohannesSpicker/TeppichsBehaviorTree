using ModularBehaviourTree;
using ModularBehaviourTree.Composites;
using NUnit.Framework;
using TreeTickerSpace;
using UnityEngine;
using UnityEngine.AI;

namespace Tests.PrimitiveTests
{
    public class SelectorTests
    {
        [Test]
        public void SelectorShouldReturnRunningOnFirstTick()
        {
            Setup(out Blackboard context, out Selector sequence);

            Assert.True(sequence.Tick(context) == Node.NodeState.Running);

            CleanUp(context);
        }

        [Test]
        public void SelectorShouldReturnSuccessOnSecondTick()
        {
            Setup(out Blackboard context, out Selector sequence);

            sequence.Tick(context);

            Assert.True(sequence.Tick(context) == Node.NodeState.Success);

            CleanUp(context);
        }

        private static void Setup(out Blackboard blackboard, out Selector sequence)
        {
            sequence = new Selector(new Node[3]
            {
                new MockPrimitives.MockLeaf(), new MockPrimitives.MockLeaf(),
                new MockPrimitives.MockLeaf()
            });

            GameObject gameObject = new GameObject();
            blackboard = new Blackboard(gameObject.AddComponent<TreeTicker>(), gameObject.GetComponent<NavMeshAgent>(), gameObject.transform);
        }

        private static void CleanUp(Blackboard blackboard) => Object.DestroyImmediate(blackboard.treeTicker.gameObject);
    }
}