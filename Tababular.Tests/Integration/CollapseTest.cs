using System;
using NUnit.Framework;
using Tababular.Tests.Ex;

namespace Tababular.Tests.Integration
{
    [TestFixture]
    public class CollapseTest
    {
        [Test]
        public void CanCollapseTable()
        {
            var objects = new[]
            {
                new {Id = "whatever01", Value = "jigeojgieojw"},
                new {Id = "whatever02", Value = "huiehguiw"},
                new {Id = "whatever03", Value = "nvnjkdnjkdsjkvds"},
                new {Id = "whatever04", Value = "fjeiufhweui"}
            };

            var formatter = new TableFormatter(new Hints { CollapseVerticallyWhenSingleLine = true });

            var text = formatter.FormatObjects(objects);

            Console.WriteLine(text);

            Assert.That(text.Normalized(), Is.EqualTo(@"

+------------+------------------+
| Id         | Value            |
+------------+------------------+
| whatever01 | jigeojgieojw     |
| whatever02 | huiehguiw        |
| whatever03 | nvnjkdnjkdsjkvds |
| whatever04 | fjeiufhweui      |
+------------+------------------+

".Normalized()));
        }

        [Test]
        public void DoesNotCollapseWhenCellHasMoreLines()
        {
            var objects = new[]
            {
                new {Id = "whatever01", Value = "jigeojgieojw"},
                new {Id = "whatever02", Value = @"huiehguiw
ruined the party!"},
            };


            var formatter = new TableFormatter(new Hints { CollapseVerticallyWhenSingleLine = true });

            var text = formatter.FormatObjects(objects);

            Console.WriteLine(text);
            
            Assert.That(text.Normalized(), Is.EqualTo(@"

+------------+-------------------+
| Id         | Value             |
+------------+-------------------+
| whatever01 | jigeojgieojw      |
+------------+-------------------+
| whatever02 | huiehguiw         |
|            | ruined the party! |
+------------+-------------------+

".Normalized()));
        }
    }
}