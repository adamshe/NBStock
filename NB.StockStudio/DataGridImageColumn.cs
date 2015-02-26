using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace StockExpert.ColumnStyle
{
    public class DataGridImageColumn : DataGridTextBoxColumn
    {
        public ImageList ilImage;

        public Hashtable htImageMap;


        public DataGridImageColumn(string format, string headerText, string mappingName, int width, ImageList ilImage, Hashtable ImageMap)
        {
            HeaderText = headerText;
            base.MappingName = mappingName;
            Width = width;
            this.ilImage = ilImage;
            htImageMap = ImageMap;
        }

        public DataGridImageColumn(string format, string headerText, string mappingName, int width, ImageList ilImage) : this(format, headerText, mappingName, width, ilImage, null)
        {
        }

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool isReadOnly, string instantText, bool cellIsVisible)
        {
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            if (DataGridTableStyle.DataGrid.CurrentRowIndex == rowNum)
            {
                g.FillRectangle(new SolidBrush(DataGridTableStyle.SelectionBackColor), bounds);
            }
            else
            {
                g.FillRectangle(backBrush, bounds);
            }
            object local1 = GetColumnValueAtRow(source, rowNum);
            object local2 = local1;
            if (htImageMap != null)
            {
                local2 = htImageMap[local1];
            }
            int i = bounds.X + (bounds.Width - ilImage.ImageSize.Width) / 2;
            int j = bounds.Y + (bounds.Height - ilImage.ImageSize.Height) / 2;
            if (local2 != null)
            {
                ilImage.Draw(g, i, j, Int32.Parse(local2.ToString()));
            }
        }
    }

}
