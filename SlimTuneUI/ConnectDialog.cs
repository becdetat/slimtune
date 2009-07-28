using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SlimTuneUI
{
	public partial class ConnectDialog : Form
	{
		MainWindow m_mainWindow;

		public ConnectDialog(MainWindow mainWindow)
		{
			InitializeComponent();
			m_mainWindow = mainWindow;
		}

		private void m_browseDbButton_Click(object sender, EventArgs e)
		{
			var result = m_saveResultsDialog.ShowDialog(this);
			if(result == DialogResult.OK)
				m_resultsFileTextBox.Text = m_saveResultsDialog.FileName;
		}

		private bool Connect(string host, int port)
		{
			string dbFile = m_resultsFileTextBox.Text;

			//connect to storage before launching the process -- we don't want to launch if this fails
			IStorageEngine storage = null;
			try
			{
				storage = new SqlServerCompactEngine(dbFile, true);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			Results resultsWindow = new Results();
			resultsWindow.Text = string.Format("{0}:{1} - {2}", host, port, 
				System.IO.Path.GetFileNameWithoutExtension(m_resultsFileTextBox.Text));
			if(resultsWindow.Connect(host, port, storage))
			{
				MessageBox.Show("Profiler is now connected to target.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				resultsWindow.Show(m_mainWindow.DockPanel);
			}
			else
			{
				MessageBox.Show("Unable to connect.", "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				resultsWindow.Close();
				storage.Dispose();
				return false;
			}

			return true;
		}

		private void m_connectButton_Click(object sender, EventArgs e)
		{
			bool result = Connect(m_hostNameTextBox.Text, int.Parse(m_portTextBox.Text));
			if(!result)
				return;

			this.Close();
		}
	}
}
