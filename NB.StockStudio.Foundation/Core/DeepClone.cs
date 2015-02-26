namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class DeepClone : ICloneable
    {
        protected DeepClone()
        {
        }

        public object Clone()
        {
            object obj2 = Activator.CreateInstance(base.GetType());
            FieldInfo[] fields = obj2.GetType().GetFields();
            int index = 0;
            foreach (FieldInfo info in base.GetType().GetFields())
            {
                if (info.FieldType.GetInterface("ICloneable", true) != null)
                {
                    ICloneable cloneable = (ICloneable) info.GetValue(this);
                    if (cloneable != null)
                    {
                        fields[index].SetValue(obj2, cloneable.Clone());
                    }
                    else
                    {
                        fields[index].SetValue(obj2, info.GetValue(this));
                    }
                }
                else
                {
                    fields[index].SetValue(obj2, info.GetValue(this));
                }
                if (info.FieldType.GetInterface("IEnumerable", true) != null)
                {
                    IEnumerable enumerable = (IEnumerable) info.GetValue(this);
                    Type type3 = fields[index].FieldType.GetInterface("IList", true);
                    Type type4 = fields[index].FieldType.GetInterface("IDictionary", true);
                    int num2 = 0;
                    if (type3 != null)
                    {
                        IList list = (IList) fields[index].GetValue(obj2);
                        foreach (object obj3 in enumerable)
                        {
                            if (obj3.GetType().GetInterface("ICloneable", true) != null)
                            {
                                list[num2] = ((ICloneable) obj3).Clone();
                            }
                            num2++;
                        }
                    }
                    else if (type4 != null)
                    {
                        IDictionary dictionary = (IDictionary) fields[index].GetValue(obj2);
                        num2 = 0;
                        foreach (DictionaryEntry entry in enumerable)
                        {
                            if (entry.Value.GetType().GetInterface("ICloneable", true) != null)
                            {
                                dictionary[entry.Key] = ((ICloneable) entry.Value).Clone();
                            }
                            num2++;
                        }
                    }
                }
                index++;
            }
            return obj2;
        }
    }
}

