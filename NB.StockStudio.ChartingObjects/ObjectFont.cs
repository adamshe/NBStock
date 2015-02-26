namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Xml.Serialization;

    [Editor(typeof(ObjectFontEditor), typeof(UITypeEditor)), TypeConverter(typeof(ExpandableObjectConverter))]
    public class ObjectFont
    {
        private StringAlignment alignment;
        private StringAlignment lineAlignment;
        private ObjectBrush textBrush = new ObjectBrush();
        private Font textFont = new Font("Verdana", 10f);

        public void DrawString(string s, Graphics g, RectangleF Rect)
        {
            StringFormat format = new StringFormat();
            format.Alignment = this.alignment;
            format.LineAlignment = this.lineAlignment;
            g.DrawString(s, this.textFont, this.textBrush.GetBrush(), Rect, format);
        }

        public SizeF Measure(Graphics g, string s)
        {
            return this.Measure(g, s, 0x3e8);
        }

        public SizeF Measure(Graphics g, string s, int w)
        {
            if (g != null)
            {
                StringFormat format = new StringFormat();
                format.Alignment = this.alignment;
                format.LineAlignment = this.lineAlignment;
                return g.MeasureString(s, this.textFont, w, format);
            }
            return SizeF.Empty;
        }

        public override string ToString()
        {
            return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(this.TextFont);
        }

        [RefreshProperties(RefreshProperties.All)]
        public StringAlignment Alignment
        {
            get
            {
                return this.alignment;
            }
            set
            {
                this.alignment = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public StringAlignment LineAlignment
        {
            get
            {
                return this.lineAlignment;
            }
            set
            {
                this.lineAlignment = value;
            }
        }

        public ObjectBrush TextBrush
        {
            get
            {
                return this.textBrush;
            }
            set
            {
                this.textBrush = value;
            }
        }

        [XmlIgnore]
        public Font TextFont
        {
            get
            {
                return this.textFont;
            }
            set
            {
                this.textFont = value;
            }
        }

        [Browsable(false), XmlElement("TextFont")]
        public string XmlTextFont
        {
            get
            {
                return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(null, ObjectHelper.enUS, this.TextFont);
            }
            set
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                this.TextFont = (Font) converter.ConvertFromString(null, ObjectHelper.enUS, value);
            }
        }
    }
}

