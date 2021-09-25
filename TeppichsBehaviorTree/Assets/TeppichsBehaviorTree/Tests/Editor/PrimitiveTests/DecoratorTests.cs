using NUnit.Framework;
using TeppichsBehaviorTree.Runtime.Core.Decorators;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using TeppichsBehaviorTree.Tests.Editor;

namespace Tests.PrimitiveTests
{
    public class DecoratorTests
    {
        [Test]
        public void DecoratorShouldReturnRunningOnFirstTick()
        {
            TestSetup.Setup(out Blackboard blackboard, out Decorator decorator);

            Node.NodeState state = decorator.Tick(blackboard);

            Assert.True(state is Node.NodeState.Running);

            TestSetup.CleanUp(blackboard);
        }

        [Test]
        public void DecoratorShouldReturnSuccessOnSecondTick()
        {
            TestSetup.Setup(out Blackboard blackboard, out Decorator decorator);

            decorator.Tick(blackboard);
            Node.NodeState state = decorator.Tick(blackboard);

            Assert.True(state is Node.NodeState.Success);

            TestSetup.CleanUp(blackboard);
        }

        [Test]
        public void InverterShouldReturnRunningOnFirstTick()
        {
            TestSetup.Setup(out Blackboard blackboard, out Inverter inverter);

            Node.NodeState state = inverter.Tick(blackboard);

            Assert.True(state is Node.NodeState.Running);

            TestSetup.CleanUp(blackboard);
        }

        [Test]
        public void InverterShouldReturnFailureOnSecondTick()
        {
            TestSetup.Setup(out Blackboard blackboard, out Inverter inverter);

            inverter.Tick(blackboard);
            Node.NodeState state = inverter.Tick(blackboard);

            Assert.True(state is Node.NodeState.Failure);

            TestSetup.CleanUp(blackboard);
        }
    }
}