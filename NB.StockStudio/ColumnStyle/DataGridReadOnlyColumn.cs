using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics ;
namespace WindowsDemo.ColumnStyle
{
    public class DataGridReadOnlyColumn : DataGridTextBoxColumn
    {
		// Methods
	public DataGridReadOnlyColumn(string format, string headerText, string mappingName, int width): this(format, headerText, mappingName, width, -2)

		{
				
		}
		public DataGridReadOnlyColumn(string format, string headerText, string mappingName, int width, int LastColumn)//:this(format,headerText,mappingName,  width)
		{			
				 base.Format = format;
				 this.HeaderText = headerText;
				 base.MappingName = mappingName;
				 this.Width = width;
				 this.NullText = "";
				 this.LastColumn = LastColumn;

		}


		protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool isReadOnly, string instantText, bool cellIsVisible){}
		private void InitializeComponent(){}
			 protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
			 {
					  if (this.DataGridTableStyle.DataGrid.CurrentRowIndex == rowNum)
					  {
							   backBrush = new SolidBrush(this.DataGridTableStyle.SelectionBackColor);
							   foreBrush = new SolidBrush(this.DataGridTableStyle.SelectionForeColor);
					  }
					  else if (this.LastColumn > -2)
					  {
							   int num1 = 0;
							   object obj1 = this.GetColumnValueAtRow(source, rowNum);
							   if ((obj1 != DBNull.Value) && (obj1 is double))
							   {
										if (this.LastColumn == -1)
										{
												 num1 = Math.Sign((double) obj1);
										}
										else
										{
												 Debug.Write (string.Format ("rowNum={0} , LastColumn={1}",rowNum, this.LastColumn ));
												 object obj2 = this.DataGridTableStyle.DataGrid[rowNum, this.LastColumn];
												 if (obj2 != DBNull.Value)
												 {
														  num1 = Math.Sign((double) (((double) obj1) - ((double) obj2)));
												 }
										}
										if (num1 > 0)
										{
												 foreBrush = Brushes.DarkGreen;
										}
										else if (num1 < 0)
										{
												 foreBrush = Brushes.Red;
										}
							   }
					  }
					  base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
			 }



		// Fields
		private int LastColumn;

    }

}
