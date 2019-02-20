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
            if (objectRows == null) throw new ArgumentNullException(nameof(objectRows));
            _objectRows = objectRows.Cast<object>().ToList();
        }

        public Table GetTable()
        {
            var columns = new Dictionary<string, Column>();
            var rows = new List<Row>();
            var typeAccessors = new Dictionary<Type, TypeAccessor>();
            var memberLists = new Dictionary<Type, Member[]>();

            foreach (var objectRow in _objectRows)
            {
                var row = new Row();
                var rowType = objectRow.GetType();

                if (Convert.GetTypeCode(objectRow) == TypeCode.Object)
                {
                    var accessor = typeAccessors.GetOrAdd(rowType, TypeAccessor.Create);
                    var members = memberLists.GetOrAdd(rowType, type => GetMemberAccessors(accessor, type));

                    foreach (var member in members)
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

        static Member[] GetMemberAccessors(TypeAccessor accessor, Type type)
        {
            var memberSet = accessor.GetMembers();

            try
            {
                var orderDictionary = type.GetProperties()
                    .Select((property, index) => new {property, index})
                    .ToDictionary(a => a.property.Name, a => a.index);

                return memberSet
                    .OrderBy(m => orderDictionary[m.Name])
                    .ToArray();
            }
            catch
            {
                return memberSet.ToArray();
            }
        }
    }
}