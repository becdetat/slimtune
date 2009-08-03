using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SlimTuneUI
{
	public partial class ChartVisualizer : WeifenLuo.WinFormsUI.Docking.DockContent, IVisualizer
	{
		MainWindow m_mainWindow;
		Connection m_connection;
		Stack<int> m_callerIds = new Stack<int>();

		public ChartVisualizer()
		{
			InitializeComponent();
			m_callerIds.Push(0);
			m_backButton.Enabled = false;
		}

		public void Initialize(MainWindow mainWindow, Connection connection)
		{
			if(mainWindow == null)
				throw new ArgumentNullException("mainWindow");
			if(connection == null)
				throw new ArgumentNullException("connection");

			m_mainWindow = mainWindow;
			m_connection = connection;

			this.Text = Utilities.GetStandardCaption(connection);
			m_refreshTimer.Enabled = true;
		}

		private void m_refreshTimer_Tick(object sender, EventArgs e)
		{
			m_connection.StorageEngine.AllowFlush = false;
			var totalHitsData = m_connection.StorageEngine.Query("SELECT SUM(HitCount) FROM Callers WHERE CalleeId = 0");
			int totalHits = (int) totalHitsData.Tables[0].Rows[0][0];
			double hitsMultiplier = 1.0f / totalHits;

			int currentId = m_callerIds.Peek();
			DataSet callersData = null;
			DataSet calleesData = null;
			if(currentId == 0)
			{
				m_chart.Titles[0].Text = "Top Functions (Inclusive)";
				m_chart.Titles[1].Text = "Top Functions (Exclusive)";

				calleesData = m_connection.StorageEngine.Query(string.Format(
					"SELECT TOP(10) Id, Name, HitCount * {0} AS \"Time\", 100.0 * HitCount * {0} AS \"Percent\" FROM Samples JOIN Functions ON Id = FunctionId WHERE ThreadId = 1 AND HitCount > {1} ORDER BY HitCount DESC",
					hitsMultiplier, totalHits / 500.0));

				callersData = m_connection.StorageEngine.Query(string.Format(
					"SELECT TOP(10) Id, Name, HitCount * {0} AS \"Time\", 100.0 * HitCount * {0} AS \"Percent\" FROM Callers JOIN Functions ON Id = CallerId WHERE ThreadId = 1 AND CalleeId = 0 ORDER BY HitCount DESC",
					hitsMultiplier));
			}
			else
			{
				var nameData = m_connection.StorageEngine.Query("SELECT Name + Signature FROM Functions WHERE Id = " + currentId);
				m_chart.Titles[0].Text = "Called by " + (string) nameData.Tables[0].Rows[0][0];
				m_chart.Titles[1].Text = "Called " + (string) nameData.Tables[0].Rows[0][0];

				calleesData = m_connection.StorageEngine.Query(string.Format(
					"SELECT TOP(10) Id, Name, Signature, HitCount * {0} AS \"Time\", 100.0 * HitCount * {0} AS \"Percent\" FROM Callers JOIN Functions ON Id = CalleeId WHERE CallerId = {1} AND ThreadId = 1 ORDER BY HitCount DESC",
					hitsMultiplier, currentId));

				callersData = m_connection.StorageEngine.Query(string.Format(
					"SELECT TOP(10) Id, Name, Signature, HitCount * {0} AS \"Time\", 100.0 * HitCount * {0} AS \"Percent\" FROM Callers JOIN Functions ON Id = CallerId WHERE CalleeId = {1} AND ThreadId = 1 ORDER BY HitCount DESC",
					hitsMultiplier, currentId));
			}

			m_chart.Series[0].Points.DataBind(calleesData.Tables[0].Rows, "Name", "Time", "ToolTip=Percent,Id=Id");
			m_chart.Series[1].Points.DataBind(callersData.Tables[0].Rows, "Name", "Time", "ToolTip=Percent,Id=Id");
			m_connection.StorageEngine.AllowFlush = true;
		}

		private void m_chart_MouseMove(object sender, MouseEventArgs e)
		{
			var hit = m_chart.HitTest(e.X, e.Y);

			foreach(Series series in m_chart.Series)
			{
				//clear attribs
				foreach(var point in series.Points)
				{
					point.BackHatchStyle = ChartHatchStyle.None;
					point.BorderWidth = 1;
				}
			}

			if(hit.Series != null && hit.PointIndex >= 0 && hit.PointIndex < hit.Series.Points.Count)
			{
				this.Cursor = Cursors.Hand;
				DataPoint selectedPoint = hit.Series.Points[hit.PointIndex];
				selectedPoint.BackSecondaryColor = Color.White;
				selectedPoint.BackHatchStyle = ChartHatchStyle.OutlinedDiamond;
				selectedPoint.BorderWidth = 2;
			}
			else
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void m_chart_MouseClick(object sender, MouseEventArgs e)
		{
			var hit = m_chart.HitTest(e.X, e.Y);
			if(hit.Series != null && hit.PointIndex >= 0 && hit.PointIndex < hit.Series.Points.Count)
			{
				DataPoint selectedPoint = hit.Series.Points[hit.PointIndex];
				m_callerIds.Push(int.Parse(selectedPoint["Id"]));
				m_backButton.Enabled = true;
			}
		}

		private void m_backButton_Click(object sender, EventArgs e)
		{
			if(m_callerIds.Count > 1)
				m_callerIds.Pop();
			if(m_callerIds.Count == 1)
				m_backButton.Enabled = false;
		}
	}
}
