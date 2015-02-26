using NB.StockStudio.Foundation.DataProvider;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

namespace NB.StockStudio
{
	public class StockDB
	{
		// Methods
		static StockDB()
		{
			StockDB.DataPath = Environment.CurrentDirectory + @"\Data\";
			StockDB.HistoricalPath = Environment.CurrentDirectory + @"\Data\Historical\";
		}

		private StockDB()
		{
		}

		public static void AddToFavorite(string Code)
		{
			try
			{
				StockDB.LoadFolderRelRow(Code, StockDB.CurrentFavoriteId);
			}
			catch
			{
			}
		}

		public static void BeginInit(string TableName)
		{
			DataTable[] tableArray1 = StockDB.AllDataBase;
			for (int num1 = 0; num1 < tableArray1.Length; num1++)
			{
				DataTable table1 = tableArray1[num1];
				if (string.Compare(table1.TableName, TableName, true) == 0)
				{
					table1.BeginInit();
				}
			}
		}

		public static void BeginLoadData(string TableName)
		{
			DataTable[] tableArray1 = StockDB.AllDataBase;
			for (int num1 = 0; num1 < tableArray1.Length; num1++)
			{
				DataTable table1 = tableArray1[num1];
				if (string.Compare(table1.TableName, TableName, true) == 0)
				{
					table1.BeginLoadData();
				}
			}
		}

		public static void CalChange(DataRow dr)
		{
			object obj1 = dr["LastA"];
			object obj2 = dr["CloseA"];
			if ((obj1 != DBNull.Value) && (obj2 != DBNull.Value))
			{
                double num1 = ((double)obj2) - ((double)obj1);
				dr["Change"] = num1;
                if (((double)obj1) != 0)
				{
                    dr["ChangeP"] = num1 / ((double)obj1);
				}
				if (num1 > 0)
				{
					dr["UpDown"] = 1;
				}
				else if (num1 < 0)
				{
					dr["UpDown"] = -1;
				}
				else
				{
					dr["UpDown"] = 0;
				}
			}
		}

		public static void ClearSymbolList()
		{
			StockDB.dtSymbolListChanged = true;
			StockDB.dtSymbolList.Clear();
		}

		public static void Close()
		{
			if (StockDB.dtSymbolListChanged)
			{
				StockDB.SaveDB(StockDB.dtSymbolList);
			}
			if (StockDB.dtFolderAllChanged)
			{
				StockDB.SaveDB(StockDB.dtFolderAll);
			}
			if (StockDB.dtFolderRelChanged)
			{
				StockDB.SaveDB(StockDB.dtFolderRel);
			}
		}

		public static void CreateDBSchema()
		{
			StockDB.dtSymbolList = new DataTable("SymbolList");
			StockDB.dtSymbolList.Columns.Add("Code", typeof(String));
			StockDB.dtSymbolList.Columns.Add("Name",typeof(String));
			StockDB.dtSymbolList.Columns.Add("Exchange",typeof(String));
			StockDB.dtSymbolList.Columns.Add("AliasCode",typeof(String));
            StockDB.dtSymbolList.Columns.Add("LastA", typeof(double));
            StockDB.dtSymbolList.Columns.Add("OpenA", typeof(double));
            StockDB.dtSymbolList.Columns.Add("High", typeof(double));
            StockDB.dtSymbolList.Columns.Add("Low", typeof(double));
            StockDB.dtSymbolList.Columns.Add("CloseA", typeof(double));
			StockDB.dtSymbolList.Columns.Add("Volume", typeof(double));
            StockDB.dtSymbolList.Columns.Add("Amount", typeof(double));
			StockDB.dtSymbolList.Columns.Add("LastTime", typeof(DateTime));
			StockDB.dtSymbolList.Columns.Add("MergeTime", typeof(DateTime));
			DataColumn[] colPrimaryKey = new DataColumn[] { StockDB.dtSymbolList.Columns["Code"] } ;
			StockDB.dtSymbolList.PrimaryKey = colPrimaryKey;

			StockDB.dtFolderAll = new DataTable("Folder");
			StockDB.dtFolderAll.Columns.Add("FolderId", typeof(int));
			StockDB.dtFolderAll.Columns.Add("ParentId", typeof(int));
			StockDB.dtFolderAll.Columns.Add("FolderName");
			StockDB.dtFolderAll.Columns.Add("CreateDate", typeof(DateTime));
			StockDB.dtFolderAll.Columns.Add("Visible", typeof(int));
			StockDB.dtFolderAll.Columns.Add("Description");
			StockDB.dtFolderAll.Columns[0].AutoIncrement = true;
            colPrimaryKey = new DataColumn[] { StockDB.dtFolderAll.Columns["FolderId"] };
            StockDB.dtFolderAll.PrimaryKey = colPrimaryKey;

			StockDB.dtFolderRel = new DataTable("FolderRel");
			StockDB.dtFolderRel.Columns.Add("Code");
			StockDB.dtFolderRel.Columns.Add("FolderId", typeof(int));
            colPrimaryKey = new DataColumn[] { StockDB.dtFolderRel.Columns["Code"], StockDB.dtFolderRel.Columns["FolderId"] };
            StockDB.dtFolderRel.PrimaryKey = colPrimaryKey;
			DataTable[] tableArray = new DataTable[] { StockDB.dtSymbolList, StockDB.dtFolderAll, StockDB.dtFolderRel } ;
            StockDB.AllDataBase = tableArray;
		}

		public static string[] DecodeCSV(string s)
		{
			int num1 = 0;
			int num2 = 0;
			ArrayList list1 = new ArrayList();
			for (int num3 = 0; num3 < s.Length; num3++)
			{
				if (s[num3] == '"')
				{
					num1++;
				}
				if (((num1 % 2) == 0) && (s[num3] == ','))
				{
					string text1 = s.Substring(num2, num3 - num2);
					if (((text1.Length > 1) && (text1[0] == '"')) && (text1[text1.Length - 1] == '"'))
					{
						text1 = text1.Substring(1, text1.Length - 2);
					}
					text1 = text1.Replace("\"\"", "\"");
					list1.Add(text1);
					num2 = num3 + 1;
				}
			}
			if (num2 < s.Length)
			{
				list1.Add(s.Substring(num2));
			}
			return (string[]) list1.ToArray(typeof(string));
		}

		public static int DeleteFolderRelRows(string Filter)
		{
			StockDB.dtFolderRelChanged = true;
			return StockDB.DeleteRows(StockDB.dtFolderRel, Filter);
		}

		public static void DeleteFolderRelRows(object Code, int FolderId)
		{
			object[] objArray1 = new object[5] { "FolderId=", FolderId, " and Code='", Code, "'" } ;
			StockDB.DeleteFolderRelRows(string.Concat(objArray1));
		}

		public static int DeleteFolderRows(string Filter)
		{
			StockDB.dtFolderAllChanged = true;
			return StockDB.DeleteRows(StockDB.dtFolderAll, Filter);
		}

		public static int DeleteFolderRows(int FolderId, bool DeleteRel)
		{
			if (DeleteRel)
			{
				StockDB.DeleteFolderRelRows("FolderId=" + FolderId);
			}
			DataRow[] rowArray1 = StockDB.GetDataRows(StockDB.dtFolderAll, "ParentId=" + FolderId);
			DataRow[] rowArray2 = rowArray1;
			for (int num2 = 0; num2 < rowArray2.Length; num2++)
			{
				DataRow row1 = rowArray2[num2];
				StockDB.DeleteFolderRows((int) row1["FolderId"], DeleteRel);
			}
			return StockDB.DeleteFolderRows("ParentId=" + FolderId);
		}

		public static int DeleteListRows(string Filter)
		{
			return StockDB.DeleteRows(StockDB.dtList, Filter);
		}

		public static int DeleteRows(DataTable dt, string Filter)
		{
			DataRow[] rowArray1 = dt.Select(Filter);
			DataRow[] rowArray2 = rowArray1;
			for (int num2 = 0; num2 < rowArray2.Length; num2++)
			{
				DataRow row1 = rowArray2[num2];
				dt.Rows.Remove(row1);
			}
			return rowArray1.Length;
		}

		public static int DeleteSymbol(string Symbol)
		{
			StockDB.DeleteListRows("Code='" + Symbol + "'");
			StockDB.DeleteSymbolRows("Code='" + Symbol + "'");
			return StockDB.DeleteFolderRelRows("Code='" + Symbol + "'");
		}

		public static int DeleteSymbolRows(string Filter)
		{
			StockDB.dtSymbolListChanged = true;
			return StockDB.DeleteRows(StockDB.dtSymbolList, Filter);
		}

		public static string EncodeCSV(string s)
		{
			s = s.Replace("\"", "\"\"");
			if (s.IndexOf(',') > 0)
			{
				s = "\"" + s + "\"";
			}
			return s;
		}

		public static void EndInit(string TableName)
		{
			DataTable[] tableArray1 = StockDB.AllDataBase;
			for (int num1 = 0; num1 < tableArray1.Length; num1++)
			{
				DataTable table1 = tableArray1[num1];
				if (string.Compare(table1.TableName, TableName, true) == 0)
				{
					table1.EndInit();
				}
			}
		}

		public static void EndLoadData(string TableName)
		{
			DataTable[] tableArray1 = StockDB.AllDataBase;
			for (int num1 = 0; num1 < tableArray1.Length; num1++)
			{
				DataTable table1 = tableArray1[num1];
				if (string.Compare(table1.TableName, TableName, true) == 0)
				{
					table1.EndLoadData();
				}
			}
		}

		public static DataRow[] GetDataRows(DataTable dt, string Filter)
		{
			return dt.Select(Filter);
		}

		public static DataRow[] GetDataRows(string TableName, string Filter)
		{
			DataTable[] tableArray1 = StockDB.AllDataBase;
			for (int num1 = 0; num1 < tableArray1.Length; num1++)
			{
				DataTable table1 = tableArray1[num1];
				if (string.Compare(table1.TableName, TableName, true) == 0)
				{
					return StockDB.GetDataRows(table1, Filter);
				}
			}
			return new DataRow[0];
		}

		public static DataTable GetDataTable(DataTable dt, string Filter)
		{
			DataRow[] rowArray1 = StockDB.GetDataRows(dt, Filter);
			DataTable table1 = dt.Clone();
			DataRow[] rowArray2 = rowArray1;
			for (int num1 = 0; num1 < rowArray2.Length; num1++)
			{
				DataRow row1 = rowArray2[num1];
				table1.ImportRow(row1);
			}
			return table1;
		}

		public static DataRow[] GetDistinctRows(DataTable dt, string Field, string Filter)
		{
			ArrayList list1 = new ArrayList();
			ArrayList list2 = new ArrayList();
			DataRow[] rowArray1 = dt.Select(Filter);
			DataRow[] rowArray3 = rowArray1;
			for (int num1 = 0; num1 < rowArray3.Length; num1++)
			{
				DataRow row1 = rowArray3[num1];
				if (list1.IndexOf(row1[Field]) < 0)
				{
					list1.Add(row1[Field]);
					list2.Add(row1);
				}
			}
			return (DataRow[]) list2.ToArray(typeof(DataRow));
		}

		public static DataRow GetFirstRow(DataTable dt, string Filter)
		{
			DataRow[] rowArray1 = dt.Select(Filter);
			if (rowArray1.Length > 0)
			{
				return rowArray1[0];
			}
			return null;
		}

		public static DataRow GetFirstRow(string TableName, string Filter)
		{
			DataTable[] tableArray1 = StockDB.AllDataBase;
			for (int num1 = 0; num1 < tableArray1.Length; num1++)
			{
				DataTable table1 = tableArray1[num1];
				if (string.Compare(table1.TableName, TableName, true) == 0)
				{
					StockDB.GetFirstRow(table1, Filter);
				}
			}
			return null;
		}

		public static DataTable GetFolderDatatable()
		{
			if (StockDB.dtFolder == null)
			{
				StockDB.dtFolder = StockDB.GetDataTable(StockDB.dtFolderAll, "visible = 1");
				DataColumn[] columnArray1 = new DataColumn[1] { StockDB.dtFolder.Columns[0] } ;
				StockDB.dtFolder.PrimaryKey = columnArray1;
			}
			return StockDB.dtFolder;
		}

		public static int GetMaxFolderId()
		{
			return StockDB.GetMaxFolderId("");
		}

		public static int GetMaxFolderId(string ParentId)
		{
			if (ParentId == "")
			{
				return StockDB.GetMaxValue(StockDB.dtFolderAll, "FolderId", "");
			}
			return StockDB.GetMaxValue(StockDB.dtFolderAll, "FolderId", "ParentId=" + ParentId);
		}

		public static int GetMaxValue(DataTable dt, string Field, string Filter)
		{
			object obj1 = dt.Compute("Max(" + Field + ")", Filter);
			if (obj1 == DBNull.Value)
			{
				return 0;
			}
			return (int) obj1;
		}

		public static DataPackage GetRealtimeData(DataRow dr)
		{
			if ((dr != null) && (dr["LastTime"] != DBNull.Value))
			{
				return new DataPackage
                    ((DateTime)dr["LastTime"], (double)dr["OpenA"], (double)dr["High"], (double)dr["Low"], (double)dr["CloseA"], (double)dr["Volume"], (double)dr["CloseA"], (string)dr["Code"], dr.IsNull("Name")?"":(string)dr["Name"]);
			}
			return null;
		}

		public static DataPackage GetRealtimeData(string Code)
		{
			DataTable table1 = StockDB.GetRealtimeList(false);
			DataRow row1 = table1.Rows.Find(Code);
			return StockDB.GetRealtimeData(row1);
		}


		public static DataTable GetRealtimeList(bool ForceRefresh)
		{
			return StockDB.dtList;
		}

		public static DataView GetStockList(int FolderId)
		{
			if (StockDB.dtList == null)
			{
				StockDB.dtList = StockDB.dtSymbolList.Copy();
				StockDB.dtList.BeginLoadData();
				StockDB.dtList.Columns.Add("Change", typeof(double));
				StockDB.dtList.Columns.Add("Num", typeof(int));
				StockDB.dtList.Columns.Add("ChangeP", typeof(double));
				StockDB.dtList.Columns.Add("UpDown", typeof(int));
				DataColumn[] columnArray1 = new DataColumn[1] { StockDB.dtList.Columns["Code"] } ;
				StockDB.dtList.PrimaryKey = columnArray1;
				for (int num1 = 0; num1 < StockDB.dtList.Rows.Count; num1++)
				{
					DataRow row1 = StockDB.dtList.Rows[num1];
					row1["Num"] = num1 + 1;
					string text1 = row1["Code"].ToString();
					StockDB.CalChange(row1);
				}
				StockDB.dtList.TableName = "NameList";
				StockDB.dtList.EndLoadData();
				StockDB.dtList.AcceptChanges();
			}
			DataRow row2 = StockDB.GetFirstRow(StockDB.dtFolderAll, "folderId=" + FolderId);
			int num2 = 0;
			if (row2 != null)
			{
				num2 = (int) row2["ParentId"];
				if (FolderId == 0)
				{
					StockDB.currentList = StockDB.dtList.DefaultView;
				}
				else if (num2 == 2)
				{
					StockDB.currentList = new DataView(StockDB.dtList, "Exchange='" + row2["FolderName"] + "'", "", DataViewRowState.CurrentRows);
				}
				else
				{
					DataRow[] rowArray1 = StockDB.GetDataRows(StockDB.dtFolderRel, "FolderId=" + FolderId);
#if (vs2005)
                    Hashtable hashtable1 = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
#else
					Hashtable hashtable1 = new Hashtable(null, CaseInsensitiveComparer.Default);
#endif
					DataRow[] rowArray2 = rowArray1;
					for (int num3 = 0; num3 < rowArray2.Length; num3++)
					{
						DataRow row3 = rowArray2[num3];
						hashtable1[row3[0]] = "1";
					}
					DataTable table1 = StockDB.dtList.Clone();
					foreach (DataRow row4 in StockDB.dtList.Rows)
					{
						if (hashtable1[row4["Code"]] != null)
						{
							table1.Rows.Add(row4.ItemArray);
						}
					}
					StockDB.currentList = table1.DefaultView;
				}
			}
			else
			{
				StockDB.currentList = StockDB.dtList.DefaultView;
			}
			return StockDB.currentList;
		}

		public static DataRow GetSymbol(string Code)
		{
			return StockDB.dtSymbolList.Rows.Find(Code);
		}

		public static DataTable GetSymbolList()
		{
			return StockDB.dtSymbolList;
		}
 

		public static void LoadDB(DataTable dt)
		{
			string text1 = Environment.CurrentDirectory + @"\Data\" + dt.TableName + ".csv";
			dt.BeginLoadData();
			try
			{
				dt.Rows.Clear();
				if (dt.TableName == "Folder")
				{
					object[] objArray1 = new object[5];
					objArray1[1] = 0;
					objArray1[2] = "All";
					objArray1[3] = DateTime.Now;
					objArray1[4] = 1;
					dt.Rows.Add(objArray1);
					object[] objArray2 = new object[5];
					objArray2[1] = 0;
					objArray2[2] = "Scan";
					objArray2[3] = DateTime.Now;
					objArray2[4] = 1;
					dt.Rows.Add(objArray2);
					object[] objArray3 = new object[5];
					objArray3[1] = 0;
					objArray3[2] = "Exchange";
					objArray3[3] = DateTime.Now;
					objArray3[4] = 1;
					dt.Rows.Add(objArray3);
					object[] objArray4 = new object[5];
					objArray4[1] = 0;
					objArray4[2] = "Favorite";
					objArray4[3] = DateTime.Now;
					objArray4[4] = 1;
					dt.Rows.Add(objArray4);
					object[] objArray5 = new object[5];
					objArray5[1] = 0;
					objArray5[2] = "Industry";
					objArray5[3] = DateTime.Now;
					objArray5[4] = 1;
					dt.Rows.Add(objArray5);
					object[] objArray6 = new object[5];
					objArray6[1] = 0;
					objArray6[2] = "Reserved";
					objArray6[3] = DateTime.Now;
					objArray6[4] = 0;
					dt.Rows.Add(objArray6);
					object[] objArray7 = new object[5];
					objArray7[1] = 0;
					objArray7[2] = "Reserved";
					objArray7[3] = DateTime.Now;
					objArray7[4] = 0;
					dt.Rows.Add(objArray7);
					object[] objArray8 = new object[5];
					objArray8[1] = 0;
					objArray8[2] = "Reserved";
					objArray8[3] = DateTime.Now;
					objArray8[4] = 0;
					dt.Rows.Add(objArray8);
					object[] objArray9 = new object[5];
					objArray9[1] = 0;
					objArray9[2] = "Reserved";
					objArray9[3] = DateTime.Now;
					objArray9[4] = 0;
					dt.Rows.Add(objArray9);
					object[] objArray10 = new object[5];
					objArray10[1] = 0;
					objArray10[2] = "Reserved";
					objArray10[3] = DateTime.Now;
					objArray10[4] = 0;
					dt.Rows.Add(objArray10);
				}
				if (!File.Exists(text1))
				{
					return;
				}
				StreamReader reader1 = File.OpenText(text1);
				try
				{
					string text2 = reader1.ReadLine();
					char[] chArray1 = new char[1] { ',' } ;
					string[] textArray1 = text2.Trim().Split(chArray1);
					int num1 = dt.Rows.Count;
					text2 = string.Empty;
					if (num1 > 0)
					{
						while (num1-- > 0)
						{
							if ((text2 = reader1.ReadLine()) == null)
							{
								break;
							}
						}
					}
					while (!string.IsNullOrEmpty(text2 = reader1.ReadLine()))
					{
						string[] textArray2 = StockDB.DecodeCSV(text2);
						DataRow row1 = dt.NewRow();
						for (int num2 = 0; (num2 < textArray1.Length) && (num2 < textArray2.Length); num2++)
						{
							if (textArray2[num2] != "")
							{
								try
								{
									row1[textArray1[num2]] = textArray2[num2];
								}
								catch
								{
								}
							}
						}
						if (!row1.IsNull(dt.PrimaryKey[0]))
						{
							dt.Rows.Add(row1);
						}
					}
				}
				finally
				{
					reader1.Close();
				}
			}
			finally
			{
				dt.EndLoadData();
			}
		}

		public static void LoadFolderRelRow(object Code, int FolderId)
		{
			StockDB.dtFolderRelChanged = true;
			object[] objArray1 = new object[2] { Code, FolderId } ;
			StockDB.dtFolderRel.LoadDataRow(objArray1, false);
		}

		public static DataRow LoadFolderRow(int ParentId, string FolderName)
		{
			return StockDB.LoadFolderRow(ParentId, FolderName, true);
		}

		public static DataRow LoadFolderRow(int ParentId, string FolderName, bool Changed)
		{
			StockDB.dtFolderAllChanged = Changed;
			object[] objArray1 = new object[5];
			objArray1[1] = ParentId;
			objArray1[2] = FolderName;
			objArray1[3] = DateTime.Now;
			objArray1[4] = 1;
			return StockDB.dtFolderAll.LoadDataRow(objArray1, true);
		}

		public static void LoadRealtimeRow(string Code, DataPackage dp, bool AlreadyMerged)
		{
			DataRow ticker = StockDB.dtSymbolList.Rows.Find(Code);
			if (ticker != null)
			{
				StockDB.dtSymbolListChanged = true;
				if (dp.Last != 0f)
				{
					ticker["LastA"] = dp.Last;
				}
                if(!string.IsNullOrEmpty(dp.CompanyName))
                    ticker["Name"] = dp.CompanyName;
				ticker["OpenA"] = dp.Open;
				ticker["High"] = dp.High;
				ticker["Low"] = dp.Low;
				ticker["CloseA"] = dp.Close;
				ticker["Volume"] = dp.Volume;
				ticker["LastTime"] = dp.Date;
				if (AlreadyMerged)
				{
					ticker["MergeTime"] = dp.Date;
				}
				StockDB.UpdateRealtime(ticker);
			}
		}

		public static void LoadSymbolRow(object[] values)
		{
			StockDB.dtSymbolListChanged = true;
			DataRow row1 = StockDB.dtSymbolList.Rows.Add(values);
			SymbolForm.Current.AddShortcut(row1, ShortcutType.Stock);
		}

		public static void LoadSymbolRow(string Symbol, string Name, string Exchange)
		{
			object[] objArray1 = new object[3] { Symbol, Name, Exchange } ;
			StockDB.LoadSymbolRow(objArray1);
		}

		public static string LookupFolderName(int FolderId)
		{
			StockDB.GetFolderDatatable();
			DataRow row1 = StockDB.dtFolder.Rows.Find(FolderId);
			if (row1 != null)
			{
				return row1["FolderName"].ToString();
			}
			return "";
		}

		public static IDataProvider MergeOneRealtime(IDataManager idm, string Code, DataPackage dp)
		{
			CommonDataProvider provider1 = (CommonDataProvider) idm[Code];
			provider1.Merge(dp);
			provider1.SaveBinary(FileDataManager.GetFileName(Code));
			return provider1;
		}

		public static void MergeRealtime()
		{
			FileDataManager manager1 = new FileDataManager(null);
			foreach (DataRow row1 in StockDB.dtSymbolList.Rows)
			{
				if (!object.Equals(row1["LastTime"], row1["MergeTime"]))
				{
					string text1 = row1["Code"].ToString().Trim();
					row1["MergeTime"] = row1["LastTime"];
					StockDB.dtSymbolListChanged = true;
					StockDB.MergeOneRealtime(manager1, text1, StockDB.GetRealtimeData(row1));
				}
			}
		}

		public static int NextFolderId(int FolderId)
		{
			DataTable table1 = StockDB.GetFolderDatatable();
			DataRow row1 = table1.Rows.Find(FolderId);
			if (row1 != null)
			{
				DataView view1 = new DataView(table1, "ParentId=" + row1["ParentId"], "FolderId", DataViewRowState.CurrentRows);
				int num1 = view1.Find(FolderId);
				if ((num1 >= 0) && (view1.Count > 0))
				{
					num1 = ((num1 + 1) + view1.Count) % view1.Count;
					return (int) view1[num1]["FolderId"];
				}
			}
			return 0;
		}

		public static void Open()
		{
			if (!Directory.Exists(StockDB.DataPath))
			{
				Directory.CreateDirectory(StockDB.DataPath);
			}
			if (!Directory.Exists(StockDB.HistoricalPath))
			{
				Directory.CreateDirectory(StockDB.HistoricalPath);
			}
			StockDB.CreateDBSchema();
			StockDB.LoadDB(StockDB.dtSymbolList);
			StockDB.LoadDB(StockDB.dtFolderAll);
			StockDB.LoadDB(StockDB.dtFolderRel);
			StockDB.RecreateFolders();
		}

		public static string QuoteStr(object o)
		{
			string text1 = "";
			if ((o != null) && (o != DBNull.Value))
			{
				text1 = "'" + o.ToString().Replace("'", "''") + "'";
			}
			return text1;
		}

		public static void RecreateFolders()
		{
			StockDB.DeleteRows(StockDB.dtFolderAll, "ParentId = " + 2);
			DataRow[] rowArray1 = StockDB.GetDistinctRows(StockDB.dtSymbolList, "Exchange", "not Code like '^%'");
			DataRow[] rowArray2 = rowArray1;
			for (int num1 = 0; num1 < rowArray2.Length; num1++)
			{
				DataRow row1 = rowArray2[num1];
				StockDB.LoadFolderRow(2, row1["Exchange"].ToString(), false);
			}
			StockDB.ResetFolderDatabase();
			StockDB.GetFolderDatatable();
		}

		public static void ResetFolderDatabase()
		{
			StockDB.dtFolder = null;
		}

		public static void ResetList()
		{
			StockDB.dtList = null;
		}

		public static void SaveDB(DataTable dt)
		{
			string text1 = Environment.CurrentDirectory + @"\Data\" + dt.TableName + ".csv";
			StreamWriter writer1 = File.CreateText(text1);
			try
			{
				StringBuilder builder1 = new StringBuilder();
				for (int num1 = 0; num1 < dt.Columns.Count; num1++)
				{
					if (num1 != 0)
					{
						builder1.Append(',');
					}
					builder1.Append(dt.Columns[num1].ColumnName);
				}
				builder1.Append("\r\n");
				foreach (DataRow row1 in dt.Rows)
				{
					for (int num2 = 0; num2 < dt.Columns.Count; num2++)
					{
						if (num2 != 0)
						{
							builder1.Append(',');
						}
						builder1.Append(StockDB.EncodeCSV(row1[num2].ToString()));
					}
					builder1.Append("\r\n");
				}
				writer1.Write(builder1.ToString());
			}
			finally
			{
				writer1.Close();
			}
		}

		public static void UpdateFolderRow(int FolderId, string FolderName)
		{
			StockDB.dtFolderAllChanged = true;
			DataRow row1 = StockDB.dtFolderAll.Rows.Find(FolderId);
			if (row1 != null)
			{
				row1["FolderName"] = FolderName;
			}
			StockDB.ResetFolderDatabase();
			StockDB.GetFolderDatatable();
		}

		public static void UpdateRealtime(DataRow drRealtime)
		{
			DataRow row1 = StockDB.dtList.Rows.Find(drRealtime["Code"]);
			if (row1 != null)
			{
                row1["Name"] = drRealtime["Name"];
				row1["LastA"] = drRealtime["LastA"];
				row1["OpenA"] = drRealtime["OpenA"];
				row1["High"] = drRealtime["High"];
				row1["Low"] = drRealtime["Low"];
				row1["CloseA"] = drRealtime["CloseA"];
				row1["Volume"] = drRealtime["Volume"];
				row1["LastTime"] = drRealtime["LastTime"];
				row1["MergeTime"] = drRealtime["MergeTime"];
				StockDB.CalChange(row1);
			}
		}


		// Properties
		public static int CurrentExchangeId
		{
			get
			{
				if (StockDB.currentExchangeId == 0)
				{
					StockDB.currentExchangeId = StockDB.GetMaxValue(StockDB.dtFolder, "FolderId", "ParentId=" + 2);
				}
				return StockDB.currentExchangeId;
			}
			set
			{
				StockDB.currentExchangeId = value;
			}
		}

		
		public static int CurrentFavoriteId
		{
			get
			{
				if (StockDB.currentFavoriteId == 0)
				{
					StockDB.currentFavoriteId = StockDB.GetMaxValue(StockDB.dtFolder, "FolderId", "ParentId=" + 3);
				}
				if (StockDB.currentFavoriteId == 0)
				{
					StockDB.currentFavoriteId = 3;
				}
				return StockDB.currentFavoriteId;
			}
			set
			{
				StockDB.currentFavoriteId = value;
			}
		}

		public static DataView CurrentList
		{
			get
			{
				return StockDB.currentList;
			}
		}

		public static int RealtimeChangedCount
		{
			get
			{
				int num1 = 0;
				foreach (DataRow row1 in StockDB.dtSymbolList.Rows)
				{
					if (!object.Equals(row1["LastTime"], row1["MergeTime"]))
					{
						num1++;
					}
				}
				return num1;
			}
		}


		// Fields
		private static DataTable[] AllDataBase;
		private static int currentExchangeId;
		private static int currentFavoriteId;
		private static DataView currentList;
		public static string DataPath;
		private static DataTable dtFolder;
		private static DataTable dtFolderAll;
		private static bool dtFolderAllChanged;
		private static DataTable dtFolderRel;
		private static bool dtFolderRelChanged;
		private static DataTable dtList;
		private static DataTable dtSymbolList;
		private static bool dtSymbolListChanged;
		public static string HistoricalPath;
	}


}
