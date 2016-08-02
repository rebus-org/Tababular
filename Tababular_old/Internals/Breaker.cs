using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tababular.Internals.Extensions;

namespace Tababular.Internals
{
    class Breaker
    {
        readonly string _text;

        public Breaker(string text)
        {
            _text = text;
        }

        public IEnumerable<string> GetLines(int maxWidth)
        {
            var lines = _text.Split(new[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.None);

            return lines
                .SelectMany(line => BreakLines(line, maxWidth)
                .ToList());
        }

        static IEnumerable<string> BreakLines(string line, int maxWidth)
        {
            if (line.Length <= maxWidth)
            {
                yield return line;
                yield break;
            }

            var words = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var builder = new StringBuilder();

            foreach (var word in words)
            {
                if (builder.Length == 0)
                {
                    if (word.Length > maxWidth)
                    {
                        var forceBrokenWords = word
                            .Batch(maxWidth)
                            .Select(characters => new string(characters.ToArray()))
                            .ToList();

                        foreach (var forceBrokenWord in forceBrokenWords.Take(forceBrokenWords.Count - 1))
                        {
                            yield return forceBrokenWord;
                        }

                        builder.Append(forceBrokenWords.Last());
                    }
                    else
                    {
                        builder.Append(word);
                    }
                }
                else if (builder.Length + word.Length < maxWidth)
                {
                    builder.Append(" " + word);
                }
                else
                {
                    yield return builder.ToString();

                    if (word.Length > maxWidth)
                    {
                        var forceBrokenWords = word
                            .Batch(maxWidth)
                            .Select(characters => new string(characters.ToArray()))
                            .ToList();

                        foreach (var forceBrokenWord in forceBrokenWords.Take(forceBrokenWords.Count - 1))
                        {
                            yield return forceBrokenWord;
                        }

                        builder = new StringBuilder(forceBrokenWords.Last());
                    }
                    else
                    {
                        builder = new StringBuilder(word);
                    }
                }
            }

            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }
    }
}