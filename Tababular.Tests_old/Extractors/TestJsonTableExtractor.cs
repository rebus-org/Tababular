using System.Linq;
using NUnit.Framework;
using Tababular.Internals.Extractors;

namespace Tababular.Tests.Extractors
{
    [TestFixture]
    public class TestJsonTableExtractor
    {
        [Test]
        public void CanExtractFromJsonlToo()
        {
            var jsonObjects = @"{""First column"": ""Value 1"", ""Second column"": ""Value 1""}
{""First column"": ""Value 1"", ""Second column"": ""Value 1""}";

            var table = new JsonTableExtractor(jsonObjects).GetTable();

            Assert.That(table.Rows.Count, Is.EqualTo(2));
            Assert.That(table.Columns.Count, Is.EqualTo(2));
        }

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

        [Test]
        public void GetsColumnsAsExpected()
        {
            var jsonObjects = @"
[
    {
        ""First column"": ""Value 1"",
        ""Second column"": ""Value 1""
    },
    {
        ""First column"": ""Value 1"",
        ""Second column"": ""Value 1""
    }
]

";

            var table = new JsonTableExtractor(jsonObjects).GetTable();

            Assert.That(table.Columns.Select(c => c.Label), Is.EqualTo(new[] { "First column", "Second column" }));
        }

        [Test]
        public void CanExtractValues_Strings()
        {
            var json = @"[{""col1"": ""v1"", ""col2"": ""v2"", ""col3"": ""v3""}]";

            var table = new JsonTableExtractor(json).GetTable();

            var row = table.Rows.Single();

            var cellTexts = row.GetAllCells().OrderBy(c => c.TextValue).Select(c => c.TextValue);

            Assert.That(cellTexts, Is.EqualTo(new[] { "v1", "v2", "v3" }));

        }

        [Test]
        public void CanExtractValues_Multiple()
        {
            var json = @"{""col1"": [""line1"", ""line2"", ""line3""]}";

            var table = new JsonTableExtractor(json).GetTable();

            var row = table.Rows.Single();

            var cellLines = row.GetAllCells().Single().Lines;

            Assert.That(cellLines, Is.EqualTo(new[] { "line1", "line2", "line3" }));

        }
    }
}