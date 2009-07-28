/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlServerCe;

namespace SlimTuneUI
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void m_profilerRunMenu_Click(object sender, EventArgs e)
		{
			var runner = new RunDialog(this);
			runner.ShowDialog(this);
		}

		private void m_fileOpenMenu_Click(object sender, EventArgs e)
		{
			DialogResult result = m_openDialog.ShowDialog(this);
			if(result == DialogResult.OK)
			{
				var results = new Results();
				if(!results.Open(m_openDialog.FileName))
				{
					results.Dispose();
					return;
				}

				results.Show(DockPanel);
			}
		}

		private void m_fileExitMenu_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void m_profilerConnectMenu_Click(object sender, EventArgs e)
		{
			var connect = new ConnectDialog(this);
			connect.Show(this);
		}
	}
}
