namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Reflection;

    public class ObjectInit
    {
        public Type BaseType;
        public string Category;
        public int CategoryOrder;
        public string Icon;
        public string InitMethod;
        public string Name;
        public int Order;

        public ObjectInit(Type BaseType) : this(BaseType.Name, BaseType, null)
        {
        }

        public ObjectInit(string Name, Type BaseType) : this(Name, BaseType, null)
        {
        }

        public ObjectInit(Type BaseType, string InitMethod) : this(BaseType.Name, BaseType, InitMethod)
        {
        }

        public ObjectInit(string Name, Type BaseType, string InitMethod) : this(Name, BaseType, InitMethod, null)
        {
        }

        public ObjectInit(string Name, Type BaseType, string InitMethod, string Category) : this(Name, BaseType, InitMethod, Category, null)
        {
        }

        public ObjectInit(string Name, Type BaseType, string InitMethod, string Category, string Icon) : this(Name, BaseType, InitMethod, Category, Icon, 0x7fffffff)
        {
        }

        public ObjectInit(string Name, Type BaseType, string InitMethod, string Category, string Icon, int CategoryOrder) : this(Name, BaseType, InitMethod, Category, Icon, CategoryOrder, 0)
        {
        }

        public ObjectInit(string Name, Type BaseType, string InitMethod, string Category, string Icon, int CategoryOrder, int Order)
        {
            this.Name = Name;
            this.BaseType = BaseType;
            this.InitMethod = InitMethod;
            this.Category = Category;
            this.Icon = Icon;
            this.CategoryOrder = CategoryOrder;
            this.Order = Order;
        }

        public BaseObject Invoke()
        {
            BaseObject target = (BaseObject) this.BaseType.InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance, null, null, null);
            if (this.InitMethod != null)
            {
                this.BaseType.InvokeMember(this.InitMethod, BindingFlags.InvokeMethod, null, target, null);
            }
            if (target.ControlPoints.Length != target.ControlPointNum)
            {
                target.Init();
            }
            return target;
        }
    }
}

