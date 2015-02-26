namespace NB.StockStudio.Foundation
{
    using System;
    using System.Xml.Serialization;

    public enum FormulaParamType
    {
        [XmlEnum("double")]
        Double,
        [XmlEnum("string")]
        String,
        [XmlEnum("symbol")]
        Symbol,
        [XmlEnum("indicator")]
        Indicator
    };

    public class FormulaParam
    {
        [XmlAttribute]
        public string DefaultValue;
        [XmlAttribute]
        public string Description;
        [XmlAttribute]
        public string MaxValue;
        [XmlAttribute]
        public string MinValue;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public FormulaParamType ParamType; //string
        [XmlAttribute]
        public string Step;
        [XmlIgnore]
        public string Value;

        public FormulaParam()
        {
            this.Value = "0";
            this.DefaultValue = "0";
            this.MinValue = "0";
            this.MaxValue = "0";
            this.ParamType = FormulaParamType.Double;
        }

        public FormulaParam(string ParamName, double DefValue, double MinValue, double MaxValue)
            : this(ParamName, DefValue.ToString(), MinValue.ToString(), MaxValue.ToString(), FormulaParamType.Double)
        {
        }

        public FormulaParam(string ParamName, string DefaultValue, string MinValue, string MaxValue, FormulaParamType ParamType)
            : this()
        {
            this.Name = ParamName;
            this.DefaultValue = DefaultValue;
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
            this.ParamType = ParamType;
        }

        public FormulaParam(string ParamName, string DefValue, string MinValue, string MaxValue, string Description, FormulaParamType ParamType)
            : this(ParamName, DefValue, MinValue, MaxValue, ParamType)
        {
            this.Description = Description;
        }
    }
}

