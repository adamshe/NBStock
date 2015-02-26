namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;

    public class CompareCategory : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            int order = (x as ObjectCategory).Order;
            int num2 = (y as ObjectCategory).Order;
            if (order > num2)
            {
                return 1;
            }
            if (order < num2)
            {
                return -1;
            }
            return 0;
        }
    }
}

