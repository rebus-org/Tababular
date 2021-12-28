using System;
using NUnit.Framework;
// ReSharper disable NotAccessedPositionalProperty.Local
// ReSharper disable ArgumentsStyleStringLiteral
// ReSharper disable ArgumentsStyleLiteral

namespace Tababular.Tests.Bugs;

[TestFixture]
public class TestRecordsAndStuff
{
    [Test]
    public void CanDoIt()
    {
        var formatter = new TableFormatter();

        var records = new[]
        {
            new SomeRecordThing(Number: 21, Text: "Big Number"), 
            new SomeRecordThing(Number: 5, Text: "Small Number"),
            new SomeRecordThing(Number: 12, Text: "Medium Number")
        };

        var text = formatter.FormatObjects(records);

        Console.WriteLine(text);
    }

    record SomeRecordThing(int Number, string Text);
}