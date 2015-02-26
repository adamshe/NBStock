namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Xml.Serialization;

    [TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(ObjectBrushEditor), typeof(UITypeEditor))]
    public class ObjectBrush
    {
        private int angle;
        private BrushStyle brushStyle;
        private System.Drawing.Color color;
        private System.Drawing.Color color2;
        private System.Drawing.Drawing2D.HatchStyle hatchStyle;

        public ObjectBrush()
        {
            this.color = System.Drawing.Color.Black;
        }

        public ObjectBrush(BrushStyle brushStyle) : this()
        {
            this.brushStyle = brushStyle;
        }

        public ObjectBrush(System.Drawing.Color Color) : this()
        {
            this.brushStyle = BrushStyle.Solid;
            this.color = Color;
        }

        public Brush GetBrush()
        {
            return this.GetBrush(new RectangleF(0f, 0f, 640f, 480f));
        }

        public Brush GetBrush(RectangleF R)
        {
            switch (this.BrushStyle)
            {
                case BrushStyle.Hatch:
                    return new HatchBrush(this.hatchStyle, this.Color, this.Color2);

                case BrushStyle.Linear:
                    return new LinearGradientBrush(R, this.Color, this.Color2, (float) this.Angle, false);

                case BrushStyle.Empty:
                    return new SolidBrush(System.Drawing.Color.Empty);
            }
            return new SolidBrush(this.color);
        }

        public override string ToString()
        {
            return this.BrushStyle.ToString();
        }

        [XmlIgnore, RefreshProperties(RefreshProperties.All)]
        public byte Alpha
        {
            get
            {
                return this.color.A;
            }
            set
            {
                this.color = System.Drawing.Color.FromArgb(value, this.color);
            }
        }

        [RefreshProperties(RefreshProperties.All), XmlIgnore]
        public byte Alpha2
        {
            get
            {
                return this.color2.A;
            }
            set
            {
                this.color2 = System.Drawing.Color.FromArgb(value, this.color2);
            }
        }

        [XmlAttribute, RefreshProperties(RefreshProperties.All)]
        public int Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
            }
        }

        [RefreshProperties(RefreshProperties.All), XmlAttribute]
        public BrushStyle BrushStyle
        {
            get
            {
                return this.brushStyle;
            }
            set
            {
                this.brushStyle = value;
            }
        }

        [XmlIgnore]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = System.Drawing.Color.FromArgb(this.color.A, value);
            }
        }

        [XmlIgnore]
        public System.Drawing.Color Color2
        {
            get
            {
                return this.color2;
            }
            set
            {
                this.color2 = System.Drawing.Color.FromArgb(this.color2.A, value);
            }
        }

        [RefreshProperties(RefreshProperties.All), XmlAttribute]
        public System.Drawing.Drawing2D.HatchStyle HatchStyle
        {
            get
            {
                return this.hatchStyle;
            }
            set
            {
                this.hatchStyle = value;
            }
        }

        [XmlAttribute(AttributeName="Color"), Browsable(false)]
        public string XmlColor
        {
            get
            {
                return TypeDescriptor.GetConverter(typeof(System.Drawing.Color)).ConvertToString(null, ObjectHelper.enUS, this.Color);
            }
            set
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                this.color = (System.Drawing.Color) converter.ConvertFromString(null, ObjectHelper.enUS, value);
            }
        }

        [XmlAttribute(AttributeName="Color2"), Browsable(false)]
        public string XmlColor2
        {
            get
            {
                return TypeDescriptor.GetConverter(typeof(System.Drawing.Color)).ConvertToString(null, ObjectHelper.enUS, this.Color2);
            }
            set
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                this.color2 = (System.Drawing.Color) converter.ConvertFromString(null, ObjectHelper.enUS, value);
            }
        }
    }
}

