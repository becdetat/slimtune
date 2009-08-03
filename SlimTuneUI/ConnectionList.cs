using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SlimTuneUI
{
	public partial class ConnectionList : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		BindingSource m_source = new BindingSource();
		BindingList<Connection> m_connections = new BindingList<Connection>();
		public IEnumerable<Connection> Connections
		{
			get
			{
				foreach(Connection conn in m_source)
				{
					yield return conn;
				}
			}
		}

		public ConnectionList()
		{
			InitializeComponent();

			m_source.DataSource = m_connections;
			m_connectionList.DataSource = m_source;
			m_connectionList.DisplayMember = "Name";
		}

		public void AddConnection(Connection connection)
		{
			m_source.Add(connection);
			connection.Disconnected += new EventHandler(Connection_Disconnected);
		}

		void Connection_Disconnected(object sender, EventArgs e)
		{
		}
	}
}
