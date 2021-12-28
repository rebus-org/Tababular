using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tababular.Internals.Extractors;
using Tababular.Internals.TableModel;

namespace Tababular.Tests.Extractors;

[TestFixture]
public class TestObjectTableExtractor
{
    [Test]
    public void DoesNotDieOnEmptyArray()
    {
        var table = new ObjectTableExtractor(new object[0]).GetTable();

        Assert.That(table.Rows.Count, Is.EqualTo(0));
        Assert.That(table.Columns.Count, Is.EqualTo(0));
    }

    [Test]
    public void DoesNotDieOnArrayWithEmptyObject()
    {
        var table = new ObjectTableExtractor(new[]
        {
            new {},
            new {}
        }).GetTable();

        Assert.That(table.Rows.Count, Is.EqualTo(2));
        Assert.That(table.Columns.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetsColumnsAsExpected()
    {
        var objects = new[]
        {
            new
            {
                FirstColumn = "Value 1",
                SecondColumn = "Value 1"
            },
            new
            {
                FirstColumn = "Value 1",
                SecondColumn = "Value 1"
            }
        };

        var table = new ObjectTableExtractor(objects).GetTable();

        Assert.That(table.Columns.Select(c => c.Label), Is.EqualTo(new[] { "FirstColumn", "SecondColumn" }));
    }

    [Test]
    public void CanExtractValues_Strings()
    {
        var objects = new[]
        {
            new
            {
                col1 = "v1",
                col2 = "v2",
                col3 = "v3",
            }
        };

        var table = new ObjectTableExtractor(objects).GetTable();

        var row = table.Rows.Single();

        var cellTexts = row.GetAllCells().OrderBy(c => c.TextValue).Select(c => c.TextValue);

        Assert.That(cellTexts, Is.EqualTo(new[] { "v1", "v2", "v3" }));

    }

    [Test]
    public void CanExtractValues_Multiple()
    {
        var objects = new[]
        {
            new {col1 = new[] {"line1", "line2", "line3"}}
        };

        var table = new ObjectTableExtractor(objects).GetTable();

        var row = table.Rows.Single();

        var cellLines = row.GetAllCells().Single().Lines;

        Assert.That(cellLines, Is.EqualTo(new[] { "line1", "line2", "line3" }));

    }


    [Test]
    public void CanExtractListOfPrimitiveObjects()
    {
        var objects = new List<string> { "line1", "line2", "line3" };

        var table = new ObjectTableExtractor(objects).GetTable();

        Assert.That(table.Rows.Count, Is.EqualTo(3));
        Assert.That(table.Rows[0].GetAllCells().Single().Lines, Is.EqualTo(new[] { "line1" }));
        Assert.That(table.Rows[1].GetAllCells().Single().Lines, Is.EqualTo(new[] { "line2" }));
        Assert.That(table.Rows[2].GetAllCells().Single().Lines, Is.EqualTo(new[] { "line3" }));
    }

    [Test]
    public void ExtractsColumnsOrderedByHowCodeIsWritten()
    {
        var objects = new List<SampleRow>
        {
            new SampleRow("1", "2", "3", "4"),
            new SampleRow("10", "20", "30", "40"),
            new SampleRow("100", "200", "300", "400"),
        };

        var table = new ObjectTableExtractor(objects).GetTable();

        string[] GetCells(Row row) => row.GetAllCells().Select(cell => cell.TextValue).ToArray();

        Assert.That(table.Rows.Count, Is.EqualTo(3));
        Assert.That(GetCells(table.Rows[0]), Is.EqualTo(new[] { "1", "2", "3", "4" }));
        Assert.That(GetCells(table.Rows[1]), Is.EqualTo(new[] { "10", "20", "30", "40" }));
        Assert.That(GetCells(table.Rows[2]), Is.EqualTo(new[] { "100", "200", "300", "400" }));
    }

    class SampleRow
    {
        public SampleRow(string firstColumn, string secondColumn, string thirdColumn, string fourthColumn)
        {
            FirstColumn = firstColumn;
            SecondColumn = secondColumn;
            ThirdColumn = thirdColumn;
            FourthColumn = fourthColumn;
        }

        public string FirstColumn { get; }
        public string SecondColumn { get; }
        public string ThirdColumn { get; }
        public string FourthColumn { get; }
    }
}