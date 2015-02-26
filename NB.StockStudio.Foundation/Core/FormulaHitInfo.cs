namespace NB.StockStudio.Foundation
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FormulaHitInfo
    {
        public float X;
        public float Y;
        public FormulaArea Area;
        public FormulaBase Formula;
        public FormulaData Data;
        public FormulaHitType HitType;
        public FormulaAxisY AxisY;
        public FormulaAxisX AxisX;
        public int CursorPos;
        public int XPart(int MaxPart)
        {
            if (this.Area != null)
            {
                Rectangle rect = this.Area.Canvas.Rect;
                return (((((int) this.X) - rect.X) * MaxPart) / rect.Width);
            }
            return -1;
        }

        public int YPart(int MaxPart)
        {
            if (this.Area != null)
            {
                Rectangle rect = this.Area.Canvas.Rect;
                return (((((int) this.Y) - rect.Y) * MaxPart) / rect.Height);
            }
            return -1;
        }
    }
}

