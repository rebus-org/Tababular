using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastMember;
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
            var accessors = new Dictionary<Type, TypeAccessor>();

            foreach (var objectRow in _objectRows)
            {
                var row = new Row();
                var rowType = objectRow.GetType();

                if (System.Convert.GetTypeCode(objectRow) == System.TypeCode.Object)
                {
                    var accessor = accessors.GetOrAdd(rowType, TypeAccessor.Create);

                    foreach (var member in accessor.GetMembers())
                    {
                        var name = member.Name;
                        var column = columns.GetOrAdd(name, _ => new Column(name));
                        var value = accessor[objectRow, name];

                        row.AddCell(column, new Cell(value));
                    }
                }
                else
                {
                    var name = rowType.Name;
                    var column = columns.GetOrAdd(name, _ => new Column(name));
                    row.AddCell(column, new Cell(objectRow));
                }

                rows.Add(row);
            }

            return new Table(columns.Values.ToList(), rows);
        }
    }
}