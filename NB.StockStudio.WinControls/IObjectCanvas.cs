namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Windows.Forms;

    public interface IObjectCanvas
    {
        FormulaChart BackChart { get; }

        Control DesignerControl { get; }

        bool Designing { get; set; }
    }
}

