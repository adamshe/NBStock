namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class SelectFormula : Form
    {
        private Button btnOK;
        private Container components = null;
        private string FormulaName;
        private Label lParameter;
        private TextBox tb1;
        private TextBox tb2;
        private TextBox tb3;
        private TextBox tb4;
        private TextBox[] tbParams;

        private bool SelectLine;
        private string[] FilterPrefixes;
        private string Result;

        public SelectFormula()
        {
            this.InitializeComponent();
            this.tbParams = new TextBox[] { this.tb1, this.tb2, this.tb3, this.tb4 };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetFormula()
        {
            return TextBoxToParam(this.FormulaName, this.tbParams);
        }

        private void InitializeComponent()
        {
            this.tb1 = new TextBox();
            this.tb2 = new TextBox();
            this.tb3 = new TextBox();
            this.btnOK = new Button();
            this.lParameter = new Label();
            this.tb4 = new TextBox();
            base.SuspendLayout();
            this.tb1.Location = new Point(0x30, 0x30);
            this.tb1.Name = "tb1";
            this.tb1.Size = new Size(0xb0, 0x16);
            this.tb1.TabIndex = 1;
            this.tb1.Text = "";
            this.tb2.Location = new Point(0x30, 80);
            this.tb2.Name = "tb2";
            this.tb2.Size = new Size(0xb0, 0x16);
            this.tb2.TabIndex = 2;
            this.tb2.Text = "";
            this.tb3.Location = new Point(0x30, 0x70);
            this.tb3.Name = "tb3";
            this.tb3.Size = new Size(0xb0, 0x16);
            this.tb3.TabIndex = 3;
            this.tb3.Text = "";
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(160, 0xb8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x40, 0x17);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.lParameter.AutoSize = true;
            this.lParameter.Location = new Point(13, 0x12);
            this.lParameter.Name = "lParameter";
            this.lParameter.Size = new Size(0x77, 0x12);
            this.lParameter.TabIndex = 5;
            this.lParameter.Text = "Adjust parameter:";
            this.tb4.Location = new Point(0x30, 0x90);
            this.tb4.Name = "tb4";
            this.tb4.Size = new Size(0xb0, 0x16);
            this.tb4.TabIndex = 6;
            this.tb4.Text = "";
            this.AutoScaleBaseSize = new Size(7, 15);
            base.ClientSize = new Size(0xf2, 0xd7);
            base.Controls.Add(this.tb4);
            base.Controls.Add(this.lParameter);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.tb3);
            base.Controls.Add(this.tb2);
            base.Controls.Add(this.tb1);
            this.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SelectFormula";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "SelectFormula";
            base.KeyDown += new KeyEventHandler(this.SelectFormula_KeyDown);
            base.ResumeLayout(false);
        }

        public static void ParamToTextBox(string NameAndParam, TextBox[] tbs, out string FormulaName)
        {
            FormulaBase formulaByName = FormulaBase.GetFormulaByName(NameAndParam);
            FormulaName = formulaByName.FormulaName;
            for (int i = 0; i < tbs.Length; i++)
            {
                if (i < formulaByName.Params.Count)
                {
                    tbs[i].Text = formulaByName.Params[i].Value;
                }
                else
                {
                    tbs[i].Text = "";
                }
            }
        }

        public static string Select(string Default)
        {
            try
            {
                SelectFormula formula = new SelectFormula();
                formula.SetFormula(Default);
                if (formula.ShowDialog() == DialogResult.OK)
                {
                    return formula.GetFormula();
                }
            }
            catch
            {
            }
            return "";
        }

        public string Select(string Default, string[] FilterPrefixes, bool SelectLine)
        {
            //SelectFormula Current = new SelectFormula();
            if (FilterPrefixes != null)
                for (int i = 0; i < FilterPrefixes.Length; i++)
                    FilterPrefixes[i] = FilterPrefixes[i].ToUpper();

            this.SelectLine = SelectLine;
            this.FilterPrefixes = FilterPrefixes;
            if (this.ShowDialog() == DialogResult.OK)
                return this.Result;
            return null;
        }

        private void SelectFormula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Close();
            }
            else if (e.KeyCode == Keys.Return)
            {
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        public void SetFormula(string r)
        {
            this.Text = r;
            ParamToTextBox(r, this.tbParams, out this.FormulaName);
        }

        public static string TextBoxToParam(string FormulaName, TextBox[] tbs)
        {
            string str = "";
            int index = FormulaName.IndexOf('(');
            if (index >= 0)
            {
                FormulaName = FormulaName.Substring(0, index);
            }
            for (int i = 0; i < tbs.Length; i++)
            {
                if (!(tbs[i].Text != ""))
                {
                    break;
                }
                if (str != "")
                {
                    str = str + ",";
                }
                str = str + tbs[i].Text;
            }
            if (str != "")
            {
                str = "(" + str + ")";
            }
            if (FormulaBase.GetFormulaByName(FormulaName) != null)
            {
                return (FormulaName + str);
            }
            return "";
        }
    }
}

