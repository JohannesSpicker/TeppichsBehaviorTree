using NUnit.Framework;
using TeppichsBehaviorTree.Editor.Tutorial;

namespace Tests.TreeBuilderTests
{
    public class SomeTreeBuilderTests
    {
        [Test]
        public void ReflectionThrowsNoError()
        {
            TreeBuilderNodeFactory.SomeFunc();
            Assert.True(true);
        }

        [Test]
        public void ShouldCreateSaveLoadEmptyGraphWithoutError()
        {
            Assert.True(false);
        }
    }
}