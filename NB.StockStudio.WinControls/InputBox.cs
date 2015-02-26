namespace NB.StockStudio.WinControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class InputBox : Form
    {
        private Button btnOK;
        private Container components = null;
        private Label lCaption;
        private TextBox tbData;

        public InputBox()
        {
            this.InitializeComponent();
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
            this.lCaption = new Label();
            this.tbData = new TextBox();
            this.btnOK = new Button();
            base.SuspendLayout();
            this.lCaption.Location = new Point(8, 11);
            this.lCaption.Name = "lCaption";
            this.lCaption.Size = new Size(0x68, 0x10);
            this.lCaption.TabIndex = 0;
            this.lCaption.Text = "Your Text:";
            this.lCaption.TextAlign = ContentAlignment.MiddleLeft;
            this.tbData.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbData.Location = new Point(8, 0x25);
            this.tbData.Name = "tbData";
            this.tbData.Size = new Size(400, 0x15);
            this.tbData.TabIndex = 1;
            this.tbData.Text = "";
            this.tbData.KeyDown += new KeyEventHandler(this.tbData_KeyDown);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x14d, 0x41);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x1a2, 0x5f);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.tbData);
            base.Controls.Add(this.lCaption);
            this.Font = new Font("Verdana", 8.5f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InputBox";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "InputBox";
            base.KeyDown += new KeyEventHandler(this.InputBox_KeyDown);
            base.ResumeLayout(false);
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Close();
            }
        }

        public static string ShowInputBox(string Caption, string Default)
        {
            InputBox box = new InputBox();
            box.lCaption.Text = Caption;
            box.tbData.Text = Default;
            if (box.ShowDialog() == DialogResult.OK)
            {
                return box.tbData.Text;
            }
            return "";
        }

        private void tbData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.btnOK.PerformClick();
            }
        }
    }
}

