namespace NB.StockStudio.WinControls
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;

    public class DynamicConfig
    {
        private DynamicConfig()
        {
        }

        public static void Save(Control C)
        {
            XmlDocument document = new XmlDocument();
            string filename = Application.ExecutablePath + ".config";
            document.Load(filename);
            XmlNode node = document.SelectSingleNode("/configuration/appSettings");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i] is XmlElement)
                {
                    XmlElement element = node.ChildNodes[i] as XmlElement;
                    string name = element.GetAttribute("key").ToString();
                    int index = name.IndexOf('.');
                    if (index > 0)
                    {
                        name = name.Substring(index + 1);
                    }
                    try
                    {
                        object obj2 = C.GetType().InvokeMember(name, BindingFlags.GetProperty, null, C, new object[0]);
                        if (obj2 != null)
                        {
                            element.SetAttribute("value", obj2.ToString());
                        }
                    }
                    catch
                    {
                    }
                }
            }
            document.Save(filename);
        }
    }
}

