namespace NB.StockStudio.WinControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Overlay : Form
    {
        private Button btnCancel;
        private Button btnDelete;
        private Button btnOK;
        private Container components = null;
        private ComboBox ddlAddOverlay;
        private bool DisableChangeEvent;
        private GroupBox gbCurrent;
        private GroupBox gbParameter;
        private ListBox lbOverlayList;
        private TextBox tbP1;
        private TextBox tbP2;
        private TextBox tbP3;
        private TextBox tbP4;

        public Overlay()
        {
            this.InitializeComponent();
            ArrayList list = new ArrayList();
            list.Add(new OverlayItem("", "Select indicators to add"));
            list.Add(new OverlayItem("NATIVE.HL", "Horizontal Line"));
            list.Add(new OverlayItem("NATIVE.MA", "Simple Moving Average"));
            list.Add(new OverlayItem("NATIVE.EMA", "Exponential Moving Average"));
            list.Add(new OverlayItem("BB", "Bollinger Bands"));
            list.Add(new OverlayItem("EXTEND.BB", "Bollinger Bands(Area)"));
            list.Add(new OverlayItem("SAR", "Parabolic SAR"));
            list.Add(new OverlayItem("ZIGLABEL", "Zig Label"));
            list.Add(new OverlayItem("ZIG", "ZigZag"));
            list.Add(new OverlayItem("SR", "Support & Resistance"));
            list.Add(new OverlayItem("EXTEND.SRAxisY", "Support & Resistance(AxisY)"));
            list.Add(new OverlayItem("EXTEND.COMPARE", "Compare"));
            list.Add(new OverlayItem("Fibonnaci", "Fibonnaci retracements"));
            list.Add(new OverlayItem("LinRegr", "Linear Regression Channels"));
            this.ddlAddOverlay.DataSource = list;
        }

        private void AddFormulas()
        {
            this.AddFormulas(((OverlayItem) this.ddlAddOverlay.SelectedItem).Name);
        }

        private void AddFormulas(string Name)
        {
            int index = Name.IndexOf('(');
            string strB = Name;
            if (index >= 0)
            {
                strB = Name.Substring(0, index);
            }
            ArrayList dataSource = (ArrayList) this.ddlAddOverlay.DataSource;
            foreach (OverlayItem item in dataSource)
            {
                if (string.Compare(item.Name, strB, true) == 0)
                {
                    this.lbOverlayList.Items.Add(Name);
                    break;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.DeleteFormulas();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
        }

        private void ddlAddOverlay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlAddOverlay.SelectedIndex > 0)
            {
                this.AddFormulas();
                this.ddlAddOverlay.SelectedIndex = 0;
                this.SelectLastOverlay();
            }
        }

        private void DeleteFormulas()
        {
            this.DisableChangeEvent = true;
            int selectedIndex = this.lbOverlayList.SelectedIndex;
            if (selectedIndex >= 0)
            {
                try
                {
                    this.lbOverlayList.Items.RemoveAt(selectedIndex);
                }
                finally
                {
                    this.DisableChangeEvent = false;
                }
                this.SelectOverlay(selectedIndex);
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

        private void InitializeComponent()
        {
            this.ddlAddOverlay = new ComboBox();
            this.lbOverlayList = new ListBox();
            this.tbP1 = new TextBox();
            this.tbP2 = new TextBox();
            this.tbP3 = new TextBox();
            this.gbParameter = new GroupBox();
            this.tbP4 = new TextBox();
            this.btnDelete = new Button();
            this.btnOK = new Button();
            this.gbCurrent = new GroupBox();
            this.btnCancel = new Button();
            this.gbParameter.SuspendLayout();
            this.gbCurrent.SuspendLayout();
            base.SuspendLayout();
            this.ddlAddOverlay.DisplayMember = "Description";
            this.ddlAddOverlay.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ddlAddOverlay.Items.AddRange(new object[] { "NATIVE.HL=Horizontal Line", "NATIVE.MA=Simple Moving Average", "NATIVE.EMA=Exponential Moving Average" });
            this.ddlAddOverlay.Location = new Point(0x10, 0x10);
            this.ddlAddOverlay.MaxDropDownItems = 12;
            this.ddlAddOverlay.Name = "ddlAddOverlay";
            this.ddlAddOverlay.Size = new Size(0x103, 0x15);
            this.ddlAddOverlay.TabIndex = 0;
            this.ddlAddOverlay.ValueMember = "Name";
            this.ddlAddOverlay.SelectedIndexChanged += new EventHandler(this.ddlAddOverlay_SelectedIndexChanged);
            this.lbOverlayList.DisplayMember = "Description";
            this.lbOverlayList.Location = new Point(8, 0x18);
            this.lbOverlayList.Name = "lbOverlayList";
            this.lbOverlayList.Size = new Size(0x100, 0xfb);
            this.lbOverlayList.TabIndex = 1;
            this.lbOverlayList.ValueMember = "Name";
            this.lbOverlayList.SelectedValueChanged += new EventHandler(this.lbOverlayList_SelectedValueChanged);
            this.tbP1.Location = new Point(0x10, 0x18);
            this.tbP1.Name = "tbP1";
            this.tbP1.TabIndex = 2;
            this.tbP1.Text = "";
            this.tbP1.Leave += new EventHandler(this.tbP4_Leave);
            this.tbP2.Location = new Point(0x10, 0x40);
            this.tbP2.Name = "tbP2";
            this.tbP2.TabIndex = 3;
            this.tbP2.Text = "";
            this.tbP2.Leave += new EventHandler(this.tbP4_Leave);
            this.tbP3.Location = new Point(0x10, 0x68);
            this.tbP3.Name = "tbP3";
            this.tbP3.TabIndex = 4;
            this.tbP3.Text = "";
            this.tbP3.Leave += new EventHandler(this.tbP4_Leave);
            this.gbParameter.Controls.Add(this.tbP4);
            this.gbParameter.Controls.Add(this.tbP3);
            this.gbParameter.Controls.Add(this.tbP2);
            this.gbParameter.Controls.Add(this.tbP1);
            this.gbParameter.Location = new Point(0x111, 0x12);
            this.gbParameter.Name = "gbParameter";
            this.gbParameter.Size = new Size(0x81, 0xdf);
            this.gbParameter.TabIndex = 5;
            this.gbParameter.TabStop = false;
            this.gbParameter.Text = "Parameters";
            this.tbP4.Location = new Point(0x10, 0x90);
            this.tbP4.Name = "tbP4";
            this.tbP4.TabIndex = 5;
            this.tbP4.Text = "";
            this.tbP4.Leave += new EventHandler(this.tbP4_Leave);
            this.btnDelete.Location = new Point(0x112, 250);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x10, 0x158);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.gbCurrent.Controls.Add(this.lbOverlayList);
            this.gbCurrent.Controls.Add(this.gbParameter);
            this.gbCurrent.Controls.Add(this.btnDelete);
            this.gbCurrent.Location = new Point(0x10, 0x30);
            this.gbCurrent.Name = "gbCurrent";
            this.gbCurrent.Size = new Size(0x198, 0x120);
            this.gbCurrent.TabIndex = 8;
            this.gbCurrent.TabStop = false;
            this.gbCurrent.Text = "Current Overlays";
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x68, 0x158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(450, 0x177);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.gbCurrent);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.ddlAddOverlay);
            this.Font = new Font("Verdana", 8.5f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.Name = "Overlay";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Overlay Manager";
            base.KeyDown += new KeyEventHandler(this.Overlay_KeyDown);
            base.Load += new EventHandler(this.Overlay_Load);
            this.gbParameter.ResumeLayout(false);
            this.gbCurrent.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lbOverlayList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!this.DisableChangeEvent)
            {
                string str;
                SelectFormula.ParamToTextBox((string) this.lbOverlayList.SelectedItem, new TextBox[] { this.tbP1, this.tbP2, this.tbP3, this.tbP4 }, out str);
            }
        }

        private void Overlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Close();
            }
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
        }

        private void SelectFirstOverlay()
        {
            this.SelectOverlay(0);
        }

        private void SelectLastOverlay()
        {
            this.SelectOverlay(this.lbOverlayList.Items.Count - 1);
        }

        private void SelectOverlay(int Index)
        {
            if (Index < 0)
            {
                Index = 0;
            }
            if (Index >= this.lbOverlayList.Items.Count)
            {
                Index = this.lbOverlayList.Items.Count - 1;
            }
            if ((Index >= 0) && (Index < this.lbOverlayList.Items.Count))
            {
                this.lbOverlayList.SelectedIndex = Index;
            }
        }

        private void tbP4_Leave(object sender, EventArgs e)
        {
            string str = SelectFormula.TextBoxToParam((string) this.lbOverlayList.SelectedItem, new TextBox[] { this.tbP1, this.tbP2, this.tbP3, this.tbP4 });
            if (str != "")
            {
                this.DisableChangeEvent = true;
                try
                {
                    this.lbOverlayList.Items[this.lbOverlayList.SelectedIndex] = str;
                }
                finally
                {
                    this.DisableChangeEvent = false;
                }
            }
        }

        public string CurrentOverlay
        {
            get
            {
                string str = "";
                foreach (string str2 in this.lbOverlayList.Items)
                {
                    if (str != "")
                    {
                        str = str + ";";
                    }
                    str = str + str2;
                }
                return str;
            }
            set
            {
                this.lbOverlayList.Items.Clear();
                if ((value != "") && (value != null))
                {
                    foreach (string str in value.Split(new char[] { ';' }))
                    {
                        this.AddFormulas(str);
                    }
                    this.SelectFirstOverlay();
                }
            }
        }
    }
}

