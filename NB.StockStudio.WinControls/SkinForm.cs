namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SkinForm : Form
    {
        private ComboBox cbSkin;
        private Container components = null;
        private PropertyGrid pg;
        private Panel pnClient;
        private Splitter spVerticle;

        public SkinForm()
        {
            this.InitializeComponent();
        }

        private void cbSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormulaSkin skinByName = FormulaSkin.GetSkinByName(this.cbSkin.SelectedItem.ToString());
            this.pg.SelectedObject = skinByName;
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
            this.pg = new PropertyGrid();
            this.pnClient = new Panel();
            this.spVerticle = new Splitter();
            this.cbSkin = new ComboBox();
            this.pnClient.SuspendLayout();
            base.SuspendLayout();
            this.pg.CommandsVisibleIfAvailable = true;
            this.pg.Dock = DockStyle.Right;
            this.pg.LargeButtons = false;
            this.pg.LineColor = SystemColors.ScrollBar;
            this.pg.Location = new Point(0x138, 0);
            this.pg.Name = "pg";
            this.pg.Size = new Size(200, 0x17d);
            this.pg.TabIndex = 0;
            this.pg.Text = "propertyGrid1";
            this.pg.ViewBackColor = SystemColors.Window;
            this.pg.ViewForeColor = SystemColors.WindowText;
            this.pnClient.Controls.Add(this.cbSkin);
            this.pnClient.Dock = DockStyle.Fill;
            this.pnClient.Location = new Point(0, 0);
            this.pnClient.Name = "pnClient";
            this.pnClient.Size = new Size(0x138, 0x17d);
            this.pnClient.TabIndex = 1;
            this.spVerticle.Dock = DockStyle.Right;
            this.spVerticle.Location = new Point(0x135, 0);
            this.spVerticle.Name = "spVerticle";
            this.spVerticle.Size = new Size(3, 0x17d);
            this.spVerticle.TabIndex = 2;
            this.spVerticle.TabStop = false;
            this.cbSkin.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbSkin.Location = new Point(0x38, 0x38);
            this.cbSkin.Name = "cbSkin";
            this.cbSkin.Size = new Size(0x79, 20);
            this.cbSkin.TabIndex = 0;
            this.cbSkin.SelectedIndexChanged += new EventHandler(this.cbSkin_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x200, 0x17d);
            base.Controls.Add(this.spVerticle);
            base.Controls.Add(this.pnClient);
            base.Controls.Add(this.pg);
            base.Name = "SkinForm";
            base.ShowInTaskbar = false;
            this.Text = "SkinForm";
            base.Load += new EventHandler(this.SkinForm_Load);
            this.pnClient.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SkinForm_Load(object sender, EventArgs e)
        {
            this.cbSkin.Items.AddRange(FormulaSkin.GetBuildInSkins());
        }
    }
}

