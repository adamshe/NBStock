namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable, TypeConverter(typeof(ArrowCapConverter)), RefreshProperties(RefreshProperties.Repaint)]
    public class ArrowCap
    {
        private bool filled;
        private int height;
        private int width;

        public ArrowCap()
        {
        }

        public ArrowCap(int Width, int Height, bool Filled)
        {
            this.width = Width;
            this.height = Height;
            this.filled = Filled;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "", this.width, ",", this.height, ",", this.filled });
        }

        [XmlAttribute]
        public bool Filled
        {
            get
            {
                return this.filled;
            }
            set
            {
                this.filled = value;
            }
        }

        [XmlAttribute]
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                if (this.width == 0)
                {
                    this.width = value;
                }
            }
        }

        [XmlAttribute]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
                if (this.height == 0)
                {
                    this.height = value;
                }
            }
        }
    }
}

