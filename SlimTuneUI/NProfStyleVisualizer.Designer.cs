namespace SlimTuneUI
{
	partial class NProfStyleVisualizer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_splitter = new System.Windows.Forms.SplitContainer();
			this.m_callees = new Aga.Controls.Tree.TreeViewAdv();
			this.m_calleesIdColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_calleesThreadIdColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_calleesNameColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_calleesTimeColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_calleesIdTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_calleesThreadTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_calleesNameTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_calleesTimeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_callers = new Aga.Controls.Tree.TreeViewAdv();
			this.m_callersIdColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_callersThreadIdColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_callersNameColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_callersTimeColumn = new Aga.Controls.Tree.TreeColumn();
			this.m_callersIdTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_callersThreadTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_callersNameTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_callersTimeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.m_splitter.Panel1.SuspendLayout();
			this.m_splitter.Panel2.SuspendLayout();
			this.m_splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_splitter
			// 
			this.m_splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_splitter.Location = new System.Drawing.Point(0, 0);
			this.m_splitter.Name = "m_splitter";
			this.m_splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// m_splitter.Panel1
			// 
			this.m_splitter.Panel1.Controls.Add(this.m_callees);
			// 
			// m_splitter.Panel2
			// 
			this.m_splitter.Panel2.Controls.Add(this.m_callers);
			this.m_splitter.Size = new System.Drawing.Size(848, 595);
			this.m_splitter.SplitterDistance = 281;
			this.m_splitter.TabIndex = 0;
			// 
			// m_callees
			// 
			this.m_callees.BackColor = System.Drawing.SystemColors.Window;
			this.m_callees.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_callees.Columns.Add(this.m_calleesIdColumn);
			this.m_callees.Columns.Add(this.m_calleesThreadIdColumn);
			this.m_callees.Columns.Add(this.m_calleesNameColumn);
			this.m_callees.Columns.Add(this.m_calleesTimeColumn);
			this.m_callees.DefaultToolTipProvider = null;
			this.m_callees.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_callees.DragDropMarkColor = System.Drawing.Color.Black;
			this.m_callees.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle) ((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
			this.m_callees.LineColor = System.Drawing.SystemColors.ControlDark;
			this.m_callees.LoadOnDemand = true;
			this.m_callees.Location = new System.Drawing.Point(0, 0);
			this.m_callees.Model = null;
			this.m_callees.Name = "m_callees";
			this.m_callees.NodeControls.Add(this.m_calleesIdTextBox);
			this.m_callees.NodeControls.Add(this.m_calleesThreadTextBox);
			this.m_callees.NodeControls.Add(this.m_calleesNameTextBox);
			this.m_callees.NodeControls.Add(this.m_calleesTimeTextBox);
			this.m_callees.SelectedNode = null;
			this.m_callees.Size = new System.Drawing.Size(848, 281);
			this.m_callees.TabIndex = 0;
			this.m_callees.UseColumns = true;
			// 
			// m_calleesIdColumn
			// 
			this.m_calleesIdColumn.Header = "Id";
			this.m_calleesIdColumn.MinColumnWidth = 100;
			this.m_calleesIdColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_calleesIdColumn.TooltipText = null;
			this.m_calleesIdColumn.Width = 200;
			// 
			// m_calleesThreadIdColumn
			// 
			this.m_calleesThreadIdColumn.Header = "Thread";
			this.m_calleesThreadIdColumn.MinColumnWidth = 50;
			this.m_calleesThreadIdColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_calleesThreadIdColumn.TooltipText = null;
			// 
			// m_calleesNameColumn
			// 
			this.m_calleesNameColumn.Header = "Callees";
			this.m_calleesNameColumn.MinColumnWidth = 200;
			this.m_calleesNameColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_calleesNameColumn.TooltipText = null;
			this.m_calleesNameColumn.Width = 500;
			// 
			// m_calleesTimeColumn
			// 
			this.m_calleesTimeColumn.Header = "Time";
			this.m_calleesTimeColumn.MinColumnWidth = 50;
			this.m_calleesTimeColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_calleesTimeColumn.TooltipText = null;
			// 
			// m_calleesIdTextBox
			// 
			this.m_calleesIdTextBox.DataPropertyName = "Id";
			this.m_calleesIdTextBox.IncrementalSearchEnabled = true;
			this.m_calleesIdTextBox.LeftMargin = 3;
			this.m_calleesIdTextBox.ParentColumn = this.m_calleesIdColumn;
			// 
			// m_calleesThreadTextBox
			// 
			this.m_calleesThreadTextBox.DataPropertyName = "Thread";
			this.m_calleesThreadTextBox.IncrementalSearchEnabled = true;
			this.m_calleesThreadTextBox.LeftMargin = 3;
			this.m_calleesThreadTextBox.ParentColumn = this.m_calleesThreadIdColumn;
			// 
			// m_calleesNameTextBox
			// 
			this.m_calleesNameTextBox.DataPropertyName = "Name";
			this.m_calleesNameTextBox.IncrementalSearchEnabled = true;
			this.m_calleesNameTextBox.LeftMargin = 3;
			this.m_calleesNameTextBox.ParentColumn = this.m_calleesNameColumn;
			// 
			// m_calleesTimeTextBox
			// 
			this.m_calleesTimeTextBox.DataPropertyName = "Time";
			this.m_calleesTimeTextBox.IncrementalSearchEnabled = true;
			this.m_calleesTimeTextBox.LeftMargin = 3;
			this.m_calleesTimeTextBox.ParentColumn = this.m_calleesTimeColumn;
			// 
			// m_callers
			// 
			this.m_callers.BackColor = System.Drawing.SystemColors.Window;
			this.m_callers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_callers.Columns.Add(this.m_callersIdColumn);
			this.m_callers.Columns.Add(this.m_callersThreadIdColumn);
			this.m_callers.Columns.Add(this.m_callersNameColumn);
			this.m_callers.Columns.Add(this.m_callersTimeColumn);
			this.m_callers.DefaultToolTipProvider = null;
			this.m_callers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_callers.DragDropMarkColor = System.Drawing.Color.Black;
			this.m_callers.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle) ((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
			this.m_callers.LineColor = System.Drawing.SystemColors.ControlDark;
			this.m_callers.LoadOnDemand = true;
			this.m_callers.Location = new System.Drawing.Point(0, 0);
			this.m_callers.Model = null;
			this.m_callers.Name = "m_callers";
			this.m_callers.NodeControls.Add(this.m_callersIdTextBox);
			this.m_callers.NodeControls.Add(this.m_callersThreadTextBox);
			this.m_callers.NodeControls.Add(this.m_callersNameTextBox);
			this.m_callers.NodeControls.Add(this.m_callersTimeTextBox);
			this.m_callers.SelectedNode = null;
			this.m_callers.Size = new System.Drawing.Size(848, 310);
			this.m_callers.TabIndex = 1;
			this.m_callers.UseColumns = true;
			// 
			// m_callersIdColumn
			// 
			this.m_callersIdColumn.Header = "Id";
			this.m_callersIdColumn.MinColumnWidth = 100;
			this.m_callersIdColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_callersIdColumn.TooltipText = null;
			this.m_callersIdColumn.Width = 200;
			// 
			// m_callersThreadIdColumn
			// 
			this.m_callersThreadIdColumn.Header = "Thread";
			this.m_callersThreadIdColumn.MinColumnWidth = 50;
			this.m_callersThreadIdColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_callersThreadIdColumn.TooltipText = null;
			// 
			// m_callersNameColumn
			// 
			this.m_callersNameColumn.Header = "Callers";
			this.m_callersNameColumn.MinColumnWidth = 200;
			this.m_callersNameColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_callersNameColumn.TooltipText = null;
			this.m_callersNameColumn.Width = 500;
			// 
			// m_callersTimeColumn
			// 
			this.m_callersTimeColumn.Header = "Time";
			this.m_callersTimeColumn.MinColumnWidth = 50;
			this.m_callersTimeColumn.SortOrder = System.Windows.Forms.SortOrder.None;
			this.m_callersTimeColumn.TooltipText = null;
			// 
			// m_callersIdTextBox
			// 
			this.m_callersIdTextBox.DataPropertyName = "Id";
			this.m_callersIdTextBox.IncrementalSearchEnabled = true;
			this.m_callersIdTextBox.LeftMargin = 3;
			this.m_callersIdTextBox.ParentColumn = this.m_callersIdColumn;
			// 
			// m_callersThreadTextBox
			// 
			this.m_callersThreadTextBox.DataPropertyName = "Thread";
			this.m_callersThreadTextBox.IncrementalSearchEnabled = true;
			this.m_callersThreadTextBox.LeftMargin = 3;
			this.m_callersThreadTextBox.ParentColumn = this.m_callersThreadIdColumn;
			// 
			// m_callersNameTextBox
			// 
			this.m_callersNameTextBox.DataPropertyName = "Name";
			this.m_callersNameTextBox.IncrementalSearchEnabled = true;
			this.m_callersNameTextBox.LeftMargin = 3;
			this.m_callersNameTextBox.ParentColumn = this.m_callersNameColumn;
			// 
			// m_callersTimeTextBox
			// 
			this.m_callersTimeTextBox.DataPropertyName = "Time";
			this.m_callersTimeTextBox.IncrementalSearchEnabled = true;
			this.m_callersTimeTextBox.LeftMargin = 3;
			this.m_callersTimeTextBox.ParentColumn = this.m_callersTimeColumn;
			// 
			// NProfStyleVisualizer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(848, 595);
			this.Controls.Add(this.m_splitter);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Name = "NProfStyleVisualizer";
			this.Text = "NProfStyleVisualizer";
			this.m_splitter.Panel1.ResumeLayout(false);
			this.m_splitter.Panel2.ResumeLayout(false);
			this.m_splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer m_splitter;
		private Aga.Controls.Tree.TreeViewAdv m_callees;
		private Aga.Controls.Tree.TreeViewAdv m_callers;
		private Aga.Controls.Tree.TreeColumn m_calleesIdColumn;
		private Aga.Controls.Tree.TreeColumn m_calleesThreadIdColumn;
		private Aga.Controls.Tree.TreeColumn m_calleesNameColumn;
		private Aga.Controls.Tree.TreeColumn m_calleesTimeColumn;
		private Aga.Controls.Tree.TreeColumn m_callersIdColumn;
		private Aga.Controls.Tree.TreeColumn m_callersThreadIdColumn;
		private Aga.Controls.Tree.TreeColumn m_callersNameColumn;
		private Aga.Controls.Tree.TreeColumn m_callersTimeColumn;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_calleesIdTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_calleesThreadTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_calleesNameTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_calleesTimeTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_callersIdTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_callersThreadTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_callersNameTextBox;
		private Aga.Controls.Tree.NodeControls.NodeTextBox m_callersTimeTextBox;
	}
}