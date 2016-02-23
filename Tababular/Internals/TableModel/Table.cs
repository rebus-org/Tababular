using System;
using System.Collections.Generic;
using System.Linq;

namespace Tababular.Internals.TableModel
{
    class Table
    {
        public Table(List<Column> columns, List<Row> rows)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (rows == null) throw new ArgumentNullException(nameof(rows));

            Rows = rows;
            Columns = columns;

            foreach (var column in Columns)
            {
                foreach (var row in Rows)
                {
                    var cellOrNull = row.GetCellOrNull(column);

                    if (cellOrNull == null) continue;

                    column.AdjustWidth(cellOrNull);
                }
            }

        }

        public List<Column> Columns { get; }
        public List<Row> Rows { get; }

        public bool HasCellWith(Func<Cell, bool> cellPredicate)
        {
            return Rows.SelectMany(r => r.GetAllCells()).Any(cellPredicate);
        }
    }
}