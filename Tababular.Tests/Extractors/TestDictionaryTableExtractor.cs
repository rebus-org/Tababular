using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tababular.Internals.Extractors;

namespace Tababular.Tests.Extractors;

[TestFixture]
public class TestDictionaryTableExtractor
{
    [Test]
    public void DoesNotDieOnEmptyArray()
    {
        var table = new DictionaryTableExtractor(Enumerable.Empty<IDictionary<string, object>>()).GetTable();

        Assert.That(table.Rows.Count, Is.EqualTo(0));
        Assert.That(table.Columns.Count, Is.EqualTo(0));
    }

    [Test]
    public void DoesNotDieOnArrayWithEmptyObject()
    {
        var table = new DictionaryTableExtractor(new[]
        {
            new Dictionary<string, object>(),
            new Dictionary<string, object>()
        }).GetTable();

        Assert.That(table.Rows.Count, Is.EqualTo(2));
        Assert.That(table.Columns.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetsColumnsAsExpected()
    {
        var dictionaries = new[]
        {
            new Dictionary<string, object>
            {
                { "First column", "Value 1" },
                { "Second column", "Value 1" },
            },
            new Dictionary<string, object>
            {
                { "First column", "Value 1" },
                { "Second column", "Value 1" },
            }
        };

        var table = new DictionaryTableExtractor(dictionaries).GetTable();

        Assert.That(table.Columns.Select(c => c.Label), Is.EqualTo(new[] { "First column", "Second column" }));
    }

    [Test]
    public void CanExtractValues_Strings()
    {
        var dictionaries = new[]
        {
            new Dictionary<string, object>
            {
                { "col1", "v1" },
                { "col2", "v2" },
                { "col3", "v3" }
            }
        };

        var table = new DictionaryTableExtractor(dictionaries).GetTable();

        var row = table.Rows.Single();

        var cellTexts = row.GetAllCells().OrderBy(c => c.TextValue).Select(c => c.TextValue);

        Assert.That(cellTexts, Is.EqualTo(new[] {"v1", "v2", "v3"}));

    }

    [Test]
    public void CanExtractValues_Multiple()
    {
        var dictionaries = new[]
        {
            new Dictionary<string, object>
            {
                { "col1", new[] {"line1", "line2", "line3"} }
            }
        };

        var table = new DictionaryTableExtractor(dictionaries).GetTable();

        var row = table.Rows.Single();

        var cellLines = row.GetAllCells().Single().Lines;

        Assert.That(cellLines, Is.EqualTo(new[] {"line1", "line2", "line3"}));

    }
}