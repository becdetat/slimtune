/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace SlimTuneUI
{
	class ThreadInfo
	{
		//public long ThreadId;
		//public string Name;
	}

	class FunctionInfo
	{
		public int FunctionId;
		public string Name;
		public int Hits;

		public FunctionInfo()
		{
		}

		public FunctionInfo(int funcId, string name)
		{
			FunctionId = funcId;
			Name = name;
			Hits = 0;
		}
	}

	/*struct CallerData
	{
		public long ThreadId;
		public int CallerId;
		public int CalleeId;
		public int HitCount;
	}*/

	class ProfilerClient : IDisposable
	{
		TcpClient m_client;
		NetworkStream m_stream;
		BinaryReader m_reader;
		BinaryWriter m_writer;
		Dictionary<int, FunctionInfo> m_functions = new Dictionary<int, FunctionInfo>();
		Dictionary<long, ThreadInfo> m_threads = new Dictionary<long, ThreadInfo>();

		//this is: ThreadId, CallerId, CalleeId, HitCount
		//Everything is stored sorted so that we can sprint through the database quickly
		SortedList<int, SortedDictionary<int, SortedList<int, int>>> m_callers;
		int m_cachedSamples = 0;
		const int MaxCachedSamples = 3000;

		SqlCeConnection m_sqlConn;
		SqlCeCommand m_addMappingCmd;
		SqlCeCommand m_callersCmd;

		public Dictionary<int, FunctionInfo> Functions
		{
			get { return m_functions; }
		}

		public ProfilerClient(string server, int port, SqlCeConnection sqlConn)
		{
			m_client = new TcpClient();
			m_client.Connect("localhost", 200);
			m_stream = m_client.GetStream();
			m_reader = new BinaryReader(m_stream, Encoding.Unicode);
			m_writer = new BinaryWriter(m_stream, Encoding.Unicode);
			m_sqlConn = sqlConn;

			CreateCommands();
			m_callers = new SortedList<int, SortedDictionary<int, SortedList<int, int>>>();

			Debug.WriteLine("Successfully connected.");
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
		}

		private static void Increment(int key1, int key2, SortedDictionary<int, SortedList<int, int>> container)
		{
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

		private void ParseSample(Messages.Sample sample)
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
		}

		void CreateRecord(SqlCeResultSet resultSet, int threadId, int callerId, int calleeId, int hits)
		{
			var row = resultSet.CreateRecord();
			row["ThreadId"] = threadId;
			row["CallerId"] = callerId;
			row["CalleeId"] = calleeId;
			row["HitCount"] = hits;
			resultSet.Insert(row, DbInsertOptions.PositionOnInsertedRow);
		}

		public void FlushData()
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();

			using(var resultSet = m_callersCmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Scrollable))
			{
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

				timer.Stop();
				Debug.WriteLine(string.Format("Database update took {0} milliseconds.", timer.ElapsedMilliseconds));
			}
		}

		public string Receive()
		{
			try
			{
				if(m_stream == null)
					return string.Empty;

				MessageId messageId = (MessageId) m_reader.ReadByte();
				switch(messageId)
				{
					case MessageId.MID_MapFunction:
						var mapFunc = Messages.MapFunction.Read(m_reader);
						FunctionInfo funcInfo = new FunctionInfo();
						funcInfo.FunctionId = mapFunc.FunctionId;
						funcInfo.Name = mapFunc.Name;
						m_functions.Add(funcInfo.FunctionId, funcInfo);

						var resultSet = m_addMappingCmd.ExecuteResultSet(ResultSetOptions.Updatable);
						var row = resultSet.CreateRecord();
						row["Id"] = funcInfo.FunctionId;
						row["Name"] = funcInfo.Name;
						resultSet.Insert(row);

						Debug.WriteLine(string.Format("Mapped {0} to {1}.", mapFunc.Name, mapFunc.FunctionId));
						break;

					case MessageId.MID_EnterFunction:
					case MessageId.MID_LeaveFunction:
					case MessageId.MID_TailCall:
						var funcEvent = Messages.FunctionEvent.Read(m_reader);
						if(!m_functions.ContainsKey(funcEvent.FunctionId))
							m_functions.Add(funcEvent.FunctionId, new FunctionInfo(funcEvent.FunctionId, "{Unknown}"));

						if(messageId == MessageId.MID_EnterFunction)
							m_functions[funcEvent.FunctionId].Hits++;

						break;

					case MessageId.MID_CreateThread:
					case MessageId.MID_DestroyThread:
						var threadEvent = Messages.CreateThread.Read(m_reader);
						Debug.WriteLine(string.Format("{0}: Thread Id {1}.", messageId, threadEvent.ThreadId));
						break;

					case MessageId.MID_NameThread:
						var nameThread = Messages.NameThread.Read(m_reader);
						Debug.WriteLine(string.Format("Renamed Thread {0} to {1}.", nameThread.ThreadId, nameThread.Name));
						break;

					case MessageId.MID_Sample:
						var sample = Messages.Sample.Read(m_reader, m_functions);
						ParseSample(sample);
						if(m_cachedSamples >= MaxCachedSamples)
							FlushData();
						break;

					default:
						throw new InvalidOperationException();
				}

				return string.Empty;
			}
			catch(IOException)
			{
				return null;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			m_stream.Dispose();
		}

		#endregion
	}
}
