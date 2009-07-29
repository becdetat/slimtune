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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Data.SqlServerCe;

/*
 * Useful queries:
 * 

SELECT M1.Name AS "Caller", M2.Name AS "Constructor", HitCount, CallerId, CalleeId
FROM Callers
JOIN Mappings M1 ON CallerId = M1.Id
JOIN Mappings M2 ON CalleeId = M2.Id
WHERE M2.Name LIKE '%..ctor'
ORDER BY HitCount DESC

SELECT Name, HitCount
FROM Callers
JOIN Mappings on CallerId = Mappings.Id
WHERE CalleeId = 0
ORDER BY HitCount DESC

SELECT Mappings.Name AS "Name", ROUND(100.0 * HitCount / @SampleCount, 2) AS "Percent", Threads.Name AS "Thread"
FROM Callers
JOIN Mappings ON CallerId = Mappings.Id
JOIN Threads ON ThreadId = Threads.Id
WHERE CalleeId = 0
ORDER BY HitCount DESC

*/

namespace SlimTuneUI
{
	public partial class Results : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		Thread m_recvThread;
		IStorageEngine m_storage;
		volatile bool m_receive = false;

		public Results()
		{
			InitializeComponent();
		}

		public bool Connect(string host, int port, IStorageEngine storage)
		{
			//TODO: select host/port
			ProfilerClient client = null;
			for(int i = 0; i < 10; ++i)
			{
				try
				{
					m_storage = storage;
					client = new ProfilerClient(host, port, m_storage);
				}
				catch(System.Net.Sockets.SocketException ex)
				{
#if DEBUG
					MessageBox.Show(ex.Message, "Connection Error");
#endif
					Thread.Sleep(1000);
					continue;
				}
				
				break;
			}

			if(client == null)
				return false;

			m_recvThread = new Thread(new ParameterizedThreadStart(ReceiveThread));
			m_receive = true;
			m_recvThread.Start(client);
			return true;
		}

		public bool Open(string dbFile)
		{
			if(!File.Exists(dbFile))
				return false;

			m_storage = new SqlServerCompactEngine(dbFile, false);
			this.Text = Path.GetFileName(dbFile);

			return true;
		}

		public void ReceiveThread(object data)
		{
			var client = (ProfilerClient) data;

			try
			{
				while(m_receive)
				{
					try
					{
						string text = client.Receive();
						if(text == null)
							break;
					}
					catch(System.Net.Sockets.SocketException)
					{
						m_receive = false;
					}
				}
			}
			finally
			{
				m_storage.Flush();
				client.Dispose();
			}
		}

		private void Results_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(m_receive && m_recvThread.ThreadState == System.Threading.ThreadState.Running)
			{
				DialogResult result = MessageBox.Show("Closing this window will disconnect the profiler. Close anyway?",
					"Profiler Still Connected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if(result == DialogResult.No)
					e.Cancel = true;
			}

			if(!e.Cancel)
			{
				m_receive = false;
				if(m_recvThread != null)
					m_recvThread.Join();

				if(m_storage != null)
					m_storage.Dispose();
			}
		}

		private void m_queryButton_Click(object sender, EventArgs e)
		{
			try
			{
				DataSet ds = m_storage.Query(m_queryTextBox.Text);
				if(ds != null)
				{
					m_dataGrid.DataSource = ds;
					m_dataGrid.DataMember = "Query";
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void m_clearDataButton_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(this, "WARNING: This will clear ALL profiling data received so far. This cannot be reversed. Are you sure?",
				"Irreversible Deletion Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if(result == DialogResult.Yes)
			{
				m_storage.ClearSamples();
			}
		}
	}
}
