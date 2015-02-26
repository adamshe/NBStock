using NB.StockStudio.ChartingObjects;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NB.StockStudio
{
    public class ToolPanel : Form
    {
        public ObjectToolPanel ObjectToolPanel;

        public PropertyGrid pg;

        private Container components = null;

        private TabControl tcAll;

        private TabPage tpTool;

        private TabPage tpProperties;

        public static ToolPanel NowToolPanel = new ToolPanel();


        public ToolPanel()
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
            ObjectToolPanel = new ObjectToolPanel();
            pg = new PropertyGrid();
            tcAll = new TabControl();
            tpTool = new TabPage();
            tpProperties = new TabPage();
            tcAll.SuspendLayout();
            tpTool.SuspendLayout();
            tpProperties.SuspendLayout();
            base.SuspendLayout();
            ObjectToolPanel.Dock = DockStyle.Fill;
            ObjectToolPanel.Location = new Point(0, 0);
            ObjectToolPanel.Name = "ObjectToolPanel";
            ObjectToolPanel.ResetAfterEachDraw = true;
            ObjectToolPanel.Size = new Size(244, 483);
            ObjectToolPanel.TabIndex = 0;
            pg.CommandsVisibleIfAvailable = true;
            pg.Dock = DockStyle.Fill;
            pg.HelpVisible = false;
            pg.LargeButtons = false;
            pg.LineColor = SystemColors.ScrollBar;
            pg.Location = new Point(0, 0);
            pg.Name = "pg";
            pg.Size = new Size(244, 483);
            pg.TabIndex = 4;
            pg.Text = "PropertyGrid";
            pg.ToolbarVisible = false;
            pg.ViewBackColor = SystemColors.Window;
            pg.ViewForeColor = SystemColors.WindowText;
            tcAll.Appearance = TabAppearance.FlatButtons;
            tcAll.Controls.Add(tpTool);
            tcAll.Controls.Add(tpProperties);
            tcAll.Dock = DockStyle.Fill;
            tcAll.Location = new Point(0, 0);
            tcAll.Name = "tcAll";
            tcAll.SelectedIndex = 0;
            tcAll.Size = new Size(252, 512);
            tcAll.TabIndex = 5;
            tpTool.Controls.Add(ObjectToolPanel);
            tpTool.Location = new Point(4, 25);
            tpTool.Name = "tpTool";
            tpTool.Size = new Size(244, 483);
            tpTool.TabIndex = 0;
            tpTool.Text = "Tools";
            tpProperties.Controls.Add(pg);
            tpProperties.Location = new Point(4, 25);
            tpProperties.Name = "tpProperties";
            tpProperties.Size = new Size(244, 483);
            tpProperties.TabIndex = 1;
            tpProperties.Text = "Properties";
            AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(252, 512);
            base.Controls.Add(tcAll);
            Font = new Font("Verdana", 8.25F);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ToolPanel";
            base.ShowInTaskbar = false;
            Text = "Tool Panel";
            base.TopMost = true;
            base.KeyDown += new KeyEventHandler(this.ToolPanel_KeyDown);
            base.Closing += new CancelEventHandler(this.ToolPanel_Closing);
            tcAll.ResumeLayout(false);
            tpTool.ResumeLayout(false);
            tpProperties.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void ToolPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Hide();
            }
        }

        private void ToolPanel_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }
    }

}
