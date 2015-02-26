using NB.StockStudio.Foundation.DataProvider;
using System;
using System.Collections;
using System.Data;
using System.IO;

namespace NB.StockStudio
{
    public class FileDataManager : DataManagerBase
    {
        private Hashtable SymbolTable;


        public FileDataManager(Hashtable SymbolTable)
        {
            this.SymbolTable = SymbolTable;
        }

        public static string GetFileName(string Code)
        {
            return String.Concat(StockDB.HistoricalPath, Code, ".dat");
        }

        public override IDataProvider GetData(string Code, int Count)
        {
            string str = GetFileName(Code);
            byte[] bs1 = new byte[0];
            CommonDataProvider commonDataProvider = new CommonDataProvider(this);
            if (File.Exists(str))
            {
                commonDataProvider.LoadBinary(str);
            }
            else
            {
                commonDataProvider.LoadBinary(bs1);
            }
            if (SymbolTable != null)
            {
                DataRow dataRow = (DataRow)SymbolTable[Code];
                if (dataRow != null)
                {
                    commonDataProvider.SetStringData("Code", Code);
                    commonDataProvider.SetStringData("Name", dataRow["Name"].ToString());
                    commonDataProvider.SetStringData("Exchange", dataRow["Exchange"].ToString());
                }
            }
            IDataProvider iDataProvider = commonDataProvider;
            return iDataProvider;
        }
    }

}
