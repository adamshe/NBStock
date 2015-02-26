namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using NB.StockStudio.Foundation.DataProvider;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Printing;
    using System.Globalization;
    using System.IO;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;

    /// </summary>
    public delegate string OnSelectFormula(string Default, string[] FilterPrefixes, bool SelectLine);

    /// <summary>
    /// Lauch when the charting system want select a string
    /// </summary>
    public delegate string OnSelectString(string Default);

    public class ChartWinControl : UserControl, IObjectCanvas
    {
        private int areaCount = 4;
        public FormulaChart Chart;
        private ChartDragMode chartDragMode;
        private ContextMenu cmMain;
        private ContextMenu cmRight;
        private Container components = null;
        private Graphics ControlGraphics;
        public string CurrentDataCycle;
        private ArrayList dataCycles = new ArrayList();
        private ArrayList defaultFormulas = new ArrayList();
        private bool designing;
        private ChartDragInfo DragInfo;
        public bool EnableResize = true;
        public bool EnableXScale = true;
        public bool EnableYScale = true;
        private ArrayList favoriteFormulas = new ArrayList();
        private int LastCursorPos = -1;
        private IDataProvider LastProvider;
        private int LastX;
        private int LastY;
        private double maxColumnWidth = 500.0;
        private MenuItem miAxisType;
        private MenuItem miCalculator;
        private MenuItem miChart;
        private MenuItem miChartCopy;
        private MenuItem miChartEdit;
        private MenuItem miCopy;
        private MenuItem miCrossCursor;
        private MenuItem miCycle;
        private MenuItem miEdit;
        private MenuItem miFavorite;
        private MenuItem miIndicator;
        private MenuItem miLog;
        private double minColumnWidth = 0.01;
        private MenuItem miNormal;
        private MenuItem miOverlay;
        private MenuItem miParameter;
        private MenuItem miSkin;
        private MenuItem miSp1;
        private MenuItem miSpliter;
        private MenuItem miStatistic;
        private MenuItem miView;
        private FormulaHitInfo MouseDownInfo;
        private bool NeedDrawCursor;
        private bool needRebind;
        private bool needRedraw;
        private bool needRefresh;
        private string overlayFormulas;
        private int Page = 10;
        private PrintPreviewDialog previewDialog;
        private PrintDialog printDialog;
        private PrintDocument printDocument;
        private PageSetupDialog setupDialog;
        private bool showCrossCursor;
        private bool showIndicatorValues;
        private bool showOverlayValues;
        private bool showTopLine;
        private string skin = "RedWhite";
        private bool m_isInitializationMode;

        static public OnSelectFormula onSelectFormulaDelegate = new OnSelectFormula(DefaultSelectFormula);
        static public OnSelectString onSelectMethodDelegate = new OnSelectString(DefaultSelectMethod);
        static public OnSelectString onSelectSymbolDelegate = null;

        [Category("Stock Chart")]
        public event BeforeApplySkin BeforeApplySkin;

        public event CursorPosChanged CursorPosChanged;

        [Category("Stock Chart")]
        public event EventHandler DataChanged;

        [Category("Stock Chart")]
        public event NativePaintHandler ExtraPaint;

        [Category("Stock Chart")]
        public event NativePaintHandler NativePaint;

        [Category("Stock Chart")]
        public event ViewChangedHandler ViewChanged;

        public ChartWinControl()
        {
 
            this.SuspendLayout();
            this.InitializeComponent();
            this.BeginUpdate();
            this.Chart = new FormulaChart();
            this.Chart.BitmapCache = true;
            this.Chart.LatestValueType = this.LatestValueType;
            base.MouseWheel += new MouseEventHandler(this.ChartWinControl_MouseWheel);
            this.Chart.NativePaint += new NativePaintHandler(this.Chart_NativePaint);
            this.Chart.ExtraPaint += new NativePaintHandler(this.Chart_ExtraPaint);
            this.Chart.ViewChanged += new ViewChangedHandler(this.Chart_ViewChanged);
            this.Chart.AddArea("NATIVE.MAIN", 3.0);
            this.FavoriteCycles = "DAY;WEEK;MONTH";
            this.OverlayFormulas = "Extend.BB;NATIVE.MA(3);NATIVE.MA(50);NATIVE.MA(200)";
            this.DefaultFormulas = "VOLMA;SlowSTO;MACD";
            this.FavoriteFormulas = "VOLMA;EXTEND.RSI;CCI;OBV;ATR;FastSTO;SlowSTO;ROC;TRIX;WR;AD;CMF;PPO;StochRSI;ULT;BBWidth;PVO;BIAS";
            this.printDocument.DefaultPageSettings.Landscape = true;
            this.printDocument.DefaultPageSettings.Margins = new Margins(30, 30, 30, 30);
            this.ColumnWidth = 4.0;
            this.CursorPosChanged = (CursorPosChanged)Delegate.Combine(this.CursorPosChanged, new CursorPosChanged(this.ShowStatisticWindow));

            this.SetStyle(ControlStyles.AllPaintingInWmPaint
              | ControlStyles.OptimizedDoubleBuffer
              | ControlStyles.ResizeRedraw
              | ControlStyles.DoubleBuffer
              | ControlStyles.UserPaint
              , true); //ControlStyles.Opaque  will disable OnPaintBackground
            this.ResumeLayout(false);
            EndUpdate();
            /*
             *  when no styles are set, your controls code looks like this behind the scenes:

 this.OnPaintBackground()
 this.OnPaint()
If you turn DoubleBuffer on, it looks like this:

 this.OnPaintBackground()
 this.BeginUpdate()
 this.OnPaint()
 this.EndUpdate()
And if you turn on the AllPaintingInWmPaint style, it looks like this:

 this.BeginUpdate ()
 this.OnPaint ()
 this.EndUpdate ()
             * */
        }

        public void BeginUpdate ()
        {
            m_isInitializationMode = true;
        }

        public void EndUpdate()
        {
            m_isInitializationMode = false;
        }

        public void ApplySkin()
        {
            FormulaSkin skinByName = FormulaSkin.GetSkinByName(this.skin);
            this.ApplySkin(skinByName);
        }

        public void ApplySkin(FormulaSkin fs)
        {
            if (fs != null)
            {
                if (this.BeforeApplySkin != null)
                {
                    this.BeforeApplySkin(fs);
                }
                this.Chart.SetSkin(fs);
                this.Chart[0].Back.TopPen.Width = this.ShowTopLine ? ((float)2) : ((float)0);
                this.Chart[0].AxisY.Back.TopPen.Width = this.ShowTopLine ? ((float)2) : ((float)0);
                this.Chart[0].AxisY.AutoMultiply = false;
                this.SetDataCycle(false);
            }
        }

        public void AutoScaleAxisY()
        {
            foreach (FormulaArea area in this.Chart.Areas)
            {
                foreach (FormulaAxisY sy in area.AxisYs)
                {
                    sy.AutoScale = true;
                }
            }
            this.NeedRedraw();
        }

        private void BuildFavoriteMenu()
        {
            this.miFavorite.MenuItems.Clear();
            foreach (string str in this.favoriteFormulas)
            {
                this.miFavorite.MenuItems.Add(str, new EventHandler(this.mmFavorite_SelectedIndexChanged));
            }
        }

        private void ChangeCursorPos()
        {
            if (((this.Chart != null) && (this.Chart.DataProvider != null)) && ((this.LastCursorPos != this.Chart.CursorPos) || (this.LastProvider != this.Chart.DataProvider)))
            {
                IDataProvider dataProvider = this.Chart.DataProvider;
                if ((this.Chart.CursorPos >= 0) && (this.Chart.CursorPos < dataProvider.Count))
                {
                    this.LastProvider = this.Chart.DataProvider;
                    this.LastCursorPos = this.Chart.CursorPos;
                    if (this.CursorPosChanged != null)
                    {
                        this.CursorPosChanged(this.Chart, this.LastCursorPos, dataProvider);
                    }
                }
            }
        }

        public void ChangeParameter()
        {
            FormulaArea selectedArea = this.Chart.SelectedArea;
            if ((selectedArea != null) && (selectedArea != this.Chart[0]))
            {
                string formulaName = SelectFormula.Select(selectedArea.Formulas[0].FormulaName);
                if ((formulaName != null) && (formulaName != ""))
                {
                    this.SetAreaByName(selectedArea, formulaName);
                }
            }
        }

        private void Chart_ExtraPaint(object sender, NativePaintArgs e)
        {
            if (this.ExtraPaint != null)
            {
                this.ExtraPaint(this, e);
            }
           // var canvas = sender as FormulaChart;
           // WaterMark(e.Graphics, canvas.Areas[0].Rect, this.Chart.Code);  
        }

        private void Chart_NativePaint(object sender, NativePaintArgs e)
        {
            if (this.NativePaint != null)
            {
                this.NativePaint(this, e);
            }
            //WaterMark(e.Graphics, e.Rect, this.Chart.Code);       
        }

        private void Chart_ViewChanged(object sender, ViewChangedArgs e)
        {
            if (this.ViewChanged != null)
            {
                this.ViewChanged(this, e);
            }
        }

        private void ChartWinControl_DoubleClick(object sender, EventArgs e)
        {
            if (this.MouseDownInfo.HitType == FormulaHitType.htAxisY)
            {
                this.MouseDownInfo.AxisY.AutoScale = true;
                this.NeedRedraw();
            }
        }

        private void ChartWinControl_Enter(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;
            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
               switch(keyData)
               {
                   case Keys.Control | Keys.Add:
                       // Chart.sc
                        return true;
                   case Keys.Control | Keys.Subtract:
                        MessageBox.Show("What the Ctrl+F?");
                        return true;
                    
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ChartWinControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Chart != null)
            {
                int start = this.Chart.Start;
                int cursorPos = this.Chart.CursorPos;
                int num3 = e.Control ? this.Page : 1;
                switch (e.KeyCode)
                {
                    case Keys.Divide:
                        this.NextFormula(-1);
                        e.Handled = true;
                        break;

                    case Keys.F8:
                        this.NextCycle(1);
                        e.Handled = true;
                        break;

                    case Keys.End:
                        this.Chart.AdjustCursorByPos(this.ControlGraphics, 0x7fffffff, this.Chart.Start);
                        e.Handled = true;
                        break;

                    case Keys.Home:
                        this.Chart.AdjustCursorByPos(this.ControlGraphics, 0, this.Chart.Start);
                        e.Handled = true;
                        break;

                    case Keys.Left:
                        this.Chart.AdjustCursorByPos(this.ControlGraphics, this.Chart.CursorPos - num3, this.Chart.Start + (e.Alt ? num3 : 0));
                        e.Handled = true;
                        break;

                    case Keys.Up:
                        if (!e.Control)
                        {
                            if (this.ColumnWidth < this.MaxColumnWidth)
                            {
                                this.ColumnWidth *= 1.05;
                                e.Handled = true;
                            }
                            break;
                        }
                        this.NextArea(-1);
                        e.Handled = true;
                        break;

                    case Keys.Right:
                        this.Chart.AdjustCursorByPos(this.ControlGraphics, this.Chart.CursorPos + num3, this.Chart.Start - (e.Alt ? num3 : 0));
                        e.Handled = true;
                        break;

                    case Keys.Down:
                        if (!e.Control)
                        {
                            if (this.ColumnWidth > this.MinColumnWidth)
                            {
                                this.ColumnWidth /= 1.05;
                                e.Handled = true;
                            }
                            break;
                        }
                        this.NextArea(1);
                        e.Handled = true;
                        break;

                    case Keys.Multiply:
                        this.NextFormula(1);
                        e.Handled = true;
                        break;
                }
                if (e.Handled)
                {
                    this.ChangeCursorPos();
                }
                if ((cursorPos != this.Chart.CursorPos) && (start != this.Chart.Start))
                {
                    this.NeedRedraw();
                }
                if (this.needRedraw)
                {
                    this.Chart.StartTime = DateTime.MinValue;
                    this.Chart.EndTime = DateTime.MaxValue;
                    this.NeedDrawCursor = true;
                }
                else
                {
                    this.Chart.DrawCursor(this.ControlGraphics);
                }
            }
        }

        private void ChartWinControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.MouseDownInfo = this.Chart.GetHitInfo((float)e.X, (float)e.Y);
            if ((this.MouseDownInfo.Area != null) && (this.MouseDownInfo.Area != this.Chart[0]))
            {
                this.Chart.SelectedArea = this.MouseDownInfo.Area;
                this.NeedRedraw();
            }
            if ((e.Button == MouseButtons.Right) && !this.Designing)
            {
                if (base.Visible)
                {
                    this.cmRight.Show(this, new Point(e.X, e.Y));
                }
            }
            else if (!this.Designing)
            {
                this.DragInfo = new ChartDragInfo(this.Chart, this.MouseDownInfo);
            }
        }

        private void ChartWinControl_MouseLeave(object sender, EventArgs e)
        {
            if (this.Chart != null)
            {
                this.Chart.HideCursor(this.ControlGraphics);
            }
        }

        private void ChartWinControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!this.Designing)
                {
                    int part = 3 - ((this.ChartDragMode == ChartDragMode.Chart) ? 1 : 0);
                    if ((this.LastX == e.X) && (this.LastY == e.Y))
                    {
                        return;
                    }
                    FormulaHitInfo hitInfo = this.Chart.GetHitInfo((float)e.X, (float)e.Y);
                    if (this.DragInfo != null)
                    {
                        float deltaY = this.DragInfo.HitInfo.Y - hitInfo.Y;
                        float deltaX = this.DragInfo.HitInfo.X - hitInfo.X;
                        FormulaArea fa = this.DragInfo.HitInfo.Area;
                        if (this.EnableResize && (this.DragInfo.HitInfo.HitType == FormulaHitType.htSize))
                        {
                            for (int i = 0; i < this.Chart.Areas.Count; i++)
                            {
                                this.Chart.Areas[i].HeightPercent = this.DragInfo.AreaHeight[i];
                            }
                            int index = this.Chart.Areas.IndexOf(fa);
                            if (index < (this.Chart.Areas.Count - 1))
                            {
                                FormulaArea area2 = this.Chart.Areas[index + 1];
                                double num6 = this.DragInfo.AreaHeight[index] - deltaY;
                                double num7 = this.DragInfo.AreaHeight[index + 1] + deltaY;
                                if (num6 < 40.0)
                                {
                                    num7 -= 40.0 - num6;
                                    num6 = 40.0;
                                }
                                if (num7 < 40.0)
                                {
                                    num6 -= 40.0 - num7;
                                    num7 = 40.0;
                                }
                                fa.HeightPercent = num6;
                                area2.HeightPercent = num7;
                                this.NeedRedraw(fa);
                                this.NeedRedraw(area2);
                            }
                        }
                        else if (this.ChartDragMode != ChartDragMode.None)
                        {
                            if (this.EnableXScale && (this.DragInfo.HitInfo.HitType == FormulaHitType.htAxisX))
                            {
                                this.MoveChartX(fa, part, this.DragInfo.HitInfo.XPart(part), deltaX);
                            }
                            else if (this.EnableYScale && (this.DragInfo.HitInfo.HitType == FormulaHitType.htAxisY))
                            {
                                this.MoveChartY(fa, part, this.DragInfo.HitInfo.YPart(part), deltaY);
                            }
                            else if ((this.ChartDragMode == ChartDragMode.Chart) && (this.DragInfo.HitInfo.HitType == FormulaHitType.htArea))
                            {
                                this.MoveChartX(fa, 3, 1, deltaX);
                                this.MoveChartY(fa, 3, 1, deltaY);
                            }
                        }
                        return;
                    }
                    this.Chart.DrawCursor(this.ControlGraphics, (float)e.X, (float)e.Y, false);                   
                    this.Chart.CursorPos = hitInfo.CursorPos;
                    this.ChangeCursorPos();
                    this.LastX = e.X;
                    this.LastY = e.Y;
                    if (hitInfo.Area == null)
                    {
                        return;
                    }
                    switch (hitInfo.HitType)
                    {
                        case FormulaHitType.htSize:
                            if (this.EnableResize)
                            {
                                this.Cursor = Cursors.HSplit;
                            }
                            return;

                        case FormulaHitType.htAxisX:
                            {
                                if (!this.EnableXScale || (this.ChartDragMode == ChartDragMode.None))
                                {
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                int num8 = hitInfo.XPart(part);
                                if ((num8 != 0) && (num8 != (part - 1)))
                                {
                                    break;
                                }
                                this.Cursor = Cursors.SizeWE;
                                return;
                            }
                        case FormulaHitType.htAxisY:
                            {
                                if (!this.EnableYScale || (this.ChartDragMode == ChartDragMode.None))
                                {
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                int num9 = hitInfo.YPart(part);
                                if ((num9 != 0) && (num9 != (part - 1)))
                                {
                                    this.Cursor = Cursors.Hand;
                                    return;
                                }
                                this.Cursor = Cursors.SizeNS;
                                return;
                            }
                        default:
                            this.Cursor = Cursors.Default;
                            break;
                    }
                    this.Cursor = Cursors.Hand;
                    //FormulaChart.WaterMark(ControlGraphics, this.Chart.Areas[0].Rect, this.Chart.Code);  
                }
            }
            catch
            {
            }
        }

        private void ChartWinControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.DragInfo != null)
            {
                this.DragInfo = null;
                base.Invalidate();
            }
        }

        private void ChartWinControl_MouseWheel(object sender, MouseEventArgs e)
        {
            int num = Math.Sign(e.Delta) * (((Control.ModifierKeys & Keys.Alt) != Keys.None) ? 1 : 12);
            if (this.Chart != null)
            {
                this.Chart.AdjustCursorByPos(this.ControlGraphics, this.Chart.CursorPos, this.Chart.Start - num, false);
                this.Chart.StartTime = DateTime.MinValue;
                this.Chart.EndTime = DateTime.MaxValue;
                this.NeedRedraw();
            }
        }

        private void ChartWinControl_Paint(object sender, PaintEventArgs e)
        {
            if (this.Chart != null)
            {
                if (this.Chart.Areas.Count != this.areaCount)
                {
                    while (this.Chart.Areas.Count > this.areaCount)
                    {
                        this.Chart.Areas.RemoveAt(this.areaCount);
                    }
                    this.ExpandDefaultFormulas(this.areaCount);
                    double percent = 1.0;
                    if (this.Chart.Areas.Count > 0)
                    {
                        percent = this.Chart[0].HeightPercent / 3.0;
                    }
                    for (int i = this.Chart.Areas.Count; i < this.areaCount; i++)
                    {
                        this.Chart.AddArea(this.defaultFormulas[i - 1].ToString(), percent);
                    }
                }

                if (this.needRefresh)
                {
                    this.RecreateFormula();
                }
                if (this.needRebind)
                {
                    this.Chart.Bind();
                    this.ApplySkin();
                }
                if (this.needRedraw)
                {
                    this.Chart.NeedRedraw = true;
                }
                this.Chart[0].Formulas[0].TextInvisible = true;
                this.Chart.Render(e.Graphics);
                if (this.NeedDrawCursor)
                {
                    this.Chart.DrawCursor(e.Graphics);
                    this.NeedDrawCursor = false;
                }
                FormulaChart.WaterMark(e.Graphics, Chart.Areas[0].Rect, this.Chart.Code);
                this.needRedraw = false;
                this.needRebind = false;
                this.needRefresh = false;
            }
        }

        private void ChartWinControl_SizeChanged(object sender, EventArgs e)
        {
            this.ControlGraphics = base.CreateGraphics();
            if (this.Chart != null)
            {
                Rectangle clientRectangle = base.ClientRectangle;
                this.Chart.Rect = clientRectangle;
            }
            else
            {
                this.ControlGraphics.Clear(this.BackColor);
            }
            if (!m_isInitializationMode)
             this.NeedRedraw();
        }

        public void CopyAreaToClipboard(string Separatar)
        {
            Clipboard.SetDataObject(this.Chart.GetAreaTextData(Separatar, true));
        }

        public void CopyToClipboard(string Separatar)
        {
            Clipboard.SetDataObject(this.Chart.GetChartTextData(Separatar, true));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExpandDefaultFormulas(int Count)
        {
            while (Count >= this.defaultFormulas.Count)
            {
                this.defaultFormulas.Add("VOLMA");
            }
        }

        public MenuItem GetAreaMenu(int Count)
        {
            this.miIndicator.MenuItems.Clear();
            for (int i = 0; i < Count; i++)
            {
                this.miIndicator.MenuItems.Add(new MenuItem(string.Concat(new object[] { "&", i + 1, " area", (i > 0) ? "s" : "" }), new EventHandler(this.miIndicator_SelectedIndexChanged), (Shortcut)(0x40031 + i)));
            }
            return this.miIndicator;
        }

        public MenuItem GetAxisMenu()
        {
            return this.miAxisType;
        }

        public MenuItem GetChartMenu()
        {
            this.miChart.MenuItems.Clear();
            this.miChart.MenuItems.Add(this.GetSkinMenu());
            this.miChart.MenuItems.Add(this.GetCycleMenu());
            this.miChart.MenuItems.Add(this.GetAreaMenu(9));
            this.miChart.MenuItems.Add(this.GetAxisMenu());
            return this.miChart;
        }

        public MenuItem GetCycleMenu()
        {
            string[] strArray = new string[] { "DAY", "WEEK", "MONTH", "QUARTER", "YEAR" };
            this.FavoriteCycles = string.Join(";", strArray);
            this.miCycle.MenuItems.Clear();
            foreach (string str in strArray)
            {
                this.miCycle.MenuItems.Add(str, new EventHandler(this.miCycle_SelectedIndexChanged));
            }
            this.miCycle.MenuItems[0].PerformClick();
            return this.miCycle;
        }

        public MenuItem GetEditMenu()
        {
            return this.miChartEdit;
        }

        public MenuItem GetSkinMenu()
        {
            string[] buildInSkins = FormulaSkin.GetBuildInSkins();
            this.miSkin.MenuItems.Clear();
            foreach (string str in buildInSkins)
            {
                this.miSkin.MenuItems.Add(str, new EventHandler(this.mmSkin_SelectedIndexChanged));
            }
            return this.miSkin;
        }

        public MenuItem GetViewMenu()
        {
            return this.miView;
        }

        public void HandleKeyEvent(KeyEventArgs e)
        {
            this.ChartWinControl_KeyDown(null, e);
        }

        public bool HasData()
        {
            return ((this.Chart != null) && (this.Chart.DataProvider != null));
        }

        private void InitializeComponent()
        {
            BeginUpdate();
            ResourceManager manager = new ResourceManager(typeof(ChartWinControl));
            this.cmRight = new ContextMenu();
            this.miFavorite = new MenuItem();
            this.miParameter = new MenuItem();
            this.miEdit = new MenuItem();
            this.miOverlay = new MenuItem();
            this.miSpliter = new MenuItem();
            this.miCopy = new MenuItem();
            this.cmMain = new ContextMenu();
            this.miChart = new MenuItem();
            this.miSkin = new MenuItem();
            this.miCycle = new MenuItem();
            this.miIndicator = new MenuItem();
            this.miAxisType = new MenuItem();
            this.miNormal = new MenuItem();
            this.miLog = new MenuItem();
            this.miView = new MenuItem();
            this.miCrossCursor = new MenuItem();
            this.miStatistic = new MenuItem();
            this.miSp1 = new MenuItem();
            this.miCalculator = new MenuItem();
            this.miChartEdit = new MenuItem();
            this.miChartCopy = new MenuItem();
            this.printDialog = new PrintDialog();
            this.printDocument = new PrintDocument();
            this.previewDialog = new PrintPreviewDialog();
            this.setupDialog = new PageSetupDialog();
            this.cmRight.MenuItems.AddRange(new MenuItem[] { this.miFavorite, this.miParameter, this.miEdit, this.miOverlay, this.miSpliter, this.miCopy });
            this.miFavorite.Index = 0;
            this.miFavorite.Text = "&Favorite Indicators";
            this.miParameter.Index = 1;
            this.miParameter.MergeOrder = 100;
            this.miParameter.Text = "&Change Parameters";
            this.miParameter.Click += new EventHandler(this.miParameter_Click);
            this.miEdit.Index = 2;
            this.miEdit.MergeOrder = 200;
            this.miEdit.Shortcut = Shortcut.CtrlE;
            this.miEdit.Text = "&Edit Formula";
            this.miEdit.Click += new EventHandler(this.miEdit_Click);
            this.miOverlay.Index = 3;
            this.miOverlay.Text = "&Overlay Editor";
            this.miOverlay.Click += new EventHandler(this.miOverlay_Click);
            this.miSpliter.Index = 4;
            this.miSpliter.MergeOrder = 300;
            this.miSpliter.Text = "-";
            this.miCopy.Index = 5;
            this.miCopy.MergeOrder = 400;
            this.miCopy.Shortcut = Shortcut.CtrlC;
            this.miCopy.Text = "&Copy";
            this.miCopy.Click += new EventHandler(this.miCopy_Click);
            this.cmMain.MenuItems.AddRange(new MenuItem[] { this.miChart, this.miView, this.miChartEdit });
            this.miChart.Index = 0;
            this.miChart.MenuItems.AddRange(new MenuItem[] { this.miSkin, this.miCycle, this.miIndicator, this.miAxisType });
            this.miChart.Text = "&Chart";
            this.miSkin.Index = 0;
            this.miSkin.Text = "&Skin";
            this.miCycle.Index = 1;
            this.miCycle.Text = "&Cycle";
            this.miCycle.Click += new EventHandler(this.miCycle_Popup);
            this.miIndicator.Index = 2;
            this.miIndicator.Text = "&Indicator Areas";
            this.miAxisType.Index = 3;
            this.miAxisType.MenuItems.AddRange(new MenuItem[] { this.miNormal, this.miLog });
            this.miAxisType.Text = "&Axis Type";
            this.miNormal.Index = 0;
            this.miNormal.RadioCheck = true;
            this.miNormal.Shortcut = Shortcut.CtrlN;
            this.miNormal.Text = "Normal";
            this.miNormal.Click += new EventHandler(this.miNormalAxis_Click);
            this.miLog.Index = 1;
            this.miLog.MergeOrder = 1;
            this.miLog.RadioCheck = true;
            this.miLog.Shortcut = Shortcut.CtrlL;
            this.miLog.Text = "Logarithm";
            this.miLog.Click += new EventHandler(this.miNormalAxis_Click);
            this.miView.Index = 1;
            this.miView.MenuItems.AddRange(new MenuItem[] { this.miCrossCursor, this.miStatistic, this.miSp1, this.miCalculator });
            this.miView.Text = "&View";
            this.miView.Popup += new EventHandler(this.miView_Popup);
            this.miCrossCursor.Checked = true;
            this.miCrossCursor.Index = 0;
            this.miCrossCursor.Text = "Cross Cursor";
            this.miCrossCursor.Click += new EventHandler(this.miCrossCursor_Click);
            this.miStatistic.Index = 1;
            this.miStatistic.Text = "Statistic Window";
            this.miStatistic.Click += new EventHandler(this.miStatistic_Click);
            this.miSp1.Index = 2;
            this.miSp1.Text = "-";
            this.miCalculator.Index = 3;
            this.miCalculator.Shortcut = Shortcut.CtrlJ;
            this.miCalculator.Text = "&Calculator";
            this.miCalculator.Click += new EventHandler(this.miCalculator_Click);
            this.miChartEdit.Index = 2;
            this.miChartEdit.MenuItems.AddRange(new MenuItem[] { this.miChartCopy });
            this.miChartEdit.Text = "&Edit";
            this.miChartCopy.Index = 0;
            this.miChartCopy.Text = "&Copy";
            this.miChartCopy.Click += new EventHandler(this.miChartCopy_Click);
            this.printDialog.Document = this.printDocument;
            this.printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            this.previewDialog.AutoScrollMargin = new Size(0, 0);
            this.previewDialog.AutoScrollMinSize = new Size(0, 0);
            this.previewDialog.ClientSize = new Size(400, 300);
            this.previewDialog.Document = this.printDocument;
            this.previewDialog.Enabled = true;
         //   this.previewDialog.Icon = (Icon)manager.GetObject("previewDialog.Icon");
            this.previewDialog.Location = new Point(0x1a1, 0x11);
            this.previewDialog.MinimumSize = new Size(0x177, 250);
            this.previewDialog.Name = "previewDialog";
            this.previewDialog.TransparencyKey = Color.Empty;
            this.previewDialog.Visible = false;
            this.setupDialog.Document = this.printDocument;
            base.CausesValidation = false;
            base.Name = "ChartWinControl";
            base.Size = new Size(0x218, 360);
            base.SizeChanged += new EventHandler(this.ChartWinControl_SizeChanged);
            base.Enter += new EventHandler(this.ChartWinControl_Enter);
            base.MouseUp += new MouseEventHandler(this.ChartWinControl_MouseUp);
            base.Paint += new PaintEventHandler(this.ChartWinControl_Paint);
            base.KeyDown += new KeyEventHandler(this.ChartWinControl_KeyDown);
            base.DoubleClick += new EventHandler(this.ChartWinControl_DoubleClick);
            base.MouseMove += new MouseEventHandler(this.ChartWinControl_MouseMove);
            base.MouseLeave += new EventHandler(this.ChartWinControl_MouseLeave);
            base.MouseDown += new MouseEventHandler(this.ChartWinControl_MouseDown);
            EndUpdate();
        }

        public void MergeRightMenu(ContextMenu cm)
        {
            this.cmRight.MergeMenu(cm);
        }

        private void miCalculator_Click(object sender, EventArgs e)
        {
            Process.Start("Calc.exe");
        }

        private void miChartCopy_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard(",");
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            this.CopyAreaToClipboard(",");
        }

        private void miCrossCursor_Click(object sender, EventArgs e)
        {
            this.ShowCrossCursor = !this.ShowCrossCursor;
        }

        private void miCycle_Popup(object sender, EventArgs e)
        {
            foreach (MenuItem item in this.miCycle.MenuItems)
            {
                item.Checked = item.Text == this.CurrentDataCycle;
            }
        }

        private void miCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetDataCycle((sender as MenuItem).Text);
        }

        private void miEdit_Click(object sender, EventArgs e)
        {
            FormulaArea selectedArea = this.Chart.SelectedArea;
            if ((selectedArea != null) && (selectedArea.Formulas.Count > 0))
            {
                FormulaBase fb = selectedArea.Formulas[0];
                string formulaFile = PluginManager.GetFormulaFile(fb);
                if (formulaFile != null)
                {
                    string directoryName = Path.GetDirectoryName(formulaFile);
                    formulaFile = Path.GetFileNameWithoutExtension(formulaFile).Replace('_', '.');
                    formulaFile = directoryName + @"\" + formulaFile;
                    if (File.Exists(formulaFile))
                    {
                        OpenFormulaEditor(formulaFile, fb.GetType().ToString());
                        return;
                    }
                }
            }
            MessageBox.Show("Can't edit the source code of this formula");
        }

        private void miIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.AreaCount = (sender as MenuItem).Index + 1;
        }

        private void miNormalAxis_Click(object sender, EventArgs e)
        {
            foreach (MenuItem item in this.miAxisType.MenuItems)
            {
                item.Checked = false;
            }
            MenuItem item2 = sender as MenuItem;
            item2.Checked = true;
            this.SetScaleType((ScaleType)item2.MergeOrder);
        }

        private void miOverlay_Click(object sender, EventArgs e)
        {
            Overlay overlay = new Overlay();
            overlay.CurrentOverlay = this.overlayFormulas;
            if (overlay.ShowDialog() == DialogResult.OK)
            {
                this.OverlayFormulas = overlay.CurrentOverlay;
                this.NeedRebind();
            }
        }

        private void miParameter_Click(object sender, EventArgs e)
        {
            this.ChangeParameter();
        }

        private void miStatistic_Click(object sender, EventArgs e)
        {
            this.ShowStatistic = !this.ShowStatistic;
        }

        private void miView_Popup(object sender, EventArgs e)
        {
            this.miStatistic.Checked = StatisticForm.Enable;
        }

        private void mmFavorite_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetAreaByName(this.Chart.SelectedArea, (sender as MenuItem).Text);
        }

        private void mmSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (MenuItem item in this.miSkin.MenuItems)
            {
                item.Checked = false;
            }
            MenuItem item2 = (MenuItem)sender;
            item2.Checked = true;
            string text = item2.Text;
            this.Skin = text;
        }

        private void MoveChartX(FormulaArea fa, int Part, int CurrentPart, float DeltaX)
        {
            if (DeltaX != 0f)
            {
                TimeSpan span = (TimeSpan)(this.Chart.EndTime - this.Chart.StartTime);
                double num = (span.TotalDays * DeltaX) / ((double)fa.Rect.Width);
                if (CurrentPart > 0)
                {
                    this.Chart.EndTime = this.DragInfo.EndTime.AddDays(num);
                }
                if (CurrentPart < (Part - 1))
                {
                    this.Chart.StartTime = this.DragInfo.StartTime.AddDays(num);
                }
                this.NeedRedraw();
            }
        }

        private void MoveChartY(FormulaArea fa, int Part, int CurrentPart, float DeltaY)
        {
            if (DeltaY != 0f)
            {
                fa.AxisY.AutoScale = false;
                double num = ((this.DragInfo.AreaMaxY - this.DragInfo.AreaMinY) / ((double)fa.Rect.Height)) * DeltaY;
                if (CurrentPart > 0)
                {
                    fa.AxisY.MinY = this.DragInfo.AreaMinY - num;
                }
                if (CurrentPart < (Part - 1))
                {
                    fa.AxisY.MaxY = this.DragInfo.AreaMaxY - num;
                }
                this.NeedRedraw(fa);
            }
        }

        public void NeedRebind()
        {
            this.needRedraw = true;
            this.needRebind = true;
            base.Invalidate();
        }

        public void NeedRedraw()
        {
            this.NeedRedraw(null);
        }

        public void NeedRedraw(FormulaArea fa)
        {
            this.needRedraw = true;
            if (fa != null)
            {
                Rectangle rect = fa.Rect;
                rect.Inflate(1, 1);
               // WaterMark(ControlGraphics, Chart.Areas[0].Rect, this.Chart.Code);
                base.Invalidate(rect);                
            }
            else
            {
                base.Invalidate();
            }
            
        }

        public void Clean ()
        {
            this.needRedraw = false;
            this.needRebind = false;
            this.needRefresh = false;
        }

        public void NeedRefresh()
        {
            this.needRedraw = true;
            this.needRebind = true;
            this.needRefresh = true;
            base.Invalidate();
        }

        private void NextArea(int Delta)
        {
            int index = this.Chart.Areas.IndexOf(this.Chart.SelectedArea);
            if (index < 0)
            {
                index = this.Chart.Areas.Count - 1;
            }
            int num2 = index;
            do
            {
                num2 = ((num2 + Delta) + this.Chart.Areas.Count) % this.Chart.Areas.Count;
                if (num2 == index)
                {
                    return;
                }
            }
            while (this.Chart[num2].IsMain());
            this.Chart.SelectedArea = this.Chart[num2];
            this.NeedRedraw();
        }

        private void NextCycle(int Delta)
        {
            int index = this.dataCycles.IndexOf(this.CurrentDataCycle);
            if (index < 0)
            {
                index = 0;
            }
            index = ((index + Delta) + this.dataCycles.Count) % this.dataCycles.Count;
            this.SetDataCycle(this.dataCycles[index].ToString());
        }

        private void NextFormula(int Delta)
        {
            int index = this.Chart.Areas.IndexOf(this.Chart.SelectedArea);
            if (index < 0)
            {
                index = this.Chart.Areas.Count - 1;
            }
            if (index >= 1)
            {
                string str = this.defaultFormulas[index - 1].ToString();
                int length = str.IndexOf('(');
                if (length >= 0)
                {
                    str = str.Substring(0, length);
                }
                int num3 = this.favoriteFormulas.IndexOf(str);
                if (num3 < 0)
                {
                    num3 = 0;
                }
                num3 = ((num3 + Delta) + this.favoriteFormulas.Count) % this.favoriteFormulas.Count;
                this.SetAreaByName(index, this.favoriteFormulas[num3].ToString());
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if ((this.Chart == null) || (this.Chart.DataProvider == null))
            {
                base.OnPaintBackground(pevent);
            }
         //   WaterMark(pevent.Graphics, pevent.ClipRectangle, Chart.Code);
        }


        #region Static
        
        public static void OpenFormulaEditor()
        {
            OpenFormulaEditor("", "");
        }

        public static void OpenFormulaEditor(string Filename, string Formula)
        {
            FormulaEditor.Open(Filename, Formula);
        }

        static public string DoSelectFormula(string Default, string[] FilterPrefixes, bool SelectLine)
        {
            if (onSelectFormulaDelegate != null)
                return onSelectFormulaDelegate(Default, FilterPrefixes, SelectLine);
            return Default;
        }

        static private string DefaultSelectFormula(string Default, string[] FilterPrefixes, bool SelectLine)
        {
            return new SelectFormula().Select(Default, FilterPrefixes, SelectLine);
        }

        static private string DefaultSelectMethod(string Default)
        {
            return new SelectMethod().Select(Default);
        }

        static public string DoSelectSymbol(string Default)
        {
            if (onSelectSymbolDelegate != null)
                return onSelectSymbolDelegate(Default);
            return Default;
        }

        #endregion

        public void Print()
        {
            if (this.printDialog.ShowDialog() == DialogResult.OK)
            {
                this.printDocument.Print();
            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap memBitmap = this.Chart.GetMemBitmap();
            Bitmap image = new Bitmap(memBitmap);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(memBitmap, 0, 0);
            g.DrawLine(Pens.Black, 0, 0, image.Width, 0);
            StatisticForm.PaintTo(g);
            e.Graphics.DrawImage(image, e.MarginBounds, new Rectangle(0, 0, memBitmap.Width, memBitmap.Height), GraphicsUnit.Pixel);
        }

        public void PrintPreview()
        {
            this.previewDialog.ShowDialog();
        }

        public void PrintSetup()
        {
            this.setupDialog.ShowDialog();
        }

        public void RecreateFormula()
        {
            if (this.Chart != null)
            {
                foreach (FormulaArea area in this.Chart.Areas)
                {
                    this.RecreateFormula(area);
                }
            }
        }

        public void RecreateFormula(FormulaArea fa)
        {
            string[] strArray = new string[fa.Formulas.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = fa.Formulas[i].FullName;
            }
            fa.Formulas.Clear();
            for (int j = 0; j < strArray.Length; j++)
            {
                fa.AddFormula(strArray[j]);
            }
        }

        public void RedrawCaption()
        {
            if (base.ParentForm != null)
            {
                IDataProvider dataProvider = this.Chart.DataProvider;
                string stringData = dataProvider.GetStringData("Code");
                string str2 = dataProvider.GetStringData("Name");
                if (str2 != null)
                {
                    stringData = stringData + " ( " + str2 + " )";
                }
                string str3 = dataProvider.GetStringData("Exchange");
                if (str3 != null)
                {
                    stringData = stringData + " @ " + str3;
                }
                base.ParentForm.Text = stringData + " - " + this.CurrentDataCycle;
            }
        }

        public void SetAreaByName(string FormulaName)
        {
            FormulaArea selectedArea = this.Chart.SelectedArea;
            if (selectedArea == null)
            {
                selectedArea = this.Chart.Areas[this.Chart.Areas.Count - 1];
            }
            this.SetAreaByName(selectedArea, FormulaName);
        }

        public void SetAreaByName(FormulaArea fa, string FormulaName)
        {
            if (fa != null)
            {
                FormulaBase fb = fa.Formulas[0];
                try
                {
                    fa.Formulas.Clear();
                    fa.AddFormula(FormulaName);
                    fa.Bind();
                    int index = this.Chart.Areas.IndexOf(fa);
                    if (index > 0)
                    {
                        this.SetDefaultFormulas(index - 1, FormulaName);
                    }
                }
                catch
                {
                    fa.Formulas.Clear();
                    fa.Formulas.Add(fb);
                    fa.Bind();
                    throw;
                }
                this.NeedRedraw();
            }
        }

        public void SetAreaByName(int Index, string FormulaName)
        {
            if ((Index >= 0) && (Index < this.Chart.Areas.Count))
            {
                this.SetAreaByName(this.Chart.Areas[Index], FormulaName);
            }
        }

        public void SetDataCycle(bool needRebind = true)
        {
            this.SetDataCycle(this.CurrentDataCycle, needRebind);
        }

        public void SetDataCycle(string CycleString, bool needRebind = true)
        {
            this.CurrentDataCycle = CycleString;
            if (this.HasData() && (this.Chart.DataProvider.DataCycle.ToString() != CycleString))
            {
                BeginUpdate();
                this.ColumnWidth = 5.0;
                this.StartBar = 0;
                EndUpdate();
                this.Chart.DataProvider.DataCycle = DataCycle.Parse(CycleString + "1");
                if ( needRebind )
                    NeedRebind();
                if (this.DataChanged != null)
                {
                    this.DataChanged(this, new EventArgs());
                }
            }
        }

        public void SetDefaultFormulas(int Index, string s)
        {
            this.ExpandDefaultFormulas(Index);
            this.defaultFormulas[Index] = s;
        }

        public void SetScaleType(ScaleType st)
        {
            if (this.HasData() && (this.Chart.Areas.Count > 0))
            {
                this.Chart[0].AxisY.Scale = st;
                this.NeedRedraw();
            }
        }

        public void ShowChart(IDataProvider idp)
        {
            this.Chart.DataProvider = idp;
            this.SetDataCycle();
        }

        private void ShowStatisticWindow(FormulaChart Chart, int Pos, IDataProvider idp)
        {
            if (StatisticForm.Enable)
            {
                double d = idp["DATE"][Chart.CursorPos];
                double num2 = idp["OPEN"][Chart.CursorPos];
                double num3 = idp["HIGH"][Chart.CursorPos];
                double num4 = idp["LOW"][Chart.CursorPos];
                double num5 = idp["CLOSE"][Chart.CursorPos];
                double num6 = idp["VOLUME"][Chart.CursorPos];
                string str = "";
                string str2 = "";
                string str3 = "";
                if (Chart.CursorPos > 0)
                {
                    double num7 = idp["CLOSE"][Chart.CursorPos - 1];
                    if (num7 != double.NaN)
                    {
                        str = num7.ToString("f2");
                        str2 = (num5 - num7).ToString("f2");
                        str3 = ((num5 - num7) / num7).ToString("p2");
                    }
                }
                StringBuilder builder = new StringBuilder();
                builder.Append("Date=" + DateTime.FromOADate(d).ToString(Chart.XCursorFormat, DateTimeFormatInfo.InvariantInfo) + ";");
                builder.Append("Current=" + num5.ToString("f2") + ";");
                builder.Append("Last=" + str + ";");
                builder.Append("Open=" + num2.ToString("f2") + ";");
                builder.Append("High=" + num3.ToString("f2") + ";");
                builder.Append("Low=" + num4.ToString("f2") + ";");
                builder.Append("Close=" + num5.ToString("f2") + ";");
                builder.Append("Volume=" + num6.ToString() + ";");
                builder.Append("Change=" + str2 + ";");
                builder.Append("Percent=" + str3 + ";");
                if (this.ShowIndicatorValues)
                {
                    try
                    {
                        for (int i = this.showOverlayValues ? 0 : 1; i < Chart.Areas.Count; i++)
                        {
                            int num9 = 0;
                            int num10 = 0;
                            foreach (FormulaData data in Chart[i].FormulaDataArray)
                            {
                                string displayName;
                                if (num10 == 0)
                                {
                                    displayName = Chart[i].Formulas[num9].DisplayName;
                                }
                                else
                                {
                                    displayName = "";
                                }
                                if (!data.TextInvisible)
                                {
                                    string str5 = (displayName + data.Name).Trim();
                                    if (str5 != "")
                                    {
                                        builder.Append(str5 + "=" + data[Chart.CursorPos].ToString(data.Format) + ";");
                                    }
                                }
                                num10++;
                                if (num10 == Chart[i].Packages[num9].Count)
                                {
                                    num9++;
                                    num10 = 0;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if (base.ParentForm != null)
                {
                    StatisticForm.ShowStatistic(base.ParentForm, builder.ToString());
                }
                this.LastProvider = Chart.DataProvider;
                this.LastCursorPos = Chart.CursorPos;
            }
            else
            {
                StatisticForm.HideStatistic();
            }
        }

        [Category("Stock Chart")]
        public int AreaCount
        {
            get
            {
                return this.areaCount;
            }
            set
            {
                this.areaCount = value;
                this.NeedRebind();
            }
        }

        [Browsable(false)]
        public FormulaChart BackChart
        {
            get
            {
                return this.Chart;
            }
        }

        [Description("Behaver when drag the chart"), Category("Stock Chart")]
        public ChartDragMode ChartDragMode
        {
            get
            {
                return this.chartDragMode;
            }
            set
            {
                this.chartDragMode = value;
            }
        }

        [Category("Stock Chart")]
        public double ColumnWidth
        {
            get
            {
                return this.Chart.ColumnWidth;
            }
            set
            {
                if (this.Chart.ColumnWidth != value)
                {
                    this.Chart.ColumnWidth = value;
                    if(!m_isInitializationMode)
                        this.NeedRedraw();
                }
            }
        }

        [Category("Stock Chart")]
        public string DefaultFormulas
        {
            get
            {
                return string.Join(";", (string[])this.defaultFormulas.ToArray(typeof(string)));
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    this.defaultFormulas.Clear();
                    this.defaultFormulas.AddRange(value.Split(new char[] { ';' }));
                }
            }
        }

        [Browsable(false)]
        public Control DesignerControl
        {
            get
            {
                return this;
            }
        }

        [Browsable(false)]
        public bool Designing
        {
            get
            {
                return this.designing;
            }
            set
            {
                this.designing = value;
            }
        }

        private string FavoriteCycles
        {
            get
            {
                return string.Join(";", (string[])this.dataCycles.ToArray(typeof(string)));
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    this.dataCycles.Clear();
                    this.dataCycles.AddRange(value.Split(new char[] { ';' }));
                }
            }
        }

        [Category("Stock Chart")]
        public string FavoriteFormulas
        {
            get
            {
                return string.Join(";", (string[])this.favoriteFormulas.ToArray(typeof(string)));
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    this.favoriteFormulas.Clear();
                    this.favoriteFormulas.AddRange(value.Split(new char[] { ';' }));
                    this.BuildFavoriteMenu();
                }
            }
        }

        [Description("How to show the latest value in the axis-Y"), Category("Stock Chart")]
        public LatestValueType LatestValueType
        {
            get
            {
                return this.Chart.LatestValueType;
            }
            set
            {
                this.Chart.LatestValueType = value;
            }
        }

        [Category("Stock Chart")]
        public double MaxColumnWidth
        {
            get
            {
                return this.maxColumnWidth;
            }
            set
            {
                this.maxColumnWidth = value;
            }
        }

        [Category("Stock Chart")]
        public double MinColumnWidth
        {
            get
            {
                return this.minColumnWidth;
            }
            set
            {
                this.minColumnWidth = value;
            }
        }

        [Category("Stock Chart")]
        public string OverlayFormulas
        {
            get
            {
                return this.overlayFormulas;
            }
            set
            {
                this.overlayFormulas = value;
                if ((this.Chart.Areas.Count > 0) && (value != null))
                {
                    while (this.Chart[0].Formulas.Count > 1)
                    {
                        this.Chart[0].Formulas.RemoveAt(1);
                    }
                    if (value != "")
                    {
                        foreach (string str in value.Split(new char[] { ';' }))
                        {
                            this.Chart[0].AddFormula("FML." + str);
                        }
                    }
                    this.NeedRebind();
                }
            }
        }

        [Category("Stock Chart"), Description("Show cross cursor on the chart")]
        public bool ShowCrossCursor
        {
            get
            {
                return this.showCrossCursor;
            }
            set
            {
                this.showCrossCursor = value;
                this.miCrossCursor.Checked = value;
                this.Chart.ShowHLine = value;
                this.Chart.ShowVLine = value;
                if (!value)
                {
                    base.Invalidate();
                }
            }
        }

        [Category("Stock Chart")]
        public bool ShowCursorLabel
        {
            get
            {
                return this.Chart.ShowCursorLabel;
            }
            set
            {
                this.Chart.ShowCursorLabel = value;
            }
        }

        [Description("Show indicator values in the statistic window"), Category("Stock Chart")]
        public bool ShowIndicatorValues
        {
            get
            {
                return this.showIndicatorValues;
            }
            set
            {
                this.showIndicatorValues = value;
            }
        }

        [Category("Stock Chart"), Description("Show overlay values in the statistic window")]
        public bool ShowOverlayValues
        {
            get
            {
                return this.showOverlayValues;
            }
            set
            {
                this.showOverlayValues = value;
            }
        }

        [Category("Stock Chart"), Description("Show statistic window")]
        public bool ShowStatistic
        {
            get
            {
                return StatisticForm.Enable;
            }
            set
            {
                StatisticForm.Enable = value;
            }
        }

        [Category("Stock Chart")]
        public bool ShowTopLine
        {
            get
            {
                return this.showTopLine;
            }
            set
            {
                this.showTopLine = value;
            }
        }

        [TypeConverter(typeof(SkinConverter)), Category("Stock Chart")]
        public string Skin
        {
            get
            {
                return this.skin;
            }
            set
            {
                this.skin = value;
                this.NeedRebind();
            }
        }

        [Category("Stock Chart")]
        public int StartBar
        {
            get
            {
                return this.Chart.Start;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                this.Chart.Start = value;
                this.Chart.StartTime = DateTime.MinValue;
                this.Chart.EndTime = DateTime.MaxValue;
                if (!m_isInitializationMode)
                    this.NeedRedraw();
            }
        }
    }
}

