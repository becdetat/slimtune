/*
Copyright (c) 2009  Promit Roy

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.

You should have received a copy of the GNU Library General Public
License along with this library; if not, write to the
Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
Boston, MA  02110-1301, USA.
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
		public string Class;
		public int Hits;

		public FunctionInfo()
		{
		}

		public FunctionInfo(int funcId, string name, string className)
		{
			FunctionId = funcId;
			Name = name;
			Class = className;
			Hits = 0;
		}
	}

	class ProfilerClient : IDisposable
	{
		TcpClient m_client;
		NetworkStream m_stream;
		BinaryReader m_reader;
		BinaryWriter m_writer;
		Dictionary<int, FunctionInfo> m_functions = new Dictionary<int,FunctionInfo>();
		Dictionary<long, ThreadInfo> m_threads = new Dictionary<long, ThreadInfo>();

		public Dictionary<int, FunctionInfo> Functions
		{
			get { return m_functions; }
		}

		public ProfilerClient(string server, int port)
		{
			m_client = new TcpClient();
			m_client.Connect("localhost", 200);
			m_stream = m_client.GetStream();
			m_reader = new BinaryReader(m_stream, Encoding.Unicode);
			m_writer = new BinaryWriter(m_stream, Encoding.Unicode);

			Debug.WriteLine("Successfully connected.");
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
						funcInfo.Class = mapFunc.Class;
						m_functions.Add(funcInfo.FunctionId, funcInfo);

						Debug.WriteLine(string.Format("Mapped {0}.{1} to {2}.", mapFunc.Class, mapFunc.Name, mapFunc.FunctionId));
						break;

					case MessageId.MID_EnterFunction:
					case MessageId.MID_LeaveFunction:
					case MessageId.MID_TailCall:
						var funcEvent = Messages.FunctionEvent.Read(m_reader);
						//Debug.WriteLine(string.Format("{3}: {0}: Function Id {1} in thread {2}.", messageId, funcEvent.FunctionId, funcEvent.ThreadId, funcEvent.TimeStamp));
						if(!m_functions.ContainsKey(funcEvent.FunctionId))
							m_functions.Add(funcEvent.FunctionId, new FunctionInfo(funcEvent.FunctionId, "{Unknown}", "{Unknown}"));

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
						foreach(int func in sample.Functions)
						{
							m_functions[func].Hits++;
						}

						break;

					default:
						throw new InvalidOperationException();
				}

				return string.Empty;
			}
			catch(Exception)
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
