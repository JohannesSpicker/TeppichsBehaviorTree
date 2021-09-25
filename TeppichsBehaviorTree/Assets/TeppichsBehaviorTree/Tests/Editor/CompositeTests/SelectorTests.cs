using NUnit.Framework;
using TeppichsBehaviorTree.Runtime.Core.Composites;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using TeppichsBehaviorTree.Tests.Editor;

namespace Tests.PrimitiveTests
{
    public class SelectorTests
    {
        [Test]
        public void SelectorShouldReturnRunningOnFirstTick()
        {
            TestSetup.Setup(out Blackboard context, out Selector selector);

            Assert.True(selector.Tick(context) == Node.NodeState.Running);

            TestSetup.CleanUp(context);
        }

        [Test]
        public void SelectorShouldReturnSuccessOnSecondTick()
        {
            TestSetup.Setup(out Blackboard context, out Selector selector);

            selector.Tick(context);

            Assert.True(selector.Tick(context) == Node.NodeState.Success);

            TestSetup.CleanUp(context);
        }
    }
}