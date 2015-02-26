namespace NB.StockStudio.WinControls
{
    using System;
    using System.Windows.Forms;

    public class KeyMessageFilter : IMessageFilter
    {
        private ChartWinControl chartWinControl;
        private static KeyMessageFilter KeyFilter;
        private const int WM_KEYDOWN = 0x100;

        public KeyMessageFilter(ChartWinControl chartWinControl)
        {
            this.chartWinControl = chartWinControl;
        }

        public static void AddMessageFilter(ChartWinControl chartWinControl)
        {
            if (KeyFilter == null)
            {
                KeyFilter = new KeyMessageFilter(chartWinControl);
                Application.AddMessageFilter(KeyFilter);
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                KeyEventArgs e = new KeyEventArgs(((Keys) m.WParam.ToInt32()) | Control.ModifierKeys);
                if ((this.chartWinControl != null) && this.chartWinControl.Focused)
                {
                    this.chartWinControl.HandleKeyEvent(e);
                }
                if (e.Handled)
                {
                    return e.Handled;
                }
            }
            return false;
        }
    }
}

