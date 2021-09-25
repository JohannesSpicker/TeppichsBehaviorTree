using NUnit.Framework;
using TeppichsBehaviorTree.Runtime.Core.Composites;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using TeppichsBehaviorTree.Tests.Editor;

namespace Tests.PrimitiveTests
{
    public class SequenceTests
    {
        [Test]
        public void SequenceShouldReturnRunningOnFirstThreeTicks([Values(0, 1, 2)] int extraTicks)
        {
            TestSetup.Setup(out Blackboard context, out Sequence sequence);

            for (int i = 0; i < extraTicks; i++)
                sequence.Tick(context);

            Assert.True(sequence.Tick(context) == Node.NodeState.Running);

            TestSetup.CleanUp(context);
        }

        [Test]
        public void SequenceShouldReturnSuccessOnFourthTick()
        {
            TestSetup.Setup(out Blackboard context, out Sequence sequence);

            for (int i = 0; i < 3; i++)
                sequence.Tick(context);

            Assert.True(sequence.Tick(context) == Node.NodeState.Success);

            TestSetup.CleanUp(context);
        }
    }
}