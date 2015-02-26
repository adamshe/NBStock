namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class SizeToolControl : UserControl
    {
        private ChartWinControl chartControl;
        private IContainer components;
        private HScrollBar hsbView;
        private ImageList ilToolBar;
        private bool Scrolling;
        private ToolBarButton tbbSizeAll;
        private ToolBarButton tbbZoomIn;
        private ToolBarButton tbbZoomOut;
        private ToolBar tnControl;

        public SizeToolControl()
        {
            this.InitializeComponent();
        }

        private void AdjustSize(double Multiply)
        {
            ChartWinControl chartControl = this.ChartControl;
            chartControl.ColumnWidth *= Multiply;
            this.ChartControl.Chart.StartTime = DateTime.MinValue;
            this.ChartControl.Chart.EndTime = DateTime.MaxValue;
            this.ChartControl.NeedRedraw();
        }

        private void chartControl_ViewChanged(object sender, ViewChangedArgs e)
        {
            if (!this.Scrolling)
            {
                this.Scrolling = true;
                try
                {
                    this.hsbView.Minimum = e.FirstBar;
                    this.hsbView.Maximum = e.LastBar;
                    this.hsbView.LargeChange = e.EndBar - e.StartBar;
                    this.hsbView.Value = e.StartBar;
                }
                finally
                {
                    this.Scrolling = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void hsbView_ValueChanged(object sender, EventArgs e)
        {
            if (!this.Scrolling && (this.ChartControl != null))
            {
                this.Scrolling = true;
                try
                {
                    this.ChartControl.StartBar = (this.hsbView.Maximum - (this.hsbView.Value + this.hsbView.LargeChange)) + 1;
                }
                finally
                {
                    this.Scrolling = false;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(SizeToolControl));
            this.hsbView = new HScrollBar();
            this.tnControl = new ToolBar();
            this.tbbSizeAll = new ToolBarButton();
            this.tbbZoomIn = new ToolBarButton();
            this.tbbZoomOut = new ToolBarButton();
            this.ilToolBar = new ImageList(this.components);
            base.SuspendLayout();
            this.hsbView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.hsbView.Location = new Point(0, 1);
            this.hsbView.Name = "hsbView";
            this.hsbView.Size = new Size(0x250, 0x16);
            this.hsbView.TabIndex = 2;
            this.hsbView.ValueChanged += new EventHandler(this.hsbView_ValueChanged);
            this.tnControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tnControl.Appearance = ToolBarAppearance.Flat;
            this.tnControl.Buttons.AddRange(new ToolBarButton[] { this.tbbSizeAll, this.tbbZoomIn, this.tbbZoomOut });
            this.tnControl.ButtonSize = new Size(0x18, 0x18);
            this.tnControl.Divider = false;
            this.tnControl.Dock = DockStyle.None;
            this.tnControl.DropDownArrows = true;
            this.tnControl.ImageList = this.ilToolBar;
            this.tnControl.Location = new Point(0x250, -1);
            this.tnControl.Name = "tnControl";
            this.tnControl.ShowToolTips = true;
            this.tnControl.Size = new Size(0x90, 30);
            this.tnControl.TabIndex = 3;
            this.tnControl.TextAlign = ToolBarTextAlign.Right;
            this.tnControl.Wrappable = false;
            this.tnControl.ButtonClick += new ToolBarButtonClickEventHandler(this.tnControl_ButtonClick);
            this.tbbSizeAll.ImageIndex = 0;
            this.tbbSizeAll.ToolTipText = "Reset";
            this.tbbZoomIn.ImageIndex = 1;
            this.tbbZoomIn.ToolTipText = "Zoom In";
            this.tbbZoomOut.ImageIndex = 2;
            this.tbbZoomOut.ToolTipText = "Zoom Out";
            this.ilToolBar.ImageSize = new Size(20, 20);
            this.ilToolBar.ImageStream = (ImageListStreamer) manager.GetObject("ilToolBar.ImageStream");
            this.ilToolBar.TransparentColor = Color.White;
            base.Controls.Add(this.tnControl);
            base.Controls.Add(this.hsbView);
            base.Name = "SizeToolControl";
            base.Size = new Size(0x2e0, 0x18);
            base.ResumeLayout(false);
        }

        private void tnControl_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (this.ChartControl != null)
            {
                if (e.Button == this.tbbSizeAll)
                {
                    this.ChartControl.StartBar = 0;
                    this.ChartControl.ColumnWidth = 5.0;
                    this.ChartControl.AutoScaleAxisY();
                }
                else if (e.Button == this.tbbZoomIn)
                {
                    this.AdjustSize(1.2);
                }
                else if (e.Button == this.tbbZoomOut)
                {
                    this.AdjustSize(0.8);
                }
            }
        }

        public ChartWinControl ChartControl
        {
            get
            {
                return this.chartControl;
            }
            set
            {
                this.chartControl = value;
                if (value != null)
                {
                    this.chartControl.ViewChanged += new ViewChangedHandler(this.chartControl_ViewChanged);
                }
            }
        }
    }
}

