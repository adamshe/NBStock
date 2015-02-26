namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Xml.Serialization;

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(ObjectPenEditor), typeof(UITypeEditor))]
    public class ObjectPen
    {
        private System.Drawing.Color color = System.Drawing.Color.Black;
        private System.Drawing.Drawing2D.DashCap dashCap;
        private System.Drawing.Drawing2D.DashStyle dashStyle;
        private ArrowCap endCap;
        private ArrowCap startCap;
        private int width = 1;

        public Pen GetPen()
        {
            Pen pen = new Pen(this.color, (float) this.width);
            pen.DashStyle = this.DashStyle;
            pen.DashCap = this.DashCap;
            if (((this.startCap != null) && (this.startCap.Width != 0)) && (this.startCap.Height != 0))
            {
                pen.CustomStartCap = new AdjustableArrowCap((float) this.startCap.Width, (float) this.startCap.Height, this.startCap.Filled);
            }
            if (((this.endCap != null) && (this.endCap.Width != 0)) && (this.endCap.Height != 0))
            {
                pen.CustomEndCap = new AdjustableArrowCap((float) this.endCap.Width, (float) this.endCap.Height, this.endCap.Filled);
            }
            return pen;
        }

        public override string ToString()
        {
            return (this.color.Name + "," + this.width);
        }

        [RefreshProperties(RefreshProperties.All), XmlIgnore]
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

        [XmlAttribute, RefreshProperties(RefreshProperties.All)]
        public System.Drawing.Drawing2D.DashCap DashCap
        {
            get
            {
                return this.dashCap;
            }
            set
            {
                this.dashCap = value;
            }
        }

        [RefreshProperties(RefreshProperties.All), XmlAttribute]
        public System.Drawing.Drawing2D.DashStyle DashStyle
        {
            get
            {
                return this.dashStyle;
            }
            set
            {
                this.dashStyle = value;
            }
        }

        public ArrowCap EndCap
        {
            get
            {
                return this.endCap;
            }
            set
            {
                this.endCap = value;
            }
        }

        public ArrowCap StartCap
        {
            get
            {
                return this.startCap;
            }
            set
            {
                this.startCap = value;
            }
        }

        [RefreshProperties(RefreshProperties.All), XmlAttribute]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
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
    }
}

