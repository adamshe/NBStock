namespace NB.StockStudio.WinControls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class AboutForm : Form
    {
        private Button btnOK;
        private Button btnSystemInfo;
        private ColumnHeader chName;
        private ColumnHeader chVersion;
        private Container components = null;
        private Label label1;
        private Label label2;
        private LinkLabel lbEMail;
        private LinkLabel lbSite;
        private Label lComponent;
        private Label lCopyright;
        private Label lName;
        private ListView lvAssembly;
        private Label lVersion;

        public AboutForm()
        {
            this.InitializeComponent();
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Close();
            }
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            this.lCopyright.Text = ((AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(executingAssembly, typeof(AssemblyCopyrightAttribute))).Copyright;
            this.lVersion.Text = "Version : " + Application.ProductVersion;
            this.lName.Text = Application.ProductName;
            this.lbSite.Links[0].LinkData = this.lbSite.Text;
            this.lbEMail.Links[0].LinkData = this.lbEMail.Text;
            foreach (AssemblyName name in executingAssembly.GetReferencedAssemblies())
            {
                this.lvAssembly.Items.Add(name.Name).SubItems.Add(name.Version.ToString());
            }
        }

        private void btnSystemInfo_Click(object sender, EventArgs e)
        {
            Process.Start("MSInfo32.exe");
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
            this.lName = new Label();
            this.lvAssembly = new ListView();
            this.chName = new ColumnHeader();
            this.chVersion = new ColumnHeader();
            this.btnOK = new Button();
            this.btnSystemInfo = new Button();
            this.lbSite = new LinkLabel();
            this.lVersion = new Label();
            this.lCopyright = new Label();
            this.lComponent = new Label();
            this.lbEMail = new LinkLabel();
            this.label1 = new Label();
            this.label2 = new Label();
            base.SuspendLayout();
            this.lName.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lName.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lName.Location = new Point(15, 0x10);
            this.lName.Name = "lName";
            this.lName.Size = new Size(360, 0x10);
            this.lName.TabIndex = 0;
            this.lName.Text = "Easy Financial Chart Windows Demo";
            this.lvAssembly.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lvAssembly.Columns.AddRange(new ColumnHeader[] { this.chName, this.chVersion });
            this.lvAssembly.GridLines = true;
            this.lvAssembly.Location = new Point(0x10, 0x4d);
            this.lvAssembly.Name = "lvAssembly";
            this.lvAssembly.Size = new Size(0x1c8, 0x7b);
            this.lvAssembly.TabIndex = 1;
            this.lvAssembly.View = View.Details;
            this.chName.Text = "Name";
            this.chName.Width = 0xcd;
            this.chVersion.Text = "Version";
            this.chVersion.Width = 0xa1;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x170, 0xd9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x17);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnSystemInfo.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnSystemInfo.Location = new Point(0x170, 0xf9);
            this.btnSystemInfo.Name = "btnSystemInfo";
            this.btnSystemInfo.Size = new Size(0x60, 0x17);
            this.btnSystemInfo.TabIndex = 3;
            this.btnSystemInfo.Text = "System Info";
            this.btnSystemInfo.Click += new EventHandler(this.btnSystemInfo_Click);
            this.lbSite.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lbSite.AutoSize = true;
            this.lbSite.Location = new Point(80, 0xd8);
            this.lbSite.Name = "lbSite";
            this.lbSite.Size = new Size(0xa6, 0x11);
            this.lbSite.TabIndex = 4;
            this.lbSite.TabStop = true;
            this.lbSite.Text = "http://finance.easychart.net";
            this.lbSite.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lbSite_LinkClicked);
            this.lVersion.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lVersion.AutoSize = true;
            this.lVersion.Location = new Point(15, 0x25);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new Size(0x41, 0x11);
            this.lVersion.TabIndex = 5;
            this.lVersion.Text = "<Version>";
            this.lCopyright.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lCopyright.AutoSize = true;
            this.lCopyright.Location = new Point(15, 0x108);
            this.lCopyright.Name = "lCopyright";
            this.lCopyright.Size = new Size(0x55, 0x11);
            this.lCopyright.TabIndex = 6;
            this.lCopyright.Text = "<Copy Right>";
            this.lComponent.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lComponent.AutoSize = true;
            this.lComponent.Location = new Point(15, 0x3a);
            this.lComponent.Name = "lComponent";
            this.lComponent.Size = new Size(0x7e, 0x11);
            this.lComponent.TabIndex = 7;
            this.lComponent.Text = "Product components:";
            this.lbEMail.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lbEMail.AutoSize = true;
            this.lbEMail.Location = new Point(80, 240);
            this.lbEMail.Name = "lbEMail";
            this.lbEMail.Size = new Size(0xb1, 0x11);
            this.lbEMail.TabIndex = 8;
            this.lbEMail.TabStop = true;
            this.lbEMail.Text = "mailto:adamshe@gmail.com";
            this.lbEMail.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lbSite_LinkClicked);
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.Location = new Point(0x10, 0xd9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x10);
            this.label1.TabIndex = 9;
            this.label1.Text = "Home:";
            this.label2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 240);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x37, 0x11);
            this.label2.TabIndex = 10;
            this.label2.Text = "Support:";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(490, 0x130);
            base.ControlBox = false;
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.lbEMail);
            base.Controls.Add(this.lComponent);
            base.Controls.Add(this.lCopyright);
            base.Controls.Add(this.lVersion);
            base.Controls.Add(this.lbSite);
            base.Controls.Add(this.btnSystemInfo);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.lvAssembly);
            base.Controls.Add(this.lName);
            this.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.Name = "AboutForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About";
            base.KeyDown += new KeyEventHandler(this.AboutForm_KeyDown);
            base.Load += new EventHandler(this.AboutForm_Load);
            base.ResumeLayout(false);
        }

        private void lbSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }

        public static void ShowIt()
        {
            new AboutForm().ShowDialog();
        }
    }
}

