namespace Tababular.Internals.TableModel
{
    class Column
    {
        public Column(string label)
        {
            Label = label ?? "";

            Padding = 1;

            AdjustForWidth(Label.Length);
        }

        public string Label { get; }

        public int Width { get; private set; }

        public int Padding { get; }

        public void AdjustWidth(Cell cell)
        {
            var cellWidth = cell.GetWidth();

            AdjustForWidth(cellWidth);
        }

        void AdjustForWidth(int cellWidth)
        {
            var width = cellWidth + 2 * Padding;

            if (width < Width) return;

            Width = width;
        }

        public void ConstrainWidth(int newWidth)
        {
            Width = newWidth;
        }
    }
}