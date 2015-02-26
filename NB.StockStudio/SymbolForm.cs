using NB.StockStudio.Foundation;
using NB.StockStudio.WinControls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using WindowsDemo.ColumnStyle;

namespace NB.StockStudio
{
	public class SymbolForm : Form
	{
		// Methods
		static SymbolForm()
		{
			SymbolForm.Current = new SymbolForm();
		}

		public SymbolForm()
		{
			this.htSymbol = new Hashtable();
			this.PY = Encoding.GetEncoding("GB2312").GetBytes("\u554a\u82ad\u64e6\u642d\u86fe\u53d1\u5676\u54c8\u51fb\u51fb\u5580\u5783\u5988\u62ff\u54e6\u556a\u671f\u7136\u6492\u584c\u6316\u6316\u6316\u6614\u538b\u531dAA");
			this.InitializeComponent();
			this.dtSymbol = new DataTable();
			this.dtSymbol.Columns.Add("ShortcutType");
			this.dtSymbol.Columns.Add("Shortcut");
			this.dtSymbol.Columns.Add("Code");
			this.dtSymbol.Columns.Add("Name");
			this.dtSymbol.Columns.Add("Key");
			this.AddStyleColumns(this.dgSymbol.TableStyles[0]);
			this.AddShortcut(FormulaBase.GetAllIndicators(false), ShortcutType.Indicator, false);
		}

		public static void AddFolder()
		{
			SymbolForm.Current.AddFolderShortcut();
		}

		public void AddFolderShortcut()
		{
			DataTable table1 = StockDB.GetFolderDatatable();
			this.RemoveShortcut(ShortcutType.Folder);
			for (int num1 = 0; num1 < table1.Rows.Count; num1++)
			{
				string text1 = table1.Rows[num1]["FolderId"].ToString();
				string text2 = table1.Rows[num1]["FolderName"].ToString();
				object[] objArray1 = new object[4] { 2, "F" + text2, text1, text2 } ;
				this.dtSymbol.Rows.Add(objArray1);
			}
		}

		private bool AddKeys(char c)
		{
			string text1 = this.tbSelect.Text;
			char ch1 = c;
			if (ch1 == '\b')
			{
				string text2 = this.tbSelect.Text;
				if (text2.Length > 0)
				{
					this.tbSelect.Text = text2.Substring(0, text2.Length - 1);
				}
				else
				{
					base.Close();
				}
			}
			else if (char.IsSymbol(c) || char.IsLetterOrDigit(c))
			{
				this.tbSelect.Text = this.tbSelect.Text + c;
			}
			if (text1 != this.tbSelect.Text)
			{
				this.dvSymbol = new DataView(this.dtSymbol, "Shortcut like '" + this.tbSelect.Text + "*'", "", DataViewRowState.CurrentRows);
				this.dgSymbol.DataSource = this.dvSymbol;
				return true;
			}
			return false;
		}


		public void AddShortcut(DataRow dr, ShortcutType ShortType)
		{
			string text1 = dr["Code"].ToString();
			object[] objArray1 = new object[5] { (int) ShortType, text1, text1, dr["Name"], text1 } ;
			this.dtSymbol.Rows.Add(objArray1);
			object[] objArray2 = new object[5] { (int) ShortType, this.GetChineseShortcut(dr["Name"].ToString()), text1, dr["Name"], text1 } ;
			this.dtSymbol.Rows.Add(objArray2);
			if (ShortType == ShortcutType.Stock)
			{
				this.htSymbol[text1] = dr;
			}
		}

		public void AddShortcut(DataTable dt, ShortcutType ShortType)
		{
			this.RemoveShortcut(ShortType);
			this.htSymbol.Clear();
			foreach (DataRow row1 in dt.Rows)
			{
				this.AddShortcut(row1, ShortType);
			}
		}

		public void AddShortcut(string[] ss, ShortcutType ShortType, bool RemoveOld)
		{
			if (RemoveOld)
			{
				this.RemoveShortcut(ShortType);
			}
			string[] textArray1 = ss;
			for (int num2 = 0; num2 < textArray1.Length; num2++)
			{
				string text1 = textArray1[num2];
				int num1 = text1.IndexOf('(');
				string text2 = text1;
				if (num1 >= 0)
				{
					text2 = text2.Substring(0, num1);
				}
				num1 = text2.LastIndexOf('.');
				if (num1 >= 0)
				{
					text2 = text2.Substring(num1 + 1);
				}
				object[] objArray1 = new object[5] { (int) ShortType, text2, text1, text1, text1 } ;
				this.dtSymbol.Rows.Add(objArray1);
			}
		}

		private void AddStyleColumns(DataGridTableStyle dgTableStyle)
		{
			dgTableStyle.GridColumnStyles.Add(new DataGridImageColumn("", "", "ShortcutType", 20, this.ilIcon));
			dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Shortcut", "Shortcut", 60));
			dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Code", "Code", 0));
			dgTableStyle.GridColumnStyles.Add(new DataGridReadOnlyColumn("", "Name", "Name", 130));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public string GetChineseShortcut(string s)
		{
			string text1 = "";
			byte[] buffer1 = Encoding.GetEncoding("GB2312").GetBytes(s.ToUpper());
			this.PY[this.PY.Length - 2] = 0xff;
			for (int num1 = 0; num1 < buffer1.Length; num1++)
			{
				if ((((buffer1[num1] >= 0x41) && (buffer1[num1] < 0x5b)) || ((buffer1[num1] >= 0x30) && (buffer1[num1] < 0x3a))) || ((buffer1[num1] >= 0x61) && (buffer1[num1] < 0x7b)))
				{
					text1 = text1 + ((char) buffer1[num1]);
				}
				else if ((buffer1[num1] > 160) && ((num1 + 1) < buffer1.Length))
				{
					int num2 = (0x100 * buffer1[num1]) + buffer1[num1 + 1];
					for (int num3 = 0; num3 < this.PY.Length; num3 += 2)
					{
						int num4 = (0x100 * this.PY[num3]) + this.PY[num3 + 1];
						if (num2 < num4)
						{
							text1 = text1 + ((char) ((ushort) (0x40 + (num3 / 2))));
							break;
						}
					}
					num1++;
				}
			}
			return text1;
		}

		public static DataRow GetSymbolRow(string Code)
		{
			if (Code == "")
			{
				return null;
			}
			return (DataRow) SymbolForm.Current.htSymbol[Code];
		}

		public static Hashtable GetSymbolTable()
		{
			return SymbolForm.Current.htSymbol;
		}

		public static void HideForm()
		{
			SymbolForm.Current.Hide();
		}

		private void InitializeComponent()
		{
				 this.components = new System.ComponentModel.Container();
				 System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SymbolForm));
				 this.tbSelect = new System.Windows.Forms.TextBox();
				 this.dgSymbol = new System.Windows.Forms.DataGrid();
				 this.dgStyle = new System.Windows.Forms.DataGridTableStyle();
				 this.tmHide = new System.Windows.Forms.Timer(this.components);
				 this.ilIcon = new System.Windows.Forms.ImageList(this.components);
				 ((System.ComponentModel.ISupportInitialize)(this.dgSymbol)).BeginInit();
				 this.SuspendLayout();
				 // 
				 // tbSelect
				 // 
				 this.tbSelect.Dock = System.Windows.Forms.DockStyle.Top;
				 this.tbSelect.Enabled = false;
				 this.tbSelect.Location = new System.Drawing.Point(0, 0);
				 this.tbSelect.Name = "tbSelect";
				 this.tbSelect.Size = new System.Drawing.Size(216, 21);
				 this.tbSelect.TabIndex = 0;
				 this.tbSelect.Text = "";
				 // 
				 // dgSymbol
				 // 
				 this.dgSymbol.AlternatingBackColor = System.Drawing.Color.Lavender;
				 this.dgSymbol.BackColor = System.Drawing.Color.WhiteSmoke;
				 this.dgSymbol.BackgroundColor = System.Drawing.Color.LightGray;
				 this.dgSymbol.BorderStyle = System.Windows.Forms.BorderStyle.None;
				 this.dgSymbol.CaptionBackColor = System.Drawing.Color.LightSteelBlue;
				 this.dgSymbol.CaptionForeColor = System.Drawing.Color.MidnightBlue;
				 this.dgSymbol.CaptionVisible = false;
				 this.dgSymbol.DataMember = "";
				 this.dgSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
				 this.dgSymbol.FlatMode = true;
				 this.dgSymbol.Font = new System.Drawing.Font("Tahoma", 8F);
				 this.dgSymbol.ForeColor = System.Drawing.Color.MidnightBlue;
				 this.dgSymbol.GridLineColor = System.Drawing.Color.Gainsboro;
				 this.dgSymbol.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
				 this.dgSymbol.HeaderBackColor = System.Drawing.Color.MidnightBlue;
				 this.dgSymbol.HeaderFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
				 this.dgSymbol.HeaderForeColor = System.Drawing.Color.WhiteSmoke;
				 this.dgSymbol.LinkColor = System.Drawing.Color.Teal;
				 this.dgSymbol.Location = new System.Drawing.Point(0, 21);
				 this.dgSymbol.Name = "dgSymbol";
				 this.dgSymbol.ParentRowsBackColor = System.Drawing.Color.Gainsboro;
				 this.dgSymbol.ParentRowsForeColor = System.Drawing.Color.MidnightBlue;
				 this.dgSymbol.ParentRowsVisible = false;
				 this.dgSymbol.ReadOnly = true;
				 this.dgSymbol.RowHeadersVisible = false;
				 this.dgSymbol.SelectionBackColor = System.Drawing.Color.CadetBlue;
				 this.dgSymbol.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
				 this.dgSymbol.Size = new System.Drawing.Size(216, 243);
				 this.dgSymbol.TabIndex = 1;
				 this.dgSymbol.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										   this.dgStyle});
				 // 
				 // dgStyle
				 // 
				 this.dgStyle.AlternatingBackColor = System.Drawing.Color.Lavender;
				 this.dgStyle.BackColor = System.Drawing.Color.WhiteSmoke;
				 this.dgStyle.DataGrid = this.dgSymbol;
				 this.dgStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
				 this.dgStyle.MappingName = "";
				 this.dgStyle.RowHeadersVisible = false;
				 // 
				 // tmHide
				 // 
				 this.tmHide.Interval = 10;
				 this.tmHide.Tick += new System.EventHandler(this.tmHide_Tick);
				 // 
				 // ilIcon
				 // 
				 this.ilIcon.ImageSize = new System.Drawing.Size(16, 16);
				 this.ilIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcon.ImageStream")));
				 this.ilIcon.TransparentColor = System.Drawing.Color.Transparent;
				 // 
				 // SymbolForm
				 // 
				 this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
				 this.ClientSize = new System.Drawing.Size(216, 264);
				 this.Controls.Add(this.dgSymbol);
				 this.Controls.Add(this.tbSelect);
				 this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				 this.KeyPreview = true;
				 this.MaximizeBox = false;
				 this.MaximumSize = new System.Drawing.Size(300, 500);
				 this.MinimizeBox = false;
				 this.Name = "SymbolForm";
				 this.ShowInTaskbar = false;
				 this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
				 this.Text = "Select a Symbol";
				 this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SymbolForm_KeyDown);
				 this.Closing += new System.ComponentModel.CancelEventHandler(this.SymbolForm_Closing);
				 this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SymbolForm_KeyPress);
				 this.Deactivate += new System.EventHandler(this.SymbolForm_Deactivate);
				 ((System.ComponentModel.ISupportInitialize)(this.dgSymbol)).EndInit();
				 this.ResumeLayout(false);

		}

		public static void InitSymbolList(DataTable dt)
		{
			SymbolForm.Current.AddShortcut(dt, ShortcutType.Stock);
		}

		public static void PressKeyAndShow(Form Owner, char c)
		{
			if (char.IsSymbol(c) || char.IsLetterOrDigit(c))
			{
				SymbolForm.ShowForm(Owner);
				SymbolForm.Current.tbSelect.Text = "";
				SymbolForm.Current.AddKeys(c);
			}
		}

		public static void RefreshIndicators()
		{
			SymbolForm.Current.AddShortcut(FormulaBase.GetAllIndicators(false), ShortcutType.Indicator, true);
		}

		public void RemoveShortcut(ShortcutType ShortType)
		{
			for (int num1 = 0; num1 < this.dtSymbol.Rows.Count; num1++)
			{
				DataRow row1 = this.dtSymbol.Rows[num1];
				int num2 = (int) ShortType;
				if (row1[0].ToString() == num2.ToString())
				{
					this.dtSymbol.Rows.Remove(row1);
					continue;
				}
			}
		}

		public static void ShowForm(Form Owner)
		{
			SymbolForm.Current.Owner = Owner;
			if (Owner != null)
			{
				SymbolForm.Current.Left = Owner.Right - SymbolForm.Current.Width;
				SymbolForm.Current.Top = Owner.Bottom - SymbolForm.Current.Height;
			}
			SymbolForm.Current.Show();
			SymbolForm.Current.Activate();
		}

		private void SymbolForm_Closing(object sender, CancelEventArgs e)
		{
			this.dgSymbol.DataSource = null;
			base.Hide();
			e.Cancel = true;
		}

		private void SymbolForm_Deactivate(object sender, EventArgs e)
		{
			this.tmHide.Enabled = true;
		}

		private void SymbolForm_KeyDown(object sender, KeyEventArgs e)
		{
			Keys keys1 = e.KeyCode;
			if (keys1 != Keys.Return)
			{
				if (keys1 == Keys.Escape)
				{
					base.Close();
					return;
				}
				return;
			}
			if ((this.dgSymbol.CurrentRowIndex < 0) || (this.dgSymbol.CurrentRowIndex >= this.dvSymbol.Count))
			{
                base.Close();
                return;
			}
			string text1 = this.dgSymbol[this.dgSymbol.CurrentRowIndex, 2].ToString();
			ShortcutType type1 = (ShortcutType) int.Parse(this.dgSymbol[this.dgSymbol.CurrentRowIndex, 0].ToString());
			if (base.Owner is ChartForm)
			{
				switch (type1)
				{
					case ShortcutType.Stock:
					{
						if (this.dgSymbol.CurrentRowIndex >= 0)
						{
							ChartForm.OpenChartForm(text1, base.Owner.MdiParent, false);
						}
                        if (type1 == ShortcutType.Folder)
                        {
                            ListForm.Current.FolderId = int.Parse(text1);
                            ListForm.Current.Activate();
                        }
					}
                    break;
					case ShortcutType.Indicator:
					{
						(base.Owner as ChartForm).ChartControl.SetAreaByName(text1);
                        if (type1 == ShortcutType.Folder)
                        {
                            ListForm.Current.FolderId = int.Parse(text1);
                            ListForm.Current.Activate();
                        }
					}
                    break;
				}
			}
			else if ((base.Owner is ListForm) && (type1 == ShortcutType.Stock))
			{
				ListForm.Current.GotoSymbol(text1);
			}
		}

		private void SymbolForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = this.AddKeys(e.KeyChar);
		}

		private void tmHide_Tick(object sender, EventArgs e)
		{
			this.tmHide.Enabled = false;
			base.Close();
		}


		// Properties
		public static bool IsVisible
		{
			get
			{
				return SymbolForm.Current.Visible;
			}
		}


		// Fields
		private IContainer components;
		public static SymbolForm Current;
		private DataGridTableStyle dgStyle;
		private DataGrid dgSymbol;
		private DataTable dtSymbol;
		private DataView dvSymbol;
		private Hashtable htSymbol;
		private ImageList ilIcon;
		private byte[] PY;
		private TextBox tbSelect;
		private Timer tmHide;
	}

}
