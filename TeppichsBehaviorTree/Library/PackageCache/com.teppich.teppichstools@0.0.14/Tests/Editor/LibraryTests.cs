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
        public void ShouldWriteAndReadValuesOfSameType()
        {
            Setup(out Library library);

            string stringIdOne    = "StringTestIdOne";
            string stringValueOne = "StringTestValueOne";

            string stringIdTwo    = "StringTestIdTwo";
            string stringValueTwo = "StringTestValueTwo";

            library.Write(stringIdOne, stringValueOne);
            library.Write(stringIdTwo, stringValueTwo);

            Assert.AreEqual((stringValueOne, stringValueTwo),
                            (library.Read<string>(stringIdOne), library.Read<string>(stringIdTwo)));
        }

        [Test]
        public void ShouldReturnNullOnWrongID()
        {
            Setup(out Library library);

            Assert.IsNull(library.Read<string>("id"));
        }

        [Test]
        public void DeletedStringShouldBeNull()
        {
            Setup(out Library library);

            string stringId    = "StringTestId";
            string stringValue = "StringTestValue";

            library.Write(stringId, stringValue);
            library.Delete<string>(stringId);

            Assert.IsNull(library.Read<string>(stringId));
        }

        [Test]
        public void DeletedIntShouldBeNull()
        {
            Setup(out Library library);

            string intId    = "IntTestName";
            int    intValue = 42;

            library.Write(intId, intValue);
            library.Delete<int>(intId);

            Assert.IsNull(library.Read<string>(intId));
        }

        private void Setup(out Library library) => library = new Library();
    }
}