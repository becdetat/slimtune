namespace SlimTuneUI
{
	partial class RunDialog
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
			this.m_runButton = new System.Windows.Forms.Button();
			this.m_openExeDialog = new System.Windows.Forms.OpenFileDialog();
			this.m_saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
			this.m_launchOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.m_waitConnectCheckBox = new System.Windows.Forms.CheckBox();
			this.m_profileTypeLabel = new System.Windows.Forms.Label();
			this.m_hybridRadio = new System.Windows.Forms.RadioButton();
			this.m_tracingRadio = new System.Windows.Forms.RadioButton();
			this.m_samplingRadio = new System.Windows.Forms.RadioButton();
			this.m_argumentsLabel = new System.Windows.Forms.Label();
			this.m_argumentsTextBox = new System.Windows.Forms.TextBox();
			this.m_browseExeButton = new System.Windows.Forms.Button();
			this.m_executableTextBox = new System.Windows.Forms.TextBox();
			this.m_executableLabel = new System.Windows.Forms.Label();
			this.m_frontendOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.m_connectCheckBox = new System.Windows.Forms.CheckBox();
			this.m_browseDbButton = new System.Windows.Forms.Button();
			this.m_resultsFileTextBox = new System.Windows.Forms.TextBox();
			this.m_saveAsLabel = new System.Windows.Forms.Label();
			this.m_portLabel = new System.Windows.Forms.Label();
			this.m_portTextBox = new System.Windows.Forms.TextBox();
			this.m_launchOptionsGroupBox.SuspendLayout();
			this.m_frontendOptionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_runButton
			// 
			this.m_runButton.Location = new System.Drawing.Point(256, 471);
			this.m_runButton.Name = "m_runButton";
			this.m_runButton.Size = new System.Drawing.Size(75, 23);
			this.m_runButton.TabIndex = 9;
			this.m_runButton.Text = "Run";
			this.m_runButton.UseVisualStyleBackColor = true;
			this.m_runButton.Click += new System.EventHandler(this.m_runButton_Click);
			// 
			// m_openExeDialog
			// 
			this.m_openExeDialog.Filter = "Applications (*.exe)|*.exe";
			// 
			// m_saveResultsDialog
			// 
			this.m_saveResultsDialog.DefaultExt = "sdf";
			this.m_saveResultsDialog.Filter = "Results (*.sdf)|*.sdf";
			this.m_saveResultsDialog.Title = "Save Results...";
			// 
			// m_launchOptionsGroupBox
			// 
			this.m_launchOptionsGroupBox.Controls.Add(this.m_portTextBox);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_portLabel);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_waitConnectCheckBox);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_profileTypeLabel);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_hybridRadio);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_tracingRadio);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_samplingRadio);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_argumentsLabel);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_argumentsTextBox);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_browseExeButton);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_executableTextBox);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_executableLabel);
			this.m_launchOptionsGroupBox.Location = new System.Drawing.Point(12, 12);
			this.m_launchOptionsGroupBox.Name = "m_launchOptionsGroupBox";
			this.m_launchOptionsGroupBox.Size = new System.Drawing.Size(563, 159);
			this.m_launchOptionsGroupBox.TabIndex = 15;
			this.m_launchOptionsGroupBox.TabStop = false;
			this.m_launchOptionsGroupBox.Text = "Launch Options";
			// 
			// m_waitConnectCheckBox
			// 
			this.m_waitConnectCheckBox.AutoSize = true;
			this.m_waitConnectCheckBox.Checked = true;
			this.m_waitConnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_waitConnectCheckBox.Location = new System.Drawing.Point(9, 103);
			this.m_waitConnectCheckBox.Name = "m_waitConnectCheckBox";
			this.m_waitConnectCheckBox.Size = new System.Drawing.Size(119, 17);
			this.m_waitConnectCheckBox.TabIndex = 24;
			this.m_waitConnectCheckBox.Text = "Wait for connection";
			this.m_waitConnectCheckBox.UseVisualStyleBackColor = true;
			// 
			// m_profileTypeLabel
			// 
			this.m_profileTypeLabel.AutoSize = true;
			this.m_profileTypeLabel.Location = new System.Drawing.Point(6, 127);
			this.m_profileTypeLabel.Name = "m_profileTypeLabel";
			this.m_profileTypeLabel.Size = new System.Drawing.Size(66, 13);
			this.m_profileTypeLabel.TabIndex = 23;
			this.m_profileTypeLabel.Text = "Profile Type:";
			// 
			// m_hybridRadio
			// 
			this.m_hybridRadio.AutoSize = true;
			this.m_hybridRadio.Enabled = false;
			this.m_hybridRadio.Location = new System.Drawing.Point(219, 125);
			this.m_hybridRadio.Name = "m_hybridRadio";
			this.m_hybridRadio.Size = new System.Drawing.Size(55, 17);
			this.m_hybridRadio.TabIndex = 22;
			this.m_hybridRadio.Text = "Hybrid";
			this.m_hybridRadio.UseVisualStyleBackColor = true;
			// 
			// m_tracingRadio
			// 
			this.m_tracingRadio.AutoSize = true;
			this.m_tracingRadio.Enabled = false;
			this.m_tracingRadio.Location = new System.Drawing.Point(152, 125);
			this.m_tracingRadio.Name = "m_tracingRadio";
			this.m_tracingRadio.Size = new System.Drawing.Size(61, 17);
			this.m_tracingRadio.TabIndex = 21;
			this.m_tracingRadio.Text = "Tracing";
			this.m_tracingRadio.UseVisualStyleBackColor = true;
			// 
			// m_samplingRadio
			// 
			this.m_samplingRadio.AutoSize = true;
			this.m_samplingRadio.Checked = true;
			this.m_samplingRadio.Location = new System.Drawing.Point(78, 125);
			this.m_samplingRadio.Name = "m_samplingRadio";
			this.m_samplingRadio.Size = new System.Drawing.Size(68, 17);
			this.m_samplingRadio.TabIndex = 20;
			this.m_samplingRadio.TabStop = true;
			this.m_samplingRadio.Text = "Sampling";
			this.m_samplingRadio.UseVisualStyleBackColor = true;
			// 
			// m_argumentsLabel
			// 
			this.m_argumentsLabel.AutoSize = true;
			this.m_argumentsLabel.Location = new System.Drawing.Point(6, 54);
			this.m_argumentsLabel.Name = "m_argumentsLabel";
			this.m_argumentsLabel.Size = new System.Drawing.Size(60, 13);
			this.m_argumentsLabel.TabIndex = 19;
			this.m_argumentsLabel.Text = "Arguments:";
			// 
			// m_argumentsTextBox
			// 
			this.m_argumentsTextBox.Location = new System.Drawing.Point(69, 51);
			this.m_argumentsTextBox.Name = "m_argumentsTextBox";
			this.m_argumentsTextBox.Size = new System.Drawing.Size(488, 20);
			this.m_argumentsTextBox.TabIndex = 18;
			// 
			// m_browseExeButton
			// 
			this.m_browseExeButton.Location = new System.Drawing.Point(482, 21);
			this.m_browseExeButton.Name = "m_browseExeButton";
			this.m_browseExeButton.Size = new System.Drawing.Size(75, 23);
			this.m_browseExeButton.TabIndex = 17;
			this.m_browseExeButton.Text = "Browse...";
			this.m_browseExeButton.UseVisualStyleBackColor = true;
			this.m_browseExeButton.Click += new System.EventHandler(this.m_browseExeButton_Click);
			// 
			// m_executableTextBox
			// 
			this.m_executableTextBox.Location = new System.Drawing.Point(69, 24);
			this.m_executableTextBox.Name = "m_executableTextBox";
			this.m_executableTextBox.Size = new System.Drawing.Size(407, 20);
			this.m_executableTextBox.TabIndex = 16;
			// 
			// m_executableLabel
			// 
			this.m_executableLabel.AutoSize = true;
			this.m_executableLabel.Location = new System.Drawing.Point(6, 27);
			this.m_executableLabel.Name = "m_executableLabel";
			this.m_executableLabel.Size = new System.Drawing.Size(63, 13);
			this.m_executableLabel.TabIndex = 15;
			this.m_executableLabel.Text = "Executable:";
			this.m_executableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_frontendOptionsGroupBox
			// 
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_connectCheckBox);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_browseDbButton);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_resultsFileTextBox);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_saveAsLabel);
			this.m_frontendOptionsGroupBox.Location = new System.Drawing.Point(12, 178);
			this.m_frontendOptionsGroupBox.Name = "m_frontendOptionsGroupBox";
			this.m_frontendOptionsGroupBox.Size = new System.Drawing.Size(562, 156);
			this.m_frontendOptionsGroupBox.TabIndex = 16;
			this.m_frontendOptionsGroupBox.TabStop = false;
			this.m_frontendOptionsGroupBox.Text = "Front-end Options";
			// 
			// m_connectCheckBox
			// 
			this.m_connectCheckBox.AutoSize = true;
			this.m_connectCheckBox.Checked = true;
			this.m_connectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_connectCheckBox.Location = new System.Drawing.Point(9, 19);
			this.m_connectCheckBox.Name = "m_connectCheckBox";
			this.m_connectCheckBox.Size = new System.Drawing.Size(108, 17);
			this.m_connectCheckBox.TabIndex = 16;
			this.m_connectCheckBox.Text = "Connect to target";
			this.m_connectCheckBox.UseVisualStyleBackColor = true;
			this.m_connectCheckBox.CheckedChanged += new System.EventHandler(this.m_connectCheckBox_CheckedChanged);
			// 
			// m_browseDbButton
			// 
			this.m_browseDbButton.Location = new System.Drawing.Point(482, 40);
			this.m_browseDbButton.Name = "m_browseDbButton";
			this.m_browseDbButton.Size = new System.Drawing.Size(75, 23);
			this.m_browseDbButton.TabIndex = 15;
			this.m_browseDbButton.Text = "Browse...";
			this.m_browseDbButton.UseVisualStyleBackColor = true;
			this.m_browseDbButton.Click += new System.EventHandler(this.m_browseDbButton_Click);
			// 
			// m_resultsFileTextBox
			// 
			this.m_resultsFileTextBox.Location = new System.Drawing.Point(76, 42);
			this.m_resultsFileTextBox.Name = "m_resultsFileTextBox";
			this.m_resultsFileTextBox.Size = new System.Drawing.Size(400, 20);
			this.m_resultsFileTextBox.TabIndex = 14;
			// 
			// m_saveAsLabel
			// 
			this.m_saveAsLabel.AutoSize = true;
			this.m_saveAsLabel.Location = new System.Drawing.Point(6, 45);
			this.m_saveAsLabel.Name = "m_saveAsLabel";
			this.m_saveAsLabel.Size = new System.Drawing.Size(64, 13);
			this.m_saveAsLabel.TabIndex = 13;
			this.m_saveAsLabel.Text = "Results File:";
			// 
			// m_portLabel
			// 
			this.m_portLabel.AutoSize = true;
			this.m_portLabel.Location = new System.Drawing.Point(6, 80);
			this.m_portLabel.Name = "m_portLabel";
			this.m_portLabel.Size = new System.Drawing.Size(60, 13);
			this.m_portLabel.TabIndex = 25;
			this.m_portLabel.Text = "Listen Port:";
			// 
			// m_portTextBox
			// 
			this.m_portTextBox.Location = new System.Drawing.Point(69, 77);
			this.m_portTextBox.Name = "m_portTextBox";
			this.m_portTextBox.Size = new System.Drawing.Size(100, 20);
			this.m_portTextBox.TabIndex = 26;
			this.m_portTextBox.Text = "3000";
			this.m_portTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_portTextBox_KeyPress);
			// 
			// RunDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(586, 506);
			this.Controls.Add(this.m_frontendOptionsGroupBox);
			this.Controls.Add(this.m_launchOptionsGroupBox);
			this.Controls.Add(this.m_runButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "RunDialog";
			this.Text = "RunDialog";
			this.m_launchOptionsGroupBox.ResumeLayout(false);
			this.m_launchOptionsGroupBox.PerformLayout();
			this.m_frontendOptionsGroupBox.ResumeLayout(false);
			this.m_frontendOptionsGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button m_runButton;
		private System.Windows.Forms.OpenFileDialog m_openExeDialog;
		private System.Windows.Forms.SaveFileDialog m_saveResultsDialog;
		private System.Windows.Forms.GroupBox m_launchOptionsGroupBox;
		private System.Windows.Forms.CheckBox m_waitConnectCheckBox;
		private System.Windows.Forms.Label m_profileTypeLabel;
		private System.Windows.Forms.RadioButton m_hybridRadio;
		private System.Windows.Forms.RadioButton m_tracingRadio;
		private System.Windows.Forms.RadioButton m_samplingRadio;
		private System.Windows.Forms.Label m_argumentsLabel;
		private System.Windows.Forms.TextBox m_argumentsTextBox;
		private System.Windows.Forms.Button m_browseExeButton;
		private System.Windows.Forms.TextBox m_executableTextBox;
		private System.Windows.Forms.Label m_executableLabel;
		private System.Windows.Forms.GroupBox m_frontendOptionsGroupBox;
		private System.Windows.Forms.Button m_browseDbButton;
		private System.Windows.Forms.TextBox m_resultsFileTextBox;
		private System.Windows.Forms.Label m_saveAsLabel;
		private System.Windows.Forms.CheckBox m_connectCheckBox;
		private System.Windows.Forms.TextBox m_portTextBox;
		private System.Windows.Forms.Label m_portLabel;
	}
}