using ModularBehaviourTree;
using NUnit.Framework;

namespace Tests
{
    public class MockPrimitives
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BehaviourTreeTestsSimplePasses() => Assert.IsTrue(true);

        //Mocks
        internal class MockLeaf : Leaf
        {
            private int tickCounter;

            protected override void Initialise(Blackboard blackboard) => tickCounter = 0;

            protected override NodeState Continue(Blackboard blackboard) =>
                0 < tickCounter++ ? NodeState.Success : NodeState.Running;

            protected override void Terminate(Blackboard blackboard) { }
        }
    }

    internal class MockDecorator : Decorator
    {
        public MockDecorator(Node                  node) : base(node) { }
        protected override void Initialise(Blackboard blackboard) { }
        protected override void Terminate(Blackboard  blackboard) { }
    }

    internal class MockCondition : Condition
    {
        protected override void      Initialise(Blackboard blackboard) { }
        protected override bool      Check(Blackboard      blackboard) => true;
        protected override NodeState Continue(Blackboard   blackboard) => NodeState.Success;
        protected override void      Terminate(Blackboard  blackboard) { }
    }

    internal class MockComposite : Composite
    {
        public MockComposite(Node[]                     nodes) : base(nodes) { }
        protected override void      Initialise(Blackboard blackboard) { }
        protected override NodeState Continue(Blackboard   blackboard) => NodeState.Success;
        protected override void      Terminate(Blackboard  blackboard) { }
    }
}