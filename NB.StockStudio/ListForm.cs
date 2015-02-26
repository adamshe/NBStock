using NB.StockStudio.WinControls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using WindowsDemo.ColumnStyle;

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;


namespace NB.StockStudio
{
		 public class ListForm : Form
		 {
				  // Fields
				  public bool CanClose;
				  private ContextMenu cmList;
				  private IContainer components;
				  public static ListForm Current;
				  public DataGrid dgList;
				  private DataGridTableStyle dgsDefault;
				  private int folderId;
				  private string folderName;
				  private ImageList ilUpDown;
				  private MenuItem miAddToFavorite;
				  private MenuItem miDeleteFavorite;
				  private MenuItem miDeleteSymbol;
				  private MenuItem miFile;
				  private MenuItem miGotoExchange;
				  private MenuItem miGotoFavorite;
				  private MenuItem miOpen;
				  private MenuItem miOpen1;
				  private MenuItem miOpenNew;
				  private MenuItem miOpenNew1;
				  private MenuItem miSp1;
				  private MenuItem miSpliter;
				  private MenuItem miSwitchFolder;
				  private MenuItem miView;
				  private MenuItem miUpdate;
				  private MainMenu mmStockList;
				  
				  private Panel pnClient;
				  public bool RealtimeChanged;
				  public bool SymbolChanged;
				  private int SymbolColumn;
				  // Methods
				  public ListForm()
				  {
						   this.SymbolColumn = 1;
						   this.folderId = 0;
						   this.SymbolChanged = true;
						   this.RealtimeChanged = true;
						   StockDB.Open();
						   this.InitializeComponent();
						   this.AddStyleColumns(this.dgList.TableStyles[0]);
				  }

				  private void AddStyleColumns(DataGridTableStyle dgTableStyle)
				  {
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "#", "Num", 40));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Code", "Code", 70));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Name", "Name", 200));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Exchange", "Exchange", 60));
						   Hashtable hashtable1 = new Hashtable();
						   hashtable1.Add(-1, 0);
						   hashtable1.Add(0, 1);
						   hashtable1.Add(1, 2);
						   dgTableStyle.GridColumnStyles.Add(new DataGridImageColumn("", "", "UpDown", 20, this.ilUpDown, hashtable1));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("f2", "Last", "LastA", 80));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("f2", "Open", "OpenA", 80, 5));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("f2", "High", "High", 80, 5));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("f2", "Low", "Low", 80, 5));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("f2", "Close", "CloseA", 80, 5));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("+0.##;-0.##;0", "Change", "Change", 80, -1));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("+0.##%;-0.##%;0", "%", "ChangeP", 80, -1));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Volume", "Volume", 80, -1));
						   dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "LastTime", "LastTime", 100, -1));
				  }

				  private void cmList_Popup(object sender, EventArgs e)
				  {
						   this.miAddToFavorite.Visible = this.FolderId != StockDB.CurrentFavoriteId;
						   this.miDeleteFavorite.Visible = this.FolderId == StockDB.CurrentFavoriteId;
				  }

				  private void DeleteSymbol()
				  {
						   string text1 = this.GetSelectedSymbol();
						   if (MessageBox.Show(this, "Do you want to delete symbol '" + text1 + "'", "Worning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
						   {
									StockDB.DeleteSymbol(text1);
									this.RefreshData();
						   }
				  }

				  private void dgList_DoubleClick(object sender, EventArgs e)
				  {
						   this.OpenChart(Control.ModifierKeys == Keys.Control);
				  }

				  private void dgList_KeyDown(object sender, KeyEventArgs e)
				  {
						   if (e.KeyCode == Keys.Return)
						   {
									this.OpenChart(e.Control || false);
						   }
						   else if (e.KeyCode == Keys.Delete)
						   {
									this.DeleteSymbol();
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

				  private string GetSelectedSymbol()
				  {
						   return this.dgList[this.dgList.CurrentRowIndex, this.SymbolColumn].ToString();
				  }

				  public string[] GetSymbolList()
				  {
						   ArrayList list1 = new ArrayList();
						   DataView view1 = (DataView) this.dgList.DataSource;
						   if (view1 != null)
						   {
									foreach (DataRowView view2 in view1)
									{
											 list1.Add(view2["Code"]);
									}
						   }
						   return (string[]) list1.ToArray(typeof(string));
				  }

				  public void GotoSymbol(string Symbol)
				  {
						   for (int num1 = 0; num1 < StockDB.CurrentList.Count; num1++)
						   {
									if (this.dgList[num1, this.SymbolColumn].ToString().ToUpper() == Symbol.ToUpper())
									{
											 this.dgList.CurrentRowIndex = num1;
											 return;
									}
						   }
						   if (this.FolderId != 1)
						   {
									this.FolderId = 1;
									this.GotoSymbol(Symbol);
						   }
				  }

				  private void InitializeComponent()
				  {
						   this.components = new System.ComponentModel.Container();
						   System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ListForm));
						   this.dgList = new System.Windows.Forms.DataGrid();
						   this.cmList = new System.Windows.Forms.ContextMenu();
					  this.miUpdate = new System.Windows.Forms.MenuItem();
						   this.miOpen1 = new System.Windows.Forms.MenuItem();
						   this.miOpenNew1 = new System.Windows.Forms.MenuItem();
						   this.miSp1 = new System.Windows.Forms.MenuItem();
						   this.miAddToFavorite = new System.Windows.Forms.MenuItem();
						   this.miDeleteFavorite = new System.Windows.Forms.MenuItem();
						   this.miDeleteSymbol = new System.Windows.Forms.MenuItem();
						   this.dgsDefault = new System.Windows.Forms.DataGridTableStyle();
						   this.mmStockList = new System.Windows.Forms.MainMenu();
						   this.miFile = new System.Windows.Forms.MenuItem();
						   this.miOpen = new System.Windows.Forms.MenuItem();
						   this.miOpenNew = new System.Windows.Forms.MenuItem();
						   this.miSpliter = new System.Windows.Forms.MenuItem();
						   this.miView = new System.Windows.Forms.MenuItem();
						   this.miGotoExchange = new System.Windows.Forms.MenuItem();
						   this.miGotoFavorite = new System.Windows.Forms.MenuItem();
						   this.miSwitchFolder = new System.Windows.Forms.MenuItem();
						   this.ilUpDown = new System.Windows.Forms.ImageList(this.components);
						   this.pnClient = new System.Windows.Forms.Panel();
						   ((System.ComponentModel.ISupportInitialize)(this.dgList)).BeginInit();
						   this.pnClient.SuspendLayout();
						   this.SuspendLayout();
						   // 
						   // dgList
						   // 
						   this.dgList.AlternatingBackColor = System.Drawing.Color.Lavender;
						   this.dgList.BackColor = System.Drawing.Color.WhiteSmoke;
						   this.dgList.BackgroundColor = System.Drawing.Color.LightGray;
						   this.dgList.BorderStyle = System.Windows.Forms.BorderStyle.None;
						   this.dgList.CaptionBackColor = System.Drawing.Color.LightSteelBlue;
						   this.dgList.CaptionForeColor = System.Drawing.Color.MidnightBlue;
						   this.dgList.CaptionVisible = false;
						   this.dgList.ContextMenu = this.cmList;
						   this.dgList.DataMember = "";
						   this.dgList.Dock = System.Windows.Forms.DockStyle.Fill;
						   this.dgList.FlatMode = true;
						   this.dgList.Font = new System.Drawing.Font("Tahoma", 8F);
						   this.dgList.ForeColor = System.Drawing.Color.MidnightBlue;
						   this.dgList.GridLineColor = System.Drawing.Color.Gainsboro;
						   this.dgList.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
						   this.dgList.HeaderBackColor = System.Drawing.Color.MidnightBlue;
						   this.dgList.HeaderFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
						   this.dgList.HeaderForeColor = System.Drawing.Color.WhiteSmoke;
						   this.dgList.LinkColor = System.Drawing.Color.Teal;
						   this.dgList.Location = new System.Drawing.Point(0, 0);
						   this.dgList.Name = "dgList";
						   this.dgList.ParentRowsBackColor = System.Drawing.Color.Gainsboro;
						   this.dgList.ParentRowsForeColor = System.Drawing.Color.MidnightBlue;
						   this.dgList.ParentRowsVisible = false;
						   this.dgList.PreferredRowHeight = 5;
						   this.dgList.ReadOnly = true;
						   this.dgList.RowHeadersVisible = false;
						   this.dgList.SelectionBackColor = System.Drawing.Color.CadetBlue;
						   this.dgList.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
						   this.dgList.Size = new System.Drawing.Size(553, 507);
						   this.dgList.TabIndex = 0;
						   this.dgList.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																												   this.dgsDefault});
						   this.dgList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgList_KeyDown);
						   this.dgList.DoubleClick += new System.EventHandler(this.dgList_DoubleClick);

						   // 
						   // cmList
						   // 
						   this.cmList.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																										this.miUpdate ,
																									   this.miOpen1,
																									   this.miOpenNew1,
																									   this.miSp1,
																									   this.miAddToFavorite,
																									   this.miDeleteFavorite,
																									   this.miDeleteSymbol});
						   this.cmList.Popup += new System.EventHandler(this.cmList_Popup);
						//miUpdate
					  this.miUpdate.Index = 0;
					  this.miUpdate.Text = "&Update Today Data";
					  this.miUpdate.Click += new System.EventHandler(this.miUpdate_Click);
						   // 
						   // miOpen1
						   // 
						   this.miOpen1.Index = 0;
						   this.miOpen1.Text = "&Open";
						   this.miOpen1.Click += new System.EventHandler(this.miOpen_Click);
						   // 
						   // miOpenNew1
						   // 
						   this.miOpenNew1.Index = 1;
						   this.miOpenNew1.Text = "Open in &New window";
						   this.miOpenNew1.Click += new System.EventHandler(this.miOpenNew_Click);
						   // 
						   // miSp1
						   // 
						   this.miSp1.Index = 2;
						   this.miSp1.Text = "-";
						   // 
						   // miAddToFavorite
						   // 
						   this.miAddToFavorite.Index = 3;
						   this.miAddToFavorite.Text = "&Add to Favorite";
						   this.miAddToFavorite.Click += new System.EventHandler(this.miFavorite_Click);
						   // 
						   // miDeleteFavorite
						   // 
						   this.miDeleteFavorite.Index = 4;
						   this.miDeleteFavorite.Text = "&Delete from Favorite";
						   this.miDeleteFavorite.Click += new System.EventHandler(this.miDeleteFavorite_Click);
						   // 
						   // miDeleteSymbol
						   // 
						   this.miDeleteSymbol.Index = 5;
						   this.miDeleteSymbol.Text = "Delete &Symbol";
						   this.miDeleteSymbol.Click += new System.EventHandler(this.miDeleteSymbol_Click);
						   // 
						   // dgsDefault
						   // 
						   this.dgsDefault.AlternatingBackColor = System.Drawing.Color.Lavender;
						   this.dgsDefault.BackColor = System.Drawing.Color.WhiteSmoke;
						   this.dgsDefault.DataGrid = this.dgList;
						   this.dgsDefault.HeaderBackColor = System.Drawing.Color.Khaki;
						   this.dgsDefault.HeaderFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						   this.dgsDefault.HeaderForeColor = System.Drawing.Color.Black;
						   this.dgsDefault.MappingName = "NameList";
						   this.dgsDefault.ReadOnly = true;
						   this.dgsDefault.RowHeadersVisible = false;
						   // 
						   // mmStockList
						   // 
						   this.mmStockList.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																											this.miFile,
																											this.miView});
						   // 
						   // miFile
						   // 
						   this.miFile.Index = 0;
						   this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																									   this.miOpen,
																									   this.miOpenNew,
																									   this.miSpliter});
						   this.miFile.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
						   this.miFile.Text = "&File";
						   // 
						   // miOpen
						   // 
						   this.miOpen.Index = 0;
						   this.miOpen.MergeOrder = 1;
						   this.miOpen.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
						   this.miOpen.Text = "&Open";
						   this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
						   // 
						   // miOpenNew
						   // 
						   this.miOpenNew.Index = 1;
						   this.miOpenNew.MergeOrder = 2;
						   this.miOpenNew.Text = "Open in &New Window";
						   this.miOpenNew.Click += new System.EventHandler(this.miOpenNew_Click);
						   // 
						   // miSpliter
						   // 
						   this.miSpliter.Index = 2;
						   this.miSpliter.MergeOrder = 3;
						   this.miSpliter.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
						   this.miSpliter.Text = "-";
						   // 
						   // miView
						   // 
						   this.miView.Index = 1;
						   this.miView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																									   this.miGotoExchange,
																									   this.miGotoFavorite,
																									   this.miSwitchFolder});
						   this.miView.Text = "&View";
						   // 
						   // miGotoExchange
						   // 
						   this.miGotoExchange.Index = 0;
						   this.miGotoExchange.Shortcut = System.Windows.Forms.Shortcut.F6;
						   this.miGotoExchange.Text = "Goto &Exchange Folders";
						   this.miGotoExchange.Click += new System.EventHandler(this.miGotoExchange_Click);
						   // 
						   // miGotoFavorite
						   // 
						   this.miGotoFavorite.Index = 1;
						   this.miGotoFavorite.Shortcut = System.Windows.Forms.Shortcut.F7;
						   this.miGotoFavorite.Text = "Goto &Favorite Folders";
						   this.miGotoFavorite.Click += new System.EventHandler(this.miGotoFavorite_Click);
						   // 
						   // miSwitchFolder
						   // 
						   this.miSwitchFolder.Index = 2;
						   this.miSwitchFolder.Shortcut = System.Windows.Forms.Shortcut.F8;
						   this.miSwitchFolder.Text = "&Switch folders";
						   this.miSwitchFolder.Click += new System.EventHandler(this.miSwitchFolder_Click);
						   // 
						   // ilUpDown
						   // 
						   this.ilUpDown.ImageSize = new System.Drawing.Size(7, 8);
						   this.ilUpDown.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilUpDown.ImageStream")));
						   this.ilUpDown.TransparentColor = System.Drawing.Color.Transparent;
						   // 
						   // pnClient
						   // 
						   this.pnClient.Controls.Add(this.dgList);
						   this.pnClient.Dock = System.Windows.Forms.DockStyle.Fill;
						   this.pnClient.Location = new System.Drawing.Point(0, 0);
						   this.pnClient.Name = "pnClient";
						   this.pnClient.Size = new System.Drawing.Size(553, 507);
						   this.pnClient.TabIndex = 2;
						   // 
						   // ListForm
						   // 
						   this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
						   this.ClientSize = new System.Drawing.Size(553, 507);
						   this.Controls.Add(this.pnClient);
						   this.KeyPreview = true;
						   this.Menu = this.mmStockList;
						   this.Name = "ListForm";
						   this.ShowInTaskbar = false;
						   this.Text = "Stock List";
						   this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
						   this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListForm_KeyDown);
						   this.Closing += new System.ComponentModel.CancelEventHandler(this.ListForm_Closing);
						   this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListForm_KeyPress);
						   this.Load += new System.EventHandler(this.ListForm_Load);
						   this.Closed += new System.EventHandler(this.ListForm_Closed);
						   this.Activated += new System.EventHandler(this.ListForm_Activated);
						   ((System.ComponentModel.ISupportInitialize)(this.dgList)).EndInit();
						   this.pnClient.ResumeLayout(false);
						   this.ResumeLayout(false);

				  }

				  private void ListForm_Activated(object sender, EventArgs e)
				  {
						   StatisticForm.HideStatistic();
				  }

				  private void ListForm_Closed(object sender, EventArgs e)
				  {
						   StockDB.Close();
				  }

				  private void ListForm_Closing(object sender, CancelEventArgs e)
				  {
						   e.Cancel = !this.CanClose;
				  }

				  private void ListForm_KeyDown(object sender, KeyEventArgs e)
				  {
						   Keys keys1 = e.KeyCode;
						   if (keys1 != Keys.Escape)
						   {
									switch (keys1)
									{
											 case Keys.Add:
											 {
													  if (this.dgList.Font.Size < 15f)
													  {
															   this.dgList.Font = new Font(this.dgList.Font.FontFamily, this.dgList.Font.Size + 1f);
													  }
													  return;
											 }
											 case Keys.Separator:
											 {
													  return;
											 }
											 case Keys.Subtract:
											 {
													  if (this.dgList.Font.Size > 7f)
													  {
															   this.dgList.Font = new Font(this.dgList.Font.FontFamily, this.dgList.Font.Size - 1f);
													  }
													  return;
											 }
									}
						   }
						   else if (this.FolderId != 0)
						   {
									this.FolderId = 0;
									this.FolderName = StockDB.LookupFolderName(this.FolderId);
						   }
				  }

				  private void ListForm_KeyPress(object sender, KeyPressEventArgs e)
				  {
						   if (e.KeyChar != '+')
						   {
									SymbolForm.PressKeyAndShow(this, e.KeyChar);
						   }
				  }

				  private void ListForm_Load(object sender, EventArgs e)
				  {
						   this.RefreshList();
						   SymbolForm.InitSymbolList(StockDB.GetSymbolList());
				  }

				  private void miDeleteFavorite_Click(object sender, EventArgs e)
				  {
						   if (this.dgList.CurrentRowIndex >= 0)
						   {
									StockDB.DeleteFolderRelRows(this.GetSelectedSymbol(), this.FolderId);
						   }
						   this.RefreshData();
				  }

				  private void miDeleteSymbol_Click(object sender, EventArgs e)
				  {
						   this.DeleteSymbol();
				  }

				  private void miFavorite_Click(object sender, EventArgs e)
				  {
						   if (this.dgList.CurrentRowIndex >= 0)
						   {
									StockDB.AddToFavorite((string) this.dgList[this.dgList.CurrentRowIndex, this.SymbolColumn]);
						   }
				  }

				  private void miGotoExchange_Click(object sender, EventArgs e)
				  {
						   if (this.FolderId == StockDB.CurrentExchangeId)
						   {
									this.FolderId = StockDB.NextFolderId(this.FolderId);
									StockDB.CurrentExchangeId = this.FolderId;
						   }
						   else
						   {
									this.FolderId = StockDB.CurrentExchangeId;
						   }
				  }

				  private void miGotoFavorite_Click(object sender, EventArgs e)
				  {
						   if (this.FolderId == StockDB.CurrentFavoriteId)
						   {
									this.FolderId = StockDB.NextFolderId(this.FolderId);
									StockDB.CurrentFavoriteId = this.FolderId;
						   }
						   else
						   {
									this.FolderId = StockDB.CurrentFavoriteId;
						   }
				  }
				
			     private void miUpdate_Click(object sender, EventArgs e)
			     {
				     YahooDataManager ydm = new YahooDataManager();
				     ydm.CacheRoot = FormulaHelper.Root +"Cache";
				 
				     ChartForm.DataManager = ydm;
                     string code = (string)dgList[dgList.CurrentRowIndex, 1];
                     ChartForm.OpenChartForm(code, MainForm.MdiMainForm, true);
			     }
				  private void miOpen_Click(object sender, EventArgs e)
				  {
						   this.OpenChart();
				  }

				  private void miOpenNew_Click(object sender, EventArgs e)
				  {
						   this.OpenChart(true);
				  }

				  private void miSwitchFolder_Click(object sender, EventArgs e)
				  {
						   int num1 = StockDB.NextFolderId(this.FolderId);
						   if (num1 != 0)
						   {
									this.FolderId = num1;
						   }
				  }

				  private void OpenChart()
				  {
						   this.OpenChart(false);
				  }

				  private void OpenChart(bool NewWindow)
				  {
						   this.OpenChart(this.dgList.CurrentRowIndex, NewWindow);
				  }

				  private void OpenChart(int i, bool NewWindow)
				  {
						   if (StockDB.CurrentList.Count > 0)
						   {
									i = (i + StockDB.CurrentList.Count) % StockDB.CurrentList.Count;
									this.dgList.CurrentRowIndex = i;
                                    string ticker = GetCurrentSelectedSymbol();
									ChartForm.OpenChartForm(ticker, base.MdiParent, NewWindow);
						   }
				  }

                  private string GetCurrentSelectedSymbol ()
                  {
                      string ticker = (string)this.dgList[dgList.CurrentRowIndex, this.SymbolColumn];
                      return ticker;
                  }

				  private void RefreshData()
				  {
						   this.dgList.DataSource = StockDB.GetStockList(this.FolderId);
				  }

				  public void RefreshList()
				  {
						   if (!this.SymbolChanged && this.RealtimeChanged)
						   {
									this.RefreshData();
						   }
						   else if (this.SymbolChanged || this.RealtimeChanged)
						   {
									StockDB.ResetList();
									this.RefreshData();
									StockDB.RecreateFolders();
						   }
						   this.RealtimeChanged = false;
						   this.SymbolChanged = false;
				  }

				  public static void ShowForm()
				  {
						   ListForm.ShowForm(null);
				  }

				  public static void ShowForm(Form Owner)
				  {
						   if (ListForm.Current == null)
						   {
									ListForm.Current = new ListForm();
									ListForm.Current.MdiParent = Owner;
									ListForm.Current.Show();
						   }
						   ListForm.Current.Activate();
				  }

				  public void ShowNextChart(int Delta)
				  {
						   this.OpenChart(this.dgList.CurrentRowIndex + Delta, false);
				  }


				  // Properties
				  public int FolderId
				  {
						   get
						   {
									return this.folderId;
						   }
						   set
						   {
									if (this.folderId != value)
									{
											 this.folderId = value;
											 this.FolderName = StockDB.LookupFolderName(this.folderId);
											 this.RefreshData();
									}
						   }
				  }

				  public string FolderName
				  {
						   get
						   {
									return this.folderName;
						   }
						   set
						   {
									this.folderName = value;
									this.Text = "Stock List -  " + this.folderName;
						   }
				  }


				  
		 }




}
