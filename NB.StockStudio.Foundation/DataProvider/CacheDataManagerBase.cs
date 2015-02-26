namespace NB.StockStudio.Foundation.DataProvider
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Caching;

    public class CacheDataManagerBase : DataManagerBase
    {
        public string CacheRoot;
        public bool EnableFileCache = true;
        public bool EnableMemoryCache = true;

        public new IDataProvider this[string Code, int Count]
        {
            get
            {
                string path = "";
                if (this.EnableMemoryCache && (HttpContext.Current != null))
                {
                    object obj2 = HttpContext.Current.Cache["Cache_" + Code];
                    if (obj2 != null)
                    {
                        return (CommonDataProvider) obj2;
                    }
                }
                if ((this.EnableFileCache && (this.CacheRoot != null)) && (this.CacheRoot != ""))
                {
                    try
                    {
                        if (!Directory.Exists(this.CacheRoot))
                        {
                            Directory.CreateDirectory(this.CacheRoot);
                        }
                        if (!this.CacheRoot.EndsWith(@"\"))
                        {
                            this.CacheRoot = this.CacheRoot + @"\";
                        }
                        path = this.CacheRoot + Code;
                        if (File.Exists(path) && (File.GetLastWriteTime(path).Date == DateTime.Now.Date))
                        {
                            CommonDataProvider provider = new CommonDataProvider(this);
                            provider.LoadBinary(path);
                            return provider;
                        }
                    }
                    catch
                    {
                        path = "";
                    }
                }
                IDataProvider data = this.GetData(Code, Count);
                if ((data is CommonDataProvider) && (data.Count > 0))
                {
                    try
                    {
                        if (this.EnableFileCache && (path != ""))
                        {
                            (data as CommonDataProvider).SaveBinary(path);
                        }
                    }
                    catch
                    {
                        if (this.EnableMemoryCache && (HttpContext.Current != null))
                        {
                            HttpContext.Current.Cache.Add("Cache_" + Code, data, null, DateTime.Now.AddDays(1.0), TimeSpan.Zero, CacheItemPriority.Normal, null);
                        }
                    }
                }
                return data;
            }
        }
    }
}

