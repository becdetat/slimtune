using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace SlimTuneUI
{
	[DisplayName("NProf-Style TreeViews")]
	public partial class NProfStyleVisualizer : WeifenLuo.WinFormsUI.Docking.DockContent, IVisualizer
	{
		MainWindow m_mainWindow;
		Connection m_connection;

		CalleesModel m_calleesModel;
		CallersModel m_callersModel;

		public NProfStyleVisualizer()
		{
			InitializeComponent();
		}

		public void Initialize(MainWindow mainWindow, Connection connection)
		{
			if(mainWindow == null)
				throw new ArgumentNullException("mainWindow");
			if(connection == null)
				throw new ArgumentNullException("connection");

			m_mainWindow = mainWindow;
			m_connection = connection;
			m_connection.Closing += new EventHandler(m_connection_Closing);

			m_calleesModel = new CalleesModel(connection.StorageEngine);
			m_callersModel = new CallersModel(connection.StorageEngine);
			m_callees.Model = new SortedTreeModel(m_calleesModel);
			m_callers.Model = new SortedTreeModel(m_callersModel);

			this.Text = Utilities.GetStandardCaption(connection);
		}

		void m_connection_Closing(object sender, EventArgs e)
		{
			if(!this.IsDisposed)
				this.Invoke((Action) delegate { this.Close(); });
		}

		private void m_refreshButton_Click(object sender, EventArgs e)
		{
			m_calleesModel.Refresh();
			m_callersModel.Refresh();
		}

		private void ColumnClicked(object sender, TreeColumnEventArgs e)
		{
			TreeColumn clicked = e.Column;

			if(clicked.SortOrder == SortOrder.None)
				clicked.SortOrder = SortOrder.Descending;
			else if(clicked.SortOrder == SortOrder.Descending)
				clicked.SortOrder = SortOrder.Ascending;
			else if(clicked.SortOrder == SortOrder.Ascending)
				clicked.SortOrder = SortOrder.None;

			var tree = sender as TreeViewAdv;
			(tree.Model as SortedTreeModel).Comparer = new FunctionComparer(clicked.Header, clicked.SortOrder);
		}
	}

	class FunctionComparer : System.Collections.IComparer, IComparer<FunctionItem>
	{
		//this is incredibly stupid
		public string Mode;
		public SortOrder Order;

		public FunctionComparer(string mode, SortOrder order)
		{
			this.Mode = mode;
			this.Order = order;
		}

		public int Compare(FunctionItem x, FunctionItem y)
		{
			int result = 0;
			switch(Mode)
			{
				case "Id":
					result = x.Id.CompareTo(y.Id);
					break;
				case "Thread":
					result = x.Thread.CompareTo(y.Thread);
					break;
				case "Callers":
				case "Callees":
					result = x.Name.CompareTo(y.Name);
					break;
				case "% of Parent":
				case "% Time":
					result = x.PercentParent.CompareTo(y.PercentParent);
					break;
				case "% Calls":
					if(x.PercentCalls.HasValue && y.PercentCalls.HasValue)
						result = x.PercentCalls.Value.CompareTo(y.PercentCalls.Value);
					else
						result = x.PercentCalls.HasValue.CompareTo(y.PercentCalls.HasValue);
					break;
			}

			if(Order == SortOrder.Ascending)
				result = -result;
			return result;
		}

		public int Compare(object x, object y)
		{
			return Compare(x as FunctionItem, y as FunctionItem);
		}
	}

	class FunctionItem
	{
		public int Id { get; set; }
		public int Thread { get; set; }
		public string Name { get; set; }
		public int HitCount { get; set; }
		public decimal PercentParent { get; set; }
		public decimal? PercentCalls { get; set; }
	}

	class CalleesModel : ITreeModel
	{
		const string kParentHits = @"
SELECT SUM(HitCount)
FROM Callers
WHERE CallerId = {0} AND ThreadId = {1}
";

		const string kTopLevelQuery = @"
SELECT Samples.ThreadId, Id, HitCount, Name + Signature AS ""Function"", (1.0 * HitCount / TotalHits) AS ""Percent""
FROM Samples
JOIN Functions
	ON Id = FunctionId
JOIN (SELECT ThreadId, MAX(HitCount) AS ""TotalHits"" FROM Samples GROUP BY ThreadId) AS ""Totals""
	ON Samples.ThreadId = Totals.ThreadId
ORDER BY HitCount DESC
";

		const string kChildQuery = @"
SELECT C1.CalleeId, HitCount, Name + Signature AS ""Function"", (1.0 * C1.HitCount / TotalCalls) AS ""% Calls""
FROM Callers AS ""C1""
JOIN Functions
	ON C1.CalleeId = Id
JOIN (SELECT CalleeId, SUM(HitCount) AS ""TotalCalls"" FROM Callers GROUP BY CalleeId) AS ""C2""
	ON C1.CalleeId = C2.CalleeId
WHERE C1.CallerId = {0} AND ThreadId = {1}
ORDER BY HitCount DESC
";

		IStorageEngine m_storage;

		public CalleesModel(IStorageEngine storage)
		{
			m_storage = storage;
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			using(var transact = new TransactionHandle(m_storage))
			{
				if(treePath.IsEmpty())
				{
					//top level queries
					var data = m_storage.Query(kTopLevelQuery);

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Id = (int) row["Id"];
						item.Thread = (int) row["ThreadId"];
						item.Name = (string) row["Function"];
						item.HitCount = (int) row["HitCount"];
						item.PercentParent = Math.Round(100 * (decimal) row["Percent"], 3);
						yield return item;
					}
				}
				else
				{
					var parent = treePath.LastNode as FunctionItem;
					var data = m_storage.Query(string.Format(kChildQuery, parent.Id, parent.Thread));

					//find out what the current number of calls by the parent is
					var parentHits = (int) m_storage.QueryScalar(string.Format(kParentHits, parent.Id, parent.Thread));
					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Thread = (int) parent.Thread;
						item.Id = (int) row["CalleeId"];
						item.Name = (string) row["Function"];
						item.HitCount = (int) row["HitCount"];
						item.PercentParent = Math.Round(100 * (decimal) item.HitCount / (decimal) parentHits, 3);
						item.PercentCalls = Math.Round(100 * (decimal) row["% Calls"], 3);
						yield return item;
					}
				}

				yield break;
			}
		}

		public bool IsLeaf(TreePath treePath)
		{
			return false;
		}

		public void Refresh()
		{
			StructureChanged(this, new TreePathEventArgs());
		}

#pragma warning disable 67
		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
#pragma warning restore
	}

	class CallersModel : ITreeModel
	{
		const string kTopLevelQuery = @"
SELECT Callers.ThreadId, Id, Name + Signature AS ""Function"", HitCount, (1.0 * HitCount / TotalHits) AS ""Percent""
FROM Callers
JOIN Functions
	ON Id = CallerId
JOIN (SELECT ThreadId, SUM(HitCount) AS ""TotalHits"" FROM Callers WHERE CalleeId = 0 GROUP BY ThreadId) AS ""Totals""
	ON Callers.ThreadId = Totals.ThreadId
WHERE CalleeId = 0
ORDER BY HitCount DESC
";

		const string kChildQuery = @"
SELECT Id, HitCount, Name + Signature AS ""Function"", (1.0 * HitCount / TotalCalls) AS ""Percent""
FROM Callers
JOIN Functions
	ON Id = CallerId
JOIN (SELECT CalleeId, SUM(HitCount) AS ""TotalCalls"" FROM Callers WHERE ThreadId = {1} GROUP BY CalleeId) AS ""Totals""
	ON Callers.CalleeId = Totals.CalleeId
WHERE Callers.CalleeId = {0} AND ThreadId = {1}
ORDER BY HitCount DESC
";

		IStorageEngine m_storage;

		public CallersModel(IStorageEngine storage)
		{
			m_storage = storage;
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			using(var transact = new TransactionHandle(m_storage))
			{
				if(treePath.IsEmpty())
				{
					//top level queries
					var data = m_storage.Query(kTopLevelQuery);

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Id = (int) row["Id"];
						item.Thread = (int) row["ThreadId"];
						item.Name = (string) row["Function"];
						item.HitCount = (int) row["HitCount"];
						item.PercentParent = Math.Round(100 * (decimal) row["Percent"], 3);
						yield return item;
					}
				}
				else
				{
					var parent = treePath.LastNode as FunctionItem;
					var data = m_storage.Query(string.Format(kChildQuery, parent.Id, parent.Thread));

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Thread = (int) parent.Thread;
						item.Id = (int) row["Id"];
						item.Name = (string) row["Function"];
						item.HitCount = (int) row["HitCount"];
						item.PercentParent = Math.Round(100 * (decimal) row["Percent"], 3);
						yield return item;
					}
				}

				yield break;
			}
		}

		public bool IsLeaf(TreePath treePath)
		{
			return false;
		}

		public void Refresh()
		{
			StructureChanged(this, new TreePathEventArgs());
		}

#pragma warning disable 67
		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
#pragma warning restore
	}
}
