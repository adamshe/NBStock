using System;
using System.Windows.Forms;
using NB.StockStudio.WinControls;


namespace Compiler
{
	/// <summary>
	/// Summary description for Start.
	/// </summary>
	public class Start
	{
		public Start()
		{
		}

		[STAThread]
		static void Main()
		{
			Application.Run(new FormulaSourceEditor());
		}
	}
}
