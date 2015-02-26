using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace NB.StockStudio.Foundation
{
    public class PluginManager
    {
        private static Hashtable htAssembly = CollectionsUtil.CreateCaseInsensitiveHashtable();
        // new Hashtable(StringComparer.InvariantCultureIgnoreCase);
        // new Hashtable (CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant  );
        // (CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
        private static Hashtable htFormulaSpace = new Hashtable();
        private static Hashtable htShadow = new Hashtable();
        private static string PluginsDir;

        public static  event FileSystemEventHandler OnPluginChanged;

        private PluginManager()
        {
        }

        public static string DllToFml(string Filename)
        {
            if ((Filename != null) && Filename.EndsWith("_fml.dll"))
            {
                return (Filename.Substring(0, Filename.Length - 8) + ".fml");
            }
            return null;
        }

        private static string GetAssemblyHash(byte[] bs)
        {
            int num = 0;
            for (int i = 0; i < bs.Length; i++)
            {
                num += bs[i];
            }
            return num.ToString();
        }

        private static byte[] GetByteFromFile(string FileName)
        {
            if (System.IO.File.Exists(FileName))
            {
                FileStream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
                finally
                {
                    stream.Close();
                }
            }
            return null;
        }

        private static byte[] GetByteFromWeb(string FileName)
        {
            WebClient client = new WebClient();
            try
            {
                return client.DownloadData(FileName);
            }
            catch
            {
                return null;
            }
        }

        public static string GetFormulaFile(FormulaBase fb)
        {
            string assemblyKey = FormulaBase.GetAssemblyKey(fb.GetType().Assembly);
            if (assemblyKey != null)
            {
                foreach (string str2 in htAssembly.Keys)
                {
                    if (htAssembly[str2].ToString() == assemblyKey)
                    {
                        return str2;
                    }
                }
            }
            return null;
        }

        public static FormulaProgram GetFormulaProgram(FormulaBase fb)
        {
            return GetFormulaProgram(DllToFml(GetFormulaFile(fb)), fb);
        }

        private static FormulaProgram GetFormulaProgram(string Filename, FormulaBase fb)
        {
            if (Filename != null)
            {
                FormulaSpace space = (FormulaSpace) htFormulaSpace[Filename];
                if ((space == null) && System.IO.File.Exists(Filename))
                {
                    space = FormulaSpace.Read(Filename);
                    htFormulaSpace[Filename] = space;
                }
                if (space != null)
                {
                    return space.FindFormulaProgram(fb);
                }
            }
            return null;
        }

        public static void Load(string Path)
        {
            PluginsDir = Path;
            if (Directory.Exists(PluginsDir))
            {
                FileSystemWatcher watcher = new FileSystemWatcher(PluginsDir, "*.dll");
                watcher.Created += new FileSystemEventHandler(PluginManager.OnFileChange);
                watcher.Changed += new FileSystemEventHandler(PluginManager.OnFileChange);
                watcher.Deleted += new FileSystemEventHandler(PluginManager.OnFileChange);
                foreach (string str in Directory.GetFiles(PluginsDir, "*.dll"))
                {
                    LoadAssembly(str);
                }
                watcher.EnableRaisingEvents = true;
            }
        }

        private static void LoadAssembly(string FileName)
        {
            if (FileName.StartsWith("http"))
            {
                Assembly a = Assembly.LoadFrom(FileName);
                FormulaBase.RegAssembly(a.GetHashCode().ToString(), a);
            }
            else
            {
                byte[] byteFromFile = GetByteFromFile(FileName);
                Assembly assembly2 = Assembly.Load(byteFromFile);
                htAssembly[FileName] = GetAssemblyHash(byteFromFile);
                FormulaBase.RegAssembly(htAssembly[FileName].ToString(), assembly2);
            }
        }

        public static void LoadFromWeb()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            int length = codeBase.LastIndexOf("/");
            if (length > 0)
            {
                codeBase = codeBase.Substring(0, length);
            }
            LoadFromWeb(codeBase);
        }

        public static void LoadFromWeb(string Path)
        {
            if (!Path.EndsWith("/"))
            {
                Path = Path + "/";
            }
            Path = Path + "Plugins/";
            PluginsDir = Path;
            LoadAssembly(Path + "Basic_fml.dll");
            LoadAssembly(Path + "Native_fml.dll");
            LoadAssembly(Path + "Extend_fml.dll");
        }

        public static Assembly LoadShadowAssembly(string FileName)
        {
            byte[] byteFromFile = GetByteFromFile(FileName);
            string str = (string) htAssembly[FileName];
            string assemblyHash = GetAssemblyHash(byteFromFile);
            if (str == assemblyHash)
            {
                return (Assembly) htShadow[assemblyHash];
            }
            htAssembly[FileName] = assemblyHash;
            Assembly assembly = Assembly.Load(byteFromFile);
            htShadow[assemblyHash] = assembly;
            return assembly;
        }

        private static void OnFileChange(object source, FileSystemEventArgs e)
        {
            try
            {
                byte[] byteFromFile = GetByteFromFile(e.FullPath);
                if ((byteFromFile.Length != 0) || (e.ChangeType == WatcherChangeTypes.Deleted))
                {
                    string assemblyHash = GetAssemblyHash(byteFromFile);
                    string key = htAssembly[e.FullPath].ToString();
                    if (key != assemblyHash)
                    {
                        FormulaBase.UnregAssembly(key);
                        if (byteFromFile.Length > 0)
                        {
                            FormulaBase.RegAssembly(assemblyHash, Assembly.Load(byteFromFile));
                            htAssembly[e.FullPath] = assemblyHash;
                        }
                        if (OnPluginChanged != null)
                        {
                            OnPluginChanged(null, e);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}

