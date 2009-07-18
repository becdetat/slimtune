namespace SlimTuneUI
{
	partial class Results
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
			this.components = new System.ComponentModel.Container();
			this.button1 = new System.Windows.Forms.Button();
			this.m_mappingCountLabel = new System.Windows.Forms.Label();
			this.m_updateTimer = new System.Windows.Forms.Timer(this.components);
			this.m_dataGrid = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize) (this.m_dataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(37, 13);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// m_mappingCountLabel
			// 
			this.m_mappingCountLabel.AutoSize = true;
			this.m_mappingCountLabel.Location = new System.Drawing.Point(34, 129);
			this.m_mappingCountLabel.Name = "m_mappingCountLabel";
			this.m_mappingCountLabel.Size = new System.Drawing.Size(65, 13);
			this.m_mappingCountLabel.TabIndex = 1;
			this.m_mappingCountLabel.Text = "Mappings: 0";
			// 
			// m_updateTimer
			// 
			this.m_updateTimer.Interval = 1000;
			this.m_updateTimer.Tick += new System.EventHandler(this.m_updateTimer_Tick);
			// 
			// m_dataGrid
			// 
			this.m_dataGrid.AllowUserToAddRows = false;
			this.m_dataGrid.AllowUserToDeleteRows = false;
			this.m_dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_dataGrid.Location = new System.Drawing.Point(145, 12);
			this.m_dataGrid.Name = "m_dataGrid";
			this.m_dataGrid.ReadOnly = true;
			this.m_dataGrid.Size = new System.Drawing.Size(526, 403);
			this.m_dataGrid.TabIndex = 2;
			// 
			// Results
			// 
			this.ClientSize = new System.Drawing.Size(683, 427);
			this.Controls.Add(this.m_dataGrid);
			this.Controls.Add(this.m_mappingCountLabel);
			this.Controls.Add(this.button1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Name = "Results";
			this.Text = "Profiling Run";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Results_FormClosing);
			((System.ComponentModel.ISupportInitialize) (this.m_dataGrid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label m_mappingCountLabel;
		private System.Windows.Forms.Timer m_updateTimer;
		private System.Windows.Forms.DataGridView m_dataGrid;
	}
}
