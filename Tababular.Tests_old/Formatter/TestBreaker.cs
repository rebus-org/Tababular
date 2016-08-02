using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tababular.Internals;

namespace Tababular.Tests.Formatter
{
    [TestFixture]
    public class TestBreaker
    {
        [Test]
        public void DoesNotBreakWhenThereIsNoNeed()
        {
            var breaker = new Breaker("this is a short line");

            var lines = breaker.GetLines(100).ToList();

            Assert.That(lines, Is.EqualTo(new[] { "this is a short line" }));
        }

        [Test]
        public void CanBreakBetweenWords()
        {
            //                         0          10         20
            var breaker = new Breaker("this is a short line");

            var lines = breaker.GetLines(10).ToList();

            Assert.That(lines, Is.EqualTo(new[]
            {
                "this is a",
                "short line"
            }));
        }

        [Test]
        public void BreaksTheRightWayWithMultipleParagraphs()
        {
            var breaker = new Breaker(@"This is the first line.

This is the second line, which is actually a paragraph, which happens to be pretty long, although it might/might not have correct punctuation.

The third line is also fairly long, and it has a bunch of CAPITALIZED FILL WORDS: BLA BLA BLA BLA BLA.");

            const int maxWidth = 50;
            var lines = breaker.GetLines(maxWidth).ToList();

            PrintLines(lines, maxWidth);

            Assert.That(lines, Is.EqualTo(new[]
            {
                "This is the first line.",
                "",
                "This is the second line, which is actually a",
                "paragraph, which happens to be pretty long,",
                "although it might/might not have correct",
                "punctuation.",
                "",
                "The third line is also fairly long, and it has a",
                "bunch of CAPITALIZED FILL WORDS: BLA BLA BLA BLA",
                "BLA."
            }));
        }

        [Test]
        public void BreaksTheRightWayWhenWordIsJustTooLong()
        {
            var breaker = new Breaker(@"This is the first line.

This is a line with an artificial too-long word: huifhueihfwiufewlfehwfeliwhfuilewhfuilehwfuilehwfulehwuflewufilehwufilehwuilfhewuifhwe.

This is the third line.");

            const int maxWidth = 60;
            var lines = breaker.GetLines(maxWidth).ToList();

            PrintLines(lines, maxWidth);

            Assert.That(lines, Is.EqualTo(new[]
            {
                "This is the first line.",
                "",
                "This is a line with an artificial too-long word:",
                "huifhueihfwiufewlfehwfeliwhfuilewhfuilehwfuilehwfulehwuflewu",
                "filehwufilehwuilfhewuifhwe.",
                "",
                "This is the third line."
            }));
        }

        [Test]
        public void WorksWhenTooLongWordIsFirst()
        {
            var breaker = new Breaker("THISISJUSTTOOLONGTOFITONALINE");

            const int maxWidth = 10;
            var lines = breaker.GetLines(maxWidth).ToList();

            PrintLines(lines, maxWidth);

            Assert.That(lines, Is.EqualTo(new[]
            {
                "THISISJUST",
                "TOOLONGTOF",
                "ITONALINE"
            }));
        }

        [Test]
        public void WorksWhenTooLongWordIsFirstAndItIsFollowedByAnotherWord()
        {
            var breaker = new Breaker("THISISJUSTTOOLONGTOFITONA LINE");

            const int maxWidth = 20;
            var lines = breaker.GetLines(maxWidth).ToList();

            PrintLines(lines, maxWidth);

            Assert.That(lines, Is.EqualTo(new[]
            {
                "THISISJUSTTOOLONGTOF",
                "ITONA LINE"
            }));
        }

        static void PrintLines(IEnumerable<string> lines, int maxWidth)
        {
            Console.WriteLine(new string('*', maxWidth));
            Console.WriteLine(string.Join(Environment.NewLine, lines));
            Console.WriteLine(new string('*', maxWidth));
        }
    }
}