namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class ArrowCapConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (((sourceType == null) || (sourceType == typeof(string))) || base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is string)
            {
                string[] strArray = (value as string).Split(new char[] { ',' });
                if (strArray.Length == 3)
                {
                    return new ArrowCap(int.Parse(strArray[0]), int.Parse(strArray[1]), bool.Parse(strArray[2]));
                }
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new string[] { "10,10,false", "10,10,true", "" });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}

