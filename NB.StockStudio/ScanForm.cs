using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace NB.StockStudio
{
    public class ScanForm : Form
    {
        private IContainer components;

        private TreeView tvFormula;

        private Splitter sp1;

        private Panel pnClient;

        private Button btnClose;

        private Button btnScan;

        private GroupBox gbParam;

        private ProgressBar pbScan;

        private ImageList ilFormula;

        private Label lMsg;

        private Label lFullName;

        private TextBox tbDesc;

        private static ScanForm Current;


        public ScanForm()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ResourceManager resourceManager = new ResourceManager(typeof(ScanForm));
            tvFormula = new TreeView();
            sp1 = new Splitter();
            pnClient = new Panel();
            tbDesc = new TextBox();
            lFullName = new Label();
            lMsg = new Label();
            pbScan = new ProgressBar();
            gbParam = new GroupBox();
            btnScan = new Button();
            btnClose = new Button();
            ilFormula = new ImageList(components);
            pnClient.SuspendLayout();
            base.SuspendLayout();
            tvFormula.BorderStyle = BorderStyle.FixedSingle;
            tvFormula.Dock = DockStyle.Left;
            tvFormula.FullRowSelect = true;
            tvFormula.HideSelection = false;
            tvFormula.ImageIndex = -1;
            tvFormula.Location = new Point(0, 0);
            tvFormula.Name = "tvFormula";
            tvFormula.SelectedImageIndex = -1;
            tvFormula.Size = new Size(200, 453);
            tvFormula.TabIndex = 0;
            tvFormula.AfterSelect += new TreeViewEventHandler(this.tvFormula_AfterSelect);
            sp1.Location = new Point(200, 0);
            sp1.Name = "sp1";
            sp1.Size = new Size(3, 453);
            sp1.TabIndex = 1;
            sp1.TabStop = false;
            pnClient.Controls.Add(tbDesc);
            pnClient.Controls.Add(lFullName);
            pnClient.Controls.Add(lMsg);
            pnClient.Controls.Add(pbScan);
            pnClient.Controls.Add(gbParam);
            pnClient.Controls.Add(btnScan);
            pnClient.Controls.Add(btnClose);
            pnClient.Dock = DockStyle.Fill;
            pnClient.Location = new Point(203, 0);
            pnClient.Name = "pnClient";
            pnClient.Size = new Size(429, 453);
            pnClient.TabIndex = 2;
            tbDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbDesc.BackColor = Color.Beige;
            tbDesc.BorderStyle = BorderStyle.FixedSingle;
            tbDesc.Location = new Point(8, 32);
            tbDesc.Multiline = true;
            tbDesc.Name = "tbDesc";
            tbDesc.ReadOnly = true;
            tbDesc.Size = new Size(416, 62);
            tbDesc.TabIndex = 6;
            tbDesc.Text = "";
            lFullName.ForeColor = Color.Blue;
            lFullName.Location = new Point(8, 8);
            lFullName.Name = "lFullName";
            lFullName.Size = new Size(384, 23);
            lFullName.TabIndex = 5;
            lMsg.Location = new Point(8, 352);
            lMsg.Name = "lMsg";
            lMsg.Size = new Size(184, 23);
            lMsg.TabIndex = 4;
            pbScan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pbScan.Location = new Point(8, 328);
            pbScan.Name = "pbScan";
            pbScan.Size = new Size(416, 16);
            pbScan.TabIndex = 3;
            pbScan.Visible = false;
            gbParam.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbParam.Location = new Point(8, 96);
            gbParam.Name = "gbParam";
            gbParam.Size = new Size(416, 224);
            gbParam.TabIndex = 2;
            gbParam.TabStop = false;
            gbParam.Text = "Parameters";
            btnScan.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnScan.Location = new Point(248, 416);
            btnScan.Name = "btnScan";
            btnScan.TabIndex = 1;
            btnScan.Text = "&Scan";
            btnScan.Click += new EventHandler(this.btnScan_Click);
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.OK;
            btnClose.Location = new Point(336, 416);
            btnClose.Name = "btnClose";
            btnClose.TabIndex = 0;
            btnClose.Text = "&Close";
            ilFormula.ImageSize = new Size(16, 16);
            ilFormula.ImageStream = (ImageListStreamer)resourceManager.GetObject("ilFormula.ImageStream");
            ilFormula.TransparentColor = Color.Transparent;
            AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(632, 453);
            base.Controls.Add(pnClient);
            base.Controls.Add(sp1);
            base.Controls.Add(tvFormula);
            Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MaximumSize = new Size(800, 600);
            base.MinimizeBox = false;
            base.MinimumSize = new Size(640, 480);
            base.Name = "ScanForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            Text = "Scan";
            base.KeyDown += new KeyEventHandler(this.ScanForm_KeyDown);
            base.Load += new EventHandler(this.ScanForm_Load);
            base.Activated += new EventHandler(this.ScanForm_Activated);
            pnClient.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void CreateNode(string Name)
        {
            int i;

            string str1 = Name;
            TreeNode treeNode1 = tvFormula.Nodes[0];
            while ((i = str1.IndexOf('.')) > 0)
            {
                string str2 = str1.Substring(0, i);
                str1 = str1.Substring(i + 1);
                TreeNode treeNode2 = null;
                for (int j = 0; j < treeNode1.Nodes.Count; j++)
                {
                    if (treeNode1.Nodes[j].Text == str2)
                    {
                        treeNode2 = treeNode1.Nodes[j];
                        break;
                    }
                }
                if (treeNode2 == null)
                {
                    treeNode1.Nodes.Add(treeNode2 = new TreeNode(str2, 0, 0));
                }
                treeNode1 = treeNode2;
            }
            TreeNode treeNode3 = new TreeNode(str1, 1, 1);
            if (Name.StartsWith("Basic"))
            {
                treeNode3.Tag = str1;
            }
            else
            {
                treeNode3.Tag = Name;
            }
            treeNode1.Nodes.Add(treeNode3);
        }

        private void RefreshTree()
        {
            tvFormula.BeginUpdate();
            try
            {
                tvFormula.ImageList = ilFormula;
                tvFormula.Nodes.Clear();
                tvFormula.Nodes.Add("Root");
                tvFormula.Nodes[0].Nodes.Add("Scan");
                string[] strs2 = FormulaBase.GetAllIndicators(false);
                for (int j = 0; j < (int)strs2.Length; j++)
                {
                    string str1 = strs2[j];
                    int i = str1.IndexOf('.');
                    if (i > 0)
                    {
                        string str2 = str1.Substring(0, i).ToUpper();
                        if (Array.IndexOf(new string[]{"NATIVE", "TRADING", "DEMO"}, str2) < 0)
                        {
                            CreateNode(str1);
                        }
                    }
                }
                tvFormula.Nodes[0].Expand();
                tvFormula.Nodes[0].Nodes[0].Expand();
                gbParam.Controls.Clear();
                if (tvFormula.Nodes[0].Nodes[0].Nodes.Count > 0)
                {
                    tvFormula.SelectedNode = tvFormula.Nodes[0].Nodes[0].Nodes[0];
                }
            }
            finally
            {
                tvFormula.EndUpdate();
            }
        }

        private void ScanForm_Load(object sender, EventArgs e)
        {
        }

        public static void ShowIt()
        {
            if (Current == null)
            {
                Current = new ScanForm();
            }
            Current.ShowDialog();
        }

        private void tvFormula_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string str = (String)e.Node.Tag;
            if (str != null)
            {
                gbParam.Controls.Clear();
                int i1 = 10;
                int j1 = 20;
                int k1 = 30;
                int i2 = 0;
                FormulaBase formulaBase = FormulaBase.GetFormulaByName(str);
                FormulaProgram formulaProgram = PluginManager.GetFormulaProgram(formulaBase);
                if (formulaProgram != null)
                {
                    lFullName.Text = formulaProgram.FullName;
                    tbDesc.Text = formulaProgram.Description;
                }
                else
                {
                    lFullName.Text = formulaBase.GetType().ToString();
                    tbDesc.Text = "";
                }
                for (int j2 = 0; j2 < formulaBase.Params.Count; j2++)
                {
                    Label label1 = new Label();
                    label1.AutoSize = true;
                    label1.Text = String.Concat(formulaBase.Params[j2].Name, "=");
                    label1.Left = i1;
                    label1.Top = j1 + 3;
                    label1.Parent = gbParam;
                    i2 = Math.Max(i2, label1.Width);
                    j1 += k1;
                }
                j1 = 20;
                i1 += i2 + 6;
                for (int k2 = 0; k2 < formulaBase.Params.Count; k2++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Left = i1;
                    textBox.Top = j1;
                    textBox.Text = formulaBase.Params[k2].DefaultValue;
                    textBox.Parent = gbParam;
                    Label label2 = new Label();
                    label2.AutoSize = true;
                    label2.Text = String.Concat(new string[]{"(", formulaBase.Params[k2].MinValue, "--", formulaBase.Params[k2].MaxValue, ")"});
                    label2.Left = i1 + textBox.Width + 6;
                    label2.Top = j1 + 3;
                    label2.Parent = gbParam;
                    j1 += k1;
                }
            }
        }

        private void ScanForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Close();
            }
        }

        private void ScanForm_Activated(object sender, EventArgs e)
        {
            if (tvFormula.Nodes.Count == 0)
            {
                RefreshTree();
            }
        }

		private string GetParam()
		{
			string text1 = "";
			foreach (Control control1 in this.gbParam.Controls)
			{
				if (!(control1 is TextBox))
				{
					continue;
				}
				if (text1 != "")
				{
					text1 = text1 + ",";
				}
				text1 = text1 + (control1 as TextBox).Text;
			}
			return ("(" + text1 + ")");
		}


		private void btnScan_Click(object sender, EventArgs e)
		{
			TreeNode node1 = this.tvFormula.SelectedNode;
			if (node1 != null)
			{
				string text1 = (string) node1.Tag;
				int num1 = 0;
				if (text1 != null)
				{
					text1 = text1 + this.GetParam();
					FormulaBase base1 = FormulaBase.GetFormulaByName(text1);
					int num2 = base1.DataCountAtLeast();
					FileDataManager manager1 = new FileDataManager(null);
					string[] textArray1 = ListForm.Current.GetSymbolList();
					ArrayList list1 = new ArrayList();
					this.pbScan.Maximum = textArray1.Length;
					this.pbScan.Visible = true;
					if (textArray1.Length > 0)
					{
						string[] textArray2 = textArray1;
						for (int num4 = 0; num4 < textArray2.Length; num4++)
						{
							string text2 = textArray2[num4];
							CommonDataProvider provider1 = (CommonDataProvider) manager1[text2, num2];
							FormulaPackage package1 = base1.Run(provider1);
							FormulaData data1 = package1[package1.Count - 1];
							if ((data1.Length > 0) && (data1.LASTDATA > 0))
							{
								list1.Add(text2);
							}
							num1++;
							if (((num1 % 10) == 0) || (num1 == textArray1.Length))
							{
								this.pbScan.Value = num1;
								double num5 = ((double) num1) / ((double) textArray1.Length);
								this.lMsg.Text = num1.ToString() + "(" + num5.ToString("p2") + ")";
								Application.DoEvents();
								if ((num1 % 100) == 0)
								{
									GC.Collect();
								}
							}
						}
					}
					StockDB.LoadFolderRow(1, text1);
					int num3 = StockDB.GetMaxFolderId();
					if (list1.Count > 0)
					{
						this.pbScan.Maximum = list1.Count;
						num1 = 0;
						IEnumerator enumerator1 = list1.GetEnumerator();
						try
						{
							while (enumerator1.MoveNext())
							{
								StockDB.LoadFolderRelRow(enumerator1.Current, num3);
								num1++;
								if ((num1 % 100) == 0)
								{
									this.pbScan.Value = num1;
									Application.DoEvents();
								}
							}
						}
						finally
						{
							IDisposable disposable1 = enumerator1 as IDisposable;
							if (disposable1 != null)
							{
								disposable1.Dispose();
							}
						}
					}
					StockDB.ResetFolderDatabase();
					ListForm.Current.FolderId = num3;
					return;
				}
			}
			MessageBox.Show("Please select a condition!");
		}

    }

}
