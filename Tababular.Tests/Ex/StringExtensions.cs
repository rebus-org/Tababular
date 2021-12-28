using System;
using System.Linq;

namespace Tababular.Tests.Ex;

public static class StringExtensions
{
    public static string Normalized(this string str)
    {
        var lines = str.Split(new[] {Environment.NewLine, "\r", "\n"}, StringSplitOptions.None)
            .SkipWhile(string.IsNullOrWhiteSpace)
            .Reverse()
            .SkipWhile(string.IsNullOrWhiteSpace)
            .Reverse();

        return string.Join(Environment.NewLine, lines.Select(l => l.TrimEnd()));
    } 
}