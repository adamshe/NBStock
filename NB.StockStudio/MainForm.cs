using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;
using NB.StockStudio.ChartingObjects;
//using NB.StockStudio.ChartingObjects.Infrastructure;
using NB.StockStudio.WinControls;
using System; 
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace NB.StockStudio
{
	public class MainForm : Form
	{
		// Methods
		public MainForm()
		{
			adamAbout = new AboutForm();
			this.eaf = new ExpertAboutForm();
			ObjectManager.LoadSettings(ToolPanel.NowToolPanel.ObjectToolPanel);
			this.InitializeComponent();
			ObjectHelper.CreateObjectPath();
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
			this.MainMenu = new MainMenu();
			this.miFile = new MenuItem();
			this.miExit = new MenuItem();
			this.miTools = new MenuItem();
			this.miDataManager = new MenuItem();
			this.miScan = new MenuItem();
			this.miFolder = new MenuItem();
			this.miEditor = new MenuItem();
			this.miQuickAdd = new MenuItem();
			this.miObjectToolPanel = new MenuItem();
			this.miWindow = new MenuItem();
			this.miArrangeIcons = new MenuItem();
			this.miCascade = new MenuItem();
			this.miTileVertical = new MenuItem();
			this.miTileHorizontal = new MenuItem();
			this.miHelp = new MenuItem();
			this.miHelpContent = new MenuItem();
			this.miSpliter2 = new MenuItem();
			this.miAbout = new MenuItem();
			this.StatasBar = new StatusBar();
			this.sbIndex = new StatusBarPanel();
			this.sbTime = new StatusBarPanel();
			this.tmCurrent = new System.Windows.Forms.Timer(this.components);
			this.tmShareware = new System.Windows.Forms.Timer(this.components);
			this.sbIndex.BeginInit();
			this.sbTime.BeginInit();
			base.SuspendLayout();
			
			this.MainMenu.MenuItems.AddRange(new MenuItem[] { this.miFile, this.miTools, this.miWindow, this.miHelp });
			this.miFile.Index = 0;
			
			this.miFile.MenuItems.AddRange(new MenuItem[] { this.miExit });
			this.miFile.MergeType = MenuMerge.MergeItems;
			this.miFile.Text = "&File";
			this.miExit.Index = 0;
			this.miExit.MergeOrder = 0x65;
			this.miExit.Shortcut = Shortcut.AltF4;
			this.miExit.Text = "&Exit";
			this.miExit.Click += new EventHandler(this.miExit_Click);
			this.miTools.Index = 1;
			
			this.miTools.MenuItems.AddRange(new MenuItem[] { this.miDataManager, this.miScan, this.miFolder, this.miEditor, this.miQuickAdd, this.miObjectToolPanel } );
			this.miTools.MergeOrder = 1;
			this.miTools.Text = "&Tools";
			this.miDataManager.Index = 0;
			this.miDataManager.Shortcut = Shortcut.CtrlD;
			this.miDataManager.Text = "&Data Manager";
			this.miDataManager.Click += new EventHandler(this.miDataManager_Click);
			this.miScan.Index = 1;
			this.miScan.Shortcut = Shortcut.CtrlS;
			this.miScan.Text = "&Scan";
			this.miScan.Click += new EventHandler(this.miScan_Click);
			this.miFolder.Index = 2;
			this.miFolder.Shortcut = Shortcut.CtrlW;
			this.miFolder.Text = "&Control Panel";
			this.miFolder.Click += new EventHandler(this.miFolder_Click);
			this.miEditor.Index = 3;
			this.miEditor.Shortcut = Shortcut.CtrlF;
			this.miEditor.Text = "&Formula Editor";
			this.miEditor.Click += new EventHandler(this.miEditor_Click);
			this.miQuickAdd.Index = 4;
			this.miQuickAdd.Shortcut = Shortcut.Ins;
			this.miQuickAdd.Text = "&Quick add stock";
			this.miQuickAdd.Click += new EventHandler(this.miQuickAdd_Click);
			this.miObjectToolPanel.Index = 5;
			this.miObjectToolPanel.Shortcut = Shortcut.CtrlT;
			this.miObjectToolPanel.Text = "&Object Tool Panel";
			this.miObjectToolPanel.Click += new EventHandler(this.miObjectToolPanel_Click);
			this.miWindow.Index = 2;
			this.miWindow.MdiList = true;
			
			this.miWindow.MenuItems.AddRange(new MenuItem[4] { this.miArrangeIcons, this.miCascade, this.miTileVertical, this.miTileHorizontal } );
			this.miWindow.MergeOrder = 10;
			this.miWindow.Text = "&Window";
			this.miArrangeIcons.Index = 0;
			this.miArrangeIcons.Text = "&Arrange Icons";
			this.miArrangeIcons.Click += new EventHandler(this.miVertical_Click);
			this.miCascade.Index = 1;
			this.miCascade.Text = "&Cascade";
			this.miCascade.Click += new EventHandler(this.miVertical_Click);
			this.miTileVertical.Index = 2;
			this.miTileVertical.Text = "Tile &Vertical";
			this.miTileVertical.Click += new EventHandler(this.miVertical_Click);
			this.miTileHorizontal.Index = 3;
			this.miTileHorizontal.Text = "Tile &Horizontal";
			this.miTileHorizontal.Click += new EventHandler(this.miVertical_Click);
			this.miHelp.Index = 3;
			
			this.miHelp.MenuItems.AddRange(new MenuItem[3] { this.miHelpContent, this.miSpliter2, this.miAbout } );
			this.miHelp.MergeOrder = 11;
			this.miHelp.Text = "&Help";
			this.miHelpContent.Index = 0;
			this.miHelpContent.Shortcut = Shortcut.F1;
			this.miHelpContent.Text = "Help &Content";
			this.miHelpContent.Click += new EventHandler(this.miHelpContent_Click);
			this.miSpliter2.Index = 1;
			this.miSpliter2.Text = "-";
			this.miAbout.Index = 2;
			this.miAbout.Text = "About";
			this.miAbout.Click += new EventHandler(this.miAbout_Click);
			this.StatasBar.Location = new Point(0, 0x1df);
			this.StatasBar.Name = "StatasBar";
			StatusBarPanel[] panelArray1 = new StatusBarPanel[2] { this.sbIndex, this.sbTime } ;
			this.StatasBar.Panels.AddRange(panelArray1);
			this.StatasBar.ShowPanels = true;
			this.StatasBar.Size = new Size(0x268, 0x16);
			this.StatasBar.TabIndex = 1;
			this.sbTime.Alignment = HorizontalAlignment.Right;
			this.sbTime.AutoSize = StatusBarPanelAutoSize.Contents;
			this.sbTime.Width = 10;
			this.tmCurrent.Enabled = true;
			this.tmCurrent.Interval = 1000;
			this.tmCurrent.Tick += new EventHandler(this.tmCurrent_Tick);
			this.tmShareware.Interval = 1000;
			this.tmShareware.Tick += new EventHandler(this.tmShareware_Tick);
			this.AutoScaleBaseSize = new Size(6, 14);
			this.BackColor = SystemColors.Control;
			base.ClientSize = new Size(0x268, 0x1f5);
			base.Controls.Add(this.StatasBar);
			this.Font = new Font("Verdana", 8.25f);
			this.ForeColor = SystemColors.GrayText;
			base.IsMdiContainer = true;
			base.Menu = this.MainMenu;
			base.Name = "MainForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Stock Expert";
			base.WindowState = FormWindowState.Maximized;
			base.Closing += new CancelEventHandler(this.MainForm_Closing);
			base.Load += new EventHandler(this.MainForm_Load);
			this.sbIndex.EndInit();
			this.sbTime.EndInit();
			base.ResumeLayout(false);
		}

		[STAThread]
		private static void Main()
		{
			MainForm.MdiMainForm = new MainForm();
			Application.Run(MainForm.MdiMainForm);
		}

		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			int num1 = StockDB.RealtimeChangedCount;
			if ((num1 != 0) && (MessageBox.Show("Do you want to merge realtime data to historical database? Total:" + num1, "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes))
			{
				StockDB.MergeRealtime();
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			try
			{
				PluginManager.Load(Environment.CurrentDirectory + @"\Plugins\");
				PluginManager.OnPluginChanged += new FileSystemEventHandler(this.OnPluginChange);
				ListForm.ShowForm(this);
				SymbolForm.AddFolder();
				tmShareware.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.InnerException.Message);
			}
		}

		private void miAbout_Click(object sender, EventArgs e)
		{
			adamAbout.ShowDialog();
		}

		private void miDataManager_Click(object sender, EventArgs e)
		{
			DataManagerForm.ShowIt();
			ListForm.Current.RefreshList();
		}

		private void miEditor_Click(object sender, EventArgs e)
		{
			ChartWinControl.OpenFormulaEditor();
		}

		private void miExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void miFolder_Click(object sender, EventArgs e)
		{
			FolderForm.ShowIt();
		}

		private void miHelpContent_Click(object sender, EventArgs e)
		{
			Process.Start("NB.StockStudio.chm");
		}

		private void miHotKey_Click(object sender, EventArgs e)
		{
			Process.Start("HotKey.html");
		}

		private void miObjectToolPanel_Click(object sender, EventArgs e)
		{
			ToolPanel.NowToolPanel.Show();
		}

		private void miQuickAdd_Click(object sender, EventArgs e)
		{
			string text1 = InputBox.ShowInputBox("Symbol", "");
			if (text1 != "")
			{
				if (StockDB.GetSymbol(text1) == null)
				{
					string[] textArray1 = YahooDataManager.GetStockName(text1);
					if (textArray1.Length == 3)
					{
						StockDB.LoadSymbolRow(text1, textArray1[1], textArray1[2]);
						StockDB.RecreateFolders();
						ListForm.Current.SymbolChanged = true;
						ListForm.Current.RefreshList();
					}
				}
				ListForm.Current.GotoSymbol(text1);
			}
		}

		private void miScan_Click(object sender, EventArgs e)
		{
			ScanForm.ShowIt();
		}

		private void miVertical_Click(object sender, EventArgs e)
		{
			base.LayoutMdi((MdiLayout) Enum.Parse(typeof(MdiLayout), ((MenuItem) sender).Text.Replace(" ", "").Replace("&", ""), true));
		}

		private void OnPluginChange(object source, FileSystemEventArgs e)
		{
			if (base.ActiveMdiChild is ChartForm)
			{
				(base.ActiveMdiChild as ChartForm).RefreshChart();
			}
			SymbolForm.RefreshIndicators();
		}

		public void OnThreadException(object sender, ThreadExceptionEventArgs t)
		{
			try
			{
				this.ShowThreadExceptionDialog(t.Exception);
				base.Focus();
				if (t.Exception is TerminateException)
				{
					base.Close();
				}
			}
			catch
			{
				try
				{
					MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
					return;
				}
				finally
				{
					Application.Exit();
				}
			}
		}

		private DialogResult ShowThreadExceptionDialog(Exception e)
		{
			return MessageBox.Show(e.Message, "Messages!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void tmCurrent_Tick(object sender, EventArgs e)
		{
		}

		private void tmShareware_Tick(object sender, EventArgs e)
		{
//			this.tmShareware.Enabled = false;
//			if (eaf != null && !this.eaf.Visible)
//			{
//				this.eaf.ShowDialog();
//			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x10)
			{
				try
				{
					if (ListForm.Current != null)
					{
						ListForm.Current.CanClose = true;
					}
				}
				catch
				{
				}
			}
			base.WndProc(ref m);
		}


		// Fields
		private IContainer components;
		private ExpertAboutForm eaf;
		private AboutForm adamAbout;
		private MainMenu MainMenu;
		public static MainForm MdiMainForm;
		private MenuItem miAbout;
		private MenuItem miArrangeIcons;
		private MenuItem miCascade;
		private MenuItem miDataManager;
		private MenuItem miEditor;
		private MenuItem miExit;
		private MenuItem miFile;
		private MenuItem miFolder;
		private MenuItem miHelp;
		private MenuItem miHelpContent;
		private MenuItem miObjectToolPanel;
		private MenuItem miQuickAdd;
		private MenuItem miScan;
		private MenuItem miSpliter2;
		private MenuItem miTileHorizontal;
		private MenuItem miTileVertical;
		private MenuItem miTools;
		private MenuItem miWindow;
		private StatusBarPanel sbIndex;
		private StatusBarPanel sbTime;
		private StatusBar StatasBar;
		private System.Windows.Forms.Timer tmCurrent;
		private System.Windows.Forms.Timer tmShareware;
	}


}
