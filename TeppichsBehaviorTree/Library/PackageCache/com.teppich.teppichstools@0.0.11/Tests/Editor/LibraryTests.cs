using NUnit.Framework;
using TeppichsTools.Data;

namespace TeppichsTools.Editor.Tests
{
    public class LibraryTests
    {
        [Test]
        public void ShouldWriteAndReadValue()
        {
            Setup(out Library library);

            string id    = "TestName";
            string value = "TestValue";

            library.Write(id, value);

            Assert.AreEqual(value, library.Read<string>(id));
        }

        [Test]
        public void ShouldWriteAndReadValuesOfDifferentTypes()
        {
            Setup(out Library library);

            string stringId    = "StringTestId";
            string stringValue = "StringTestValue";

            string intId    = "IntTestName";
            int    intValue = 42;

            library.Write(stringId, stringValue);
            library.Write(intId,    intValue);

            Assert.AreEqual((stringValue, intValue), (library.Read<string>(stringId), library.Read<int>(intId)));
        }

        [Test]
        public void ShouldReturnNullOnWrongID()
        {
            Setup(out Library library);
            
            Assert.IsNull(library.Read<string>("id"));
        }

        private void Setup(out Library library) => library = new Library();
    }
}