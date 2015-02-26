namespace NB.StockStudio.Foundation
{
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(IsNullable=false, ElementName="Namespace")]
    public class FormulaSpace
    {
        public string Description;
        public bool GroupOnly;
        private int LineNum;
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Version;

        [XmlArrayItem(ElementName="Namespace")]
        public FormulaSpaceCollection Namespaces;

        [XmlArrayItem(ElementName="Program")]
        public ProgramCollection Programs;

        private string usingNamspace = "using NB.StockStudio.Foundation;\r\nusing NB.StockStudio.Foundation.DataProvider;\r\n";
        
        public FormulaSpace()
        {
            this.Namespaces = new FormulaSpaceCollection();
            this.Programs = new ProgramCollection();
        }

        public FormulaSpace(string Name) : this()
        {
            this.Name = Name;
        }

        public void Compile(string Filename)
        {
            this.Compile(Filename, "");
        }

        public void Compile(string Filename, string ReferenceRoot)
        {
            CompilerResults compiledAssembly = this.GetCompiledAssembly(Filename, ReferenceRoot);
            compiledAssembly = null;
        }

        public void AddVersion()
        {
            if (Version == null || Version == "")
                Version = "1.0.0.0";
            else
            {
                string[] ss = Version.Split('.');
                if (ss.Length > 0)
                {
                    int i = 0;
                    try
                    {
                        i = int.Parse(ss[ss.Length - 1]) + 1;
                    }
                    catch
                    {
                    }
                    ss[ss.Length - 1] = i.ToString();
                }
                Version = string.Join(".", ss);
            }
        }

        public static CompilerResults Compile(string Code, string DestFileName, string ReferenceRoot)
        {
            CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");
            //ICodeCompiler compiler = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.IncludeDebugInformation = false;
            if ((DestFileName != null) && (DestFileName != ""))
            {
                options.OutputAssembly = DestFileName;
            }
            else
            {
                options.GenerateInMemory = true;
            }
            options.ReferencedAssemblies.Add(ReferenceRoot + "dll");
            return compiler.CompileAssemblyFromSource(options, Code);
        }

        public Assembly CompileInMemory(string ReferenceRoot)
        {
            return this.GetCompiledAssembly("", ReferenceRoot).CompiledAssembly;
        }

        public string CSharpSource()
        {
            return (usingNamspace + this.GetSource());
        }

        public FormulaProgram FindFormulaProgram(FormulaBase fb)
        {
            return this.FindFormulaProgram("", fb);
        }

        public FormulaProgram FindFormulaProgram(string Path, FormulaBase fb)
        {
            if (Path == "")
            {
                Path = this.Name;
            }
            else if (!this.GroupOnly)
            {
                Path = Path + "." + this.Name;
            }
            foreach (FormulaProgram program in this.Programs)
            {
                if (string.Compare(Path + "." + program.Name, fb.GetType().ToString(), true) == 0)
                {
                    return program;
                }
            }
            foreach (FormulaSpace space in this.Namespaces)
            {
                FormulaProgram program2 = space.FindFormulaProgram(Path, fb);
                if (program2 != null)
                {
                    return program2;
                }
            }
            return null;
        }

        private CompilerResults GetCompiledAssembly(string Filename, string ReferenceRoot)
        {
            CompilerResults results = Compile(this.CSharpSource(), Filename, ReferenceRoot);
            CompilerErrorCollection ces = results.Errors;
            if (ces.Count > 0)
            {
                throw new FormulaErrorException(ces);
            }
            return results;
        }

        public FormulaProgram GetProgramByLineNum(int Line)
        {
            if (this.Namespaces != null)
            {
                foreach (FormulaSpace space in this.Namespaces)
                {
                    FormulaProgram programByLineNum = space.GetProgramByLineNum(Line);
                    if (programByLineNum != null)
                    {
                        return programByLineNum;
                    }
                }
            }
            if (this.Programs != null)
            {
                foreach (FormulaProgram program2 in this.Programs)
                {
                    if (program2.InProgram(Line))
                    {
                        return program2;
                    }
                }
            }
            return null;
        }

        public string GetSource()
        {
            return this.GetSource("");
        }

        public string GetSource(string Tab)
        {
            int startLine = 2;
            return this.GetSource(Tab, ref startLine);
        }

        public string GetSource(string Tab, ref int StartLine)
        {
            this.LineNum = StartLine;
            string str = Tab + "namespace " + this.Name + "\r\n";
            string tab = Tab;
            StartLine++;
            if (this.GroupOnly)
            {
                str = Tab + "#region Formula Group " + this.Name + "\r\n";
            }
            else
            {
                str = str + Tab + "{\r\n";
                tab = tab + "\t";
                StartLine++;
            }
            if (this.Namespaces != null)
            {
                foreach (FormulaSpace space in this.Namespaces)
                {
                    str = str + space.GetSource(tab, ref StartLine) + "\r\n";
                    StartLine++;
                }
            }
            if (this.Programs != null)
            {
                foreach (FormulaProgram program in this.Programs)
                {
                    str = str + program.GetSource(tab, ref StartLine) + "\r\n";
                    StartLine++;
                }
            }
            if (this.GroupOnly)
            {
                str = str + Tab + "#endregion\r\n";
            }
            else
            {
                string str4 = str;
                str = str4 + Tab + "} // namespace " + this.Name + "\r\n";
            }
            StartLine++;
            return str;
        }

        public static FormulaSpace Read(Stream s)
        {
            TextReader reader = new StreamReader(s);
            return Read(reader);
        }

        public static FormulaSpace Read(TextReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FormulaSpace));
            return (FormulaSpace) serializer.Deserialize(reader);
        }

        public static FormulaSpace Read(string FileName)
        {
            TextReader reader = new StreamReader(FileName);
            FormulaSpace space = Read(reader);
            reader.Close();
            return space;
        }

        public void SaveCShartSource(string FileName)
        {
            TextWriter writer = new StreamWriter(FileName);
            writer.Write(this.CSharpSource());
            writer.Close();
        }

        public static void ThrowCompileException(CompilerErrorCollection ces)
        {
            if (ces.Count > 0)
            {
                string message = "CompilerError :\n";
                foreach (CompilerError error in ces)
                {
                    message = message + string.Format("line:{0} column:{1} error:{2} '{3}'\n", new object[] { error.Line, error.Column, error.ErrorNumber, error.ErrorText });
                }
                throw new InvalidProgramException(message);
            }
        }

        public void Write(TextWriter writer)
        {
            new XmlSerializer(typeof(FormulaSpace)).Serialize(writer, this, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName("Formula", "urn:NB.StockStudio") }));
        }

        public void Write(string FileName)
        {
            TextWriter writer = new StreamWriter(FileName);
            writer.NewLine = "\r\n";
            this.Write(writer);
            writer.Close();
        }
    }
}

