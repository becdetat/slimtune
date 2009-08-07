using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SlimTuneUI
{
	[DisplayName("dotTrace Style Tree")]
	public partial class DotTraceStyle : WeifenLuo.WinFormsUI.Docking.DockContent, IVisualizer
	{
		const string kParentHits = @"
SELECT SUM(HitCount)
FROM Callers
WHERE CallerId = {0} AND ThreadId = {1}
";

		const string kTopLevelQuery = @"
SELECT Id, ThreadId, HitCount, Name + Signature AS ""Function""
FROM Samples
JOIN Functions ON FunctionId = Id
WHERE FunctionId NOT IN
(SELECT CalleeId FROM Callers)
AND Samples.HitCount IN
(SELECT MAX(S2.HitCount) FROM Samples AS ""S2"" GROUP BY ThreadId)
";

		const string kChildQuery = @"
SELECT C1.CalleeId AS ""Id"", HitCount, Name + Signature AS ""Function"", CASE TotalCalls
	WHEN 0 THEN 0
	ELSE (1.0 * C1.HitCount / TotalCalls)
	END AS ""Percent""
FROM Callers AS ""C1""
JOIN Functions
	ON C1.CalleeId = Id
JOIN (SELECT CallerId, SUM(HitCount) AS ""TotalCalls"" FROM Callers GROUP BY CallerId) AS ""C2""
	ON C1.CallerId = C2.CallerId
WHERE C1.CallerId = {0} AND ThreadId = {1}
ORDER BY HitCount DESC
";

		class NodeData
		{
			public int Id;
			public int ThreadId;
			public decimal Percent;
			public string FormattedString;

			public NodeData(int id, int threadId, decimal percent, string formattedString)
			{
				Id = id;
				ThreadId = threadId;
				Percent = percent;
				FormattedString = formattedString;
			}
		}

		Regex m_regex;
		List<Font> m_fontList = new List<Font>();
		List<Brush> m_brushList = new List<Brush>();

		MainWindow m_mainWindow;
		Connection m_connection;

		public DotTraceStyle()
		{
			InitializeComponent();

			m_regex = new Regex(@"\\([0-9])");
			const string fontName = "Arial";
			const int fontSize = 9;
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Regular), Brushes.Black);
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Regular), Brushes.Gray);
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Bold), Brushes.Gray);
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Bold), Brushes.Blue);
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Bold), Brushes.Black);
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Regular), Brushes.Blue);
			AddTreeFont(new Font(fontName, fontSize, FontStyle.Bold), Brushes.Green);
		}

		private void AddTreeFont(Font font, Brush brush)
		{
			m_fontList.Add(font);
			m_brushList.Add(brush);
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

			this.Text = Utilities.GetStandardCaption(connection);
			UpdateTopLevel();
		}

		void m_connection_Closing(object sender, EventArgs e)
		{
			if(!this.IsDisposed)
				this.Invoke((Action) delegate { this.Close(); });
		}

		private static void BreakName(string name, out string signature, out string funcName, out string classAndFunc, out string baseName)
		{
			int sigIndex = name.IndexOf('(');
			signature = sigIndex >= 0 ? name.Substring(sigIndex) : string.Empty;
			if(sigIndex == 0)
				sigIndex = name.Length - 1;

			int funcNameIndex = name.LastIndexOf('.', sigIndex) + 1;
			if(funcNameIndex <= 1)
			{
				//special case for ..ctor mainly
				funcName = name.Substring(0, sigIndex);
				classAndFunc = funcName;
				baseName = string.Empty;
				return;
			}
			funcName = funcNameIndex >= 0 ? name.Substring(funcNameIndex, sigIndex - funcNameIndex) : string.Empty;

			int classIndex = name.LastIndexOf('.', funcNameIndex - 2) + 1;
			classAndFunc = classIndex >= 1 ? name.Substring(classIndex, sigIndex - classIndex) : funcName;
			if(classIndex <= 1)
				classIndex = 0;

			baseName = name.Substring(0, classIndex);
		}

		private void UpdateTopLevel()
		{
			using(var transact = new TransactionHandle(m_connection.StorageEngine))
			{
				//top level queries
				var data = m_connection.StorageEngine.Query(kTopLevelQuery);

				int totalHits = 0;
				foreach(DataRow row in data.Tables[0].Rows)
				{
					totalHits += (int) row["HitCount"];
				}

				foreach(DataRow row in data.Tables[0].Rows)
				{
					string name = (string) row["Function"];
					//TODO: Replace with proper filters
					string rawString = @"{0:P2} Thread #{1} - {2}{3}{4}";
					string niceString = @"\3{0:P2} \4Thread #{1} \0- {2}\4{3}\0{4}";

					string signature, funcName, classAndFunc, baseName;
					BreakName(name, out signature, out funcName, out classAndFunc, out baseName);
					decimal percent = (int) row["HitCount"] / (decimal) totalHits;
					int threadId = (int) row["ThreadId"];

					string nodeText = string.Format(rawString, percent, threadId, baseName, classAndFunc, signature);
					string formatString = string.Format(niceString, percent, threadId, baseName, classAndFunc, signature);

					TreeNode newNode = new TreeNode(nodeText, new TreeNode[] { new TreeNode("dummy") });
					newNode.Tag = new NodeData((int) row["Id"], (int) row["ThreadId"], percent, formatString);
					m_treeView.Nodes.Add(newNode);
				}
			}
		}

		private void UpdateChildren(TreeNode node)
		{
			using(var transact = new TransactionHandle(m_connection.StorageEngine))
			{
				var parent = (NodeData) node.Tag;
				var data = m_connection.StorageEngine.Query(string.Format(kChildQuery, parent.Id, parent.ThreadId));

				//find out what the current number of calls by the parent is
				var parentHits = (int) m_connection.StorageEngine.QueryScalar(string.Format(kParentHits, parent.Id, parent.ThreadId));
				foreach(DataRow row in data.Tables[0].Rows)
				{
					string name = (string) row["Function"];
					string rawString = @"{0:P2} {1} - {2:P2} - {3}{4}{5}";
					string niceString;
					//TODO: Replace with proper filters
					if(name.StartsWith("Microsoft.") || name.StartsWith("System."))
					{
						niceString = @"\2{0:P2} {1} \1- {2:P2} - {3}\2{4}\1{5}";
					}
					else
					{
						niceString = @"\3{0:P2} \4{1} \0- \5{2:P2} \0- {3}\4{4}\0{5}";
					}

					string signature, funcName, classAndFunc, baseName;
					BreakName(name, out signature, out funcName, out classAndFunc, out baseName);
					decimal percentOfParent = (decimal) row["Percent"];
					decimal percent = percentOfParent * parent.Percent;

					string nodeText = string.Format(rawString, percent, funcName, percentOfParent, baseName, classAndFunc, signature);
					string formatString = string.Format(niceString, percent, funcName, percentOfParent,
						baseName, classAndFunc, signature);

					TreeNode newNode = new TreeNode(nodeText, new TreeNode[] { new TreeNode() });
					newNode.Tag = new NodeData((int) row["Id"], (int) parent.ThreadId, percent, formatString);
					node.Nodes.Add(newNode);
				}
			}
		}

		private void m_treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			e.Node.Nodes.Clear();
			UpdateChildren(e.Node);
			if(e.Node.Nodes.Count == 1)
				e.Node.Nodes[0].Expand();
		}

		private void m_treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			NodeData data = e.Node.Tag as NodeData;
			if(data == null)
			{
				e.DrawDefault = true;
				return;
			}

			string text = data.FormattedString;
			var graphics = e.Graphics;
			Font currentFont = e.Node.NodeFont ?? e.Node.TreeView.Font;
			Brush currentBrush = (e.State & TreeNodeStates.Selected) != 0 ? Brushes.White : Brushes.Black;

			var matches = m_regex.Matches(text);
			if(matches.Count == 0)
			{
				e.DrawDefault = true;
				return;
			}

			bool parentSelected = e.Node.Parent != null &&  e.Node.Parent.IsSelected;
			if(parentSelected)
				graphics.FillRectangle(Brushes.AliceBlue, e.Bounds);

			CharacterRange[] ranges = new CharacterRange[1];
			ranges[0].First = 0;
			StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);

			int offset = 0;
			float drawPos = e.Bounds.X;
			foreach(Match m in matches)
			{
				if(m.Index != offset)
				{
					string substr = text.Substring(offset, m.Index - offset);
					graphics.DrawString(substr, currentFont, currentBrush, drawPos, e.Bounds.Y);

					ranges[0].Length = substr.Length;
					format.SetMeasurableCharacterRanges(ranges);
					var regions = graphics.MeasureCharacterRanges(substr, currentFont, new RectangleF(0, 0, 1000, 1000), format);
					var rect = regions[0].GetBounds(graphics);
					drawPos += rect.Width;
				}

				offset = m.Index + m.Length;
				int index = int.Parse(m.Groups[1].Value);
				currentFont = m_fontList[index];
				if((e.State & TreeNodeStates.Selected) == 0)
					currentBrush = m_brushList[index];
			}
			string final = text.Substring(offset);
			graphics.DrawString(final, currentFont, currentBrush, drawPos, e.Bounds.Y);
		}

		private void m_treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			foreach(TreeNode child in e.Node.Nodes)
			{
				m_treeView.Invalidate(child.Bounds);
			}
		}

		private void m_treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if(m_treeView.SelectedNode == null)
				return;

			foreach(TreeNode child in m_treeView.SelectedNode.Nodes)
			{
				m_treeView.Invalidate(child.Bounds);
			}
		}

		private void m_treeView_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			//clear the node's children back to dummy
			e.Node.Nodes.Clear();
			e.Node.Nodes.Add(new TreeNode());
		}
	}
}
