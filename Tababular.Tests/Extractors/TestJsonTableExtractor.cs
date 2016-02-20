using NUnit.Framework;
using Tababular.Internals.Extractors;

namespace Tababular.Tests.Extractors
{
    [TestFixture]
    public class TestJsonTableExtractor
    {
        [Test]
        public void DoesNotDieOnEmptyObject()
        {
            var table = new JsonTableExtractor("{}").GetTable();

            Assert.That(table.Rows.Count, Is.EqualTo(1));
            Assert.That(table.Columns.Count, Is.EqualTo(0));
        }

        [Test]
        public void DoesNotDieOnEmptyArray()
        {
            var table = new JsonTableExtractor("[]").GetTable();

            Assert.That(table.Rows.Count, Is.EqualTo(0));
            Assert.That(table.Columns.Count, Is.EqualTo(0));
        }

        [Test]
        public void DoesNotDieOnArrayWithEmptyObject()
        {
            var table = new JsonTableExtractor("[{}, {}]").GetTable();

            Assert.That(table.Rows.Count, Is.EqualTo(2));
            Assert.That(table.Columns.Count, Is.EqualTo(0));
        }
    }
}