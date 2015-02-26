using NB.StockStudio.WinControls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NB.StockStudio
{
    public class ExpertAboutForm : AboutForm
    {

        private IContainer components = null;


        public ExpertAboutForm()
        {
            InitializeComponent();
       //     llOrder.Visible = true;
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
            this.SuspendLayout();
            // 
            // ExpertAboutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(498, 312);
            this.Name = "ExpertAboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void llOrder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://finance.easychart.net/order.aspx");
        }
    }

}
