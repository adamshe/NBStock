namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;

    public class ChartDragInfo
    {
        public int[] AreaHeight;
        public double AreaMaxY;
        public double AreaMinY;
        public DateTime EndTime;
        public FormulaHitInfo HitInfo;
        public DateTime StartTime;

        public ChartDragInfo(FormulaChart Chart, FormulaHitInfo HitInfo)
        {
            this.HitInfo = HitInfo;
            this.AreaHeight = new int[Chart.Areas.Count];
            FormulaAxisY axisY = HitInfo.AxisY;
            if ((axisY == null) && (HitInfo.Area != null))
            {
                axisY = HitInfo.Area.AxisY;
            }
            if (axisY != null)
            {
                this.AreaMinY = axisY.MinY;
                this.AreaMaxY = axisY.MaxY;
            }
            for (int i = 0; i < Chart.Areas.Count; i++)
            {
                FormulaArea area = Chart.Areas[i];
                this.AreaHeight[i] = area.Rect.Height;
            }
            this.StartTime = Chart.StartTime;
            this.EndTime = Chart.EndTime;
        }
    }
}

