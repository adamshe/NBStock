namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class ImgControl : UserControl
    {
        private Button btnAdd;
        private Button btnDel;
        private Container components;
        private IWindowsFormsEditorService edSvc;
        public string ImgName;
        private ListBox lbImgName;
        private OpenFileDialog odIcon;

        public ImgControl()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ImgControl(string Value, IWindowsFormsEditorService edSvc) : this()
        {
            this.ImgName = Value;
            this.edSvc = edSvc;
            this.LoadFileList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.odIcon.ShowDialog() == DialogResult.OK)
            {
                string imageRoot = ObjectHelper.GetImageRoot();
                string fileName = this.odIcon.FileName;
                string str3 = Path.GetFileName(fileName);
                try
                {
                    File.Copy(fileName, imageRoot + str3, true);
                    this.ImgName = str3;
                    this.LoadFileList();
                }
                catch
                {
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.lbImgName.SelectedItem != null)
            {
                string imageRoot = ObjectHelper.GetImageRoot();
                try
                {
                    File.Delete(imageRoot + this.lbImgName.SelectedItem);
                    this.LoadFileList();
                }
                catch
                {
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

        private void InitializeComponent()
        {
            this.lbImgName = new ListBox();
            this.btnDel = new Button();
            this.btnAdd = new Button();
            this.odIcon = new OpenFileDialog();
            base.SuspendLayout();
            this.lbImgName.BorderStyle = BorderStyle.FixedSingle;
            this.lbImgName.Location = new Point(0, 0);
            this.lbImgName.Name = "lbImgName";
            this.lbImgName.Size = new Size(0xa8, 0xb8);
            this.lbImgName.TabIndex = 0;
            this.lbImgName.DoubleClick += new EventHandler(this.lbImgName_DoubleClick);
            this.lbImgName.SelectedIndexChanged += new EventHandler(this.lbImgName_SelectedIndexChanged);
            this.btnDel.Location = new Point(11, 190);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new Size(0x38, 20);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "&Delete";
            this.btnDel.Click += new EventHandler(this.btnDel_Click);
            this.btnAdd.Location = new Point(0x58, 190);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x38, 20);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.BackColor = SystemColors.Control;
            base.Controls.Add(this.btnAdd);
            base.Controls.Add(this.btnDel);
            base.Controls.Add(this.lbImgName);
            this.Font = new Font("Verdana", 8.25f);
            base.Name = "ImgControl";
            base.Size = new Size(0xa8, 0xd8);
            base.ResumeLayout(false);
        }

        private void lbImgName_DoubleClick(object sender, EventArgs e)
        {
            this.edSvc.CloseDropDown();
        }

        private void lbImgName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ImgName = this.lbImgName.SelectedItem.ToString();
        }

        private void LoadFileList()
        {
            string[] files = Directory.GetFiles(ObjectHelper.GetImageRoot());
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            this.lbImgName.Items.Clear();
            this.lbImgName.Items.AddRange(files);
        }
    }
}

