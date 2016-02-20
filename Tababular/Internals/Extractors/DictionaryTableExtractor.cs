using System.Collections.Generic;
using System.Linq;
using Tababular.Internals.Extensions;
using Tababular.Internals.TableModel;

namespace Tababular.Internals.Extractors
{
    class DictionaryTableExtractor : ITableExtractor
    {
        readonly List<IDictionary<string, object>> _rows;

        public DictionaryTableExtractor(IEnumerable<IDictionary<string, object>> rows)
        {
            _rows = rows.ToList();
        }

        public Table GetTable()
        {
            var columns = new Dictionary<string, Column>();
            var rows = new List<Row>();

            foreach (var dictRow in _rows)
            {
                var row = new Row();

                foreach (var name in dictRow.Keys)
                {
                    var column = columns.GetOrAdd(name, _ => new Column(name));
                    var value = dictRow[name];

                    row.AddCell(column, new Cell(value));
                }

                rows.Add(row);
            }

            return new Table(columns.Values.ToList(), rows);

        }
    }
}