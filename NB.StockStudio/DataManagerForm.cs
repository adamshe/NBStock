using NB.StockStudio;
using NB.StockStudio.Foundation.DataProvider;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace NB.StockStudio
{
	public class DataManagerForm : Form
	{
		private object batenHistoryownload = new object();
		private object batenRealtimeDownload = new object();
		private ParallelOptions m_po;
		private CancellationTokenSource m_cts;
		private int historyDownloadCount;
		private int realtimeDownloadCount;
		private Label lblResult;
		private Button btnStopDownloadHistory;
		private int downloadSelectMode;
		// Methods
		public DataManagerForm()
		{
			this.components = null;
			this.DownloadHistoryThread = null;
			this.DownloadRealtimeThread = null;
			this.InitializeComponent();
		}

		#region Button Click
		
		private void btnBeginDownload_Click(object sender, EventArgs e)
		{
			lock (batenHistoryownload)
			{//btnBeginDownload.Text.Contains("Start")
				if (this.DownloadHistoryThread != null)
				{
					StopDownloadHistory();
					DownloadHistoryThread = null;
				}
				ListForm.Current.RealtimeChanged = true;
				StartDownloadHistory();
				//Task.Factory.StartNew(()=>StartDownloadHistory());
				//	this.EnableDisableActions(false);  

				//DownloadHistoryThread.Join();
				DownloadHistoryThread = null;
				this.btnBeginDownload.Text = "&Start Download";
				this.EnableDisableActions(true);
			}
			//else
			//{
			//    Canceller.Cancel();
			//    StopDownloadHistory();
			//}			
		}

		private void btnStopDownloadHistory_Click(object sender, EventArgs e)
		{
		  //  Canceller.Cancel();
			StopDownloadHistory();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			StockDB.ClearSymbolList();
			this.SymbolChanged();
			MessageBox.Show("All symbols were removed!");
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			char[] chArray1 = new char[1] { '\n' } ;
			string[] textArray1 = this.tbAddStock.Text.Split(chArray1);
			int num1 = 0;
			string[] textArray2 = textArray1;
			for (int num3 = 0; num3 < textArray2.Length; num3++)
			{
				string text1 = textArray2[num3];
				string text2 = text1;
				int num2 = text2.IndexOf(';');
				if (num2 > 0)
				{
					text2 = text2.Substring(0, num2);
				}
				try
				{
					string text3 = "Code='" + text2.Trim() + "'";
					num1 += StockDB.DeleteSymbolRows(text3);
					StockDB.DeleteFolderRelRows(text3);
					StockDB.DeleteListRows(text3);
				}
				catch
				{
				}
			}
			MessageBox.Show(this, "Delete success , total :" + num1);
		}

		private void btnDownloadRealtime_Click(object sender, EventArgs e)
		{
			if (this.DownloadRealtimeThread == null)
			{
				this.EnableDisableActions(false);
				ListForm.Current.RealtimeChanged = true;
				this.StartDownloadRealtime();
			//	MethodInvoker mi = new MethodInvoker(this.DownloadRealtime );				
			//	 this.BeginInvoke(mi);
						
			}
		}
			 
		private void btnExport_Click(object sender, EventArgs e)
		{
			DataRow[] rowArray1 = StockDB.GetDataRows("SymbolList", "");
			StringBuilder builder1 = new StringBuilder();
			DataRow[] rowArray2 = rowArray1;
			for (int num1 = 0; num1 < rowArray2.Length; num1++)
			{
				DataRow row1 = rowArray2[num1];
				object[] objArray1 = new object[6] { row1["Code"], ";", row1["Name"], ";", row1["Exchange"], "\r\n" } ;
				builder1.Append(string.Concat(objArray1));
			}
			this.tbAddStock.Text = builder1.ToString();
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			this.EnableDisableActions(false, false);
			try
			{
				char[] chArray1 = new char[1] { '\n' } ;
				string[] textArray1 = this.tbAddStock.Text.Split(chArray1);
				int num1 = 0;
				int num2 = 0;
				this.pbImport.Maximum = textArray1.Length;
				string[] textArray3 = textArray1;
				for (int num3 = 0; num3 < textArray3.Length; num3++)
				{
					string text1 = textArray3[num3];
					char[] chArray2 = new char[1] { ';' } ;
					string[] textArray2 = text1.Trim().Split(chArray2);
					if (textArray2.Length < 3)
					{
						goto Label_009A;
					}
					try
					{
						StockDB.LoadSymbolRow(textArray2);
						num1++;
					}
					catch
					{
					}
					num2++;
					if ((num2 % 100) == 0)
					{
						this.pbImport.Value = num2;
						Application.DoEvents();
					}
				Label_009A:;
				}
				this.SymbolChanged();
				MessageBox.Show(this, "Import success , total :" + num1);
			}
			finally
			{
				this.EnableDisableActions(true);
			}
		}

		private void btnImportSymbol_Click(object sender, EventArgs e)
		{
			this.StartImportSymbol();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			this.StopDownloadHistory();
			this.StopDownloadRealtime();
			this.StopImportSymbol();
			this.StopUpdateEODData();
			this.StopUpdateIndustry();
			this.EnableDisableActions(true);
		}

		private void btnUpdateEOD_Click(object sender, EventArgs e)
		{
			this.StartUpdateEODData();
		}

		private void btnUpdateIndustry_Click(object sender, EventArgs e)
		{
			this.StartUpdateIndustry();
		}

		private void DataManagerForm_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = !this.btnOK.Enabled;
		}

		private void DataManagerForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.Close();
			}
		}

		private void DataManagerForm_Load(object sender, EventArgs e)
		{
			this.dpStop.Value = DateTime.Now.Date;
			this.cbMode.SelectedIndex = 0;
			this.cbExchange.SelectedIndex = 0;
		}

		#endregion

		#region Property

		public ParallelOptions ParallelParameter
		{
			get
			{
				if (m_po == null ) 
					m_po = new ParallelOptions
					{
						CancellationToken = Canceller.Token, 
						MaxDegreeOfParallelism = System.Environment.ProcessorCount
					};
				return m_po;
			}
		}

		public CancellationTokenSource Canceller
		{
			get
			{                
				m_cts = new CancellationTokenSource();
				return m_cts;
			}
		}

		#endregion
		

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
				StopDownloadHistory();
				StopDownloadRealtime();
			}
			base.Dispose(disposing);
		}

		private string DownloadData(string URL)
		{
			WebClient client1 = new WebClient();
			byte[] buffer1 = client1.DownloadData(URL);
			return Encoding.ASCII.GetString(buffer1);
		}

		delegate void SetTextCallback(int number, int totalItems, string text);

		private void UpdateProgressbar (int number, int totalItems, string text)
		{
			if (pbDownload.InvokeRequired)
			{
				pbDownload.BeginInvoke(new SetTextCallback(UpdateProgressbar), number, totalItems, text);
			}
			else
			{
				Interlocked.Increment(ref this.historyDownloadCount);
				this.pbDownload.Value = historyDownloadCount;
				this.pbDownload.PerformStep();
				this.lDownload.Text = historyDownloadCount + "/" + totalItems;
				this.lDownload.Refresh();

				if (Interlocked.CompareExchange(ref historyDownloadCount, 0, totalItems) == totalItems)
				{
					lDownload.Text += " download completed!";
					this.pbDownload.Value = historyDownloadCount;
				}
				Application.DoEvents();
				//Task.Delay(100);
			  
			}
		}
		
		public void DownloadHistory(string ticker, int totalItems)
		{
			string urlAddress = "http://table.finance.yahoo.com/table.csv?s={0}&d={4}&e={5}&f={6}&g=d&a={1}&b={2}&c={3}&ignore=.csv";
			var wc = new WebClient();
			FileDataManager fileDataManager = new FileDataManager(null);
					
			try
			{
				DateTime time1 = this.dpStart.Value;
				DateTime time2 = this.dpStop.Value;
				CommonDataProvider tickerFileDataProvider = (CommonDataProvider)fileDataManager[ticker];
				if ((downloadSelectMode == 0) && (tickerFileDataProvider.Count > 0))
				{
					double[] numArray1 = tickerFileDataProvider["DATE"];
					DateTime time3 = DateTime.FromOADate(numArray1[numArray1.Length - 1]);
					time1 = time3.AddDays(1);
				}
				if (time1 <= DateTime.Now.Date)
				{
					object[] objArray1 = new object[7] { ticker, time1.Month - 1, time1.Day, time1.Year, time2.Month - 1, time2.Day, time2.Year };
					string text3 = string.Format(urlAddress, objArray1);
					byte[] buffer1 = wc.DownloadData(text3);
					CommonDataProvider yahooDataProvider = YahooDataManager.LoadYahooCSV((IDataManager)null, buffer1);
					if (tickerFileDataProvider != null)
					{
						yahooDataProvider.Merge(tickerFileDataProvider);
					}
					yahooDataProvider.SaveBinary(FileDataManager.GetFileName(ticker));
					this.UpdateRealtimeFromHistorical(ticker, yahooDataProvider);
				}
			}
			catch
			{
							  
			}
												
			UpdateProgressbar(historyDownloadCount, totalItems, historyDownloadCount + "/" + totalItems);
			
		}

		private void DownloadHistoryInParallel(object list)
		{
			string[] symbolList = list as string[];
			try
			{
				Parallel.ForEach(symbolList, ParallelParameter, ticker =>
				{
					ParallelParameter.CancellationToken.ThrowIfCancellationRequested();
					DownloadHistory(ticker, symbolList.Length);
				});
			}
			catch (OperationCanceledException ex)
			{
				lblResult.Invoke(new Action(()=>lblResult.Text = ex.Message));
			}
		}

		private void StartDownloadHistory()
		{
			string[] symbolList = ListForm.Current.GetSymbolList();
			int totalDownloads = symbolList.Length;
			this.pbDownload.Maximum = totalDownloads;
			this.pbDownload.Value = 0;
			downloadSelectMode = this.cbMode.SelectedIndex;
			try
			{
				this.btnBeginDownload.Text = "&Stop Download";
				this.DownloadHistoryThread = new Thread(new ParameterizedThreadStart(DownloadHistoryInParallel));
				this.DownloadHistoryThread.IsBackground = true;
				this.DownloadHistoryThread.Start(symbolList);
			}
			catch (AggregateException ex)
			{
				string msg = ReportAggregateError(ex);
				MessageBox.Show(msg);
			}
			finally
			{
			}
		}

		private static string ReportAggregateError(AggregateException aggregate)
		{
			var sb = new StringBuilder(200);
			foreach (var exception in aggregate.InnerExceptions)
				if (exception is AggregateException)
					ReportAggregateError(exception as AggregateException);
				else
					sb.AppendLine(exception.Message);
			return sb.ToString();
		}

		private void StartDownloadRealtime()
		{
			this.DownloadRealtimeThread = new Thread(new ThreadStart(DownloadRealtime));
			this.DownloadRealtimeThread.Start();
			this.btnDownloadRealtime.Text = "&Stop Download";
		}		

		public void DownloadRealtime()
		{
			if (pbRealtime.InvokeRequired)
			{
				this.Invoke((Action) delegate()
				{
					DownloadRealtime();
				});
			}
			else
			{
					string[] symbolist = ListForm.Current.GetSymbolList();
					int tickerCount = 0;
					this.pbRealtime.Maximum = symbolist.Length;
					this.pbRealtime.Value = 0;
					this.lProgress.Text = "0/" + symbolist.Length;
					//Thread.Sleep(1);
					try
					{
						
						string[] textArray2 = symbolist;
						Parallel.ForEach(textArray2, ticker =>
						{
							//for (int num2 = 0; num2 < textArray2.Length; num2++)
							//{
								   
								try
								{
									DataPackage package1 = DataPackage.DownloadFromYahoo(ticker);
									if (!package1.IsZeroValue)
									{
										StockDB.LoadRealtimeRow(ticker, package1, false);
									}
								}
								catch
								{
									// Thread.Sleep(1);
								}
								tickerCount++;

								this.pbRealtime.Value = tickerCount;

								this.lProgress.Text = tickerCount + "/" + symbolist.Length;
								Application.DoEvents();
							//}
						});
					}
					finally
					{
						this.btnDownloadRealtime.Text = "&Start Download";
						this.DownloadRealtimeThread = null;
						this.EnableDisableActions(true);
					}
				}
		}

		private void EnableDisableActions(bool Enable)
		{
			this.EnableDisableActions(Enable, true);
		}

		private void EnableDisableActions(bool Enable, bool ShowStop)
		{
			this.tcManager.Enabled = Enable;
			this.btnStop.Visible = !Enable && ShowStop;
			this.btnOK.Enabled = Enable;
		}

		private void ImportSymbol()
		{
			try
			{
				string text1 = this.DownloadData("http://data.easychart.net/Symbol.aspx?Exchange=" + this.cbExchange.SelectedItem.ToString());
				char[] chArray1 = new char[1] { '\r' } ;
				string[] textArray1 = text1.Trim().Split(chArray1);
				string[] textArray3 = textArray1;
				for (int num1 = 0; num1 < textArray3.Length; num1++)
				{
					string text2 = textArray3[num1];
					char[] chArray2 = new char[1] { ';' } ;
					string[] textArray2 = text2.Trim().Split(chArray2);
					try
					{
						StockDB.LoadSymbolRow(textArray2);
					}
					catch
					{
					}
				}
				this.SymbolChanged();
				MessageBox.Show(this, "Import success , total :" + textArray1.Length);
			}
			finally
			{
				this.ImportSymbolThread = null;
				this.EnableDisableActions(true);
			}
		}

		private void InitializeComponent()
		{
			this.tcManager = new System.Windows.Forms.TabControl();
			this.tsDataService = new System.Windows.Forms.TabPage();
			this.lData = new System.Windows.Forms.Label();
			this.pbData = new System.Windows.Forms.ProgressBar();
			this.lDesc = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.btnUpdateIndustry = new System.Windows.Forms.Button();
			this.btnUpdateEOD = new System.Windows.Forms.Button();
			this.dpDate = new System.Windows.Forms.DateTimePicker();
			this.btnImportSymbol = new System.Windows.Forms.Button();
			this.lExchange = new System.Windows.Forms.Label();
			this.cbExchange = new System.Windows.Forms.ComboBox();
			this.tpNameList = new System.Windows.Forms.TabPage();
			this.pbImport = new System.Windows.Forms.ProgressBar();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.tb = new System.Windows.Forms.TextBox();
			this.btnImport = new System.Windows.Forms.Button();
			this.tbAddStock = new System.Windows.Forms.TextBox();
			this.tsRealtime = new System.Windows.Forms.TabPage();
			this.lProgress = new System.Windows.Forms.Label();
			this.pbRealtime = new System.Windows.Forms.ProgressBar();
			this.btnDownloadRealtime = new System.Windows.Forms.Button();
			this.tpDownloadFromYahoo = new System.Windows.Forms.TabPage();
			this.lblResult = new System.Windows.Forms.Label();
			this.lMode = new System.Windows.Forms.Label();
			this.cbMode = new System.Windows.Forms.ComboBox();
			this.lStopDate = new System.Windows.Forms.Label();
			this.lStartDate = new System.Windows.Forms.Label();
			this.dpStart = new System.Windows.Forms.DateTimePicker();
			this.dpStop = new System.Windows.Forms.DateTimePicker();
			this.lDownload = new System.Windows.Forms.Label();
			this.pbDownload = new System.Windows.Forms.ProgressBar();
			this.btnBeginDownload = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStopDownloadHistory = new System.Windows.Forms.Button();
			this.tcManager.SuspendLayout();
			this.tsDataService.SuspendLayout();
			this.tpNameList.SuspendLayout();
			this.tsRealtime.SuspendLayout();
			this.tpDownloadFromYahoo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcManager
			// 
			this.tcManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tcManager.Controls.Add(this.tsDataService);
			this.tcManager.Controls.Add(this.tpNameList);
			this.tcManager.Controls.Add(this.tsRealtime);
			this.tcManager.Controls.Add(this.tpDownloadFromYahoo);
			this.tcManager.ItemSize = new System.Drawing.Size(97, 18);
			this.tcManager.Location = new System.Drawing.Point(8, 16);
			this.tcManager.Name = "tcManager";
			this.tcManager.SelectedIndex = 0;
			this.tcManager.Size = new System.Drawing.Size(688, 472);
			this.tcManager.TabIndex = 0;
			// 
			// tsDataService
			// 
			this.tsDataService.Controls.Add(this.lData);
			this.tsDataService.Controls.Add(this.pbData);
			this.tsDataService.Controls.Add(this.lDesc);
			this.tsDataService.Controls.Add(this.linkLabel1);
			this.tsDataService.Controls.Add(this.btnUpdateIndustry);
			this.tsDataService.Controls.Add(this.btnUpdateEOD);
			this.tsDataService.Controls.Add(this.dpDate);
			this.tsDataService.Controls.Add(this.btnImportSymbol);
			this.tsDataService.Controls.Add(this.lExchange);
			this.tsDataService.Controls.Add(this.cbExchange);
			this.tsDataService.Location = new System.Drawing.Point(4, 22);
			this.tsDataService.Name = "tsDataService";
			this.tsDataService.Size = new System.Drawing.Size(680, 446);
			this.tsDataService.TabIndex = 3;
			this.tsDataService.Text = "Internet Data Service";
			// 
			// lData
			// 
			this.lData.AutoSize = true;
			this.lData.Location = new System.Drawing.Point(24, 248);
			this.lData.Name = "lData";
			this.lData.Size = new System.Drawing.Size(26, 13);
			this.lData.TabIndex = 9;
			this.lData.Text = "0/0";
			// 
			// pbData
			// 
			this.pbData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pbData.Location = new System.Drawing.Point(24, 224);
			this.pbData.Name = "pbData";
			this.pbData.Size = new System.Drawing.Size(632, 16);
			this.pbData.TabIndex = 8;
			// 
			// lDesc
			// 
			this.lDesc.AutoSize = true;
			this.lDesc.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
			this.lDesc.Location = new System.Drawing.Point(24, 16);
			this.lDesc.Name = "lDesc";
			this.lDesc.Size = new System.Drawing.Size(549, 17);
			this.lDesc.TabIndex = 7;
			this.lDesc.Text = "All data and symbols in this page are from easychart.net data service!";
			// 
			// linkLabel1
			// 
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(0, 25);
			this.linkLabel1.Location = new System.Drawing.Point(24, 48);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(208, 16);
			this.linkLabel1.TabIndex = 6;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://data.easychart.net";
			// 
			// btnUpdateIndustry
			// 
			this.btnUpdateIndustry.Location = new System.Drawing.Point(240, 184);
			this.btnUpdateIndustry.Name = "btnUpdateIndustry";
			this.btnUpdateIndustry.Size = new System.Drawing.Size(192, 23);
			this.btnUpdateIndustry.TabIndex = 5;
			this.btnUpdateIndustry.Text = "Update &Industry";
			this.btnUpdateIndustry.Click += new System.EventHandler(this.btnUpdateIndustry_Click);
			// 
			// btnUpdateEOD
			// 
			this.btnUpdateEOD.Location = new System.Drawing.Point(240, 136);
			this.btnUpdateEOD.Name = "btnUpdateEOD";
			this.btnUpdateEOD.Size = new System.Drawing.Size(192, 23);
			this.btnUpdateEOD.TabIndex = 4;
			this.btnUpdateEOD.Text = "&Update end of day data";
			this.btnUpdateEOD.Click += new System.EventHandler(this.btnUpdateEOD_Click);
			// 
			// dpDate
			// 
			this.dpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dpDate.Location = new System.Drawing.Point(24, 136);
			this.dpDate.Name = "dpDate";
			this.dpDate.Size = new System.Drawing.Size(200, 21);
			this.dpDate.TabIndex = 3;
			// 
			// btnImportSymbol
			// 
			this.btnImportSymbol.Location = new System.Drawing.Point(240, 88);
			this.btnImportSymbol.Name = "btnImportSymbol";
			this.btnImportSymbol.Size = new System.Drawing.Size(192, 23);
			this.btnImportSymbol.TabIndex = 2;
			this.btnImportSymbol.Text = "&Import Symbol";
			this.btnImportSymbol.Click += new System.EventHandler(this.btnImportSymbol_Click);
			// 
			// lExchange
			// 
			this.lExchange.AutoSize = true;
			this.lExchange.Location = new System.Drawing.Point(24, 96);
			this.lExchange.Name = "lExchange";
			this.lExchange.Size = new System.Drawing.Size(67, 13);
			this.lExchange.TabIndex = 1;
			this.lExchange.Text = "Exchange:";
			// 
			// cbExchange
			// 
			this.cbExchange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbExchange.ItemHeight = 13;
			this.cbExchange.Items.AddRange(new object[] {
			"Amex",
			"Nyse",
			"Nasdaq",
			"^",
			"Shanghai",
			"Shenzhen"});
			this.cbExchange.Location = new System.Drawing.Point(99, 88);
			this.cbExchange.Name = "cbExchange";
			this.cbExchange.Size = new System.Drawing.Size(121, 21);
			this.cbExchange.TabIndex = 0;
			// 
			// tpNameList
			// 
			this.tpNameList.Controls.Add(this.pbImport);
			this.tpNameList.Controls.Add(this.btnDelete);
			this.tpNameList.Controls.Add(this.btnExport);
			this.tpNameList.Controls.Add(this.btnClear);
			this.tpNameList.Controls.Add(this.tb);
			this.tpNameList.Controls.Add(this.btnImport);
			this.tpNameList.Controls.Add(this.tbAddStock);
			this.tpNameList.Location = new System.Drawing.Point(4, 22);
			this.tpNameList.Name = "tpNameList";
			this.tpNameList.Size = new System.Drawing.Size(680, 446);
			this.tpNameList.TabIndex = 0;
			this.tpNameList.Text = "Stock Symbols";
			// 
			// pbImport
			// 
			this.pbImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pbImport.Location = new System.Drawing.Point(152, 412);
			this.pbImport.Name = "pbImport";
			this.pbImport.Size = new System.Drawing.Size(264, 16);
			this.pbImport.TabIndex = 6;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Location = new System.Drawing.Point(424, 408);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 5;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Location = new System.Drawing.Point(592, 408);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 4;
			this.btnExport.Text = "&Export";
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClear.Location = new System.Drawing.Point(8, 408);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(136, 23);
			this.btnClear.TabIndex = 3;
			this.btnClear.Text = "Clear All Symbols";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// tb
			// 
			this.tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tb.Location = new System.Drawing.Point(8, 8);
			this.tb.Multiline = true;
			this.tb.Name = "tb";
			this.tb.ReadOnly = true;
			this.tb.Size = new System.Drawing.Size(664, 80);
			this.tb.TabIndex = 2;
			this.tb.Text = "Format : QuoteCode;QuoteName;Exchange:\r\nSamples:\r\nABCW;Anchor BanCorp Wisconsin I" +
	"nc.;NASDAQ\r\nABM;Abm Industries Inc;NYSE\r\n... ...";
			// 
			// btnImport
			// 
			this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnImport.Location = new System.Drawing.Point(508, 408);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 1;
			this.btnImport.Text = "&Import";
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// tbAddStock
			// 
			this.tbAddStock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tbAddStock.Location = new System.Drawing.Point(8, 96);
			this.tbAddStock.MaxLength = 327670;
			this.tbAddStock.Multiline = true;
			this.tbAddStock.Name = "tbAddStock";
			this.tbAddStock.Size = new System.Drawing.Size(664, 304);
			this.tbAddStock.TabIndex = 0;
			// 
			// tsRealtime
			// 
			this.tsRealtime.Controls.Add(this.lProgress);
			this.tsRealtime.Controls.Add(this.pbRealtime);
			this.tsRealtime.Controls.Add(this.btnDownloadRealtime);
			this.tsRealtime.Location = new System.Drawing.Point(4, 22);
			this.tsRealtime.Name = "tsRealtime";
			this.tsRealtime.Size = new System.Drawing.Size(680, 446);
			this.tsRealtime.TabIndex = 2;
			this.tsRealtime.Text = "Realtime Data from yahoo";
			// 
			// lProgress
			// 
			this.lProgress.AutoSize = true;
			this.lProgress.Location = new System.Drawing.Point(24, 88);
			this.lProgress.Name = "lProgress";
			this.lProgress.Size = new System.Drawing.Size(26, 13);
			this.lProgress.TabIndex = 5;
			this.lProgress.Text = "0/0";
			// 
			// pbRealtime
			// 
			this.pbRealtime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pbRealtime.Location = new System.Drawing.Point(24, 64);
			this.pbRealtime.Name = "pbRealtime";
			this.pbRealtime.Size = new System.Drawing.Size(632, 16);
			this.pbRealtime.TabIndex = 4;
			// 
			// btnDownloadRealtime
			// 
			this.btnDownloadRealtime.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDownloadRealtime.Location = new System.Drawing.Point(24, 32);
			this.btnDownloadRealtime.Name = "btnDownloadRealtime";
			this.btnDownloadRealtime.Size = new System.Drawing.Size(168, 23);
			this.btnDownloadRealtime.TabIndex = 3;
			this.btnDownloadRealtime.Text = "&Start Download";
			this.btnDownloadRealtime.Click += new System.EventHandler(this.btnDownloadRealtime_Click);
			// 
			// tpDownloadFromYahoo
			// 
			this.tpDownloadFromYahoo.Controls.Add(this.lblResult);
			this.tpDownloadFromYahoo.Controls.Add(this.lMode);
			this.tpDownloadFromYahoo.Controls.Add(this.cbMode);
			this.tpDownloadFromYahoo.Controls.Add(this.lStopDate);
			this.tpDownloadFromYahoo.Controls.Add(this.lStartDate);
			this.tpDownloadFromYahoo.Controls.Add(this.dpStart);
			this.tpDownloadFromYahoo.Controls.Add(this.dpStop);
			this.tpDownloadFromYahoo.Controls.Add(this.lDownload);
			this.tpDownloadFromYahoo.Controls.Add(this.pbDownload);
			this.tpDownloadFromYahoo.Controls.Add(this.btnStopDownloadHistory);
			this.tpDownloadFromYahoo.Controls.Add(this.btnBeginDownload);
			this.tpDownloadFromYahoo.Location = new System.Drawing.Point(4, 22);
			this.tpDownloadFromYahoo.Name = "tpDownloadFromYahoo";
			this.tpDownloadFromYahoo.Size = new System.Drawing.Size(680, 446);
			this.tpDownloadFromYahoo.TabIndex = 1;
			this.tpDownloadFromYahoo.Text = "Historical data from yahoo";
			// 
			// lblResult
			// 
			this.lblResult.AutoSize = true;
			this.lblResult.Location = new System.Drawing.Point(21, 225);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(15, 13);
			this.lblResult.TabIndex = 9;
			this.lblResult.Text = "  ";
			// 
			// lMode
			// 
			this.lMode.AutoSize = true;
			this.lMode.Location = new System.Drawing.Point(52, 50);
			this.lMode.Name = "lMode";
			this.lMode.Size = new System.Drawing.Size(42, 13);
			this.lMode.TabIndex = 8;
			this.lMode.Text = "Mode:";
			// 
			// cbMode
			// 
			this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMode.ItemHeight = 13;
			this.cbMode.Items.AddRange(new object[] {
			"Append",
			"Override"});
			this.cbMode.Location = new System.Drawing.Point(100, 48);
			this.cbMode.Name = "cbMode";
			this.cbMode.Size = new System.Drawing.Size(121, 21);
			this.cbMode.TabIndex = 7;
			// 
			// lStopDate
			// 
			this.lStopDate.AutoSize = true;
			this.lStopDate.Location = new System.Drawing.Point(264, 19);
			this.lStopDate.Name = "lStopDate";
			this.lStopDate.Size = new System.Drawing.Size(69, 13);
			this.lStopDate.TabIndex = 6;
			this.lStopDate.Text = "Stop Date:";
			// 
			// lStartDate
			// 
			this.lStartDate.AutoSize = true;
			this.lStartDate.Location = new System.Drawing.Point(24, 20);
			this.lStartDate.Name = "lStartDate";
			this.lStartDate.Size = new System.Drawing.Size(71, 13);
			this.lStartDate.TabIndex = 5;
			this.lStartDate.Text = "Start Date:";
			// 
			// dpStart
			// 
			this.dpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dpStart.Location = new System.Drawing.Point(102, 16);
			this.dpStart.Name = "dpStart";
			this.dpStart.Size = new System.Drawing.Size(136, 21);
			this.dpStart.TabIndex = 4;
			this.dpStart.Value = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
			// 
			// dpStop
			// 
			this.dpStop.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dpStop.Location = new System.Drawing.Point(344, 16);
			this.dpStop.Name = "dpStop";
			this.dpStop.Size = new System.Drawing.Size(152, 21);
			this.dpStop.TabIndex = 3;
			// 
			// lDownload
			// 
			this.lDownload.AutoSize = true;
			this.lDownload.Location = new System.Drawing.Point(24, 136);
			this.lDownload.Name = "lDownload";
			this.lDownload.Size = new System.Drawing.Size(26, 13);
			this.lDownload.TabIndex = 2;
			this.lDownload.Text = "0/0";
			// 
			// pbDownload
			// 
			this.pbDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pbDownload.Location = new System.Drawing.Point(24, 112);
			this.pbDownload.Name = "pbDownload";
			this.pbDownload.Size = new System.Drawing.Size(632, 16);
			this.pbDownload.TabIndex = 1;
			// 
			// btnBeginDownload
			// 
			this.btnBeginDownload.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnBeginDownload.Location = new System.Drawing.Point(24, 80);
			this.btnBeginDownload.Name = "btnBeginDownload";
			this.btnBeginDownload.Size = new System.Drawing.Size(168, 23);
			this.btnBeginDownload.TabIndex = 0;
			this.btnBeginDownload.Text = "&Start Download";
			this.btnBeginDownload.Click += new System.EventHandler(this.btnBeginDownload_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(600, 496);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&Close";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStop.Location = new System.Drawing.Point(8, 496);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "&Stop";
			this.btnStop.Visible = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStopDownloadHistory
			// 
			this.btnStopDownloadHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnStopDownloadHistory.Location = new System.Drawing.Point(213, 80);
			this.btnStopDownloadHistory.Name = "btnStopDownloadHistory";
			this.btnStopDownloadHistory.Size = new System.Drawing.Size(168, 23);
			this.btnStopDownloadHistory.TabIndex = 0;
			this.btnStopDownloadHistory.Text = "&Stop Download";
			this.btnStopDownloadHistory.Click += new System.EventHandler(this.btnStopDownloadHistory_Click);
			// 
			// DataManagerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(704, 525);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tcManager);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(600, 500);
			this.Name = "DataManagerForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DataManager";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DataManagerForm_Closing);
			this.Load += new System.EventHandler(this.DataManagerForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataManagerForm_KeyDown);
			this.tcManager.ResumeLayout(false);
			this.tsDataService.ResumeLayout(false);
			this.tsDataService.PerformLayout();
			this.tpNameList.ResumeLayout(false);
			this.tpNameList.PerformLayout();
			this.tsRealtime.ResumeLayout(false);
			this.tsRealtime.PerformLayout();
			this.tpDownloadFromYahoo.ResumeLayout(false);
			this.tpDownloadFromYahoo.PerformLayout();
			this.ResumeLayout(false);

		}

		public static void ShowIt()
		{
			if (DataManagerForm.Current == null)
			{
				DataManagerForm.Current = new DataManagerForm();
			}
			DataManagerForm.Current.ShowDialog();
		}

		private void StartImportSymbol()
		{
			if (this.ImportSymbolThread == null)
			{
				this.ImportSymbolThread = new Thread(new ThreadStart(this.ImportSymbol));
				this.ImportSymbolThread.Start();
				this.EnableDisableActions(false);
			}
		}

		private void StartUpdateEODData()
		{
			if (this.UpdateEodDataThread == null)
			{
				this.UpdateEodDataThread = new Thread(new ThreadStart(this.UpdateEODData));
				this.UpdateEodDataThread.Start();
				this.EnableDisableActions(false);
			}
		}

		private void StartUpdateIndustry()
		{
			if (this.UpdateIndustryThread == null)
			{
				this.UpdateIndustryThread = new Thread(new ThreadStart(this.UpdateIndustry));
				this.UpdateIndustryThread.Start();
				this.EnableDisableActions(false);
			}
		}

		private void StopDownloadHistory()
		{
			if (this.DownloadHistoryThread != null)
			{
				this.DownloadHistoryThread.Abort();
				this.DownloadHistoryThread.Join();
			}
		}

		private void StopDownloadRealtime()
		{
			if (this.DownloadRealtimeThread != null)
			{
				this.DownloadRealtimeThread.Abort();
				this.DownloadRealtimeThread.Join();
			}
		}

		private void StopImportSymbol()
		{
			if (this.ImportSymbolThread != null)
			{
				this.ImportSymbolThread.Abort();
				this.ImportSymbolThread.Join();
			}
		}

		private void StopUpdateEODData()
		{
			if (this.UpdateEodDataThread != null)
			{
				this.UpdateEodDataThread.Abort();
				this.UpdateEodDataThread.Join();
			}
		}

		private void StopUpdateIndustry()
		{
			if (this.UpdateIndustryThread != null)
			{
				this.UpdateIndustryThread.Abort();
				this.UpdateIndustryThread.Join();
			}
		}

		private void SymbolChanged()
		{
			ListForm.Current.SymbolChanged = true;
		}

		private void UpdateEODData()
		{
			try
			{
				string text1 = this.DownloadData("http://data.easychart.net/EOD.aspx?Exchange=" + this.cbExchange.SelectedItem.ToString() + "&Date=" + this.dpDate.Value.ToString("yyyy-MM-dd"));
				char[] chArray1 = new char[1] { '\r' } ;
				string[] textArray1 = text1.Trim().Split(chArray1);
				FileDataManager manager1 = new FileDataManager(null);
				int num1 = 0;
				this.pbData.Maximum = textArray1.Length;
				this.pbData.Value = 0;
				this.lData.Text = "0/" + textArray1.Length;
				StockDB.BeginInit("SymbolList");
				StockDB.BeginLoadData("SymbolList");
				try
				{//http://data.easychart.net/EOD.aspx?Exchange=Nasdaq&Date=2006-08-30
					string[] textArray3 = textArray1;
					for (int num2 = 0; num2 < textArray3.Length; num2++)
					{
						string text2 = textArray3[num2];
						char[] chArray2 = new char[1] { ',' } ;
						string[] textArray2 = text2.Trim().Split(chArray2);
						if (textArray2.Length == 7)
						{
							try
							{
								DataPackage package1 = DataPackage.ParseEODData(text2);
								if (!package1.IsZeroValue)
								{
									CommonDataProvider provider1 = (CommonDataProvider) StockDB.MergeOneRealtime(manager1, textArray2[0], package1);
									this.UpdateRealtimeFromHistorical(textArray2[0], provider1);
								}
							}
							catch
							{
								Thread.Sleep(1);
							}
							num1++;
							if ((num1 % 10) == 0)
							{
								this.pbData.Value = num1;
								this.lData.Text = num1 + "/" + textArray1.Length;
								Application.DoEvents();
							}
						}
					}
				}
				finally
				{
					StockDB.EndLoadData("SymbolList");
					StockDB.EndInit("SymbolList");
				}
				MessageBox.Show(this, "Update success , total :" + textArray1.Length);
			}
			finally
			{
				this.UpdateEodDataThread = null;
				this.EnableDisableActions(true);
			}
		}

		private void UpdateIndustry()
		{
			try
			{
				try
				{
					string text1 = this.DownloadData("http://data.easychart.net/Industry.aspx");
					char[] chArray1 = new char[1] { '\r' } ;
					string[] textArray1 = text1.Split(chArray1);
					StockDB.DeleteFolderRows(4, true);
					int num1 = 0;
					Hashtable hashtable1 = new Hashtable();
					StockDB.BeginLoadData("Folder");
					try
					{
						for (int num2 = 0; num2 < textArray1.Length; num2++)
						{// textArray each new line of data
							if (textArray1[num2].StartsWith("----"))
							{ // start with symbol for each industry
								num1 = num2;
								break;
							}
							char[] chArray2 = new char[1] { ',' } ;
								 //each line split by ','
							string[] textArray2 = textArray1[num2].Split(chArray2);
							if ((textArray2.Length == 3) && (textArray2[1] == "0"))
							{
								DataRow row1 = StockDB.LoadFolderRow(4, textArray2[2]);
								hashtable1[textArray2[0]] = row1["FolderId"];
							}
						}
						for (int num3 = 0; num3 < num1; num3++)
						{
							char[] chArray3 = new char[1] { ',' } ;
							string[] textArray3 = textArray1[num3].Split(chArray3);
							if ((textArray3.Length == 3) && (textArray3[1] != "0"))
							{
								DataRow row2 = StockDB.LoadFolderRow((int) hashtable1[textArray3[1]], textArray3[2]);
								hashtable1[textArray3[0]] = row2["FolderId"];
							}
						}
					}
					finally
					{
						StockDB.EndLoadData("Folder");
					}
					StockDB.BeginLoadData("FolderRel");
					try
					{
						for (int num4 = num1 + 1; num4 < textArray1.Length; num4++)
						{
							char[] chArray4 = new char[1] { ',' } ;
							string[] textArray4 = textArray1[num4].Split(chArray4);
							if (textArray4.Length == 2)
							{
								StockDB.LoadFolderRelRow(textArray4[1], (int) hashtable1[textArray4[0]]);
							}
						}
					}
					finally
					{
						StockDB.EndLoadData("FolderRel");
					}
					StockDB.ResetFolderDatabase();
					object[] objArray1 = new object[4] { "Industry database import success , total :", num1, ",stocks:", textArray1.Length - num1 } ;
					MessageBox.Show(this, string.Concat(objArray1));
				}
				finally
				{
					this.EnableDisableActions(true);
				}
			}
			finally
			{
				this.UpdateIndustryThread = null;
			}
		}

		public void UpdateRealtimeFromHistorical(string Code, CommonDataProvider cdp)
		{
			DataPackage[] packageArray1 = cdp.GetLastDataPackages(2);//cdp.GetLastDataPackages_Win(2);
			float single1 = 0f;
			if (packageArray1[0] != null)
			{
				single1 = (float)packageArray1[0].Close;
			}
			if (packageArray1[1] != null)
			{
				packageArray1[1].Last = single1;
				StockDB.LoadRealtimeRow(Code, packageArray1[1], true);
			}
		}


		// Fields
		private Button btnBeginDownload;
		private Button btnClear;
		private Button btnDelete;
		private Button btnDownloadRealtime;
		private Button btnExport;
		private Button btnImport;
		private Button btnImportSymbol;
		private Button btnOK;
		private Button btnStop;
		private Button btnUpdateEOD;
		private Button btnUpdateIndustry;
		private ComboBox cbExchange;
		private ComboBox cbMode;
		private IContainer components;
		private static DataManagerForm Current;
		private Thread DownloadHistoryThread;
		private Thread DownloadRealtimeThread;
		private DateTimePicker dpDate;
		private DateTimePicker dpStart;
		private DateTimePicker dpStop;
		private Thread ImportSymbolThread;
		private Label lData;
		private Label lDesc;
		private Label lDownload;
		private Label lExchange;
		private LinkLabel linkLabel1;
		private Label lMode;
		private Label lProgress;
		private Label lStartDate;
		private Label lStopDate;
		private ProgressBar pbData;
		private ProgressBar pbDownload;
		private ProgressBar pbImport;
		private ProgressBar pbRealtime;
		private TextBox tb;
		private TextBox tbAddStock;
		private TabControl tcManager;
		private TabPage tpDownloadFromYahoo;
		private TabPage tpNameList;
		private TabPage tsDataService;
		private TabPage tsRealtime;
		private Thread UpdateEodDataThread;
		private Thread UpdateIndustryThread;

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.Close ();
		}


	}


}
