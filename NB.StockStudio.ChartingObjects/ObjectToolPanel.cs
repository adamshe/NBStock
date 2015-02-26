namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ObjectToolPanel : UserControl
    {
        private ToolBarButton ArrowButton;
        private ColorDialog cdPen;
        private IContainer components;
        private ComboBox ddlPenWidth;
        private ComboBox ddlStyle;
        private ImageList ilTools;
        private Label label1;
        private ToolBarButton LastButton;
        private Label lPenWidth;
        private Label lStyle;
        public ObjectInit ObjectType;
        private Panel pnColor;
        private Panel pnDefault;
        private bool resetAfterEachDraw = true;
        private ToolBar tbToolPanel;

        public event EventHandler ToolsChanged;

        public ObjectToolPanel()
        {
            this.InitializeComponent();
        }

        private void AddObject(ObjectInit ObjectType)
        {
            string iconFile;
            ToolBarButton button = new ToolBarButton();
            button.Tag = ObjectType;
            if (ObjectType != null)
            {
                button.ToolTipText = ObjectType.Name;
                iconFile = ObjectHelper.GetIconFile(ObjectType.Icon);
            }
            else
            {
                this.ArrowButton = button;
                button.ToolTipText = "Select";
                iconFile = ObjectHelper.GetIconFile("Arrow");
            }
            if (File.Exists(iconFile))
            {
                Image image = Image.FromFile(iconFile);
                this.ilTools.Images.Add(image);
                button.ImageIndex = this.ilTools.Images.Count - 1;
            }
            this.tbToolPanel.Buttons.Add(button);
        }

        private void AddSeparator()
        {
            ToolBarButton button = new ToolBarButton();
            button.Style = ToolBarButtonStyle.Separator;
            this.tbToolPanel.Buttons.Add(button);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tbToolPanel = new ToolBar();
            this.ilTools = new ImageList(this.components);
            this.pnDefault = new Panel();
            this.label1 = new Label();
            this.pnColor = new Panel();
            this.ddlStyle = new ComboBox();
            this.lStyle = new Label();
            this.lPenWidth = new Label();
            this.ddlPenWidth = new ComboBox();
            this.cdPen = new ColorDialog();
            this.pnDefault.SuspendLayout();
            base.SuspendLayout();
            this.tbToolPanel.AllowDrop = true;
            this.tbToolPanel.ButtonSize = new Size(0x18, 0x18);
            this.tbToolPanel.Divider = false;
            this.tbToolPanel.DropDownArrows = true;
            this.tbToolPanel.ImageList = this.ilTools;
            this.tbToolPanel.Location = new Point(0, 0);
            this.tbToolPanel.Name = "tbToolPanel";
            this.tbToolPanel.ShowToolTips = true;
            this.tbToolPanel.Size = new Size(0x60, 0x1c);
            this.tbToolPanel.TabIndex = 0;
            this.tbToolPanel.ButtonClick += new ToolBarButtonClickEventHandler(this.tbToolPanel_ButtonClick);
            this.ilTools.ImageSize = new Size(20, 20);
            this.ilTools.TransparentColor = Color.Transparent;
            this.pnDefault.Controls.Add(this.label1);
            this.pnDefault.Controls.Add(this.pnColor);
            this.pnDefault.Controls.Add(this.ddlStyle);
            this.pnDefault.Controls.Add(this.lStyle);
            this.pnDefault.Controls.Add(this.lPenWidth);
            this.pnDefault.Controls.Add(this.ddlPenWidth);
            this.pnDefault.Dock = DockStyle.Bottom;
            this.pnDefault.Location = new Point(0, 0x108);
            this.pnDefault.Name = "pnDefault";
            this.pnDefault.Size = new Size(0x60, 0xb0);
            this.pnDefault.TabIndex = 1;
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 0x10);
            this.label1.TabIndex = 6;
            this.label1.Text = "Default Pen:";
            this.pnColor.BackColor = Color.Black;
            this.pnColor.BorderStyle = BorderStyle.Fixed3D;
            this.pnColor.Location = new Point(40, 0x90);
            this.pnColor.Name = "pnColor";
            this.pnColor.Size = new Size(0x18, 0x18);
            this.pnColor.TabIndex = 5;
            this.pnColor.Click += new EventHandler(this.pnColor_Click);
            this.ddlStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ddlStyle.Location = new Point(8, 0x74);
            this.ddlStyle.Name = "ddlStyle";
            this.ddlStyle.Size = new Size(0x38, 20);
            this.ddlStyle.TabIndex = 3;
            this.lStyle.AutoSize = true;
            this.lStyle.Location = new Point(7, 0x60);
            this.lStyle.Name = "lStyle";
            this.lStyle.Size = new Size(0x2a, 0x11);
            this.lStyle.TabIndex = 2;
            this.lStyle.Text = "Style:";
            this.lPenWidth.AutoSize = true;
            this.lPenWidth.Location = new Point(7, 0x30);
            this.lPenWidth.Name = "lPenWidth";
            this.lPenWidth.Size = new Size(0x2a, 0x11);
            this.lPenWidth.TabIndex = 1;
            this.lPenWidth.Text = "Width:";
            this.ddlPenWidth.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.ddlPenWidth.Location = new Point(8, 0x41);
            this.ddlPenWidth.Name = "ddlPenWidth";
            this.ddlPenWidth.Size = new Size(0x38, 20);
            this.ddlPenWidth.TabIndex = 0;
            this.ddlPenWidth.Text = "1";
            base.Controls.Add(this.pnDefault);
            base.Controls.Add(this.tbToolPanel);
            base.Name = "ObjectToolPanel";
            base.Size = new Size(0x60, 440);
            this.pnDefault.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void LoadObjectTool()
        {
            if (this.tbToolPanel.Buttons.Count == 0)
            {
                this.ddlStyle.Items.AddRange(Enum.GetNames(typeof(DashStyle)));
                this.ddlStyle.SelectedIndex = 0;
                this.AddObject(null);
                ObjectManager.SortCategory();
                foreach (ObjectCategory category in ObjectManager.alCategory)
                {
                    foreach (ObjectInit init in category.ObjectList)
                    {
                        this.AddObject(init);
                    }
                    this.AddSeparator();
                }
            }
        }

        public void Manager_AfterCreateFinished(object sender, BaseObject Object)
        {
            if (this.resetAfterEachDraw)
            {
                this.SetButton(this.ArrowButton);
            }
        }

        private void pnColor_Click(object sender, EventArgs e)
        {
            if (this.cdPen.ShowDialog() == DialogResult.OK)
            {
                this.pnColor.BackColor = this.cdPen.Color;
            }
        }

        private void SetButton(ToolBarButton tbb)
        {
            if (tbb != null)
            {
                this.ObjectType = (ObjectInit) tbb.Tag;
                tbb.Pushed = true;
                if (this.LastButton != null)
                {
                    this.LastButton.Pushed = false;
                }
                this.LastButton = tbb;
                if (this.ToolsChanged != null)
                {
                    this.ToolsChanged(this, new EventArgs());
                }
            }
        }

        private void tbToolPanel_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            this.SetButton(e.Button);
        }

        [Browsable(false)]
        public ObjectPen DefaultPen
        {
            get
            {
                ObjectPen pen = new ObjectPen();
                pen.Width = int.Parse(this.ddlPenWidth.Text);
                pen.Color = this.pnColor.BackColor;
                pen.DashStyle = (DashStyle) Enum.Parse(typeof(DashStyle), this.ddlStyle.Text);
                return pen;
            }
        }

        [Category("Stock Object"), Description("Reset to design mode after each draw")]
        public bool ResetAfterEachDraw
        {
            get
            {
                return this.resetAfterEachDraw;
            }
            set
            {
                this.resetAfterEachDraw = value;
            }
        }
    }
}

