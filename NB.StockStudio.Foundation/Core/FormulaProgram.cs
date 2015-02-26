namespace NB.StockStudio.Foundation
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml.Serialization;

    public class FormulaProgram
    {
        public string Code;
        public DataCycle DefaultCycle;
        public string Description;
        public DataCycleCollection DisabledCycle = new DataCycleCollection();
        private int EndLineNum;
        private int[] ExtColumn;
        private int ExtLine;
        public string FullName;
        [XmlAttribute]
        public bool IsMainView;
        [XmlAttribute]
        public string Name;
        public ParamCollection Params = new ParamCollection();
        private int StartLineNum;

        public void AdjustErrors(CompilerError ce)
        {
            int num = (ce.Line - this.ExtLine) - this.StartLineNum;
            ce.Line = num;
            if ((num < 1) || (num > this.ExtColumn.Length))
            {
                num = 1;
            }
            ce.Column -= this.ExtColumn[num - 1];
        }

        private string AppendAttrs(string s, string Var, string Attrs)
        {
            if (Attrs != null)
            {
                string str2 = s;
                s = str2 + Var.ToUpper() + ".SetAttrs(\"" + Attrs + "\");";
            }
            return s;
        }

        public Assembly Compile()
        {
            return this.Compile("");
        }

        public Assembly Compile(string FileName)
        {
            return this.Compile(FileName, "");
        }

        public Assembly Compile(string FileName, string ReferenceRoot)
        {
            CompilerResults results = FormulaSpace.Compile("using NB.StockStudio.Foundation;\r\nusing NB.StockStudio.Foundation.DataProvider;\r\n" + this.GetSource(""), FileName, ReferenceRoot);
            this.StartLineNum = 2;
            if (results.Errors.Count > 0)
            {
                for (int i = 0; i < results.Errors.Count; i++)
                {
                    CompilerError ce = results.Errors[i];
                    this.AdjustErrors(ce);
                }
                throw new FormulaErrorException(results.Errors);
            }
            return results.CompiledAssembly;
        }

        private string GetAttrs(string s, out string Attrs)
        {
            int num = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] == ',') && (num == 0))
                {
                    Attrs = s.Substring(i + 1);
                    return s.Substring(0, i);
                }
                if (s[i] == '(')
                {
                    num++;
                }
                else if (s[i] == ')')
                {
                    num--;
                }
            }
            Attrs = null;
            return s;
        }

        public string GetParam(string Tab)
        {
            string str = "";
            string str2 = "";
            foreach (FormulaParam param in this.Params)
            {
                if (param.ParamType == FormulaParamType.Double)
                {
                    string str4 = str;
                    str = str4 + Tab + "\tpublic double " + param.Name.ToUpper() + "=0;\r\n";
                    string str5 = str2;
                    str2 = str5 + Tab + "\t\tAddParam(\"" + param.Name + "\"," + param.DefaultValue + "," + param.MinValue + "," + param.MaxValue + ");\r\n";
                }
                else
                {
                    string str6 = str;
                    str = str6 + Tab + "\tpublic string " + param.Name.ToUpper() + "=\"\";\r\n";
                    string str7 = str2;
                    str2 = str7 + Tab + "\t\tAddParam(\"" + param.Name + "\",\"" + param.DefaultValue + "\",\"" + param.MinValue + "\",\"" + param.MaxValue + "\");\r\n";
                }
            }
            return (str + Tab + "\tpublic " + this.Name + "():base()\r\n" + Tab + "\t{\r\n" + str2 + Tab + "\t}\r\n");
        }

        public string GetSource()
        {
            return this.GetSource("");
        }

        public string GetSource(string Tab)
        {
            return this.GetSource(Tab, this.Code);
        }

        public string GetSource(string Tab, string TheCode)
        {
            string str = TheCode;
            if (str.EndsWith(";"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            string[] strArray = str.Split(new char[] { ';' });
            ArrayList list = new ArrayList();
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            int num2 = 0;
            this.ExtLine = 8 + (this.Params.Count * 2);
            this.ExtColumn = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Trim();
                if (strArray[i] != "")
                {
                    string str3;
                    strArray[i] = this.GetAttrs(strArray[i], out str3);
                    strArray[i] = this.ReplaceRef(strArray[i]);
                    strArray[i] = this.ReplaceDollar(strArray[i].Replace("'", "\""));
                    string str4 = "";
                    if (strArray[i].StartsWith("@"))
                    {
                        strArray[i] = this.ToUpper(strArray[i].Substring(1)) + ";";
                        this.ExtColumn[i] = Tab.Length + 1;
                    }
                    else
                    {
                        string str2;
                        int index = strArray[i].IndexOf(":=");
                        if (index > 0)
                        {
                            str4 = strArray[i].Substring(0, index).Trim();
                            str2 = "";
                            if (hashtable[str4] == null)
                            {
                                str2 = "FormulaData ";
                            }
                            hashtable[str4] = "1";
                            strArray[i] = str2 + str4.ToUpper() + "=" + this.ToUpper(strArray[i].Substring(index + 2)) + "; " + str4.ToUpper() + ".Name=\"" + str4 + "\";";
                            this.ExtColumn[i] = (Tab.Length + 2) + 11;
                        }
                        else
                        {
                            index = strArray[i].IndexOf(":");
                            if (index > 0)
                            {
                                str4 = strArray[i].Substring(0, index).Trim();
                                str2 = "";
                                if (hashtable[str4] == null)
                                {
                                    str2 = "FormulaData ";
                                }
                                hashtable[str4] = "1";
                                strArray[i] = str2 + str4.ToUpper() + "=" + this.ToUpper(strArray[i].Substring(index + 1)) + "; " + str4.ToUpper() + ".Name=\"" + str4 + "\";";
                                list.Add(str4.ToUpper());
                                this.ExtColumn[i] = (Tab.Length + 2) + 12;
                            }
                            else
                            {
                                str4 = "NONAME" + num2++;
                                list.Add(str4);
                                strArray[i] = "FormulaData " + str4 + "=" + this.ToUpper(strArray[i]) + ";";
                                this.ExtColumn[i] = ((Tab.Length + 2) + 13) + str4.Length;
                            }
                        }
                    }
                    if (str4 != "")
                    {
                        strArray[i] = this.AppendAttrs(strArray[i], str4, str3);
                    }
                }
            }
            return (Tab + "public class " + this.Name + ":FormulaBase\r\n" + Tab + "{\r\n" + this.GetParam(Tab) + Tab + "\tpublic override FormulaPackage Run(IDataProvider DP)\r\n" + Tab + "\t{\r\n" + Tab + "\t\tthis.DataProvider = DP;\r\n" + Tab + "\t\t" + string.Join("\r\n" + Tab + "\t\t", strArray) + "\r\n" + Tab + "\t\treturn new FormulaPackage(new FormulaData[]{" + string.Join(",", (string[]) list.ToArray(typeof(string))) + "},\"\");\r\n" + Tab + "\t}\r\n" + Tab + "} //class " + this.Name + "\r\n");
        }

        public string GetSource(string Tab, ref int StartLine)
        {
            this.StartLineNum = StartLine;
            string source = this.GetSource(Tab, this.Code);
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '\r')
                {
                    StartLine++;
                }
            }
            this.EndLineNum = StartLine;
            return source;
        }

        public bool InProgram(int Line)
        {
            return ((Line >= this.StartLineNum) && (Line <= this.EndLineNum));
        }

        private string ReplaceDollar(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '$')
                {
                    continue;
                }
                int index = i + 1;
                while (index < s.Length)
                {
                    if (!char.IsLetterOrDigit(s, index) && (s[index] != '_'))
                    {
                        break;
                    }
                    index++;
                }
                string str = s.Substring(i + 1, (index - i) - 1);
                s = s.Remove(i, index - i).Insert(i, "ORGDATA(\"" + str + "\")");
            }
            return s;
        }

        private string ReplaceRef(string s)
        {
            int startIndex = 0;
            while (startIndex < s.Length)
            {
                int index = s.IndexOf('"', startIndex);
                if (index < 0)
                {
                    return s;
                }
                int num3 = s.IndexOf('"', index + 1);
                if (num3 > index)
                {
                    string str = s.Substring(index + 1, (num3 - index) - 1);
                    str = "FML(DP,\"" + str + "\")";
                    s = s.Remove(index, (num3 - index) + 1).Insert(index, str);
                    startIndex = index + str.Length;
                }
            }
            return s;
        }

        public string ToUpper(string s)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] == '\'') || (s[i] == '"'))
                {
                    num = 1 - num;
                }
                if (num == 0)
                {
                    builder.Append(char.ToUpper(s[i]));
                }
                else
                {
                    builder.Append(s[i]);
                }
            }
            return builder.ToString();
        }
    }
}

