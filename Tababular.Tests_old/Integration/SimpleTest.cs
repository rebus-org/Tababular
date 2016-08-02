using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tababular.Tests.Ex;

namespace Tababular.Tests.Integration
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void CanFormatAsTableWithTwoColumns()
        {
            var tableFormatter = new TableFormatter();

            var text = tableFormatter.FormatObjects(new[]
            {
                new {FirstColumn = "r1", SecondColumn = "hej"},
                new {FirstColumn = "r2", SecondColumn = "hej"},
                new {FirstColumn = "r3", SecondColumn = "hej"},
                new {FirstColumn = "r4", SecondColumn = "hej"}
            });

            Console.WriteLine(text);

            const string expected = @"
==============================
| FirstColumn | SecondColumn |
==============================
| r1          | hej          |
==============================
| r2          | hej          |
==============================
| r3          | hej          |
==============================
| r4          | hej          |
==============================
";
            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void CanFormatAsTableWithThreeColumns()
        {
            var tableFormatter = new TableFormatter();

            var objects = new[]
            {
                new {FirstColumn = "r1", SecondColumn = "hej", ThirdColumn = "hej igen"},
                new {FirstColumn = "r2", SecondColumn = "hej", ThirdColumn = "hej igen"},
            };

            var text = tableFormatter.FormatObjects(objects);

            Console.WriteLine(text);

            const string expected = @"

============================================
| FirstColumn | SecondColumn | ThirdColumn |
============================================
| r1          | hej          | hej igen    |
============================================
| r2          | hej          | hej igen    |
============================================

";

            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void SimplePaddingTest_OneColumn()
        {
            var tableFormatter = new TableFormatter();

            var objects = new[]
            {
                new {A = "A"}
            };

            var text = tableFormatter.FormatObjects(objects);

            Console.WriteLine(text);

            const string expected = @"

=====
| A |
=====
| A |
=====
";

            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void SimplePaddingTest_TwoColumns()
        {
            var tableFormatter = new TableFormatter();

            var objects = new[]
            {
                new {A = "A", B = "B"}
            };

            var text = tableFormatter.FormatObjects(objects);

            Console.WriteLine(text);

            const string expected = @"

=========
| A | B |
=========
| A | B |
=========
";

            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void CanFormatAsTableWithCellWiderThanColumnLabel()
        {
            var tableFormatter = new TableFormatter();

            var text = tableFormatter.FormatObjects(new[]
            {
                new {FirstColumn = "This is a fairly long text"}
            });

            Console.WriteLine(text);

            const string expected = @"
==============================
| FirstColumn                |
==============================
| This is a fairly long text |
==============================
";
            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void CanUseDictionaryAsInput()
        {
            var tableFormatter = new TableFormatter();

            var text = tableFormatter.FormatDictionaries(new[]
            {
                new Dictionary<string, string> { {"Headline with space", "Some value"} },
                new Dictionary<string, string> { {"Headline with space", "Another value"} },
                new Dictionary<string, string> { {"Another headline with space", "Third value"} },
                new Dictionary<string, string> { {"Yet another headline with space", "Fourth value"} },
            });

            Console.WriteLine(text);

            const string expected = @"


=======================================================================================
| Headline with space | Another headline with space | Yet another headline with space |
=======================================================================================
| Some value          |                             |                                 |
=======================================================================================
| Another value       |                             |                                 |
=======================================================================================
|                     | Third value                 |                                 |
=======================================================================================
|                     |                             | Fourth value                    |
=======================================================================================

";

            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void CanFormatAsTableWithCellWithMultipleLines()
        {
            var tableFormatter = new TableFormatter();

            var text = tableFormatter.FormatObjects(new[]
            {
                new {FirstColumn = @"This is the first line
This is the second line
And this is the third and last line"}
            });

            Console.WriteLine(text);

            const string expected = @"

=======================================
| FirstColumn                         |
=======================================
| This is the first line              |
| This is the second line             |
| And this is the third and last line |
=======================================
";

            Assert.That(text.Normalized(), Is.EqualTo(expected.Normalized()));
        }

        [Test]
        public void MultipleCellsWithMultipleLines()
        {
            var objects = new[]
            {
                new  { MachineName = "ctxtest01", Ip = "10.0.0.10", Ports = new[] {80, 8080, 9090}},
                new  { MachineName = "ctxtest02", Ip = "10.0.0.11", Ports = new[] {80, 5432}}
            };

            var text = new TableFormatter().FormatObjects(objects);

            Console.WriteLine(text);
        }
    }
}
