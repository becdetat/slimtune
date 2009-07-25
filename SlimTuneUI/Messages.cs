/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SlimTuneUI
{
	public enum MessageId : byte
	{
		MID_MapFunction = 0x01,

		MID_EnterFunction = 0x10,
		MID_LeaveFunction,
		MID_TailCall,

		MID_ObjectAllocated = 0x20,
		MID_CollectionStarted,
		MID_CollectionFinished,

		MID_TransitionOut = 0x30,		//From managed to unmanaged (out of the runtime)
		MID_TransitionIn,				//From unmanaged to managed (back into the runtime)

		MID_CreateThread = 0x40,
		MID_DestroyThread,
		MID_NameThread,

		MID_Sample = 0x50,

		MID_BeginEvent = 0xf0,
		MID_EndEvent
	};

	public enum ClientRequest : byte
	{
		CR_GetFunctionMapping = 0x01,
		CR_GetClassInfo,
	};

	namespace Messages
	{
		public struct MapFunction
		{
			public const int MaxNameSize = 1024;
			public const int MaxSignatureSize = 2048;

			public int FunctionId;
			public bool IsNative;
			public string Name;
			public string Signature;

			public static MapFunction Read(BinaryReader reader)
			{
				MapFunction result = new MapFunction();

				result.FunctionId = Utilities.Read7BitEncodedInt(reader);
				result.IsNative = Utilities.Read7BitEncodedInt(reader) != 0;
				result.Name = reader.ReadString();
				result.Signature = reader.ReadString();

				return result;
			}
		}

		public struct FunctionEvent
		{
			public int ThreadId;
			public int FunctionId;
			public long TimeStamp;

			public static FunctionEvent Read(BinaryReader reader)
			{
				FunctionEvent result = new FunctionEvent();

				result.ThreadId = Utilities.Read7BitEncodedInt(reader);
				result.FunctionId = Utilities.Read7BitEncodedInt(reader);
				result.TimeStamp = Utilities.Read7BitEncodedInt64(reader);

				return result;
			}
		}

		public struct CreateThread
		{
			public int ThreadId;

			public static CreateThread Read(BinaryReader reader)
			{
				CreateThread result = new CreateThread();

				result.ThreadId = Utilities.Read7BitEncodedInt(reader);

				return result;
			}
		}

		public struct NameThread
		{
			public int ThreadId;
			public string Name;

			public static NameThread Read(BinaryReader reader)
			{
				NameThread result = new NameThread();

				result.ThreadId = Utilities.Read7BitEncodedInt(reader);
				result.Name = reader.ReadString();

				return result;
			}
		}

		public struct Sample
		{
			public int ThreadId;
			public List<int> Functions;

			public static Sample Read(BinaryReader reader, Dictionary<int, FunctionInfo> funcDict)
			{
				Sample result = new Sample();

				result.ThreadId = Utilities.Read7BitEncodedInt(reader);
				int count = Utilities.Read7BitEncodedInt(reader);
				result.Functions = new List<int>(count);
				for(int i = 0; i < count; ++i)
				{
					int id = Utilities.Read7BitEncodedInt(reader);
					result.Functions.Add(id);
				}

				return result;
			}
		}
	}

	/*namespace Requests
	{
		struct GetFunctionMapping
		{
			public int FunctionId;
		};
	}*/
}
