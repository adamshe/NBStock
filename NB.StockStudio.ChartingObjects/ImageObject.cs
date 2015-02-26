namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;

    public class ImageObject : BaseObject
    {
        private byte alpha = 160;
        private Bitmap B;
        private ColorMatrix cm = new ColorMatrix();
        private ImageAttributes ia = new ImageAttributes();
        private string imageFile;
        private ImageType imageType = ImageType.Resizable;
        private SnapType snap;

        public ImageObject()
        {
            this.ImageFile = "up.gif";
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            if (this.B != null)
            {
                base.SetSnapPrice(this.snap);
                PointF[] destPoints = base.ToPoints(base.ControlPoints);
                this.cm.Matrix33 = ((float) this.alpha) / 255f;
                this.ia.SetColorMatrix(this.cm);
                Rectangle srcRect = new Rectangle(0, 0, this.B.Width, this.B.Height);
                Rectangle destRect = Rectangle.Truncate(this.GetDestRect(0));
                if (this.imageType == ImageType.Rotate)
                {
                    g.DrawImage(this.B, destPoints, srcRect, GraphicsUnit.Pixel, this.ia);
                }
                else
                {
                    g.DrawImage(this.B, destRect, 0, 0, this.B.Width, this.B.Height, GraphicsUnit.Pixel, this.ia);
                }
            }
        }

        public void FixedImage()
        {
            this.ImageType = ImageType.FixedSize;
        }

        private RectangleF GetDestRect(int w)
        {
            base.SetSnapPrice(this.snap);
            PointF[] c = base.ToPoints(base.ControlPoints);
            if (c.Length == 1)
            {
                c[0].X -= this.B.Width / 2;
            }
            ArrayList list = new ArrayList();
            list.AddRange(c);
            if (this.imageType == ImageType.Rotate)
            {
                list.Add(new PointF((c[1].X - c[0].X) + c[2].X, (c[1].Y - c[0].Y) + c[2].Y));
            }
            else if ((this.imageType == ImageType.FixedSize) && (this.B != null))
            {
                list.Add(new PointF(c[0].X + this.B.Width, c[0].Y + this.B.Height));
            }
            return base.GetMaxRect((PointF[]) list.ToArray(typeof(PointF)), w);
        }

        public override RectangleF GetMaxRect()
        {
            return this.GetDestRect(base.LinePen.Width + 6);
        }

        public override bool InObject(int X, int Y)
        {
            return this.GetMaxRect().Contains((float) X, (float) Y);
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Fixed Image", typeof(ImageObject), "FixedImage", "Image", "ImgFix", 900), new ObjectInit("Resizable Image", typeof(ImageObject), "ResizableImage", "Image", "ImgResize"), new ObjectInit("Rotate Image", typeof(ImageObject), "RotateImage", "Image", "ImgRotate") };
        }

        public void ResizableImage()
        {
            this.ImageType = ImageType.Resizable;
        }

        public void RotateImage()
        {
            this.ImageType = ImageType.Rotate;
        }

        public byte Alpha
        {
            get
            {
                return this.alpha;
            }
            set
            {
                this.alpha = value;
            }
        }

        public override int ControlPointNum
        {
            get
            {
                if (this.imageType == ImageType.FixedSize)
                {
                    return 1;
                }
                if (this.imageType == ImageType.Resizable)
                {
                    return 2;
                }
                return 3;
            }
        }

        [Editor(typeof(ImageFileEditor), typeof(UITypeEditor))]
        public string ImageFile
        {
            get
            {
                return this.imageFile;
            }
            set
            {
                this.imageFile = value;
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                if (codeBase.StartsWith("file:///"))
                {
                    codeBase = codeBase.Substring(8).Replace("/", @"\");
                }
                codeBase = codeBase.Substring(0, codeBase.Length - Path.GetFileName(codeBase).Length) + @"Images\" + value;
                if (File.Exists(codeBase))
                {
                    this.B = (Bitmap) Image.FromFile(codeBase);
                }
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public ImageType ImageType
        {
            get
            {
                return this.imageType;
            }
            set
            {
                this.imageType = value;
                ObjectPoint[] controlPoints = base.ControlPoints;
                base.ControlPoints = new ObjectPoint[this.ControlPointNum];
                for (int i = 0; i < base.ControlPoints.Length; i++)
                {
                    if (i < controlPoints.Length)
                    {
                        base.ControlPoints[i] = controlPoints[i];
                    }
                }
                if (controlPoints.Length < base.ControlPoints.Length)
                {
                    if (controlPoints.Length == 1)
                    {
                        base.ControlPoints[1] = controlPoints[0];
                        if (base.ControlPoints.Length == 2)
                        {
                            base.ControlPoints[1].X += 2.0;
                            base.ControlPoints[1].Y--;
                        }
                        else
                        {
                            base.ControlPoints[2] = controlPoints[0];
                            base.ControlPoints[2].X += 2.0;
                            base.ControlPoints[2].Y--;
                            base.ControlPoints[1].X = base.ControlPoints[2].X;
                            base.ControlPoints[1].Y = base.ControlPoints[0].Y;
                        }
                    }
                    else
                    {
                        base.ControlPoints[0] = controlPoints[0];
                        base.ControlPoints[1].X = controlPoints[1].X;
                        base.ControlPoints[1].Y = controlPoints[0].Y;
                        base.ControlPoints[2].X = controlPoints[0].X;
                        base.ControlPoints[2].Y = controlPoints[1].Y;
                    }
                }
            }
        }

        public override int InitNum
        {
            get
            {
                return this.ControlPointNum;
            }
        }

        public SnapType Snap
        {
            get
            {
                return this.snap;
            }
            set
            {
                this.snap = value;
            }
        }
    }
}

