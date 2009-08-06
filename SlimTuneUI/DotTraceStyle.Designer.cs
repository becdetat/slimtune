namespace SlimTuneUI
{
	partial class DotTraceStyle
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
			this.m_treeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// m_treeView
			// 
			this.m_treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_treeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.m_treeView.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.m_treeView.Location = new System.Drawing.Point(0, 0);
			this.m_treeView.Name = "m_treeView";
			this.m_treeView.Size = new System.Drawing.Size(576, 401);
			this.m_treeView.TabIndex = 0;
			this.m_treeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.m_treeView_DrawNode);
			this.m_treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.m_treeView_BeforeExpand);
			// 
			// DotTraceStyle
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(576, 401);
			this.Controls.Add(this.m_treeView);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Name = "DotTraceStyle";
			this.Text = "DotTraceStyle";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView m_treeView;
	}
}