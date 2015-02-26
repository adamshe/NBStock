namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Collections;
    using System.Reflection;

    public class SegmentCollection : CollectionBase
    {
        public virtual void Add(ObjectSegment os)
        {
            base.List.Add(os);
        }

        public virtual void Add(ObjectPoint op1, ObjectPoint op2)
        {
            this.Add(new ObjectSegment(op1, op2));
        }

        public virtual ObjectSegment this[int Index]
        {
            get
            {
                return (ObjectSegment) base.List[Index];
            }
        }
    }
}

