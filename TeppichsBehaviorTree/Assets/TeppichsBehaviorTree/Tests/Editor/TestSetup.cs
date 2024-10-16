﻿using System.Collections.Generic;
using TeppichsBehaviorTree.Runtime.Core.Composites;
using TeppichsBehaviorTree.Runtime.Core.Decorators;
using TeppichsBehaviorTree.Runtime.Core.Primitives;
using TeppichsBehaviorTree.Runtime.ModularBehaviourTree.Core;
using Tests;
using UnityEngine;
using UnityEngine.AI;

namespace TeppichsBehaviorTree.Tests.Editor
{
    internal static class TestSetup
    {
        internal static void Setup(out Blackboard blackboard) => SetupBlackboard(out blackboard);

        internal static void Setup(out Blackboard blackboard, out MockPrimitives.MockLeaf leaf)
        {
            leaf = new MockPrimitives.MockLeaf();
            SetupBlackboard(out blackboard);
        }

        internal static void Setup(out Blackboard blackboard, out Decorator decorator)
        {
            decorator = new MockDecorator(new MockPrimitives.MockLeaf());
            SetupBlackboard(out blackboard);
        }

        internal static void Setup(out Blackboard blackboard, out Inverter inverter)
        {
            inverter = new Inverter(new MockPrimitives.MockLeaf());
            SetupBlackboard(out blackboard);
        }

        internal static void Setup(out Blackboard blackboard, out Selector selector)
        {
            selector = new Selector(new List<Node>
            {
                new MockPrimitives.MockLeaf(),
                new MockPrimitives.MockLeaf(),
                new MockPrimitives.MockLeaf()
            });

            SetupBlackboard(out blackboard);
        }

        internal static void Setup(out Blackboard blackboard, out Sequence sequence)
        {
            sequence = new Sequence(new List<Node>
            {
                new MockPrimitives.MockLeaf(),
                new MockPrimitives.MockLeaf(),
                new MockPrimitives.MockLeaf()
            });

            SetupBlackboard(out blackboard);
        }

        private static void SetupBlackboard(out Blackboard blackboard) =>
            blackboard = new Blackboard(new GameObject().AddComponent<NavMeshAgent>());

        internal static void CleanUp(Blackboard blackboard) => Object.DestroyImmediate(blackboard.transform.gameObject);
    }
}