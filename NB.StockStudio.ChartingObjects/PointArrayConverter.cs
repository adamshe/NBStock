namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class PointArrayConverter : ArrayConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (((destinationType == null) || (destinationType == typeof(string))) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(string)) && (value is ObjectPoint[]))
            {
                return ((value as ObjectPoint[]).Length + " Points");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

