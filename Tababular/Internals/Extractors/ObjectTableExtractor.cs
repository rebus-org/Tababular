using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tababular.Internals.Extensions;
using Tababular.Internals.TableModel;

namespace Tababular.Internals.Extractors
{
    class ObjectTableExtractor : ITableExtractor
    {
        readonly List<object> _objectRows;

        public ObjectTableExtractor(IEnumerable objectRows)
        {
            _objectRows = objectRows.Cast<object>().ToList();
        }

        public Table GetTable()
        {
            var columns = new Dictionary<string, Column>();
            var rows = new List<Row>();

            foreach (var objectRow in _objectRows)
            {
                var row = new Row();

                if (System.Convert.GetTypeCode(objectRow) == System.TypeCode.Object)
                {
                    foreach (var property in objectRow.GetType().GetProperties())
                    {
                        var name = property.Name;
                        var column = columns.GetOrAdd(name, _ => new Column(name));
                        var value = property.GetValue(objectRow, null);

                        row.AddCell(column, new Cell(value));
                    }
                }
                else
                {
                    string name = objectRow.GetType().Name;
                    var column = columns.GetOrAdd(name, _ => new Column(name));
                    row.AddCell(column, new Cell(objectRow));
                }

                rows.Add(row);
            }

            return new Table(columns.Values.ToList(), rows);
        }

    }
}