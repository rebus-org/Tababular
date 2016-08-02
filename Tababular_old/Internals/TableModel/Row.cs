using System;
using System.Collections.Generic;

namespace Tababular.Internals.TableModel
{
    class Row
    {
        readonly Dictionary<Column, Cell> _cells = new Dictionary<Column, Cell>();

        public void AddCell(Column column, Cell cell)
        {
            try
            {
                _cells.Add(column, cell);

                AdjustHeight(cell);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Tried to add cell '{cell}' to row as column '{column.Label}', but the row had this cell already: '{_cells[column]}'", exception);
            }
        }

        public Cell GetCellOrNull(Column column)
        {
            Cell cell;

            return _cells.TryGetValue(column, out cell)
                ? cell
                : null;
        }

        void AdjustHeight(Cell cell)
        {
            var numberOfLines = cell.GetHeight();

            if (numberOfLines < Height) return;

            Height = numberOfLines;
        }

        public int Height { get; private set; }

        public IEnumerable<Cell> GetAllCells()
        {
            return _cells.Values;
        }
    }
}