namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Windows.Forms.Design;

    public class ImageFileEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                ImgControl control = new ImgControl((string) value, edSvc);
                edSvc.DropDownControl(control);
                return control.ImgName;
            }
            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            string path = ObjectHelper.GetImageRoot() + e.Value;
            if (File.Exists(path))
            {
                Bitmap image = (Bitmap) Image.FromFile(path);
                Rectangle bounds = e.Bounds;
                bounds.Inflate(-1, -1);
                e.Graphics.DrawImage(image, bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }
            base.PaintValue(e);
        }
    }
}

