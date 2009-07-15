/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
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

namespace SlimTuneUI
{
	public partial class Results : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		private const string ProfilerGuid_x86 = "{38A7EA35-B221-425a-AD07-D058C581611D}";

		Thread m_recvThread;
		volatile bool m_receive = false;

		public Results()
		{
			InitializeComponent();
		}

		public bool LaunchLocal(string exe, string args, string dbFile)
		{
			if(!File.Exists(exe))
				return false;

			var psi = new ProcessStartInfo(exe, args);
			psi.UseShellExecute = false;

			if(psi.EnvironmentVariables.ContainsKey("COR_ENABLE_PROFILING") == true)
				psi.EnvironmentVariables["COR_ENABLE_PROFILING"] = "1";
			else
				psi.EnvironmentVariables.Add("COR_ENABLE_PROFILING", "1");

			if(psi.EnvironmentVariables.ContainsKey("COR_PROFILER") == true)
				psi.EnvironmentVariables["COR_PROFILER"] = ProfilerGuid_x86;
			else
				psi.EnvironmentVariables.Add("COR_PROFILER", ProfilerGuid_x86);

			Process.Start(psi);

			//TODO: select host/port
			ProfilerClient client = null;
			for(int i = 0; i < 5; ++i)
			{
				try
				{
					client = new ProfilerClient("localhost", 200);
				}
				catch(System.Net.Sockets.SocketException)
				{
					Thread.Sleep(500);
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
					catch
					{
						m_receive = false;
					}
				}
			}
			finally
			{
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
				m_recvThread.Join();
			}
		}
	}
}
