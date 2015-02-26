using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsDemo.ColumnStyle
{
    public class DataGridImageColumn : DataGridTextBoxColumn
    {
        public ImageList ilImage;

        public Hashtable htImageMap;


        public DataGridImageColumn(string format, string headerText, string mappingName, int width, ImageList ilImage, Hashtable ImageMap)
        {
			base.Format = format;
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
			if (this.DataGridTableStyle.DataGrid.CurrentRowIndex == rowNum)
			{
				g.FillRectangle(new SolidBrush(this.DataGridTableStyle.SelectionBackColor), bounds);
			}
			else
			{
				g.FillRectangle(backBrush, bounds);
			}
			object obj1 = this.GetColumnValueAtRow(source, rowNum);
			object obj2 = obj1;
			if (this.htImageMap != null)
			{
				obj2 = this.htImageMap[obj1];
			}
			int num1 = bounds.X + ((bounds.Width - this.ilImage.ImageSize.Width) / 2);
			int num2 = bounds.Y + ((bounds.Height - this.ilImage.ImageSize.Height) / 2);
			if (obj2 != null)
			{
				this.ilImage.Draw(g, num1, num2, int.Parse(obj2.ToString()));
			}
		}

    }

}
