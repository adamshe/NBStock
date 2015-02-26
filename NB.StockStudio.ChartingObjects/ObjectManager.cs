namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using NB.StockStudio.WinControls;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(IsNullable=true, ElementName="Root")]
    public class ObjectManager
    {
        public static ArrayList alCategory = new ArrayList();
        public static ArrayList AllTypes = new ArrayList();
        [XmlIgnore]
        public IObjectCanvas Canvas;
        private bool ControlSettingSaved;
        [XmlIgnore]
        public Control Designer;
        private Bitmap DragMemBmp;
        private ObjectDragging DragObject;
        private ObjectPoint EndPoint;
        [XmlIgnore]
        public Graphics FormGraphics;
        private static Hashtable htAssembly = new Hashtable();
        private static Hashtable htCategory = new Hashtable();
        private Bitmap MemBmp;
        private ObjectCollection objects;
        private int ObjectSteps;
        [XmlIgnore]
        public ObjectInit ObjectType;
        private PropertyGrid propertyGrid;
        private bool SavedShowCrossCursor;
        private bool SavedShowStatistic;
        private BaseObject selectedObject;
        private ObjectPoint StartPoint;
        private ObjectToolPanel ToolPanel;
        private static XmlSerializer xsReadWrite;

        public event ObjectEventHandler AfterCreateFinished;

        public event ObjectEventHandler AfterCreateStart;

        public event ObjectEventHandler AfterSelect;

        public ObjectManager() : this(null)
        {
        }

        public ObjectManager(IObjectCanvas Canvas) : this(Canvas, null, null)
        {
        }

        public ObjectManager(IObjectCanvas Canvas, PropertyGrid propertyGrid, ObjectToolPanel ToolPanel)
        {
            this.StartPoint = ObjectPoint.Empty;
            this.EndPoint = ObjectPoint.Empty;
            this.objects = new ObjectCollection();
            LoadSettings(ToolPanel);
            this.SetHandler(Canvas, propertyGrid, ToolPanel);
            if (ToolPanel != null)
            {
                ToolPanel.LoadObjectTool();
            }
        }

        public void AddObject(BaseObject bo)
        {
            this.objects.Add(bo);
            this.SetObjectManager(bo);
        }

        public static void BackgroundLoading(object o)
        {
            RegAssembly(Assembly.GetExecutingAssembly());
            RegAssembly(Assembly.GetCallingAssembly());
            CreateSerializer();
            if (o is ObjectToolPanel)
            {
                ((ObjectToolPanel) o).LoadObjectTool();
            }
        }

        public void Clear()
        {
            this.objects.Clear();
            if (this.Designer != null)
            {
                this.Designer.Invalidate();
            }
        }

        public void Copy()
        {
            Clipboard.SetDataObject(this.SelectedObject);
        }

        private void CreateMemBmp()
        {
            if ((this.MemBmp == null) && (this.Canvas != null))
            {
                this.MemBmp = new Bitmap(this.Canvas.BackChart.Rect.Width, this.Canvas.BackChart.Rect.Height, PixelFormat.Format32bppPArgb);
            }
        }

        public static void CreateSerializer()
        {
            if (xsReadWrite == null)
            {
                xsReadWrite = new XmlSerializer(typeof(ObjectManager), (System.Type[]) AllTypes.ToArray(typeof(System.Type)));
            }
        }

        public void Delete()
        {
            int index = this.objects.IndexOf(this.SelectedObject);
            if (index >= 0)
            {
                this.objects.Remove(this.SelectedObject);
                if (index >= this.objects.Count)
                {
                    index = this.objects.Count - 1;
                }
                if (index >= 0)
                {
                    this.SelectedObject = this.objects[index];
                }
                else
                {
                    this.SelectedObject = null;
                }
                this.Designer.Invalidate();
                if (this.AfterSelect != null)
                {
                    this.AfterSelect(this, this.SelectedObject);
                }
            }
        }

        private void Designer_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode == Keys.Delete)
            {
                this.Delete();
            }
            else if (keyCode == Keys.C)
            {
                if (e.Control)
                {
                    this.Copy();
                }
            }
            else if ((keyCode == Keys.V) && e.Control)
            {
                this.Paste();
            }
        }

        private void DesignerControl_MouseDown(object sender, MouseEventArgs e)
        {
            FormulaArea fa = null;
            BaseObject objectAt;
            this.StartPoint = this.GetValueFromPos((float) e.X, (float) e.Y, ref fa);
            this.SaveChartControlSetting();
            if (this.ObjectType == null)
            {
                this.SelectedObject = null;
                if (e.Button == MouseButtons.Left)
                {
                    int controlPointIndex = this.GetPointIndex(e.X, e.Y, out objectAt);
                    if (objectAt == null)
                    {
                        objectAt = this.GetObjectAt(e.X, e.Y);
                    }
                    if (objectAt != null)
                    {
                        this.SelectedObject = objectAt;
                        this.DragObject = new ObjectDragging(new PointF((float) e.X, (float) e.Y), controlPointIndex, objectAt);
                        this.Designer.Invalidate(objectAt.GetRegion());
                        objectAt.InMove = true;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.objects.Remove(this.DragObject.Object);
                this.DragObjectFinished();
            }
            else if (this.ObjectSteps == 0)
            {
                objectAt = this.ObjectType.Invoke();
                objectAt.AreaName = fa.Name;
                objectAt.Area = fa;
                objectAt.InSetup = true;
                objectAt.InMove = true;
                if (this.AfterCreateStart != null)
                {
                    this.AfterCreateStart(this, objectAt);
                }
                objectAt.SetObjectManager(this);
                for (int i = 0; i < objectAt.ControlPointNum; i++)
                {
                    objectAt.ControlPoints[i] = this.StartPoint;
                }
                this.objects.Add(objectAt);
                this.SelectedObject = objectAt;
                this.DragObject = new ObjectDragging(new PointF((float) e.X, (float) e.Y), ((this.ObjectSteps + objectAt.InitNum) > 1) ? 1 : 0, objectAt);
            }
            this.DragMemBmp = null;
        }

        private void DesignerControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.DragObject != null)
            {
                FormulaArea fa = this.DragObject.Object.Area;
                float num = e.X - this.DragObject.StartPoint.X;
                float num2 = e.Y - this.DragObject.StartPoint.Y;
                this.InvalidateObject(this.DragObject.Object);
                if (this.DragObject.ControlPointIndex < 0)
                {
                    for (int i = 0; i < this.DragObject.Object.ControlPoints.Length; i++)
                    {
                        PointF tf = this.DragObject.Object.ToPointF(this.DragObject.ControlPoints[i]);
                        this.DragObject.Object.ControlPoints[i] = this.GetValueFromPos(tf.X + num, tf.Y + num2, ref fa);
                    }
                }
                else
                {
                    BaseObject obj2 = this.DragObject.Object;
                    obj2.ControlPoints[this.DragObject.ControlPointIndex] = this.GetValueFromPos((float) e.X, (float) e.Y, ref fa);
                    if (((obj2.InitNum > 0) && (obj2.InitPoints != null)) && (obj2.InSetup && (this.DragObject.ControlPoints.Length > 1)))
                    {
                        PointF tf2 = obj2.ToPointF(obj2.ControlPoints[1]);
                        PointF tf3 = obj2.ToPointF(obj2.ControlPoints[0]);
                        float num4 = (tf2.X - tf3.X) / obj2.InitPoints[1].X;
                        float num5 = (tf2.Y - tf3.Y) / obj2.InitPoints[1].Y;
                        for (int j = 2; j < obj2.ControlPoints.Length; j++)
                        {
                            obj2.ControlPoints[j] = this.GetValueFromPos(tf3.X + (num4 * obj2.InitPoints[j].X), tf3.Y + (num5 * obj2.InitPoints[j].Y), ref fa);
                        }
                    }
                }
                this.InvalidateObject(this.DragObject.Object);
            }
            else
            {
                Cursor cursor = this.Designer.Cursor;
                if (this.GetPointIndex(e.X, e.Y) >= 0)
                {
                    this.Designer.Cursor = Cursors.SizeAll;
                }
                else if (this.GetObjectAt(e.X, e.Y) != null)
                {
                    this.Designer.Cursor = Cursors.Hand;
                }
                else if (this.ObjectType == null)
                {
                    this.Designer.Cursor = cursor;
                }
                else
                {
                    this.Designer.Cursor = Cursors.Cross;
                }
            }
        }

        private void DesignerControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.ObjectType == null)
            {
                this.DragObjectFinished();
            }
            else if (this.DragObject != null)
            {
                BaseObject obj2 = this.DragObject.Object;
                if ((this.ObjectSteps == 0) && (this.DragObject.ControlPoints.Length > 1))
                {
                    PointF tf = obj2.ToPointF(obj2.ControlPoints[1]);
                    PointF tf2 = obj2.ToPointF(obj2.ControlPoints[0]);
                    if (Math.Sqrt((double) (((tf.X - tf2.X) * (tf.X - tf2.X)) + ((tf.Y - tf2.Y) * (tf.Y - tf2.Y)))) > 20.0)
                    {
                        this.ObjectSteps++;
                    }
                }
                if (this.ObjectSteps != 0)
                {
                    this.DragObject.ControlPointIndex = this.ObjectSteps + 1;
                }
                this.ObjectSteps++;
                if (this.ObjectSteps == obj2.InitNum)
                {
                    this.DragObjectFinished();
                }
            }
        }

        private void DesignerControl_Paint(object sender, PaintEventArgs e)
        {
            Bitmap image = this.ObjectPaint(e.Graphics, null);
            e.Graphics.DrawImage(image, 0, 0);
        }

        private void DesignerControl_SizeChanged(object sender, EventArgs e)
        {
            this.MemBmp = null;
            this.FormGraphics = this.Designer.CreateGraphics();
        }

        private void DragObjectFinished()
        {
            this.RestoreChartControlSetting();
            if ((this.AfterCreateFinished != null) && (this.DragObject != null))
            {
                this.DragObject.Object.InSetup = false;
                this.DragObject.Object.InMove = false;
                this.AfterCreateFinished(this, this.DragObject.Object);
            }
            this.DragObject = null;
            this.ObjectType = null;
            this.Canvas.Designing = false;
            this.ObjectSteps = 0;
            this.Designer.Invalidate();
        }

        public void Draw(Graphics g, BaseObject SelectedObject, BaseObject MovingObject)
        {
            try
            {
                foreach (BaseObject obj2 in this.objects)
                {
                    if (obj2 != MovingObject)
                    {
                        this.DrawObject(g, obj2, obj2 == SelectedObject);
                    }
                }
            }
            catch (Exception exception)
            {
                g.DrawString(exception.Message, new Font("Verdana", 13f), Brushes.Black, (float) 10f, (float) 10f);
            }
        }

        private void DrawMemBmp(Bitmap BackImage, Region Clip)
        {
            this.CreateMemBmp();
            if (this.Designer != null)
            {
                Graphics g = Graphics.FromImage(this.MemBmp);
                g.SetClip(Clip, CombineMode.Replace);
                if (BackImage != null)
                {
                    g.DrawImage(BackImage, 0, 0);
                }
                else
                {
                    g.Clear(this.Designer.BackColor);
                }
                BaseObject movingObject = null;
                if (this.DragObject != null)
                {
                    movingObject = this.DragObject.Object;
                }
                this.Draw(g, this.SelectedObject, movingObject);
            }
        }

        public void DrawObject(Graphics g, BaseObject ob, bool Selected)
        {
            Region clip = null;
            if (ob.Area != null)
            {
                clip = g.Clip;
                g.SetClip(ob.Area.Canvas.Rect, CombineMode.Intersect);
            }
            ob.Draw(g);
            if (Selected)
            {
                ob.DrawControlPoint(g);
            }
            if (clip != null)
            {
                g.SetClip(clip, CombineMode.Replace);
            }
        }

        public static ObjectManager FromChart(FormulaChart fc)
        {
            ObjectManager manager = new ObjectManager(new WebCanvas(fc));
            fc.NativePaint += new NativePaintHandler(manager.ObjectManager_DirectPaint);
            return manager;
        }

        public BaseObject GetObjectAt(int X, int Y)
        {
            foreach (BaseObject obj2 in this.objects)
            {
                if (obj2.InObject(X, Y))
                {
                    return obj2;
                }
            }
            return null;
        }

        public int GetPointIndex(int X, int Y)
        {
            BaseObject obj2;
            return this.GetPointIndex(X, Y, out obj2);
        }

        public int GetPointIndex(int X, int Y, out BaseObject CurrentObject)
        {
            foreach (BaseObject obj2 in this.objects)
            {
                int controlPoint = obj2.GetControlPoint(X, Y);
                if (controlPoint >= 0)
                {
                    CurrentObject = obj2;
                    return controlPoint;
                }
            }
            CurrentObject = null;
            return -1;
        }

        private ObjectPoint GetValueFromPos(float X, float Y, ref FormulaArea fa)
        {
            return this.Canvas.BackChart.GetValueFromPos(X, Y, ref fa);
        }

        private void InvalidateObject(BaseObject ob)
        {
            Region region = ob.GetRegion();
            if (ob.Area != null)
            {
                region.Intersect(ob.Area.Canvas.Rect);
            }
            this.Designer.Invalidate(region);
        }

        public void LoadObject(string Symbol)
        {
            if ((Symbol != null) && (Symbol != ""))
            {
                string objectFile = ObjectHelper.GetObjectFile(Symbol);
                if (File.Exists(objectFile))
                {
                    this.ReadXml(objectFile);
                }
                else
                {
                    this.Clear();
                }
            }
        }

        public static void LoadSettings(ObjectToolPanel ToolPanel)
        {
            if (htAssembly.Count == 0)
            {
                BackgroundLoading(ToolPanel);
            }
        }

        private void ObjectManager_AfterCreateFinished(object sender, BaseObject Object)
        {
            this.propertyGrid.Refresh();
        }

        private void ObjectManager_AfterCreateStart(object sender, BaseObject Object)
        {
            ObjectPen defaultPen = this.ToolPanel.DefaultPen;
            Object.LinePen.Color = defaultPen.Color;
            Object.LinePen.Width = defaultPen.Width;
            Object.LinePen.DashStyle = defaultPen.DashStyle;
        }

        private void ObjectManager_AfterSelect(object sender, BaseObject Object)
        {
            if (this.propertyGrid.SelectedObject != Object)
            {
                this.propertyGrid.SelectedObject = Object;
            }
        }

        public void ObjectManager_DirectPaint(object sender, NativePaintArgs e)
        {
            this.Draw(e.Graphics, null, null);
        }

        public void ObjectManager_ExtraPaint(object sender, NativePaintArgs e)
        {
            e.NewBitmap = this.ObjectPaint(e.Graphics, e.NativeBitmap);
        }

        private Bitmap ObjectPaint(Graphics ObjectG, Bitmap BackImage)
        {
            if (this.DragObject != null)
            {
                bool flag = false;
                if (this.DragMemBmp == null)
                {
                    this.DragMemBmp = new Bitmap(this.MemBmp.Width, this.MemBmp.Height, PixelFormat.Format32bppPArgb);
                    this.DrawMemBmp(BackImage, ObjectG.Clip);
                    flag = true;
                }
                Graphics g = Graphics.FromImage(this.DragMemBmp);
                if (flag)
                {
                    g.DrawImage(this.MemBmp, 0, 0);
                }
                g.SetClip(ObjectG.Clip, CombineMode.Replace);
                if (BackImage != null)
                {
                    g.DrawImage(BackImage, 0, 0);
                }
                g.DrawImage(this.MemBmp, 0, 0);
                this.DrawObject(g, this.DragObject.Object, true);
                return this.DragMemBmp;
            }
            this.DrawMemBmp(BackImage, ObjectG.Clip);
            return this.MemBmp;
        }

        public void Paste()
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            string[] formats = dataObject.GetFormats();
            if (((BaseObject) dataObject.GetData("TestEpg.ObjectBase", true)) != null)
            {
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.Designer.Invalidate();
        }

        public void ReadXml(TextReader reader)
        {
            CreateSerializer();
            ObjectManager manager = (ObjectManager) xsReadWrite.Deserialize(reader);
            this.objects.Clear();
            foreach (BaseObject obj2 in manager.objects)
            {
                this.objects.Add(obj2);
            }
            this.SetCanvas(this.Canvas);
            if (this.Designer != null)
            {
                this.Designer.Invalidate();
            }
        }

        public void ReadXml(string FileName)
        {
            using (TextReader reader = new StreamReader(FileName))
            {
                this.ReadXml(reader);
            }
        }

        private static void RegAssembly(Assembly A)
        {
            if ((A != null) && (htAssembly[A] == null))
            {
                htAssembly[A] = 1;
                foreach (System.Type type in A.GetTypes())
                {
                    if (type.Name.EndsWith("Object"))
                    {
                        object obj2 = type.InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance, null, null, null);
                        if (obj2 is BaseObject)
                        {
                            ObjectInit[] ois = (obj2 as BaseObject).RegObject();
                            if ((ois != null) && (ois.Length > 0))
                            {
                                if (AllTypes.IndexOf(type) < 0)
                                {
                                    xsReadWrite = null;
                                    AllTypes.Add(type);
                                }
                                RegObjects(ois);
                            }
                        }
                    }
                }
            }
        }

        public static void RegObject(ObjectInit oi)
        {
            ObjectCategory category;
            object obj2 = htCategory[oi.Category];
            if (obj2 == null)
            {
                category = new ObjectCategory(oi.Category, oi.CategoryOrder);
                htCategory[oi.Category] = category;
                alCategory.Add(category);
            }
            else
            {
                category = (ObjectCategory) obj2;
                category.Order = Math.Min(oi.CategoryOrder, category.Order);
            }
            category.ObjectList.Add(oi);
        }

        public static void RegObjects(ObjectInit[] ois)
        {
            if (ois != null)
            {
                foreach (ObjectInit init in ois)
                {
                    RegObject(init);
                }
            }
        }

        private void RestoreChartControlSetting()
        {
            if ((this.Designer is ChartWinControl) && this.ControlSettingSaved)
            {
                (this.Designer as ChartWinControl).ShowCrossCursor = this.SavedShowCrossCursor;
                (this.Designer as ChartWinControl).ShowStatistic = this.SavedShowStatistic;
                this.ControlSettingSaved = false;
            }
        }

        private void SaveChartControlSetting()
        {
            if ((this.Designer is ChartWinControl) && !this.ControlSettingSaved)
            {
                this.SavedShowCrossCursor = (this.Designer as ChartWinControl).ShowCrossCursor;
                this.SavedShowStatistic = (this.Designer as ChartWinControl).ShowStatistic;
                this.ControlSettingSaved = true;
                (this.Designer as ChartWinControl).ShowCrossCursor = false;
                (this.Designer as ChartWinControl).ShowStatistic = false;
            }
        }

        public void SaveObject(string Symbol)
        {
            if ((Symbol != null) && (Symbol != ""))
            {
                string objectFile = ObjectHelper.GetObjectFile(Symbol);
                if (File.Exists(objectFile) || (this.objects.Count > 0))
                {
                    this.WriteXml(objectFile);
                }
            }
        }

        public void SetCanvas(IObjectCanvas Canvas)
        {
            this.Canvas = Canvas;
            this.Designer = Canvas.DesignerControl;
            this.SetObjectManager();
            if ((this.Designer != null) && (this.Designer.Tag == null))
            {
                this.Designer.Tag = 1;
                this.Designer.MouseDown += new MouseEventHandler(this.DesignerControl_MouseDown);
                this.Designer.MouseMove += new MouseEventHandler(this.DesignerControl_MouseMove);
                this.Designer.MouseUp += new MouseEventHandler(this.DesignerControl_MouseUp);
                if (this.Designer is ChartWinControl)
                {
                    (this.Designer as ChartWinControl).ExtraPaint += new NativePaintHandler(this.ObjectManager_ExtraPaint);
                }
                else
                {
                    this.Designer.Paint += new PaintEventHandler(this.DesignerControl_Paint);
                }
                this.Designer.SizeChanged += new EventHandler(this.DesignerControl_SizeChanged);
                this.Designer.KeyDown += new KeyEventHandler(this.Designer_KeyDown);
            }
        }

        private void SetHandler(IObjectCanvas Canvas, PropertyGrid propertyGrid, ObjectToolPanel ToolPanel)
        {
            if (Canvas != null)
            {
                this.SetCanvas(Canvas);
            }
            if (propertyGrid != null)
            {
                this.SetPropertyGrid(propertyGrid);
            }
            if (ToolPanel != null)
            {
                this.SetToolPanel(ToolPanel);
                this.AfterCreateStart = (ObjectEventHandler) Delegate.Combine(this.AfterCreateStart, new ObjectEventHandler(this.ObjectManager_AfterCreateStart));
                this.AfterSelect = (ObjectEventHandler) Delegate.Combine(this.AfterSelect, new ObjectEventHandler(this.ObjectManager_AfterSelect));
                this.AfterCreateFinished = (ObjectEventHandler) Delegate.Combine(this.AfterCreateFinished, new ObjectEventHandler(ToolPanel.Manager_AfterCreateFinished));
                this.AfterCreateFinished = (ObjectEventHandler) Delegate.Combine(this.AfterCreateFinished, new ObjectEventHandler(this.ObjectManager_AfterCreateFinished));
            }
        }

        public void SetObjectManager()
        {
            foreach (BaseObject obj2 in this.Objects)
            {
                this.SetObjectManager(obj2);
            }
        }

        public void SetObjectManager(BaseObject bo)
        {
            bo.SetObjectManager(this);
            if (this.Canvas.BackChart != null)
            {
                FormulaArea area = this.Canvas.BackChart[bo.AreaName];
                if (area == null)
                {
                    area = this.Canvas.BackChart[0];
                    bo.AreaName = area.Name;
                }
                bo.Area = area;
            }
        }

        public void SetPropertyGrid(PropertyGrid propertyGrid)
        {
            this.propertyGrid = propertyGrid;
            propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
        }

        public void SetToolPanel(ObjectToolPanel ToolPanel)
        {
            this.ToolPanel = ToolPanel;
            ToolPanel.ToolsChanged += new EventHandler(this.ToolPanel_ToolsChanged);
        }

        public static void ShowObjectOnChart(FormulaChart fc, TextReader reader)
        {
            FromChart(fc).ReadXml(reader);
        }

        public static void ShowObjectOnChart(FormulaChart fc, string FileName)
        {
            if (File.Exists(FileName))
            {
                using (TextReader reader = new StreamReader(FileName))
                {
                    ShowObjectOnChart(fc, reader);
                }
            }
        }

        public static void SortCategory()
        {
            alCategory.Sort(new CompareCategory());
        }

        private void ToolPanel_ToolsChanged(object sender, EventArgs e)
        {
            this.ObjectType = this.ToolPanel.ObjectType;
            this.Canvas.Designing = this.ObjectType != null;
        }

        public void WriteXml(TextWriter writer)
        {
            CreateSerializer();
            xsReadWrite.Serialize(writer, this, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName("EasyChart", "http://finance.easychart.net") }));
        }

        public void WriteXml(string FileName)
        {
            TextWriter writer = new StreamWriter(FileName);
            try
            {
                writer.NewLine = "\r\n";
                this.WriteXml(writer);
            }
            finally
            {
                writer.Close();
            }
        }

        public ObjectCollection Objects
        {
            get
            {
                return this.objects;
            }
            set
            {
                this.objects = value;
            }
        }

        [XmlIgnore]
        public BaseObject SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;
                if ((value != null) && (this.AfterSelect != null))
                {
                    this.AfterSelect(this, value);
                }
            }
        }
    }
}

