using ModularBehaviourTree;
using ModularBehaviourTree.Composites;
using NUnit.Framework;
using TreeTickerSpace;
using UnityEngine;
using UnityEngine.AI;

namespace Tests.PrimitiveTests
{
    public class SequenceTests
    {
        [Test]
        public void SequenceShouldReturnRunningOnFirstThreeTicks([Values(0, 1, 2)] int extraTicks)
        {
            Setup(out Blackboard context, out Sequence sequence);

            for (int i = 0; i < extraTicks; i++)
                sequence.Tick(context);

            Assert.True(sequence.Tick(context) == Node.NodeState.Running);

            CleanUp(context);
        }

        [Test]
        public void SequenceShouldReturnSuccessOnFourthTick()
        {
            Setup(out Blackboard context, out Sequence sequence);

            for (int i = 0; i < 3; i++)
                sequence.Tick(context);

            Assert.True(sequence.Tick(context) == Node.NodeState.Success);

            CleanUp(context);
        }

        private static void Setup(out Blackboard blackboard, out Sequence sequence)
        {
            Node[] nodes = new Node[3]
            {
                new MockPrimitives.MockLeaf(), new MockPrimitives.MockLeaf(), new MockPrimitives.MockLeaf()
            };

            sequence = new Sequence(nodes);

            GameObject gameObject = new GameObject();

            blackboard = new Blackboard(gameObject.AddComponent<TreeTicker>(), gameObject.GetComponent<NavMeshAgent>(),
                                  gameObject.transform);
        }

        private static void CleanUp(Blackboard blackboard) => Object.DestroyImmediate(blackboard.treeTicker.gameObject);
    }
}