/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/
using System;
using System.IO;
using System.Diagnostics;
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
		private const string ProfilerGuid_x86 = "{38A7EA35-B221-425a-AD07-D058C581611D}";

		MainWindow m_mainWindow;

		public RunDialog(MainWindow mainWindow)
		{
			InitializeComponent();
			m_mainWindow = mainWindow;
		}

		private bool LaunchLocal()
		{
			string exe = m_executableTextBox.Text;
			string args = m_argumentsTextBox.Text;
			int port = int.Parse(m_portTextBox.Text);
			ProfilerMode mode = ProfilerMode.PM_Sampling;
			string dbFile = m_resultsFileTextBox.Text;

			if(!File.Exists(exe))
			{
				MessageBox.Show("Executable does not exist.", "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			//connect to storage before launching the process -- we don't want to launch if this fails
			IStorageEngine storage = null;
			if(m_connectCheckBox.Checked)
			{
				try
				{
					storage = new SqlServerCompactEngine(dbFile);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}

			string config = string.Empty;

			config += string.Format("Mode={0};", (int) mode);
			config += string.Format("Port={0};", port);
			config += string.Format("Arch={0};", "x86");
			config += string.Format("Wait={0};", m_waitConnectCheckBox.Checked);

			var psi = new ProcessStartInfo(exe, args);
			psi.UseShellExecute = false;

			if(psi.EnvironmentVariables.ContainsKey("COR_ENABLE_PROFILING"))
				psi.EnvironmentVariables["COR_ENABLE_PROFILING"] = "1";
			else
				psi.EnvironmentVariables.Add("COR_ENABLE_PROFILING", "1");

			if(psi.EnvironmentVariables.ContainsKey("COR_PROFILER"))
				psi.EnvironmentVariables["COR_PROFILER"] = ProfilerGuid_x86;
			else
				psi.EnvironmentVariables.Add("COR_PROFILER", ProfilerGuid_x86);

			if(psi.EnvironmentVariables.ContainsKey("SLIMTUNE_CONFIG"))
				psi.EnvironmentVariables["SLIMTUNE_CONFIG"] = config;
			else
				psi.EnvironmentVariables.Add("SLIMTUNE_CONFIG", config);

			try
			{
				Process.Start(psi);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Launch Error");
				if(storage != null)
					storage.Dispose();

				return false;
			}

			//connect, if we're asked to
			if(m_connectCheckBox.Checked)
			{
				Results resultsWindow = new Results();
				resultsWindow.Text = System.IO.Path.GetFileNameWithoutExtension(m_executableTextBox.Text) + " - " +
					System.IO.Path.GetFileNameWithoutExtension(m_resultsFileTextBox.Text);
				resultsWindow.Show(m_mainWindow.DockPanel);
				resultsWindow.Connect("localhost", port, storage);
			}

			return true;
		}

		private void m_runButton_Click(object sender, EventArgs e)
		{
			bool result = LaunchLocal();
			if(!result)
				return;

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

		private void m_connectCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			m_resultsFileTextBox.Enabled = m_connectCheckBox.Enabled;
			m_browseDbButton.Enabled = m_connectCheckBox.Enabled;
		}

		private void m_portTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char) Keys.Back)
				return;
			if(e.KeyChar < '0' || e.KeyChar > '9')
				e.Handled = true;
		}
	}
}
