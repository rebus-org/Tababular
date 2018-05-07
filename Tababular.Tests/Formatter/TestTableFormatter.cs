using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tababular.Internals.TableModel;
using Tababular.Tests.Ex;

namespace Tababular.Tests.Formatter
{
    [TestFixture]
    public class TestTableFormatter
    {
        [Test]
        public void CanTakeAHint()
        {
            var column = new Column("Just a colum");
            var row = new Row();
            row.AddCell(column, new Cell(new string('*', 80)));
            var table = new Table(new List<Column> {column}, new List<Row> {row});
            var text = TableFormatter.FormatTable(table, new Hints { MaxTableWidth = 50 });

            Console.WriteLine(text);
            
            Assert.That(text.Normalized(), Is.EqualTo(@"

+---------------------------------------------+
| Just a colum                                |
+---------------------------------------------+
| ******************************************* |
| *************************************       |
+---------------------------------------------+

".Normalized()));

//            Assert.That(text.Normalized(), Is.EqualTo(@"

//===============================================
//| Just a colum                                |
//===============================================
//| ******************************************* |
//| *************************************       |
//===============================================

//".Normalized()));

        }

        [Test]
        public void NoColumnsAndNoRows_EmptyResult()
        {
            var text = TableFormatter.FormatTable(new Table(new List<Column>(), new List<Row>()));

            Console.WriteLine(text);

            Assert.That(text.Normalized(), Is.EqualTo("".Normalized()));
        }

        [Test]
        public void NoRows_JustColumnHeaders()
        {
            var columns = new List<Column>
            {
                new Column("Bimse"),
                new Column("Hej"),
            };
            var text = TableFormatter.FormatTable(new Table(columns, new List<Row>()));

            Console.WriteLine(text);

            Assert.That(text.Normalized(), Is.EqualTo(@"
+-------+-----+
| Bimse | Hej |
+-------+-----+
".Normalized()));

        }

        [Test]
        public void NoColumnsAndEmptyRows_EmptyResult()
        {
            var rows = new List<Row>
            {
                new Row(),
                new Row()
            };
            var text = TableFormatter.FormatTable(new Table(new List<Column>(), rows));

            Console.WriteLine(text);

            Assert.That(text.Normalized(), Is.EqualTo("".Normalized()));

        }
    }
}