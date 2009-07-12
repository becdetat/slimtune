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
			this.m_executableLabel = new System.Windows.Forms.Label();
			this.m_executableTextBox = new System.Windows.Forms.TextBox();
			this.m_browseButton = new System.Windows.Forms.Button();
			this.m_argumentsTextBox = new System.Windows.Forms.TextBox();
			this.m_argumentsLabel = new System.Windows.Forms.Label();
			this.m_samplingRadio = new System.Windows.Forms.RadioButton();
			this.m_tracingRadio = new System.Windows.Forms.RadioButton();
			this.m_hybridRadio = new System.Windows.Forms.RadioButton();
			this.m_profileTypeLabel = new System.Windows.Forms.Label();
			this.m_runButton = new System.Windows.Forms.Button();
			this.m_openDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// m_executableLabel
			// 
			this.m_executableLabel.AutoSize = true;
			this.m_executableLabel.Location = new System.Drawing.Point(12, 15);
			this.m_executableLabel.Name = "m_executableLabel";
			this.m_executableLabel.Size = new System.Drawing.Size(63, 13);
			this.m_executableLabel.TabIndex = 0;
			this.m_executableLabel.Text = "Executable:";
			this.m_executableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_executableTextBox
			// 
			this.m_executableTextBox.Location = new System.Drawing.Point(81, 12);
			this.m_executableTextBox.Name = "m_executableTextBox";
			this.m_executableTextBox.Size = new System.Drawing.Size(413, 20);
			this.m_executableTextBox.TabIndex = 1;
			// 
			// m_browseButton
			// 
			this.m_browseButton.Location = new System.Drawing.Point(500, 10);
			this.m_browseButton.Name = "m_browseButton";
			this.m_browseButton.Size = new System.Drawing.Size(75, 23);
			this.m_browseButton.TabIndex = 2;
			this.m_browseButton.Text = "Browse...";
			this.m_browseButton.UseVisualStyleBackColor = true;
			// 
			// m_argumentsTextBox
			// 
			this.m_argumentsTextBox.Location = new System.Drawing.Point(81, 39);
			this.m_argumentsTextBox.Name = "m_argumentsTextBox";
			this.m_argumentsTextBox.Size = new System.Drawing.Size(494, 20);
			this.m_argumentsTextBox.TabIndex = 3;
			// 
			// m_argumentsLabel
			// 
			this.m_argumentsLabel.AutoSize = true;
			this.m_argumentsLabel.Location = new System.Drawing.Point(12, 42);
			this.m_argumentsLabel.Name = "m_argumentsLabel";
			this.m_argumentsLabel.Size = new System.Drawing.Size(60, 13);
			this.m_argumentsLabel.TabIndex = 4;
			this.m_argumentsLabel.Text = "Arguments:";
			// 
			// m_samplingRadio
			// 
			this.m_samplingRadio.AutoSize = true;
			this.m_samplingRadio.Checked = true;
			this.m_samplingRadio.Location = new System.Drawing.Point(81, 66);
			this.m_samplingRadio.Name = "m_samplingRadio";
			this.m_samplingRadio.Size = new System.Drawing.Size(68, 17);
			this.m_samplingRadio.TabIndex = 5;
			this.m_samplingRadio.TabStop = true;
			this.m_samplingRadio.Text = "Sampling";
			this.m_samplingRadio.UseVisualStyleBackColor = true;
			// 
			// m_tracingRadio
			// 
			this.m_tracingRadio.AutoSize = true;
			this.m_tracingRadio.Location = new System.Drawing.Point(156, 66);
			this.m_tracingRadio.Name = "m_tracingRadio";
			this.m_tracingRadio.Size = new System.Drawing.Size(61, 17);
			this.m_tracingRadio.TabIndex = 6;
			this.m_tracingRadio.Text = "Tracing";
			this.m_tracingRadio.UseVisualStyleBackColor = true;
			// 
			// m_hybridRadio
			// 
			this.m_hybridRadio.AutoSize = true;
			this.m_hybridRadio.Enabled = false;
			this.m_hybridRadio.Location = new System.Drawing.Point(224, 66);
			this.m_hybridRadio.Name = "m_hybridRadio";
			this.m_hybridRadio.Size = new System.Drawing.Size(55, 17);
			this.m_hybridRadio.TabIndex = 7;
			this.m_hybridRadio.Text = "Hybrid";
			this.m_hybridRadio.UseVisualStyleBackColor = true;
			// 
			// m_profileTypeLabel
			// 
			this.m_profileTypeLabel.AutoSize = true;
			this.m_profileTypeLabel.Location = new System.Drawing.Point(9, 68);
			this.m_profileTypeLabel.Name = "m_profileTypeLabel";
			this.m_profileTypeLabel.Size = new System.Drawing.Size(66, 13);
			this.m_profileTypeLabel.TabIndex = 8;
			this.m_profileTypeLabel.Text = "Profile Type:";
			// 
			// m_runButton
			// 
			this.m_runButton.Location = new System.Drawing.Point(256, 153);
			this.m_runButton.Name = "m_runButton";
			this.m_runButton.Size = new System.Drawing.Size(75, 23);
			this.m_runButton.TabIndex = 9;
			this.m_runButton.Text = "Run";
			this.m_runButton.UseVisualStyleBackColor = true;
			// 
			// m_openDialog
			// 
			this.m_openDialog.Filter = "Applications (*.exe)|*.exe";
			// 
			// RunDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(586, 187);
			this.Controls.Add(this.m_runButton);
			this.Controls.Add(this.m_profileTypeLabel);
			this.Controls.Add(this.m_hybridRadio);
			this.Controls.Add(this.m_tracingRadio);
			this.Controls.Add(this.m_samplingRadio);
			this.Controls.Add(this.m_argumentsLabel);
			this.Controls.Add(this.m_argumentsTextBox);
			this.Controls.Add(this.m_browseButton);
			this.Controls.Add(this.m_executableTextBox);
			this.Controls.Add(this.m_executableLabel);
			this.Name = "RunDialog";
			this.Text = "RunDialog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label m_executableLabel;
		private System.Windows.Forms.TextBox m_executableTextBox;
		private System.Windows.Forms.Button m_browseButton;
		private System.Windows.Forms.TextBox m_argumentsTextBox;
		private System.Windows.Forms.Label m_argumentsLabel;
		private System.Windows.Forms.RadioButton m_samplingRadio;
		private System.Windows.Forms.RadioButton m_tracingRadio;
		private System.Windows.Forms.RadioButton m_hybridRadio;
		private System.Windows.Forms.Label m_profileTypeLabel;
		private System.Windows.Forms.Button m_runButton;
		private System.Windows.Forms.OpenFileDialog m_openDialog;
	}
}