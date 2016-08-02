using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tababular.Internals.TableModel
{
    class Cell
    {
        public Cell(object value)
        {
            if (value is string)
            {
                TextValue = value?.ToString() ?? "";
            }
            else if (value is IEnumerable)
            {
                TextValue = string.Join(Environment.NewLine, ((IEnumerable)value).Cast<object>());
            }
            else
            {
                TextValue = value?.ToString() ?? "";
            }

            Lines = GetLines();
        }

        string[] GetLines()
        {
            return TextValue
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public string[] Lines { get; private set; }

        public string TextValue { get; }

        public int GetWidth()
        {
            return Lines.Any()
                ? Lines.Max(l => l.Length)
                : 0;
        }

        public int GetHeight()
        {
            return Lines.Length;
        }

        public void Rearrange(Column column)
        {
            var maxLineLength = column.Width - 2 * column.Padding;

            Lines = Lines
                .SelectMany(line =>
                {
                    var breaker = new Breaker(line);

                    return breaker.GetLines(maxLineLength);
                })
                .ToArray();
        }
    }
}