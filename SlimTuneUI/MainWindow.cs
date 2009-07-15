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

			/*string connStr = "Data Source='Temp1.sdf'; LCID=1033;";
			SqlCeEngine engine = new SqlCeEngine(connStr);
			engine.CreateDatabase();

			SqlCeConnection conn = new SqlCeConnection(connStr);

			SqlCeCommand command = conn.CreateCommand();*/

			//var form = new Results();
			//form.Show(m_dockPanel);
		}

		private void m_profilerRunMenu_Click(object sender, EventArgs e)
		{
			var runner = new RunDialog(this);
			runner.ShowDialog(this);
		}
	}
}
