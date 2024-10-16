﻿using NUnit.Framework;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using TeppichsBehaviorTree.Tests.Editor;

namespace Tests.PrimitiveTests
{
    public class LeafTests
    {
        [Test]
        public void LeafShouldReturnRunningOnFirstTick()
        {
            TestSetup.Setup(out Blackboard blackboard, out MockPrimitives.MockLeaf leaf);

            Node.NodeState state = leaf.Tick(blackboard);

            Assert.True(state is Node.NodeState.Running);

            TestSetup.CleanUp(blackboard);
        }

        [Test]
        public void LeafShouldReturnSuccessOnSecondTick()
        {
            TestSetup.Setup(out Blackboard blackboard, out MockPrimitives.MockLeaf leaf);

            leaf.Tick(blackboard);
            Node.NodeState state = leaf.Tick(blackboard);

            Assert.True(state is Node.NodeState.Success);

            TestSetup.CleanUp(blackboard);
        }
    }
}