/*
* Copyright (c) 2007-2009 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
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
			this.components = new System.ComponentModel.Container();
			this.m_runButton = new System.Windows.Forms.Button();
			this.m_openExeDialog = new System.Windows.Forms.OpenFileDialog();
			this.m_saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
			this.m_launchOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.m_portTextBox = new System.Windows.Forms.TextBox();
			this.m_portLabel = new System.Windows.Forms.Label();
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
			this.m_visualizerCombo = new System.Windows.Forms.ComboBox();
			this.m_visualizerLabel = new System.Windows.Forms.Label();
			this.m_browseWorkingDirButton = new System.Windows.Forms.Button();
			this.m_workingDirTextBox = new System.Windows.Forms.TextBox();
			this.m_workingDirLabel = new System.Windows.Forms.Label();
			this.m_dirBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
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
			this.m_toolTip.SetToolTip(this.m_runButton, "Launches the application.");
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
			this.m_launchOptionsGroupBox.Controls.Add(this.m_browseWorkingDirButton);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_workingDirTextBox);
			this.m_launchOptionsGroupBox.Controls.Add(this.m_workingDirLabel);
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
			this.m_launchOptionsGroupBox.Size = new System.Drawing.Size(563, 176);
			this.m_launchOptionsGroupBox.TabIndex = 15;
			this.m_launchOptionsGroupBox.TabStop = false;
			this.m_launchOptionsGroupBox.Text = "Launch Options";
			// 
			// m_portTextBox
			// 
			this.m_portTextBox.Location = new System.Drawing.Point(69, 103);
			this.m_portTextBox.Name = "m_portTextBox";
			this.m_portTextBox.Size = new System.Drawing.Size(100, 20);
			this.m_portTextBox.TabIndex = 26;
			this.m_portTextBox.Text = "3000";
			this.m_toolTip.SetToolTip(this.m_portTextBox, "The port that the profiler should use. Change this if you are profiling multiple " +
					"applications at once.");
			this.m_portTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_portTextBox_KeyPress);
			// 
			// m_portLabel
			// 
			this.m_portLabel.AutoSize = true;
			this.m_portLabel.Location = new System.Drawing.Point(6, 106);
			this.m_portLabel.Name = "m_portLabel";
			this.m_portLabel.Size = new System.Drawing.Size(60, 13);
			this.m_portLabel.TabIndex = 25;
			this.m_portLabel.Text = "Listen Port:";
			// 
			// m_waitConnectCheckBox
			// 
			this.m_waitConnectCheckBox.AutoSize = true;
			this.m_waitConnectCheckBox.Location = new System.Drawing.Point(9, 129);
			this.m_waitConnectCheckBox.Name = "m_waitConnectCheckBox";
			this.m_waitConnectCheckBox.Size = new System.Drawing.Size(119, 17);
			this.m_waitConnectCheckBox.TabIndex = 24;
			this.m_waitConnectCheckBox.Text = "Wait for connection";
			this.m_toolTip.SetToolTip(this.m_waitConnectCheckBox, "If checked, the executable will be prevented from launching until a profiler fron" +
					"t-end connects. Not recommended.");
			this.m_waitConnectCheckBox.UseVisualStyleBackColor = true;
			// 
			// m_profileTypeLabel
			// 
			this.m_profileTypeLabel.AutoSize = true;
			this.m_profileTypeLabel.Location = new System.Drawing.Point(6, 153);
			this.m_profileTypeLabel.Name = "m_profileTypeLabel";
			this.m_profileTypeLabel.Size = new System.Drawing.Size(66, 13);
			this.m_profileTypeLabel.TabIndex = 23;
			this.m_profileTypeLabel.Text = "Profile Type:";
			// 
			// m_hybridRadio
			// 
			this.m_hybridRadio.AutoSize = true;
			this.m_hybridRadio.Enabled = false;
			this.m_hybridRadio.Location = new System.Drawing.Point(219, 151);
			this.m_hybridRadio.Name = "m_hybridRadio";
			this.m_hybridRadio.Size = new System.Drawing.Size(55, 17);
			this.m_hybridRadio.TabIndex = 22;
			this.m_hybridRadio.Text = "Hybrid";
			this.m_toolTip.SetToolTip(this.m_hybridRadio, "Use hybrid profiling mode. Starts out in sampling mode, and lets you selectively " +
					"trace functions.");
			this.m_hybridRadio.UseVisualStyleBackColor = true;
			// 
			// m_tracingRadio
			// 
			this.m_tracingRadio.AutoSize = true;
			this.m_tracingRadio.Enabled = false;
			this.m_tracingRadio.Location = new System.Drawing.Point(152, 151);
			this.m_tracingRadio.Name = "m_tracingRadio";
			this.m_tracingRadio.Size = new System.Drawing.Size(61, 17);
			this.m_tracingRadio.TabIndex = 21;
			this.m_tracingRadio.Text = "Tracing";
			this.m_toolTip.SetToolTip(this.m_tracingRadio, "Use tracing based profiling.  Very accurate but very slow.");
			this.m_tracingRadio.UseVisualStyleBackColor = true;
			// 
			// m_samplingRadio
			// 
			this.m_samplingRadio.AutoSize = true;
			this.m_samplingRadio.Checked = true;
			this.m_samplingRadio.Location = new System.Drawing.Point(78, 151);
			this.m_samplingRadio.Name = "m_samplingRadio";
			this.m_samplingRadio.Size = new System.Drawing.Size(68, 17);
			this.m_samplingRadio.TabIndex = 20;
			this.m_samplingRadio.TabStop = true;
			this.m_samplingRadio.Text = "Sampling";
			this.m_toolTip.SetToolTip(this.m_samplingRadio, "Use sample-based profiling. Very fast but not especially accurate. Good for getti" +
					"ng an overview of performance.");
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
			this.m_toolTip.SetToolTip(this.m_argumentsTextBox, "The command line that should be passed to the executable when launched.");
			// 
			// m_browseExeButton
			// 
			this.m_browseExeButton.Location = new System.Drawing.Point(482, 21);
			this.m_browseExeButton.Name = "m_browseExeButton";
			this.m_browseExeButton.Size = new System.Drawing.Size(75, 23);
			this.m_browseExeButton.TabIndex = 17;
			this.m_browseExeButton.Text = "Browse...";
			this.m_toolTip.SetToolTip(this.m_browseExeButton, "Opens a file browser to select an executable.");
			this.m_browseExeButton.UseVisualStyleBackColor = true;
			this.m_browseExeButton.Click += new System.EventHandler(this.m_browseExeButton_Click);
			// 
			// m_executableTextBox
			// 
			this.m_executableTextBox.Location = new System.Drawing.Point(69, 24);
			this.m_executableTextBox.Name = "m_executableTextBox";
			this.m_executableTextBox.Size = new System.Drawing.Size(407, 20);
			this.m_executableTextBox.TabIndex = 16;
			this.m_toolTip.SetToolTip(this.m_executableTextBox, "The path of the executable to launch for profiling.");
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
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_visualizerLabel);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_visualizerCombo);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_connectCheckBox);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_browseDbButton);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_resultsFileTextBox);
			this.m_frontendOptionsGroupBox.Controls.Add(this.m_saveAsLabel);
			this.m_frontendOptionsGroupBox.Location = new System.Drawing.Point(12, 194);
			this.m_frontendOptionsGroupBox.Name = "m_frontendOptionsGroupBox";
			this.m_frontendOptionsGroupBox.Size = new System.Drawing.Size(563, 156);
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
			this.m_toolTip.SetToolTip(this.m_connectCheckBox, "If checked, the front-end will connect and begin gathering data immediately. ");
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
			this.m_toolTip.SetToolTip(this.m_browseDbButton, "Opens a file browser to select a database file.");
			this.m_browseDbButton.UseVisualStyleBackColor = true;
			this.m_browseDbButton.Click += new System.EventHandler(this.m_browseDbButton_Click);
			// 
			// m_resultsFileTextBox
			// 
			this.m_resultsFileTextBox.Location = new System.Drawing.Point(76, 42);
			this.m_resultsFileTextBox.Name = "m_resultsFileTextBox";
			this.m_resultsFileTextBox.Size = new System.Drawing.Size(400, 20);
			this.m_resultsFileTextBox.TabIndex = 14;
			this.m_toolTip.SetToolTip(this.m_resultsFileTextBox, "The filename of the database to store profiling data to.");
			// 
			// m_saveAsLabel
			// 
			this.m_saveAsLabel.AutoSize = true;
			this.m_saveAsLabel.Location = new System.Drawing.Point(9, 45);
			this.m_saveAsLabel.Name = "m_saveAsLabel";
			this.m_saveAsLabel.Size = new System.Drawing.Size(64, 13);
			this.m_saveAsLabel.TabIndex = 13;
			this.m_saveAsLabel.Text = "Results File:";
			// 
			// m_visualizerCombo
			// 
			this.m_visualizerCombo.DisplayMember = "Name";
			this.m_visualizerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_visualizerCombo.FormattingEnabled = true;
			this.m_visualizerCombo.Location = new System.Drawing.Point(76, 69);
			this.m_visualizerCombo.Name = "m_visualizerCombo";
			this.m_visualizerCombo.Size = new System.Drawing.Size(400, 21);
			this.m_visualizerCombo.TabIndex = 17;
			this.m_toolTip.SetToolTip(this.m_visualizerCombo, "The visualizer to open when the front-end connects.");
			// 
			// m_visualizerLabel
			// 
			this.m_visualizerLabel.AutoSize = true;
			this.m_visualizerLabel.Location = new System.Drawing.Point(9, 72);
			this.m_visualizerLabel.Name = "m_visualizerLabel";
			this.m_visualizerLabel.Size = new System.Drawing.Size(54, 13);
			this.m_visualizerLabel.TabIndex = 18;
			this.m_visualizerLabel.Text = "Visualizer:";
			// 
			// m_browseWorkingDirButton
			// 
			this.m_browseWorkingDirButton.Location = new System.Drawing.Point(483, 74);
			this.m_browseWorkingDirButton.Name = "m_browseWorkingDirButton";
			this.m_browseWorkingDirButton.Size = new System.Drawing.Size(75, 23);
			this.m_browseWorkingDirButton.TabIndex = 29;
			this.m_browseWorkingDirButton.Text = "Browse...";
			this.m_toolTip.SetToolTip(this.m_browseWorkingDirButton, "Opens a file browser to select a working directory.");
			this.m_browseWorkingDirButton.UseVisualStyleBackColor = true;
			this.m_browseWorkingDirButton.Click += new System.EventHandler(this.m_browseWorkingDirButton_Click);
			// 
			// m_workingDirTextBox
			// 
			this.m_workingDirTextBox.Location = new System.Drawing.Point(70, 77);
			this.m_workingDirTextBox.Name = "m_workingDirTextBox";
			this.m_workingDirTextBox.Size = new System.Drawing.Size(407, 20);
			this.m_workingDirTextBox.TabIndex = 28;
			this.m_toolTip.SetToolTip(this.m_workingDirTextBox, "The working directory to use when launching the executable. If left blank, the co" +
					"ntaining directory will be used.");
			// 
			// m_workingDirLabel
			// 
			this.m_workingDirLabel.AutoSize = true;
			this.m_workingDirLabel.Location = new System.Drawing.Point(3, 80);
			this.m_workingDirLabel.Name = "m_workingDirLabel";
			this.m_workingDirLabel.Size = new System.Drawing.Size(66, 13);
			this.m_workingDirLabel.TabIndex = 27;
			this.m_workingDirLabel.Text = "Working Dir:";
			this.m_workingDirLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_dirBrowser
			// 
			this.m_dirBrowser.Description = "Select the working directory to be used when launching the application.";
			// 
			// m_toolTip
			// 
			this.m_toolTip.IsBalloon = true;
			// 
			// RunDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(586, 506);
			this.Controls.Add(this.m_frontendOptionsGroupBox);
			this.Controls.Add(this.m_launchOptionsGroupBox);
			this.Controls.Add(this.m_runButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
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
		private System.Windows.Forms.Label m_visualizerLabel;
		private System.Windows.Forms.ComboBox m_visualizerCombo;
		private System.Windows.Forms.Button m_browseWorkingDirButton;
		private System.Windows.Forms.TextBox m_workingDirTextBox;
		private System.Windows.Forms.Label m_workingDirLabel;
		private System.Windows.Forms.FolderBrowserDialog m_dirBrowser;
		private System.Windows.Forms.ToolTip m_toolTip;
	}
}