using ModularBehaviourTree;
using NUnit.Framework;
using TreeTickerSpace;
using UnityEngine;
using UnityEngine.AI;

namespace Tests.PrimitiveTests
{
    public class DecoratorTests
    {
        [Test]
        public void SomeDecoratorTest()
        {
            //Setup();
        }

        private static void Setup(out Blackboard blackboard, out MockDecorator decorator)
        {
            decorator = new MockDecorator(new MockPrimitives.MockLeaf());

            GameObject gameObject = new GameObject();
            blackboard = new Blackboard(gameObject.AddComponent<TreeTicker>(), gameObject.AddComponent<NavMeshAgent>(), gameObject.transform);
        }

        private static void CleanUp(Blackboard blackboard) => Object.DestroyImmediate(blackboard.treeTicker.gameObject);
    }
}