using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace SlimTuneUI
{
	class SqlServerCompactEngine : IStorageEngine
	{
		//this is: ThreadId, CallerId, CalleeId, HitCount
		//Everything is stored sorted so that we can sprint through the database quickly
		SortedList<int, SortedDictionary<int, SortedList<int, int>>> m_callers;
		int m_cachedSamples = 0;
		const int MaxCachedSamples = 2000;

		SqlCeConnection m_sqlConn;
		SqlCeCommand m_addMappingCmd;
		SqlCeCommand m_callersCmd;
		SqlCeCommand m_threadsCmd;

		object m_lock = new object();

		public SqlServerCompactEngine(string dbFile)
		{
			string connStr = "Data Source='" + dbFile + "'; LCID=1033;";
			if(File.Exists(dbFile))
				File.Delete(dbFile);

			using(SqlCeEngine engine = new SqlCeEngine(connStr))
			{
				engine.CreateDatabase();
			}

			m_sqlConn = new SqlCeConnection(connStr);
			m_sqlConn.Open();
			CreateSchema();

			CreateCommands();
			m_callers = new SortedList<int, SortedDictionary<int, SortedList<int, int>>>();
		}

		public void MapFunction(FunctionInfo funcInfo)
		{
			var resultSet = m_addMappingCmd.ExecuteResultSet(ResultSetOptions.Updatable);
			var row = resultSet.CreateRecord();
			row["Id"] = funcInfo.FunctionId;
			row["IsNative"] = funcInfo.IsNative ? 1 : 0;
			row["Name"] = funcInfo.Name;
			resultSet.Insert(row);
		}

		public void ParseSample(Messages.Sample sample)
		{
			lock(m_lock)
			{
				//Update callers
				SortedDictionary<int, SortedList<int, int>> perThread;
				bool foundThread = m_callers.TryGetValue(sample.ThreadId, out perThread);
				if(!foundThread)
				{
					perThread = new SortedDictionary<int, SortedList<int, int>>();
					m_callers.Add(sample.ThreadId, perThread);
				}

				Increment(sample.Functions[0], 0, perThread);
				for(int f = 1; f < sample.Functions.Count; ++f)
				{
					Increment(sample.Functions[f], sample.Functions[f - 1], perThread);
				}

				++m_cachedSamples;
				if(m_cachedSamples >= MaxCachedSamples)
					Flush();
			}
		}

		public void ClearSamples()
		{
			lock(m_lock)
			{
				foreach(KeyValuePair<int, SortedDictionary<int, SortedList<int, int>>> threadKvp in m_callers)
				{
					int threadId = threadKvp.Key;
					foreach(KeyValuePair<int, SortedList<int, int>> callerKvp in threadKvp.Value)
					{
						callerKvp.Value.Clear();
					}
				}
				m_cachedSamples = 0;

				new SqlCeCommand("UPDATE Callers SET HitCount = 0", m_sqlConn).ExecuteNonQuery();
			}
		}

		public void UpdateThread(int threadId, bool? alive, string name)
		{
			using(var resultSet = m_threadsCmd.ExecuteResultSet(ResultSetOptions.Updatable))
			{
				int isAliveOrdinal = resultSet.GetOrdinal("IsAlive");
				int nameOrdinal = resultSet.GetOrdinal("Name");

				if(!resultSet.Seek(DbSeekOptions.FirstEqual, threadId))
				{
					var threadRow = resultSet.CreateRecord();
					threadRow["Id"] = threadId;
					if(alive.HasValue)
						threadRow[isAliveOrdinal] = alive.Value ? 1 : 0;
					else
						threadRow[isAliveOrdinal] = null;

					if(name != null)
						threadRow[nameOrdinal] = name;
					else
						threadRow[nameOrdinal] = threadId.ToString();

					resultSet.Insert(threadRow);
					return;
				}

				if(!resultSet.Read())
					return;

				if(alive.HasValue)
					resultSet.SetInt32(isAliveOrdinal, alive.Value ? 1 : 0);
				if(name != null)
					resultSet.SetString(nameOrdinal, name);
			}
		}

		public void Flush()
		{
			lock(m_lock)
			{
				Stopwatch timer = new Stopwatch();
				timer.Start();

				using(var resultSet = m_callersCmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Scrollable))
				{
					FlushInternal(resultSet);
					timer.Stop();
					Debug.WriteLine(string.Format("Database update took {0} milliseconds.", timer.ElapsedMilliseconds));
				}
			}
		}

		public DataSet Query(string query)
		{
			var command = new SqlCeCommand(query, m_sqlConn);

			if(query.Contains("@SampleCount"))
			{
				var sampleCountCmd = new SqlCeCommand("SELECT SUM(HitCount) FROM Callers WHERE CalleeId = 0", m_sqlConn);
				int sampleCount = (int) sampleCountCmd.ExecuteScalar();

				command.Parameters.Add("@SampleCount", sampleCount);
			}

			var adapter = new SqlCeDataAdapter(command);
			var ds = new DataSet();
			adapter.Fill(ds, "Query");
			return ds;
		}

		public void Dispose()
		{
			if(m_sqlConn != null)
				m_sqlConn.Dispose();
		}

		private void CreateSchema()
		{
			new SqlCeCommand("CREATE TABLE Mappings (Id INT PRIMARY KEY, IsNative INT NOT NULL, Name NVARCHAR (1024))", m_sqlConn).ExecuteNonQuery();

			//We will look up results in CallerId order when updating this table
			new SqlCeCommand("CREATE TABLE Callers (ThreadId INT NOT NULL, CallerId INT NOT NULL, CalleeId INT NOT NULL, HitCount INT)", m_sqlConn).ExecuteNonQuery();
			new SqlCeCommand("CREATE INDEX CallerIndex ON Callers(CallerId);", m_sqlConn).ExecuteNonQuery();
			new SqlCeCommand("CREATE INDEX CalleeIndex ON Callers(CalleeId);", m_sqlConn).ExecuteNonQuery();
			new SqlCeCommand("CREATE INDEX Compound ON Callers(ThreadId, CallerId, CalleeId);", m_sqlConn).ExecuteNonQuery();

			new SqlCeCommand("CREATE TABLE Threads (Id INT NOT NULL, IsAlive INT, Name NVARCHAR(256))", m_sqlConn).ExecuteNonQuery();
			new SqlCeCommand("ALTER TABLE Threads ADD CONSTRAINT pk_Id PRIMARY KEY (Id)", m_sqlConn).ExecuteNonQuery();
		}

		private void CreateCommands()
		{
			m_addMappingCmd = m_sqlConn.CreateCommand();
			m_addMappingCmd.CommandType = CommandType.TableDirect;
			m_addMappingCmd.CommandText = "Mappings";
			m_addMappingCmd.Parameters.Add("@Id", SqlDbType.Int);
			m_addMappingCmd.Parameters.Add("@Name", SqlDbType.NVarChar, Messages.MapFunction.MaxNameSize);

			m_callersCmd = m_sqlConn.CreateCommand();
			m_callersCmd.CommandType = CommandType.TableDirect;
			m_callersCmd.CommandText = "Callers";
			m_callersCmd.IndexName = "Compound";

			m_threadsCmd = m_sqlConn.CreateCommand();
			m_threadsCmd.CommandType = CommandType.TableDirect;
			m_threadsCmd.CommandText = "Threads";
			m_threadsCmd.IndexName = "pk_Id";
		}

		private void FlushInternal(SqlCeResultSet resultSet)
		{
			//a lock is already taken at this point
			int hitsOrdinal = resultSet.GetOrdinal("HitCount");
			int calleeOrdinal = resultSet.GetOrdinal("CalleeId");
			int callerOrdinal = resultSet.GetOrdinal("CallerId");
			int threadOrdinal = resultSet.GetOrdinal("ThreadId");

			//The CallerId has been set as the index, so that's going to be our main seek
			foreach(KeyValuePair<int, SortedDictionary<int, SortedList<int, int>>> threadKvp in m_callers)
			{
				int threadId = threadKvp.Key;
				foreach(KeyValuePair<int, SortedList<int, int>> callerKvp in threadKvp.Value)
				{
					int callerId = callerKvp.Key;
					foreach(KeyValuePair<int, int> hitsKvp in callerKvp.Value)
					{
						int calleeId = hitsKvp.Key;
						int hits = hitsKvp.Value;

						resultSet.Seek(DbSeekOptions.FirstEqual, threadId, callerId, calleeId);
						if(resultSet.Read())
						{
							//found it, update the hit count and move on
							hits += (int) resultSet[hitsOrdinal];
							resultSet.SetInt32(hitsOrdinal, hits);
							resultSet.Update();
						}
						else
						{
							//not in the db, create a new record
							CreateRecord(resultSet, threadId, callerId, calleeId, hits);
						}
					}
					//data is added, clear out the list
					callerKvp.Value.Clear();
				}
			}

			m_cachedSamples = 0;
		}

		private static void Increment(int key1, int key2, SortedDictionary<int, SortedList<int, int>> container)
		{
			//a lock is already taken at this point
			SortedList<int, int> key1Table;
			bool foundKey1Table = container.TryGetValue(key1, out key1Table);
			if(!foundKey1Table)
			{
				key1Table = new SortedList<int, int>();
				container.Add(key1, key1Table);
			}

			if(!key1Table.ContainsKey(key2))
			{
				key1Table.Add(key2, 1);
			}
			else
			{
				++key1Table[key2];
			}
		}

		private void CreateRecord(SqlCeResultSet resultSet, int threadId, int callerId, int calleeId, int hits)
		{
			//a lock is not needed
			var row = resultSet.CreateRecord();
			row["ThreadId"] = threadId;
			row["CallerId"] = callerId;
			row["CalleeId"] = calleeId;
			row["HitCount"] = hits;
			resultSet.Insert(row, DbInsertOptions.PositionOnInsertedRow);
		}
	}
}
