namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;

    public class SkinConverter : StringConverter
    {
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(FormulaSkin.GetBuildInSkins());
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}

