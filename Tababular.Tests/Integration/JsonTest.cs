using System;
using NUnit.Framework;
using Tababular.Tests.Ex;

namespace Tababular.Tests.Integration
{
    [TestFixture]
    public class JsonTest
    {
        [Test]
        public void CanGenerateTableFromJsonArray()
        {
            var text = new TableFormatter().FormatJson(@"
[
    {""A property"": ""A value"", ""Another property"": 123}, 
    {""A property"": ""Another value"", ""Another property"": 2567}
]");

            Console.WriteLine(text);

            Assert.That(text.Normalized(), Is.EqualTo(@"
======================================
| A property     | Another property  |
======================================
| A value        | 123               |
======================================
| Another value  | 2567              |
======================================
".Normalized()));

        }

        [Test]
        public void CanGenerateTableFromSingleJsonObject()
        {
            var text = new TableFormatter().FormatJson(@"{""A property"": ""A value"", ""Another property"": 123}");

            Console.WriteLine(text);

            Assert.That(text.Normalized(), Is.EqualTo(@"
===================================
| A property  | Another property  |
===================================
| A value     | 123               |
===================================
".Normalized()));
        }
    }
}