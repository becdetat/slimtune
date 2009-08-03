namespace SlimTuneUI
{
	partial class ConnectionList
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
			this.m_connectionList = new System.Windows.Forms.ListBox();
			this.m_splitter.Panel1.SuspendLayout();
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
			this.m_splitter.Panel1.Controls.Add(this.m_connectionList);
			this.m_splitter.Size = new System.Drawing.Size(288, 494);
			this.m_splitter.SplitterDistance = 181;
			this.m_splitter.TabIndex = 0;
			// 
			// m_connectionList
			// 
			this.m_connectionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_connectionList.FormattingEnabled = true;
			this.m_connectionList.Location = new System.Drawing.Point(0, 0);
			this.m_connectionList.Name = "m_connectionList";
			this.m_connectionList.Size = new System.Drawing.Size(288, 173);
			this.m_connectionList.TabIndex = 0;
			// 
			// ConnectionList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(288, 494);
			this.Controls.Add(this.m_splitter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ConnectionList";
			this.Text = "Connections";
			this.m_splitter.Panel1.ResumeLayout(false);
			this.m_splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer m_splitter;
		private System.Windows.Forms.ListBox m_connectionList;
	}
}