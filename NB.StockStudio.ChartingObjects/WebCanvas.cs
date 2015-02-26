namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using NB.StockStudio.WinControls;
    using System;
    using System.Windows.Forms;

    public class WebCanvas : IObjectCanvas
    {
        private FormulaChart backChart;

        public WebCanvas(FormulaChart fc)
        {
            this.backChart = fc;
        }

        public FormulaChart BackChart
        {
            get
            {
                return this.backChart;
            }
        }

        public Control DesignerControl
        {
            get
            {
                return null;
            }
        }

        public bool Designing
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
    }
}

