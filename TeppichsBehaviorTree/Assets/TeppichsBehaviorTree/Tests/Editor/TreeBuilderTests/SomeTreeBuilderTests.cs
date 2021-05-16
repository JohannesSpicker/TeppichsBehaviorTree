using NUnit.Framework;
using UnityEngine;
using System;
using System.Reflection;
using TeppichsBehaviorTree.Editor.Tutorial;

namespace Tests.TreeBuilderTests
{
    public class SomeTreeBuilderTests
    {
        [Test]
        public void SomeFalseTest() => Assert.True(false);

        [Test]
        public void SomeTrueTest() => Assert.True(true);

        [Test]
        public void TestingReflection()
        {
            TreeBuilderNodeFactory.SomeFunc();
            Assert.True(true);
        }
    }
}