using System;
using System.Drawing;
using System.Windows.Forms;

namespace StockExpert.ColumnStyle
{
    public class DataGridReadOnlyColumn : DataGridTextBoxColumn
    {
        private int LastColumn;


        public DataGridReadOnlyColumn(string format, string headerText, string mappingName, int width, int LastColumn)
        {
            base.Format = format;
            HeaderText = headerText;
            base.MappingName = mappingName;
            Width = width;
            NullText = "";
            this.LastColumn = LastColumn;
        }

        public DataGridReadOnlyColumn(string format, string headerText, string mappingName, int width) : this(format, headerText, mappingName, width, -2)
        {
        }

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool isReadOnly, string instantText, bool cellIsVisible)
        {
        }

        private void InitializeComponent()
        {
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            if (DataGridTableStyle.DataGrid.CurrentRowIndex == rowNum)
            {
                backBrush = new SolidBrush(DataGridTableStyle.SelectionBackColor);
                foreBrush = new SolidBrush(DataGridTableStyle.SelectionForeColor);
            }
            else if (LastColumn > -2)
            {
                int i = 0;
                object local1 = GetColumnValueAtRow(source, rowNum);
                if (local1 != DBNull.Value && local1 is Double)
                {
                    if (LastColumn == -1)
                    {
                        i = Math.Sign((double)local1);
                    }
                    else
                    {
                        object local2 = DataGridTableStyle.DataGrid[rowNum, LastColumn];
                        if (local2 != DBNull.Value)
                        {
                            i = Math.Sign((double)local1 - (double)local2);
                        }
                    }
                    if (i > 0)
                    {
                        foreBrush = Brushes.DarkGreen;
                    }
                    else if (i < 0)
                    {
                        foreBrush = Brushes.Red;
                    }
                }
            }
            base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
        }
    }

}
