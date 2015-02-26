namespace NB.StockStudio.WinControls
{
    using NB.StockStudio.Foundation;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;

    public class FormulaEditor : Form
    {
        private Button btnCompile;
        private Button btnDebug;
        private CheckBox cbIsMainView;
        private CheckBox cbNotRealNameSpace;
        private ComboBox cbParamType;
        private ContextMenu cmTree;
        private IContainer components;
        private static FormulaEditor CurrentEditor;
        private bool DisableChange;
        private string filename = "";
        private TreeView FormulaTree;
        private FormulaSpace fs;
        private ColumnHeader HeaderColumn;
        private ColumnHeader HeaderLine;
        private ColumnHeader HeaderMessage;
        private ColumnHeader HeaderName;
        private ColumnHeader HeaderNumber;
        private ImageList ilFormula;
        private TreeNode LastNode;
        private ListBox lbIntelliSence;
        private Label lDefaultValue;
        private Label lFormulaProgramCode;
        private Label lFormulaProgramDesc;
        private Label lFormulaProgramName;
        private Label lFullName;
        private Label lMaxValue;
        private Label lMinValue;
        private Label lNamespaceDescription;
        private Label lNamespaceName;
        private Label lOverride = new Label();
        private Label lParamDesc;
        private Label lParamName;
        private Label lParamType;
        private ListView lvErrors;
        private MenuItem menuItem1;
        private MenuItem menuItem4;
        private MenuItem miAddFormulaProgram;
        private MenuItem miAddNamespace;
        private MenuItem miAddParam;
        private MenuItem miCompile;
        private MenuItem miDeleteNode;
        private MenuItem miExit;
        private MenuItem miNew;
        private MenuItem miOpen;
        private MenuItem miSave;
        private MenuItem miSaveAs;
        private MainMenu mmFumular;
        private bool modified;
        private OpenFileDialog odFormula;
        private Panel pnCode;
        private Panel pnFormula;
        private Panel pnNamespace;
        private Panel pnProgram;
        private SaveFileDialog sdFormula;
        private Splitter spFormula;
        private Splitter spProgram;
        private TextBox tbDefaultValue;
        private RichTextBox tbFormulaProgramCode;
        private TextBox tbFormulaProgramDesc;
        private TextBox tbFormulaProgramName;
        private TextBox tbMaxValue;
        private TextBox tbMinValue;
        private TextBox tbNamespaceDesc;
        private TextBox tbNamespaceName;
        private TextBox tbParamDesc;
        private TextBox tbParamName;
        private TextBox tbProgramFullName;
        private TabControl tcFormula;
        private TabPage tpFormulaProgram;
        private TabPage tpNamespace;
        private TabPage tpParameter;

        public FormulaEditor()
        {
            this.InitializeComponent();
            this.fs = new FormulaSpace("FML");
            this.fs.Description = "Namespace description";
            this.LoadToTree(this.fs);
            this.lOverride.Parent = this.pnFormula;
            this.lOverride.Width = 0x7d0;
            this.lOverride.Height = 0x18;
            this.lOverride.Top = this.tcFormula.Top;
            this.lOverride.Left = 0;
            this.lOverride.BringToFront();
            this.lOverride.BackColor = Color.WhiteSmoke;
            this.lOverride.Font = new Font("verdana", 11f, FontStyle.Bold);
            this.lOverride.ForeColor = Color.DarkGray;
            this.AddChangeEvent(this);
        }

        private void AddChangeEvent(Control c)
        {
            if (c is TextBox)
            {
                ((TextBox) c).TextChanged += new EventHandler(this.cbIsMainView_CheckedChanged);
            }
            else if (c is CheckBox)
            {
                ((CheckBox) c).TextChanged += new EventHandler(this.cbIsMainView_CheckedChanged);
            }
            else if (c is RichTextBox)
            {
                ((RichTextBox) c).TextChanged += new EventHandler(this.cbIsMainView_CheckedChanged);
            }
            foreach (Control control in c.Controls)
            {
                this.AddChangeEvent(control);
            }
        }

        private void AddError(CompilerErrorCollection ces, FormulaProgram fp)
        {
            this.lvErrors.Items.Clear();
            foreach (CompilerError error in ces)
            {
                ListViewItem item = null;
                if (fp != null)
                {
                    item = this.lvErrors.Items.Add(fp.Name);
                    item.Tag = fp;
                }
                else
                {
                    FormulaProgram programByLineNum = this.fs.GetProgramByLineNum(error.Line);
                    if (programByLineNum != null)
                    {
                        item = this.lvErrors.Items.Add(programByLineNum.Name);
                        programByLineNum.AdjustErrors(error);
                        item.Tag = programByLineNum;
                    }
                    else
                    {
                        item = this.lvErrors.Items.Add("");
                    }
                }
                if (item != null)
                {
                    item.SubItems.Add(error.Line.ToString());
                    item.SubItems.Add(error.Column.ToString());
                    item.SubItems.Add(error.ErrorNumber.ToString());
                    item.SubItems.Add(error.ErrorText);
                }
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            this.Compile();
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            FormulaProgram tag = (FormulaProgram) this.FormulaTree.SelectedNode.Tag;
            try
            {
                tag.Compile();
                this.lvErrors.Items.Clear();
                this.lvErrors.Items.Add("OK!");
            }
            catch (FormulaErrorException exception)
            {
                this.AddError(exception.ces, tag);
            }
        }

        private void cbIsMainView_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.DisableChange)
            {
                this.Modified = true;
            }
        }

        private bool Compile()
        {
            bool flag;
            try
            {
                long ticks = DateTime.Now.Ticks;
                this.SaveDataToNode();
                this.fs.Compile(this.Filename.Replace('.', '_') + ".dll", "");
                this.lvErrors.Items.Clear();
                this.lvErrors.Items.Add("OK! - " + ((DateTime.Now.Ticks - ticks) / 0x2710L) + "ms");
                flag = true;
            }
            catch (FormulaErrorException exception)
            {
                this.AddError(exception.ces, null);
                flag = false;
            }
            finally
            {
                this.LocateFirstProgram();
            }
            return flag;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private TreeNode FindNode(TreeNode tn, FormulaProgram fp)
        {
            if (tn.Tag == fp)
            {
                return tn;
            }
            foreach (TreeNode node in tn.Nodes)
            {
                TreeNode node2 = this.FindNode(node, fp);
                if (node2 != null)
                {
                    return node2;
                }
            }
            return null;
        }

        private TreeNode FindNode(TreeNode tn, string ProgramName)
        {
            if ((tn.Tag is FormulaProgram) && ((tn.Tag as FormulaProgram).Name == ProgramName))
            {
                return tn;
            }
            foreach (TreeNode node in tn.Nodes)
            {
                TreeNode node2 = this.FindNode(node, ProgramName);
                if (node2 != null)
                {
                    return node2;
                }
            }
            return null;
        }

        private void FormulaEditor_Activated(object sender, EventArgs e)
        {
            this.tbFormulaProgramCode.Focus();
        }

        private void FormulaEditor_Closing(object sender, CancelEventArgs e)
        {
            if (this.Modified)
            {
                e.Cancel = true;
            }
        }

        private void FormulaEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.Close();
            }
        }

        private void FormulaEditor_Load(object sender, EventArgs e)
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Length == 2)
            {
                this.LoadEditor(commandLineArgs[1], null);
            }
            else if (commandLineArgs.Length == 3)
            {
                this.LoadEditor(commandLineArgs[1], commandLineArgs[2]);
            }
        }

        private void FormulaTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.DisableChange = true;
            try
            {
                if (e.Node.Tag is FormulaSpace)
                {
                    FormulaSpace tag = (FormulaSpace) e.Node.Tag;
                    this.tbNamespaceName.Text = tag.Name;
                    this.tbNamespaceDesc.Text = tag.Description;
                    this.cbNotRealNameSpace.Checked = tag.GroupOnly;
                    this.tcFormula.SelectedTab = this.tpNamespace;
                    this.lOverride.Text = "Namespace properties:" + tag.Name;
                }
                else if (e.Node.Tag is FormulaProgram)
                {
                    FormulaProgram program = (FormulaProgram) e.Node.Tag;
                    this.tbFormulaProgramName.Text = program.Name;
                    this.tbProgramFullName.Text = program.FullName;
                    if (program.Code != null)
                    {
                        this.tbFormulaProgramCode.Lines = this.Trim(program.Code.Split(new char[] { '\n' }));
                    }
                    else
                    {
                        this.tbFormulaProgramCode.Text = "";
                    }
                    if (program.Description != null)
                    {
                        this.tbFormulaProgramDesc.Lines = this.Trim(program.Description.Split(new char[] { '\n' }));
                    }
                    else
                    {
                        this.tbFormulaProgramDesc.Text = "";
                    }
                    this.cbIsMainView.Checked = program.IsMainView;
                    this.tcFormula.SelectedTab = this.tpFormulaProgram;
                    this.lOverride.Text = "Formula script code:" + program.Name;
                }
                else if (e.Node.Tag is FormulaParam)
                {
                    FormulaParam param = (FormulaParam) e.Node.Tag;
                    this.tbParamName.Text = param.Name;
                    this.tbParamDesc.Text = param.Description;
                    this.tbMinValue.Text = param.MinValue;
                    this.tbMaxValue.Text = param.MaxValue;
                    this.tbDefaultValue.Text = param.DefaultValue;
                    int num = this.cbParamType.FindString(param.ParamType.ToString());
                    if (num >= 0)
                    {
                        this.cbParamType.SelectedIndex = num;
                    }
                    this.tcFormula.SelectedTab = this.tpParameter;
                    this.lOverride.Text = "Formula script parameter:" + param.Name;
                }
                this.FormulaTree.Focus();
                this.LastNode = e.Node;
            }
            finally
            {
                this.DisableChange = false;
            }
        }

        private void FormulaTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            this.SaveDataToNode(this.LastNode);
        }

        private void FormulaTree_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode nodeAt = this.FormulaTree.GetNodeAt(e.X, e.Y);
                if (nodeAt == null)
                {
                    nodeAt = this.FormulaTree.SelectedNode;
                }
                else
                {
                    this.FormulaTree.SelectedNode = nodeAt;
                }
                if (nodeAt.Tag is FormulaSpace)
                {
                    this.miAddFormulaProgram.Visible = true;
                    this.miAddNamespace.Visible = true;
                    this.miAddParam.Visible = false;
                }
                else if (nodeAt.Tag is FormulaProgram)
                {
                    this.miAddFormulaProgram.Visible = false;
                    this.miAddNamespace.Visible = false;
                    this.miAddParam.Visible = true;
                }
                else if (nodeAt.Tag is FormulaParam)
                {
                    this.miAddFormulaProgram.Visible = false;
                    this.miAddNamespace.Visible = false;
                    this.miAddParam.Visible = false;
                }
                this.cmTree.Show(this.FormulaTree, new Point(e.X, e.Y));
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(FormulaEditor));
            this.FormulaTree = new TreeView();
            this.ilFormula = new ImageList(this.components);
            this.tcFormula = new TabControl();
            this.tpNamespace = new TabPage();
            this.pnNamespace = new Panel();
            this.cbNotRealNameSpace = new CheckBox();
            this.lNamespaceDescription = new Label();
            this.tbNamespaceDesc = new TextBox();
            this.lNamespaceName = new Label();
            this.tbNamespaceName = new TextBox();
            this.tpFormulaProgram = new TabPage();
            this.spProgram = new Splitter();
            this.pnProgram = new Panel();
            this.btnCompile = new Button();
            this.lbIntelliSence = new ListBox();
            this.lFullName = new Label();
            this.tbProgramFullName = new TextBox();
            this.btnDebug = new Button();
            this.lFormulaProgramDesc = new Label();
            this.tbFormulaProgramDesc = new TextBox();
            this.cbIsMainView = new CheckBox();
            this.tbFormulaProgramName = new TextBox();
            this.lFormulaProgramName = new Label();
            this.lFormulaProgramCode = new Label();
            this.pnCode = new Panel();
            this.tbFormulaProgramCode = new RichTextBox();
            this.lvErrors = new ListView();
            this.HeaderName = new ColumnHeader();
            this.HeaderLine = new ColumnHeader();
            this.HeaderColumn = new ColumnHeader();
            this.HeaderNumber = new ColumnHeader();
            this.HeaderMessage = new ColumnHeader();
            this.tpParameter = new TabPage();
            this.cbParamType = new ComboBox();
            this.lParamType = new Label();
            this.tbParamDesc = new TextBox();
            this.lParamDesc = new Label();
            this.tbParamName = new TextBox();
            this.lParamName = new Label();
            this.tbMaxValue = new TextBox();
            this.tbMinValue = new TextBox();
            this.tbDefaultValue = new TextBox();
            this.lDefaultValue = new Label();
            this.lMaxValue = new Label();
            this.lMinValue = new Label();
            this.spFormula = new Splitter();
            this.mmFumular = new MainMenu();
            this.menuItem1 = new MenuItem();
            this.miNew = new MenuItem();
            this.miOpen = new MenuItem();
            this.miSave = new MenuItem();
            this.miSaveAs = new MenuItem();
            this.miCompile = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.miExit = new MenuItem();
            this.cmTree = new ContextMenu();
            this.miAddNamespace = new MenuItem();
            this.miAddFormulaProgram = new MenuItem();
            this.miAddParam = new MenuItem();
            this.miDeleteNode = new MenuItem();
            this.odFormula = new OpenFileDialog();
            this.sdFormula = new SaveFileDialog();
            this.pnFormula = new Panel();
            this.tcFormula.SuspendLayout();
            this.tpNamespace.SuspendLayout();
            this.pnNamespace.SuspendLayout();
            this.tpFormulaProgram.SuspendLayout();
            this.pnProgram.SuspendLayout();
            this.pnCode.SuspendLayout();
            this.tpParameter.SuspendLayout();
            this.pnFormula.SuspendLayout();
            base.SuspendLayout();
            this.FormulaTree.BorderStyle = BorderStyle.FixedSingle;
            this.FormulaTree.Dock = DockStyle.Left;
            this.FormulaTree.FullRowSelect = true;
            this.FormulaTree.HideSelection = false;
            this.FormulaTree.ImageList = this.ilFormula;
            this.FormulaTree.Location = new Point(0, 0);
            this.FormulaTree.Name = "FormulaTree";
            this.FormulaTree.Size = new Size(200, 0x229);
            this.FormulaTree.TabIndex = 0;
            this.FormulaTree.MouseDown += new MouseEventHandler(this.FormulaTree_MouseDown);
            this.FormulaTree.AfterSelect += new TreeViewEventHandler(this.FormulaTree_AfterSelect);
            this.FormulaTree.BeforeSelect += new TreeViewCancelEventHandler(this.FormulaTree_BeforeSelect);
            this.ilFormula.ImageSize = new Size(0x10, 0x10);
            this.ilFormula.ImageStream = (ImageListStreamer) manager.GetObject("ilFormula.ImageStream");
            this.ilFormula.TransparentColor = Color.Transparent;
            this.tcFormula.AllowDrop = true;
            this.tcFormula.Appearance = TabAppearance.FlatButtons;
            this.tcFormula.Controls.Add(this.tpNamespace);
            this.tcFormula.Controls.Add(this.tpFormulaProgram);
            this.tcFormula.Controls.Add(this.tpParameter);
            this.tcFormula.Dock = DockStyle.Fill;
            this.tcFormula.ItemSize = new Size(1, 0);
            this.tcFormula.Location = new Point(0, 0);
            this.tcFormula.Name = "tcFormula";
            this.tcFormula.SelectedIndex = 0;
            this.tcFormula.Size = new Size(540, 0x229);
            this.tcFormula.TabIndex = 4;
            this.tcFormula.TabStop = false;
            this.tpNamespace.Controls.Add(this.pnNamespace);
            this.tpNamespace.Location = new Point(4, 0x1a);
            this.tpNamespace.Name = "tpNamespace";
            this.tpNamespace.Size = new Size(0x214, 0x20b);
            this.tpNamespace.TabIndex = 0;
            this.tpNamespace.Text = "Namespace";
            this.pnNamespace.Controls.Add(this.cbNotRealNameSpace);
            this.pnNamespace.Controls.Add(this.lNamespaceDescription);
            this.pnNamespace.Controls.Add(this.tbNamespaceDesc);
            this.pnNamespace.Controls.Add(this.lNamespaceName);
            this.pnNamespace.Controls.Add(this.tbNamespaceName);
            this.pnNamespace.Dock = DockStyle.Fill;
            this.pnNamespace.Location = new Point(0, 0);
            this.pnNamespace.Name = "pnNamespace";
            this.pnNamespace.Size = new Size(0x214, 0x20b);
            this.pnNamespace.TabIndex = 3;
            this.cbNotRealNameSpace.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbNotRealNameSpace.Location = new Point(0x10, 0x1e8);
            this.cbNotRealNameSpace.Name = "cbNotRealNameSpace";
            this.cbNotRealNameSpace.Size = new Size(0x1e8, 0x18);
            this.cbNotRealNameSpace.TabIndex = 7;
            this.cbNotRealNameSpace.Text = "Group Only";
            this.lNamespaceDescription.BackColor = SystemColors.Control;
            this.lNamespaceDescription.Location = new Point(20, 0x30);
            this.lNamespaceDescription.Name = "lNamespaceDescription";
            this.lNamespaceDescription.Size = new Size(80, 0x10);
            this.lNamespaceDescription.TabIndex = 6;
            this.lNamespaceDescription.Text = "Description";
            this.tbNamespaceDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tbNamespaceDesc.BackColor = SystemColors.Info;
            this.tbNamespaceDesc.BorderStyle = BorderStyle.FixedSingle;
            this.tbNamespaceDesc.Location = new Point(0x6c, 0x30);
            this.tbNamespaceDesc.Multiline = true;
            this.tbNamespaceDesc.Name = "tbNamespaceDesc";
            this.tbNamespaceDesc.Size = new Size(0x194, 0x1a8);
            this.tbNamespaceDesc.TabIndex = 5;
            this.tbNamespaceDesc.Text = "";
            this.lNamespaceName.AccessibleName = "";
            this.lNamespaceName.BackColor = SystemColors.Control;
            this.lNamespaceName.Location = new Point(0x15, 0x10);
            this.lNamespaceName.Name = "lNamespaceName";
            this.lNamespaceName.Size = new Size(0x48, 14);
            this.lNamespaceName.TabIndex = 4;
            this.lNamespaceName.Text = "Name";
            this.tbNamespaceName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbNamespaceName.BorderStyle = BorderStyle.FixedSingle;
            this.tbNamespaceName.Location = new Point(0x6c, 0x10);
            this.tbNamespaceName.Name = "tbNamespaceName";
            this.tbNamespaceName.Size = new Size(0x194, 0x16);
            this.tbNamespaceName.TabIndex = 3;
            this.tbNamespaceName.Text = "";
            this.tpFormulaProgram.Controls.Add(this.spProgram);
            this.tpFormulaProgram.Controls.Add(this.pnProgram);
            this.tpFormulaProgram.Controls.Add(this.lvErrors);
            this.tpFormulaProgram.Location = new Point(4, 0x1a);
            this.tpFormulaProgram.Name = "tpFormulaProgram";
            this.tpFormulaProgram.Size = new Size(0x214, 0x20b);
            this.tpFormulaProgram.TabIndex = 1;
            this.tpFormulaProgram.Text = "FormulaProgram";
            this.spProgram.Dock = DockStyle.Bottom;
            this.spProgram.Location = new Point(0, 0x1c0);
            this.spProgram.MinExtra = 300;
            this.spProgram.MinSize = 0;
            this.spProgram.Name = "spProgram";
            this.spProgram.Size = new Size(0x214, 3);
            this.spProgram.TabIndex = 14;
            this.spProgram.TabStop = false;
            this.pnProgram.Controls.Add(this.btnCompile);
            this.pnProgram.Controls.Add(this.lbIntelliSence);
            this.pnProgram.Controls.Add(this.lFullName);
            this.pnProgram.Controls.Add(this.tbProgramFullName);
            this.pnProgram.Controls.Add(this.btnDebug);
            this.pnProgram.Controls.Add(this.lFormulaProgramDesc);
            this.pnProgram.Controls.Add(this.tbFormulaProgramDesc);
            this.pnProgram.Controls.Add(this.cbIsMainView);
            this.pnProgram.Controls.Add(this.tbFormulaProgramName);
            this.pnProgram.Controls.Add(this.lFormulaProgramName);
            this.pnProgram.Controls.Add(this.lFormulaProgramCode);
            this.pnProgram.Controls.Add(this.pnCode);
            this.pnProgram.Dock = DockStyle.Fill;
            this.pnProgram.Location = new Point(0, 0);
            this.pnProgram.Name = "pnProgram";
            this.pnProgram.Size = new Size(0x214, 0x1c3);
            this.pnProgram.TabIndex = 13;
            this.btnCompile.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnCompile.Location = new Point(320, 420);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.TabIndex = 0x17;
            this.btnCompile.Text = "&Compile";
            this.btnCompile.Click += new EventHandler(this.btnCompile_Click);
            this.lbIntelliSence.BorderStyle = BorderStyle.FixedSingle;
            this.lbIntelliSence.ItemHeight = 14;
            this.lbIntelliSence.Location = new Point(0x20, 0x88);
            this.lbIntelliSence.Name = "lbIntelliSence";
            this.lbIntelliSence.Size = new Size(0xe0, 0x8e);
            this.lbIntelliSence.TabIndex = 0x16;
            this.lbIntelliSence.TabStop = false;
            this.lbIntelliSence.Visible = false;
            this.lbIntelliSence.KeyDown += new KeyEventHandler(this.lbIntelliSence_KeyDown);
            this.lFullName.Location = new Point(13, 0x29);
            this.lFullName.Name = "lFullName";
            this.lFullName.Size = new Size(0x48, 0x17);
            this.lFullName.TabIndex = 0x15;
            this.lFullName.Text = "Full Name:";
            this.tbProgramFullName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbProgramFullName.BackColor = Color.SeaShell;
            this.tbProgramFullName.BorderStyle = BorderStyle.FixedSingle;
            this.tbProgramFullName.Location = new Point(0x65, 0x27);
            this.tbProgramFullName.Name = "tbProgramFullName";
            this.tbProgramFullName.Size = new Size(0x1a3, 0x16);
            this.tbProgramFullName.TabIndex = 13;
            this.tbProgramFullName.Text = "";
            this.btnDebug.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnDebug.Location = new Point(0xe8, 420);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.TabIndex = 0x11;
            this.btnDebug.Text = "&Debug";
            this.btnDebug.Click += new EventHandler(this.btnDebug_Click);
            this.lFormulaProgramDesc.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lFormulaProgramDesc.Location = new Point(8, 0x128);
            this.lFormulaProgramDesc.Name = "lFormulaProgramDesc";
            this.lFormulaProgramDesc.Size = new Size(80, 0x17);
            this.lFormulaProgramDesc.TabIndex = 0x12;
            this.lFormulaProgramDesc.Text = "Description:";
            this.tbFormulaProgramDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tbFormulaProgramDesc.BorderStyle = BorderStyle.FixedSingle;
            this.tbFormulaProgramDesc.Location = new Point(100, 0x12b);
            this.tbFormulaProgramDesc.Multiline = true;
            this.tbFormulaProgramDesc.Name = "tbFormulaProgramDesc";
            this.tbFormulaProgramDesc.Size = new Size(0x1a3, 0x70);
            this.tbFormulaProgramDesc.TabIndex = 15;
            this.tbFormulaProgramDesc.Text = "";
            this.tbFormulaProgramDesc.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.cbIsMainView.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbIsMainView.FlatStyle = FlatStyle.Flat;
            this.cbIsMainView.Location = new Point(0x10, 0x1a3);
            this.cbIsMainView.Name = "cbIsMainView";
            this.cbIsMainView.TabIndex = 0x10;
            this.cbIsMainView.Text = "Main View";
            this.cbIsMainView.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.tbFormulaProgramName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbFormulaProgramName.BorderStyle = BorderStyle.FixedSingle;
            this.tbFormulaProgramName.Location = new Point(0x65, 8);
            this.tbFormulaProgramName.Name = "tbFormulaProgramName";
            this.tbFormulaProgramName.Size = new Size(0x1a3, 0x16);
            this.tbFormulaProgramName.TabIndex = 12;
            this.tbFormulaProgramName.Text = "";
            this.tbFormulaProgramName.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.lFormulaProgramName.Location = new Point(13, 10);
            this.lFormulaProgramName.Name = "lFormulaProgramName";
            this.lFormulaProgramName.Size = new Size(0x48, 0x17);
            this.lFormulaProgramName.TabIndex = 0x11;
            this.lFormulaProgramName.Text = "Name:";
            this.lFormulaProgramCode.Location = new Point(13, 0x48);
            this.lFormulaProgramCode.Name = "lFormulaProgramCode";
            this.lFormulaProgramCode.Size = new Size(0x48, 0x17);
            this.lFormulaProgramCode.TabIndex = 0x10;
            this.lFormulaProgramCode.Text = "Code:";
            this.pnCode.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnCode.BorderStyle = BorderStyle.FixedSingle;
            this.pnCode.Controls.Add(this.tbFormulaProgramCode);
            this.pnCode.Location = new Point(0x65, 0x48);
            this.pnCode.Name = "pnCode";
            this.pnCode.Size = new Size(0x1a3, 0xd8);
            this.pnCode.TabIndex = 14;
            this.pnCode.TabStop = true;
            this.tbFormulaProgramCode.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tbFormulaProgramCode.BackColor = SystemColors.Info;
            this.tbFormulaProgramCode.BorderStyle = BorderStyle.None;
            this.tbFormulaProgramCode.Location = new Point(0, 0);
            this.tbFormulaProgramCode.Name = "tbFormulaProgramCode";
            this.tbFormulaProgramCode.Size = new Size(0x1a1, 0xd6);
            this.tbFormulaProgramCode.TabIndex = 14;
            this.tbFormulaProgramCode.Text = "";
            this.tbFormulaProgramCode.KeyDown += new KeyEventHandler(this.tbFormulaProgramCode_KeyDown);
            this.tbFormulaProgramCode.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.lvErrors.BorderStyle = BorderStyle.FixedSingle;
            this.lvErrors.Columns.AddRange(new ColumnHeader[] { this.HeaderName, this.HeaderLine, this.HeaderColumn, this.HeaderNumber, this.HeaderMessage });
            this.lvErrors.Dock = DockStyle.Bottom;
            this.lvErrors.FullRowSelect = true;
            this.lvErrors.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvErrors.Location = new Point(0, 0x1c3);
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.Size = new Size(0x214, 0x48);
            this.lvErrors.TabIndex = 0x12;
            this.lvErrors.View = View.Details;
            this.lvErrors.DoubleClick += new EventHandler(this.lvErrors_DoubleClick);
            this.HeaderName.Text = "Name";
            this.HeaderName.Width = 80;
            this.HeaderLine.Text = "Line";
            this.HeaderColumn.Text = "Column";
            this.HeaderNumber.Text = "Number";
            this.HeaderNumber.Width = 80;
            this.HeaderMessage.Text = "Message";
            this.HeaderMessage.Width = 200;
            this.tpParameter.Controls.Add(this.cbParamType);
            this.tpParameter.Controls.Add(this.lParamType);
            this.tpParameter.Controls.Add(this.tbParamDesc);
            this.tpParameter.Controls.Add(this.lParamDesc);
            this.tpParameter.Controls.Add(this.tbParamName);
            this.tpParameter.Controls.Add(this.lParamName);
            this.tpParameter.Controls.Add(this.tbMaxValue);
            this.tpParameter.Controls.Add(this.tbMinValue);
            this.tpParameter.Controls.Add(this.tbDefaultValue);
            this.tpParameter.Controls.Add(this.lDefaultValue);
            this.tpParameter.Controls.Add(this.lMaxValue);
            this.tpParameter.Controls.Add(this.lMinValue);
            this.tpParameter.Location = new Point(4, 0x1a);
            this.tpParameter.Name = "tpParameter";
            this.tpParameter.Size = new Size(0x214, 0x20b);
            this.tpParameter.TabIndex = 2;
            this.tpParameter.Text = "Parameter";
            this.cbParamType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbParamType.Items.AddRange(new object[] { "double", "string" });
            this.cbParamType.Location = new Point(0x60, 0x88);
            this.cbParamType.Name = "cbParamType";
            this.cbParamType.Size = new Size(0x79, 0x15);
            this.cbParamType.TabIndex = 10;
            this.lParamType.Location = new Point(8, 0x88);
            this.lParamType.Name = "lParamType";
            this.lParamType.Size = new Size(0x58, 0x17);
            this.lParamType.TabIndex = 9;
            this.lParamType.Text = "Type:";
            this.tbParamDesc.BorderStyle = BorderStyle.FixedSingle;
            this.tbParamDesc.Location = new Point(0x60, 0xa8);
            this.tbParamDesc.Multiline = true;
            this.tbParamDesc.Name = "tbParamDesc";
            this.tbParamDesc.Size = new Size(0x138, 0x68);
            this.tbParamDesc.TabIndex = 5;
            this.tbParamDesc.Text = "";
            this.tbParamDesc.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.lParamDesc.Location = new Point(8, 0xa8);
            this.lParamDesc.Name = "lParamDesc";
            this.lParamDesc.Size = new Size(0x58, 0x17);
            this.lParamDesc.TabIndex = 8;
            this.lParamDesc.Text = "Description:";
            this.tbParamName.BorderStyle = BorderStyle.FixedSingle;
            this.tbParamName.Location = new Point(0x60, 8);
            this.tbParamName.Name = "tbParamName";
            this.tbParamName.Size = new Size(0x88, 0x16);
            this.tbParamName.TabIndex = 0;
            this.tbParamName.Text = "";
            this.tbParamName.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.lParamName.Location = new Point(8, 8);
            this.lParamName.Name = "lParamName";
            this.lParamName.Size = new Size(80, 0x17);
            this.lParamName.TabIndex = 6;
            this.lParamName.Text = "Name:";
            this.tbMaxValue.BackColor = SystemColors.Info;
            this.tbMaxValue.BorderStyle = BorderStyle.FixedSingle;
            this.tbMaxValue.Location = new Point(0x60, 0x68);
            this.tbMaxValue.Name = "tbMaxValue";
            this.tbMaxValue.Size = new Size(0x88, 0x16);
            this.tbMaxValue.TabIndex = 3;
            this.tbMaxValue.Text = "";
            this.tbMaxValue.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.tbMinValue.BackColor = SystemColors.Info;
            this.tbMinValue.BorderStyle = BorderStyle.FixedSingle;
            this.tbMinValue.Location = new Point(0x60, 0x48);
            this.tbMinValue.Name = "tbMinValue";
            this.tbMinValue.Size = new Size(0x88, 0x16);
            this.tbMinValue.TabIndex = 2;
            this.tbMinValue.Text = "";
            this.tbMinValue.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.tbDefaultValue.BackColor = SystemColors.Info;
            this.tbDefaultValue.BorderStyle = BorderStyle.FixedSingle;
            this.tbDefaultValue.Location = new Point(0x60, 40);
            this.tbDefaultValue.Name = "tbDefaultValue";
            this.tbDefaultValue.Size = new Size(0x88, 0x16);
            this.tbDefaultValue.TabIndex = 1;
            this.tbDefaultValue.Text = "";
            this.tbDefaultValue.Leave += new EventHandler(this.tbFormulaProgramCode_Leave);
            this.lDefaultValue.Location = new Point(8, 40);
            this.lDefaultValue.Name = "lDefaultValue";
            this.lDefaultValue.Size = new Size(80, 0x17);
            this.lDefaultValue.TabIndex = 2;
            this.lDefaultValue.Text = "Default:";
            this.lMaxValue.Location = new Point(8, 0x68);
            this.lMaxValue.Name = "lMaxValue";
            this.lMaxValue.Size = new Size(80, 0x17);
            this.lMaxValue.TabIndex = 1;
            this.lMaxValue.Text = "Maximum:";
            this.lMinValue.Location = new Point(8, 0x48);
            this.lMinValue.Name = "lMinValue";
            this.lMinValue.Size = new Size(80, 0x17);
            this.lMinValue.TabIndex = 0;
            this.lMinValue.Text = "Minimum:";
            this.spFormula.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.spFormula.Location = new Point(200, 0);
            this.spFormula.MinExtra = 400;
            this.spFormula.MinSize = 150;
            this.spFormula.Name = "spFormula";
            this.spFormula.Size = new Size(4, 0x229);
            this.spFormula.TabIndex = 5;
            this.spFormula.TabStop = false;
            this.mmFumular.MenuItems.AddRange(new MenuItem[] { this.menuItem1 });
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new MenuItem[] { this.miNew, this.miOpen, this.miSave, this.miSaveAs, this.miCompile, this.menuItem4, this.miExit });
            this.menuItem1.Text = "&File";
            this.miNew.Index = 0;
            this.miNew.Text = "&New";
            this.miNew.Click += new EventHandler(this.miNew_Click);
            this.miOpen.Index = 1;
            this.miOpen.Shortcut = Shortcut.F3;
            this.miOpen.Text = "&Open";
            this.miOpen.Click += new EventHandler(this.miOpen_Click);
            this.miSave.Index = 2;
            this.miSave.Shortcut = Shortcut.F2;
            this.miSave.Text = "&Save";
            this.miSave.Click += new EventHandler(this.miSave_Click);
            this.miSaveAs.Index = 3;
            this.miSaveAs.Text = "Save &As";
            this.miSaveAs.Click += new EventHandler(this.miSaveAs_Click);
            this.miCompile.Index = 4;
            this.miCompile.Shortcut = Shortcut.F9;
            this.miCompile.Text = "&Compile";
            this.miCompile.Click += new EventHandler(this.miCompile_Click);
            this.menuItem4.Index = 5;
            this.menuItem4.Text = "-";
            this.miExit.Index = 6;
            this.miExit.Text = "&Exit";
            this.miExit.Click += new EventHandler(this.miExit_Click);
            this.cmTree.MenuItems.AddRange(new MenuItem[] { this.miAddNamespace, this.miAddFormulaProgram, this.miAddParam, this.miDeleteNode });
            this.miAddNamespace.Index = 0;
            this.miAddNamespace.Text = "Add Namespace";
            this.miAddNamespace.Click += new EventHandler(this.miAddNamespace_Click);
            this.miAddFormulaProgram.Index = 1;
            this.miAddFormulaProgram.Text = "Add Formula FormulaProgram";
            this.miAddFormulaProgram.Click += new EventHandler(this.miAddFormulaProgram_Click);
            this.miAddParam.Index = 2;
            this.miAddParam.Text = "Add Formula Parameter";
            this.miAddParam.Click += new EventHandler(this.miAddParam_Click);
            this.miDeleteNode.Index = 3;
            this.miDeleteNode.Text = "Delete Node";
            this.miDeleteNode.Click += new EventHandler(this.miDeleteNode_Click);
            this.odFormula.DefaultExt = "fml";
            this.odFormula.Filter = "Formula File(*.fml)|*.fml|All files (*.*)|*.*";
            this.odFormula.RestoreDirectory = true;
            this.sdFormula.DefaultExt = "fml";
            this.sdFormula.Filter = "Formula File(*.fml)|*.fml|All files (*.*)|*.*";
            this.sdFormula.RestoreDirectory = true;
            this.pnFormula.Controls.Add(this.tcFormula);
            this.pnFormula.Dock = DockStyle.Fill;
            this.pnFormula.Location = new Point(0xcc, 0);
            this.pnFormula.Name = "pnFormula";
            this.pnFormula.Size = new Size(540, 0x229);
            this.pnFormula.TabIndex = 6;
            this.AutoScaleBaseSize = new Size(7, 15);
            base.ClientSize = new Size(0x2e8, 0x229);
            base.Controls.Add(this.pnFormula);
            base.Controls.Add(this.spFormula);
            base.Controls.Add(this.FormulaTree);
            this.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.KeyPreview = true;
            base.Menu = this.mmFumular;
            base.MinimumSize = new Size(640, 480);
            base.Name = "FormulaEditor";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Formula Editor";
            base.KeyDown += new KeyEventHandler(this.FormulaEditor_KeyDown);
            base.Click += new EventHandler(this.btnDebug_Click);
            base.Closing += new CancelEventHandler(this.FormulaEditor_Closing);
            base.Load += new EventHandler(this.FormulaEditor_Load);
            base.Activated += new EventHandler(this.FormulaEditor_Activated);
            this.tcFormula.ResumeLayout(false);
            this.tpNamespace.ResumeLayout(false);
            this.pnNamespace.ResumeLayout(false);
            this.tpFormulaProgram.ResumeLayout(false);
            this.pnProgram.ResumeLayout(false);
            this.pnCode.ResumeLayout(false);
            this.tpParameter.ResumeLayout(false);
            this.pnFormula.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lbIntelliSence_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.lbIntelliSence.Visible = false;
            }
            else if (e.KeyCode == Keys.Return)
            {
                Clipboard.SetDataObject(this.lbIntelliSence.SelectedItem.ToString());
                this.tbFormulaProgramCode.Paste();
                this.lbIntelliSence.Visible = false;
            }
        }

        private void LoadEditor(string Filename, string Formula)
        {
            if (File.Exists(Filename))
            {
                this.LoadFromFile(Filename);
                if (Formula != null)
                {
                    int num = Formula.LastIndexOf('.');
                    if (num >= 0)
                    {
                        Formula = Formula.Substring(num + 1);
                    }
                    TreeNode node = this.FindNode(this.FormulaTree.Nodes[0], Formula);
                    if (node != null)
                    {
                        this.FormulaTree.SelectedNode = node;
                        this.FormulaTree.SelectedNode.Expand();
                    }
                }
            }
            base.ShowDialog();
        }

        private void LoadFromFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                this.fs = FormulaSpace.Read(FileName);
                this.Filename = FileName;
                this.sdFormula.FileName = this.Filename;
                this.LoadToTree(this.fs);
                this.Modified = false;
                this.FormulaTree.Nodes[0].Expand();
            }
        }

        private void LoadToTree(FormulaSpace fs)
        {
            this.FormulaTree.BeginUpdate();
            try
            {
                this.FormulaTree.Nodes.Clear();
                TreeNode node = new TreeNode("FML", 0, 0);
                this.FormulaTree.Nodes.Add(node);
                this.LoadToTree(this.FormulaTree.Nodes[0], fs);
            }
            finally
            {
                this.FormulaTree.EndUpdate();
            }
            this.FormulaTree.SelectedNode = this.FormulaTree.TopNode;
        }

        private void LoadToTree(TreeNode tn, FormulaParam fp)
        {
            tn.Text = fp.Name;
            tn.Tag = fp;
        }

        private void LoadToTree(TreeNode tn, FormulaProgram p)
        {
            tn.Text = p.Name;
            tn.Tag = p;
            foreach (FormulaParam param in p.Params)
            {
                TreeNode node = new TreeNode();
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
                tn.Nodes.Add(node);
                this.LoadToTree(node, param);
            }
        }

        private void LoadToTree(TreeNode tn, FormulaSpace fs)
        {
            tn.Text = fs.Name;
            tn.Tag = fs;
            foreach (FormulaSpace space in fs.Namespaces)
            {
                TreeNode node = new TreeNode();
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
                tn.Nodes.Add(node);
                this.LoadToTree(node, space);
            }
            foreach (FormulaProgram program in fs.Programs)
            {
                TreeNode node2 = new TreeNode();
                node2.ImageIndex = 1;
                node2.SelectedImageIndex = 1;
                tn.Nodes.Add(node2);
                this.LoadToTree(node2, program);
            }
        }

        private void LocateFirstProgram()
        {
            TreeNode selectedNode = this.FormulaTree.SelectedNode;
            if (!(selectedNode.Tag is FormulaProgram))
            {
                selectedNode = this.FormulaTree.Nodes[0];
                while (!(selectedNode.Tag is FormulaProgram) && (selectedNode.Nodes.Count > 0))
                {
                    selectedNode = selectedNode.Nodes[0];
                }
            }
            this.FormulaTree.SelectedNode = selectedNode;
        }

        private void lvErrors_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem focusedItem = this.lvErrors.FocusedItem;
            if (focusedItem != null)
            {
                FormulaProgram tag = (FormulaProgram) focusedItem.Tag;
                if (tag != null)
                {
                    TreeNode node = this.FindNode(this.FormulaTree.Nodes[0], tag);
                    if (node != null)
                    {
                        this.FormulaTree.SelectedNode = node;
                        int num = int.Parse(focusedItem.SubItems[1].Text);
                        int num2 = int.Parse(focusedItem.SubItems[2].Text) - 1;
                        for (int i = 0; (i < (num - 1)) && (i < this.tbFormulaProgramCode.Lines.Length); i++)
                        {
                            num2 += this.tbFormulaProgramCode.Lines[i].Length + 2;
                        }
                        if (num2 < 0)
                        {
                            num2 = 0;
                        }
                        this.tbFormulaProgramCode.SelectionStart = num2;
                        this.tbFormulaProgramCode.Focus();
                    }
                }
            }
        }

        private void miAddFormulaProgram_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.FormulaTree.SelectedNode;
            FormulaSpace tag = (FormulaSpace) selectedNode.Tag;
            FormulaProgram program = new FormulaProgram();
            tag.Programs.Add(program);
            TreeNode node = new TreeNode("NewCode");
            program.Name = node.Text;
            selectedNode.Nodes.Add(node);
            node.ImageIndex = 1;
            node.SelectedImageIndex = 1;
            node.Tag = program;
            this.FormulaTree.SelectedNode = node;
        }

        private void miAddNamespace_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.FormulaTree.SelectedNode;
            FormulaSpace tag = (FormulaSpace) selectedNode.Tag;
            FormulaSpace space2 = new FormulaSpace();
            tag.Namespaces.Add(space2);
            TreeNode node = new TreeNode("NewSpace");
            space2.Name = node.Text;
            selectedNode.Nodes.Add(node);
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            node.Tag = space2;
            this.FormulaTree.SelectedNode = node;
        }

        private void miAddParam_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.FormulaTree.SelectedNode;
            FormulaProgram tag = (FormulaProgram) selectedNode.Tag;
            FormulaParam fp = new FormulaParam();
            tag.Params.Add(fp);
            TreeNode node = new TreeNode("NewParam");
            fp.Name = node.Text;
            selectedNode.Nodes.Add(node);
            node.ImageIndex = 2;
            node.SelectedImageIndex = 2;
            node.Tag = fp;
            this.FormulaTree.SelectedNode = node;
        }

        private void miCompile_Click(object sender, EventArgs e)
        {
            this.Compile();
        }

        private void miDeleteNode_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.FormulaTree.SelectedNode;
            TreeNode parent = selectedNode.Parent;
            if (parent != null)
            {
                object tag = parent.Tag;
                object obj3 = selectedNode.Tag;
                if (tag is FormulaSpace)
                {
                    if (obj3 is FormulaSpace)
                    {
                        ((FormulaSpace) tag).Namespaces.Remove((FormulaSpace) obj3);
                    }
                    else if (obj3 is FormulaProgram)
                    {
                        ((FormulaSpace) tag).Programs.Remove((FormulaProgram) obj3);
                    }
                }
                else if ((tag is FormulaProgram) && (obj3 is FormulaParam))
                {
                    ((FormulaProgram) tag).Params.Remove((FormulaParam) obj3);
                }
                selectedNode.Remove();
            }
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            if (!this.Modified)
            {
                this.Filename = "";
                this.fs = new FormulaSpace("FML");
                this.LoadToTree(this.fs);
            }
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            if (!this.Modified && (this.odFormula.ShowDialog() == DialogResult.OK))
            {
                try
                {
                    this.LoadFromFile(this.odFormula.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        public static void Open(string Filename, string Formula)
        {
            if (CurrentEditor == null)
            {
                CurrentEditor = new FormulaEditor();
            }
            CurrentEditor.LoadEditor(Filename, Formula);
        }

        private bool Save()
        {
            if (this.Filename == "")
            {
                return this.SaveAs();
            }
            try
            {
                if (this.Compile())
                {
                    this.SaveDataToNode();
                    this.fs.Write(this.Filename);
                    this.Modified = false;
                    return true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return false;
        }

        public bool SaveAs()
        {
            if (this.sdFormula.ShowDialog() == DialogResult.OK)
            {
                this.SaveDataToNode();
                this.Filename = this.sdFormula.FileName;
                this.odFormula.FileName = this.Filename;
                try
                {
                    if (this.Compile())
                    {
                        this.fs.Write(this.Filename);
                        this.Modified = false;
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            return false;
        }

        private void SaveDataToNode()
        {
            this.SaveDataToNode(this.FormulaTree.SelectedNode);
        }

        private void SaveDataToNode(TreeNode tn)
        {
            if (tn != null)
            {
                if (tn.Tag is FormulaProgram)
                {
                    FormulaProgram tag = tn.Tag as FormulaProgram;
                    tag.Name = this.tbFormulaProgramName.Text;
                    tag.FullName = this.tbProgramFullName.Text;
                    tag.Code = this.tbFormulaProgramCode.Text;
                    tag.Description = this.tbFormulaProgramDesc.Text;
                    tag.IsMainView = this.cbIsMainView.Checked;
                    tn.Text = tag.Name;
                }
                else if (tn.Tag is FormulaSpace)
                {
                    FormulaSpace space = tn.Tag as FormulaSpace;
                    space.Name = this.tbNamespaceName.Text;
                    space.Description = this.tbNamespaceDesc.Text;
                    space.GroupOnly = this.cbNotRealNameSpace.Checked;
                    tn.Text = space.Name;
                }
                else if (tn.Tag is FormulaParam)
                {
                    FormulaParam param = tn.Tag as FormulaParam;
                    param.Name = this.tbParamName.Text;
                    param.MinValue = this.tbMinValue.Text;
                    param.MaxValue = this.tbMaxValue.Text;
                    param.DefaultValue = this.tbDefaultValue.Text;
                    if (this.cbParamType.Text == "")
                    {
                        this.cbParamType.SelectedIndex = 0;
                    }
                    param.ParamType = (FormulaParamType)Enum.Parse(typeof(FormulaParamType), this.cbParamType.SelectedItem.ToString());
                    param.Description = this.tbParamDesc.Text;
                    tn.Text = param.Name;
                }
            }
        }

        private void tbFormulaProgramCode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.J) && e.Control)
            {
                this.lbIntelliSence.Items.Clear();
                this.lbIntelliSence.Items.AddRange(FormulaBase.GetAllFormulas());
                int selectionStart = this.tbFormulaProgramCode.SelectionStart;
                int lineFromCharIndex = this.tbFormulaProgramCode.GetLineFromCharIndex(selectionStart);
                Point location = this.pnCode.Location;
                Point positionFromCharIndex = this.tbFormulaProgramCode.GetPositionFromCharIndex(selectionStart);
                positionFromCharIndex.Offset(location.X + 8, location.Y + 14);
                this.lbIntelliSence.Location = positionFromCharIndex;
                this.lbIntelliSence.Visible = true;
                this.lbIntelliSence.Focus();
            }
        }

        private void tbFormulaProgramCode_Leave(object sender, EventArgs e)
        {
            this.SaveDataToNode();
        }

        private string[] Trim(string[] ss)
        {
            for (int i = 0; i < ss.Length; i++)
            {
                ss[i] = ss[i].Trim();
            }
            return ss;
        }

        public string Filename
        {
            get
            {
                return this.filename;
            }
            set
            {
                this.filename = value;
                this.Text = this.filename;
            }
        }

        private bool Modified
        {
            get
            {
                if (this.modified)
                {
                    switch (MessageBox.Show("Text modified. Save modifications ?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            return !this.Save();

                        case DialogResult.No:
                            return false;
                    }
                    return true;
                }
                return false;
            }
            set
            {
                if (this.modified != value)
                {
                    this.modified = value;
                    if (this.modified)
                    {
                        if (!this.Text.EndsWith("*"))
                        {
                            this.Text = this.Text + "*";
                        }
                    }
                    else
                    {
                        this.Text = this.Filename;
                    }
                }
            }
        }
    }
}

