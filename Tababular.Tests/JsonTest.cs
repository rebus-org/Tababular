using System;
using NUnit.Framework;

namespace Tababular.Tests
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
        }
    }
}