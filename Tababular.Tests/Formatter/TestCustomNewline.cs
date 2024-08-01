using System;
using NUnit.Framework;

namespace Tababular.Tests.Formatter;

[TestFixture]
public class TestCustomNewline
{
    [TestCase(true)]
    [TestCase(false)]
    public void ItWorks(bool customize)
    {
        var rows = new[]
        {
            new {Name = "Mogens", Role = "Salesperson"},
            new {Name = "Michael", Role = "Terraformer"},
        };

        var formatter = new TableFormatter(new() { NewLineSeparator = customize ? $"🙂{Environment.NewLine}" : Environment.NewLine });

        var output = formatter.FormatObjects(rows);

        Console.WriteLine(output);

        if (customize)
        {
            Assert.That(output, Is.EqualTo(@"+---------+-------------+🙂
| Name    | Role        |🙂
+---------+-------------+🙂
| Mogens  | Salesperson |🙂
+---------+-------------+🙂
| Michael | Terraformer |🙂
+---------+-------------+🙂
"));
        }
        else
        {
            Assert.That(output, Is.EqualTo(@"+---------+-------------+
| Name    | Role        |
+---------+-------------+
| Mogens  | Salesperson |
+---------+-------------+
| Michael | Terraformer |
+---------+-------------+
"));
        }
    }
}