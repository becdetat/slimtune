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

namespace SlimTuneUI
{
	public enum ProfilerMode
	{
		PM_Disabled = 0,

		PM_Sampling = 0x01,
		PM_Tracing = 0x02,

		PM_Hybrid = PM_Sampling | PM_Tracing,
	}

	class ThreadInfo
	{
		//public long ThreadId;
		//public string Name;
	}

	class ProfilerClient : IDisposable
	{
		TcpClient m_client;
		NetworkStream m_stream;
		BinaryReader m_reader;
		BinaryWriter m_writer;
		Dictionary<int, FunctionInfo> m_functions = new Dictionary<int, FunctionInfo>();
		Dictionary<int, ClassInfo> m_classes = new Dictionary<int, ClassInfo>();
		Dictionary<long, ThreadInfo> m_threads = new Dictionary<long, ThreadInfo>();

		IStorageEngine m_storage;

		public Dictionary<int, FunctionInfo> Functions
		{
			get { return m_functions; }
		}

		public ProfilerClient(string host, int port, IStorageEngine storage)
		{
			m_client = new TcpClient();
			m_client.Connect(host, port);
			m_stream = m_client.GetStream();
			m_reader = new BinaryReader(m_stream, Encoding.Unicode);
			m_writer = new BinaryWriter(m_stream, Encoding.Unicode);
			m_storage = storage;

			m_classes.Add(0, new ClassInfo(0, "$INVALID$"));
			m_functions.Add(0, new FunctionInfo(0, 0, false, "$INVALID$", string.Empty));

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
						MapFunction(mapFunc);
						break;

					case MessageId.MID_MapClass:
						var mapClass = Messages.MapClass.Read(m_reader);
						MapClass(mapClass);
						break;

					case MessageId.MID_EnterFunction:
					case MessageId.MID_LeaveFunction:
					case MessageId.MID_TailCall:
						var funcEvent = Messages.FunctionEvent.Read(m_reader);
						break;

					case MessageId.MID_CreateThread:
					case MessageId.MID_DestroyThread:
						var threadEvent = Messages.CreateThread.Read(m_reader);
						UpdateThread(threadEvent.ThreadId, messageId == MessageId.MID_CreateThread ? true : false, null);
						break;

					case MessageId.MID_NameThread:
						var nameThread = Messages.NameThread.Read(m_reader);
						//asume that dead threads can't be renamed
						UpdateThread(nameThread.ThreadId, true, nameThread.Name);
						break;

					case MessageId.MID_Sample:
						var sample = Messages.Sample.Read(m_reader, m_functions);
						ParseSample(sample);
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

		private void MapFunction(Messages.MapFunction mapFunc)
		{
			if(m_functions.ContainsKey(mapFunc.FunctionId))
				return;

			FunctionInfo funcInfo = new FunctionInfo();
			funcInfo.FunctionId = mapFunc.FunctionId;
			funcInfo.ClassId = mapFunc.ClassId;
			funcInfo.Name = mapFunc.Name;
			funcInfo.Signature = mapFunc.Signature;
			funcInfo.IsNative = mapFunc.IsNative;
			m_functions.Add(funcInfo.FunctionId, funcInfo);

			if(!m_classes.ContainsKey(funcInfo.ClassId))
				RequestClassMapping(funcInfo.ClassId);

			m_storage.MapFunction(funcInfo);

			Debug.WriteLine(string.Format("Mapped {0} to {1}.", mapFunc.Name, mapFunc.FunctionId));
		}

		private void MapClass(Messages.MapClass mapClass)
		{
			if(m_classes.ContainsKey(mapClass.ClassId))
				return;

			ClassInfo classInfo = new ClassInfo();
			classInfo.ClassId = mapClass.ClassId;
			classInfo.Name = mapClass.Name;
			m_classes.Add(classInfo.ClassId, classInfo);

			m_storage.MapClass(classInfo);
		}

		private void ParseSample(Messages.Sample sample)
		{
			foreach(var id in sample.Functions)
			{
				if(!m_functions.ContainsKey(id))
				{
					RequestFunctionMapping(id);
				}
			}

			m_storage.ParseSample(sample);
		}

		private void UpdateThread(int threadId, bool? alive, string name)
		{
			m_storage.UpdateThread(threadId, alive, name);
		}

		private void RequestFunctionMapping(int functionId)
		{
			var request = new Requests.GetFunctionMapping(functionId);
			request.Write(m_writer);
		}

		private void RequestClassMapping(int classId)
		{
			var request = new Requests.GetClassMapping(classId);
			request.Write(m_writer);
		}

		#region IDisposable Members

		public void Dispose()
		{
			m_stream.Dispose();
		}

		#endregion
	}
}
