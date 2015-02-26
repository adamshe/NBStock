namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;

    public class ObjectCategory
    {
        public string CategoryName;
        public ArrayList ObjectList = new ArrayList();
        public int Order;

        public ObjectCategory(string CategoryName, int Order)
        {
            this.CategoryName = CategoryName;
            this.Order = Order;
        }
    }
}

