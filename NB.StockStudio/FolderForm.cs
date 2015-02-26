using NB.StockStudio.WinControls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace NB.StockStudio
{
	public class FolderForm : Form
	{
		// Methods
		public FolderForm()
		{
			this.components = null;
			this.InitializeComponent();
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			string text1 = InputBox.ShowInputBox("Folder name:", "Name");
			if (this.CanCreate)
			{
				int num1 = (int) this.CurrentRow["FolderId"];
				StockDB.LoadFolderRow(num1, text1);
				StockDB.ResetFolderDatabase();
				ListForm.Current.FolderId = StockDB.GetMaxFolderId();
			}
			this.CreateTree();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.CanDelete)
			{
				int num1 = (int) this.CurrentRow["ParentId"];
				int num2 = (int) this.CurrentRow["FolderId"];
				StockDB.DeleteFolderRows("FolderId=" + num2);
				StockDB.DeleteFolderRelRows("FolderId=" + num2);
				StockDB.ResetFolderDatabase();
				num2 = StockDB.GetMaxFolderId(num1.ToString());
				if (num2 == 0)
				{
					num2 = num1;
				}
				ListForm.Current.FolderId = num2;
				this.CreateTree();
			}
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (this.tvFolder.SelectedNode != null)
			{
				this.tvFolder.SelectedNode.BeginEdit();
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void CreateTree()
		{
			this.tvFolder.BeginUpdate();
			this.Updating = true;
			try
			{
				Hashtable hashtable1 = new Hashtable();
				this.tvFolder.Nodes.Clear();
				DataTable table1 = StockDB.GetFolderDatatable();
				foreach (DataRow row1 in table1.Rows)
				{
					if (((int) row1["Visible"]) != 1)
					{
						continue;
					}
					TreeNode node1 = new TreeNode(row1["FolderName"].ToString());
					int num1 = (int) row1["FolderId"];
					int num2 = (int) row1["ParentId"];
					hashtable1[num1] = node1;
					node1.Tag = row1;
					if (num2 == 0)
					{
						this.tvFolder.Nodes.Add(node1);
					}
					else
					{
						TreeNode node2 = (TreeNode) hashtable1[num2];
						if (node2 != null)
						{
							node1.ImageIndex = 1;
							node1.SelectedImageIndex = 1;
							node2.Nodes.Add(node1);
						}
					}
					if (ListForm.Current.FolderId == num1)
					{
						this.tvFolder.SelectedNode = node1;
						this.CurrentRow = (DataRow) node1.Tag;
					}
				}
			}
			finally
			{
				this.tvFolder.EndUpdate();
				this.Updating = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FolderForm_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Return))
			{
				base.Close();
			}
		}

		private void FolderForm_Load(object sender, EventArgs e)
		{
			this.CreateTree();
			this.SetEnable();
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager1 = new ResourceManager(typeof(FolderForm));
			this.tvFolder = new TreeView();
			this.ilFolder = new ImageList(this.components);
			this.btnOK = new Button();
			this.btnCreate = new Button();
			this.btnDelete = new Button();
			this.btnEdit = new Button();
			base.SuspendLayout();
			this.tvFolder.Anchor = AnchorStyles.Right | (AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top));
			this.tvFolder.BorderStyle = BorderStyle.FixedSingle;
			this.tvFolder.HideSelection = false;
			this.tvFolder.ImageList = this.ilFolder;
			this.tvFolder.LabelEdit = true;
			this.tvFolder.Location = new Point(0, 0);
			this.tvFolder.Name = "tvFolder";
			this.tvFolder.Size = new Size(0x110, 0x1bd);
			this.tvFolder.TabIndex = 0;
			this.tvFolder.AfterSelect += new TreeViewEventHandler(this.tvFolder_AfterSelect);
			this.tvFolder.AfterLabelEdit += new NodeLabelEditEventHandler(this.tvFolder_AfterLabelEdit);
			this.ilFolder.ImageSize = new Size(0x10, 0x10);
			this.ilFolder.ImageStream = (ImageListStreamer) manager1.GetObject("ilFolder.ImageStream");
			this.ilFolder.TransparentColor = Color.Transparent;
			this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			
			this.btnOK.Location = new Point(0x11d, 0x198);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new EventHandler(this.btnOK_Click);
			this.btnCreate.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btnCreate.Location = new Point(0x11e, 0x90);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 2;
			this.btnCreate.Text = "&Create";
			this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
			this.btnDelete.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btnDelete.Location = new Point(0x11e, 0xb8);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
			this.btnEdit.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btnEdit.Location = new Point(0x11e, 0xe0);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 4;
			this.btnEdit.Text = "&Edit";
			this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
			this.AutoScaleBaseSize = new Size(6, 14);
			base.ClientSize = new Size(0x170, 0x1bd);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.btnDelete);
			base.Controls.Add(this.btnCreate);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.tvFolder);
			this.Font = new Font("Verdana", 8.25f);
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FolderForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Folder";
			base.KeyDown += new KeyEventHandler(this.FolderForm_KeyDown);
			base.Load += new EventHandler(this.FolderForm_Load);
			base.ResumeLayout(false);
		}

		private void SetEnable()
		{
			this.btnCreate.Enabled = this.CanCreate;
			this.btnDelete.Enabled = this.CanDelete;
			this.btnEdit.Enabled = this.CanEdit;
		}

		public static void ShowIt()
		{
			if (FolderForm.Current == null)
			{
				FolderForm.Current = new FolderForm();
			}
			FolderForm.Current.ShowDialog();
		}

		private void tvFolder_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			DataRow row1 = (DataRow) e.Node.Tag;
			StockDB.UpdateFolderRow((int) row1["FolderId"], e.Label);
		}

		private void tvFolder_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (!this.Updating)
			{
				this.CurrentRow = (DataRow) e.Node.Tag;
				int num1 = (int) this.CurrentRow["FolderId"];
				int num2 = (int) this.CurrentRow["ParentId"];
				if (ListForm.Current.FolderId != num1)
				{
					ListForm.Current.FolderId = num1;
					ListForm.Current.Activate();
					base.Activate();
				}
				if (num2 == 3)
				{
					StockDB.CurrentFavoriteId = num1;
				}
				this.SetEnable();
			}
		}


		// Properties
		private bool CanCreate
		{
			get
			{
				return ((this.CurrentRow != null) && (((int) this.CurrentRow["FolderId"]) == 3));
			}
		}

		private bool CanDelete
		{
			get
			{
				if (this.CurrentRow == null)
				{
					return false;
				}
				int num1 = (int) this.CurrentRow["ParentId"];
				return ((num1 == 3) || (num1 == 1));
			}
		}

		private bool CanEdit
		{
			get
			{
				return ((this.CurrentRow != null) && (((int) this.CurrentRow["ParentId"]) == 3));
			}
		}


		// Fields
		private Button btnCreate;
		private Button btnDelete;
		private Button btnEdit;
		private Button btnOK;
		private IContainer components;
		private static FolderForm Current;
		private DataRow CurrentRow;
		private ImageList ilFolder;
		private TreeView tvFolder;
		private bool Updating;
	}


}
