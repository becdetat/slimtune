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
			m_callees.Model = m_calleesModel;
			m_callers.Model = m_callersModel;

			this.Text = Utilities.GetStandardCaption(connection);
		}

		void m_connection_Closing(object sender, EventArgs e)
		{
			this.Close();
		}

		private void m_refreshButton_Click(object sender, EventArgs e)
		{
			m_calleesModel.Refresh();
			m_callersModel.Refresh();
		}
	}

	class FunctionItem
	{
		public int Id { get; set; }
		public int Thread { get; set; }
		public string Name { get; set; }
		public double Time { get; set; }
	}

	class CalleesModel : ITreeModel
	{
		IStorageEngine m_storage;

		public CalleesModel(IStorageEngine storage)
		{
			m_storage = storage;
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			using(var transact = new TransactionHandle(m_storage))
			{
				var totalHitsData = m_storage.Query("SELECT MAX(HitCount) FROM Samples");
				if(totalHitsData.Tables[0].Rows.Count == 0)
					yield break;
				object totalHitsObj = totalHitsData.Tables[0].Rows[0][0];
				if(!(totalHitsObj is int))
					yield break;

				int totalHits = (int) totalHitsObj;
				double hitsMultiplier = 100.0f / totalHits;

				if(treePath.IsEmpty())
				{
					//top level queries
					var data = m_storage.Query("SELECT ThreadId, Id, Name, Signature, HitCount FROM Samples JOIN Functions ON Id = FunctionId ORDER BY HitCount DESC");

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Id = (int) row["Id"];
						item.Thread = (int) row["ThreadId"];
						item.Name = (string) row["Name"] + (string) row["Signature"];
						item.Time = Math.Round(((int) row["HitCount"]) * hitsMultiplier, 2);
						yield return item;
					}
				}
				else
				{
					var parent = treePath.LastNode as FunctionItem;
					var data = m_storage.Query(string.Format(
						"SELECT Id, Name, Signature, HitCount FROM Callers JOIN Functions ON Id = CalleeId WHERE CallerId = {0} AND ThreadId = {1} ORDER BY HitCount DESC",
						parent.Id, parent.Thread));

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Thread = (int) parent.Thread;
						item.Id = (int) row["Id"];
						if(item.Id == 0)
							item.Name = "(self)";
						else
							item.Name = (string) row["Name"] + (string) row["Signature"];
						item.Time = Math.Round(((int) row["HitCount"]) * hitsMultiplier, 2);
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
		IStorageEngine m_storage;

		public CallersModel(IStorageEngine storage)
		{
			m_storage = storage;
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			using(var transact = new TransactionHandle(m_storage))
			{
				var totalHitsData = m_storage.Query("SELECT MAX(HitCount) FROM Samples");
				if(totalHitsData.Tables[0].Rows.Count == 0)
					yield break;
				object totalHitsObj = totalHitsData.Tables[0].Rows[0][0];
				if(!(totalHitsObj is int))
					yield break;

				int totalHits = (int) totalHitsData.Tables[0].Rows[0][0];
				double hitsMultiplier = 100.0f / totalHits;

				if(treePath.IsEmpty())
				{
					//top level queries
					var data = m_storage.Query("SELECT ThreadId, Id, Name, Signature, HitCount FROM Callers JOIN Functions ON Id = CallerId WHERE CalleeId = 0 ORDER BY HitCount DESC");

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Id = (int) row["Id"];
						item.Thread = (int) row["ThreadId"];
						item.Name = (string) row["Name"] + (string) row["Signature"];
						item.Time = Math.Round(((int) row["HitCount"]) * hitsMultiplier, 2);
						yield return item;
					}
				}
				else
				{
					var parent = treePath.LastNode as FunctionItem;
					var data = m_storage.Query(string.Format(
						"SELECT Id, Name, Signature, HitCount FROM Callers JOIN Functions ON Id = CallerId WHERE CalleeId = {0} AND ThreadId = {1} ORDER BY HitCount DESC",
						parent.Id, parent.Thread));

					foreach(DataRow row in data.Tables[0].Rows)
					{
						var item = new FunctionItem();
						item.Thread = (int) parent.Thread;
						item.Id = (int) row["Id"];
						if(item.Id == 0)
							item.Name = "(self)";
						else
							item.Name = (string) row["Name"] + (string) row["Signature"];
						item.Time = Math.Round(((int) row["HitCount"]) * hitsMultiplier, 2);
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
