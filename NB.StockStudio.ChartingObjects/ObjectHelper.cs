namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    public class ObjectHelper
    {
        public static CultureInfo enUS = new CultureInfo("en-US");

        public static void CreateObjectPath()
        {
            Directory.CreateDirectory(GetObjectRoot());
        }

        public static string GetIconFile(string IconName)
        {
            if (IconName != null)
            {
                return (GetIconRoot() + IconName + ".gif");
            }
            return "";
        }

        public static string GetIconRoot()
        {
            return (GetRoot() + @"Icon\");
        }

        public static string GetImageRoot()
        {
            return (GetRoot() + @"Images\");
        }

        public static string GetObjectFile(string Symbol)
        {
            return (GetObjectRoot() + Symbol + ".xml");
        }

        public static string GetObjectRoot()
        {
            return (GetRoot() + @"Object\");
        }

        public static string GetRoot()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            return location.Substring(0, location.Length - Path.GetFileName(location).Length);
        }
    }
}

