using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SlimTuneUI
{
	public partial class RunDialog : Form
	{
		MainWindow m_mainWindow;

		public RunDialog(MainWindow mainWindow)
		{
			InitializeComponent();
			m_mainWindow = mainWindow;
		}

		private void m_runButton_Click(object sender, EventArgs e)
		{
			Results resultsWindow = new Results();
			resultsWindow.Show(m_mainWindow.DockPanel);
			bool result = resultsWindow.LaunchLocal(m_executableTextBox.Text, m_argumentsTextBox.Text, m_resultsFileTextBox.Text);

			if(result)
				this.Close();
		}

		private void m_browseExeButton_Click(object sender, EventArgs e)
		{
			var result = m_openExeDialog.ShowDialog(this);
			if(result == DialogResult.OK)
				m_executableTextBox.Text = m_openExeDialog.FileName;
		}

		private void m_browseDbButton_Click(object sender, EventArgs e)
		{
			var result = m_saveResultsDialog.ShowDialog(this);
			if(result == DialogResult.OK)
				m_resultsFileTextBox.Text = m_saveResultsDialog.FileName;
		}
	}
}
