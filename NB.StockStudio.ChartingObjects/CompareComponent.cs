namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;

    public class CompareComponent : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            int order = (x as ObjectInit).Order;
            int num2 = (y as ObjectInit).Order;
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

