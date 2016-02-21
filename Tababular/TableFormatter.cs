using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tababular.Internals;
using Tababular.Internals.Extractors;
using Tababular.Internals.TableModel;

namespace Tababular
{
    /// <summary>
    /// This is the table formatter. New it up and call one of the Format*** methods on it, e.g.
    /// <see cref="FormatObjects"/> with a sequence of objects whose properties will become columns,
    /// or <see cref="FormatDictionaries{TValue}"/> with a sequence of dictionaries whose keys will become columns.
    /// </summary>
    public class TableFormatter
    {
        readonly Hints _hints;

        /// <summary>
        /// Creates the table formatter possibly using the given <see cref="Hints"/> to do its thing
        /// </summary>
        public TableFormatter(Hints hints = null)
        {
            _hints = hints ?? new Hints();
        }

        /// <summary>
        /// Formats a sequence of objects as rows of a table, using the property names as column names
        /// </summary>
        public string FormatObjects(IEnumerable rows)
        {
            var extractor = new ObjectTableExtractor(rows);

            return UseExtractor(extractor);
        }

        /// <summary>
        /// Formats a sequence of dictionaries as rows of a table, using the dictionary keys as column names
        /// </summary>
        public string FormatDictionaries<TValue>(IEnumerable<IDictionary<string, TValue>> rows)
        {
            var extractor = new DictionaryTableExtractor(rows.Select(r => r.ToDictionary(d => d.Key, d => (object)d.Value)));

            return UseExtractor(extractor);
        }

        /// <summary>
        /// Formats the given JSON object or array of objects as a table
        /// </summary>
        public string FormatJson(string jsonText)
        {
            var extractor = new JsonTableExtractor(jsonText);

            return UseExtractor(extractor);
        }

        string UseExtractor(ITableExtractor extractor)
        {
            var table = extractor.GetTable();

            return FormatTable(table, _hints);
        }

        internal static string FormatTable(Table table, Hints hints = null)
        {
            hints = hints ?? new Hints();

            if (hints.MaxTableWidth.HasValue)
            {
                EnforceMaxWidth(table, hints.MaxTableWidth.Value);
            }

            const char horizontalLineChar = '=';
            const char verticalLineChar = '|';

            var builder = new StringBuilder();

            if (!table.Columns.Any()) return "";

            BuildHorizontalLine(table, builder, horizontalLineChar);

            BuildColumnLabels(table, builder, verticalLineChar);

            if (table.Rows.Any())
            {
                foreach (var row in table.Rows)
                {
                    BuildHorizontalLine(table, builder, horizontalLineChar);

                    BuildTableRow(row, table, builder, verticalLineChar);
                }
            }

            BuildHorizontalLine(table, builder, horizontalLineChar);

            return builder.ToString();
        }

        static void EnforceMaxWidth(Table table, int maxWidth)
        {
            if (!table.Columns.Any()) return;

            while (true)
            {
                var totalWidth = table.Columns.Sum(c => c.Width);

                if (totalWidth < maxWidth) break;

                var widestColumn = table.Columns
                    .OrderByDescending(c => c.Width)
                    .First();

                widestColumn.ConstrainWidth(3*widestColumn.Width/4);
            }

            foreach (var column in table.Columns)
            {
                foreach (var row in table.Rows)
                {
                    var cell = row.GetCellOrNull(column);
                    if (cell == null) continue;

                    cell.Rearrange(column);
                }
            }
        }

        static void BuildColumnLabels(Table table, StringBuilder builder, char verticalLineChar)
        {
            var texts = table.Columns
                .Select(column => new
                {
                    Column = column,
                    Text = new[] { column.Label }
                })
                .ToDictionary(a => a.Column, a => a.Text);

            BuildRow(texts, table, builder, verticalLineChar);
        }

        static void BuildTableRow(Row row, Table table, StringBuilder builder, char verticalLineChar)
        {
            var texts = table.Columns
                .Select(column => new
                {
                    Column = column,
                    Text = row.GetCellOrNull(column)?.Lines ?? new string[0]
                })
                .ToDictionary(a => a.Column, a => a.Text);

            BuildRow(texts, table, builder, verticalLineChar);
        }

        static void BuildRow(Dictionary<Column, string[]> texts, Table table, StringBuilder builder, char verticalLineChar)
        {
            for (var index = 0; index < texts.Values.Max(l => l.Length); index++)
            {
                builder.Append(verticalLineChar);

                foreach (var colum in table.Columns)
                {
                    var lines = texts[colum];
                    var text = index < lines.Length ? lines[index] : "";

                    builder.Append(new string(' ', colum.Padding));
                    builder.Append(text.PadRight(colum.Width - colum.Padding));
                    builder.Append(verticalLineChar);
                }

                builder.AppendLine();
            }
        }

        static void BuildHorizontalLine(Table table, StringBuilder builder, char c)
        {
            foreach (var column in table.Columns)
            {
                builder.Append(new string(c, column.Width + column.Padding));
            }

            builder.Append(c);

            builder.AppendLine();
        }
    }
}
