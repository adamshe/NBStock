namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ObjectCollection : CollectionBase
    {
        public virtual void Add(BaseObject bo)
        {
            base.List.Add(bo);
        }

        public int IndexOf(BaseObject ob)
        {
            return base.List.IndexOf(ob);
        }

        public virtual void Insert(int Index, BaseObject bo)
        {
            base.List.Insert(Index, bo);
        }

        public void Remove(BaseObject value)
        {
            base.List.Remove(value);
        }

        public virtual BaseObject this[int Index]
        {
            get
            {
                return (BaseObject) base.List[Index];
            }
        }
    }
}

