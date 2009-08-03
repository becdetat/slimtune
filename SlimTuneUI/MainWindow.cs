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
				try
				{
					IStorageEngine engine = new SqlServerCompactEngine(m_openDialog.FileName, false);

					Connection conn = new Connection(engine);
					var results = new ChartVisualizer();
					//var results = new NProfStyleVisualizer();
					//var results = new SqlVisualizer();
					results.Initialize(this, conn);
					results.Show(DockPanel);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
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
