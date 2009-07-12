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
using System.Text;
using System.Collections.Generic;

namespace ProfilerLauncher
{
	enum MessageId : byte
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

	enum ClientRequest : byte
	{
		CR_GetFunctionMapping = 0x01,
		CR_GetClassInfo,
	};

	namespace Messages
	{
		struct MapFunction
		{
			public int FunctionId;
			public string Name;
			public string Class;

			public static MapFunction Read(BinaryReader reader)
			{
				MapFunction result = new MapFunction();

				result.FunctionId = Utilities.Read7BitEncodedInt(reader);
				result.Name = reader.ReadString();
				result.Class = reader.ReadString();

				return result;
			}
		}

		struct FunctionEvent
		{
			public long ThreadId;
			public int FunctionId;
			public long TimeStamp;

			public static FunctionEvent Read(BinaryReader reader)
			{
				FunctionEvent result = new FunctionEvent();

				result.ThreadId = Utilities.Read7BitEncodedInt64(reader);
				result.FunctionId = Utilities.Read7BitEncodedInt(reader);
				result.TimeStamp = Utilities.Read7BitEncodedInt64(reader);

				return result;
			}
		}

		struct CreateThread
		{
			public long ThreadId;

			public static CreateThread Read(BinaryReader reader)
			{
				CreateThread result = new CreateThread();

				result.ThreadId = Utilities.Read7BitEncodedInt64(reader);

				return result;
			}
		}

		struct NameThread
		{
			public long ThreadId;
			public string Name;

			public static NameThread Read(BinaryReader reader)
			{
				NameThread result = new NameThread();

				result.ThreadId = Utilities.Read7BitEncodedInt64(reader);
				result.Name = reader.ReadString();

				return result;
			}
		}

		struct Sample
		{
			public long ThreadId;
			public List<int> Functions;

			public static Sample Read(BinaryReader reader, Dictionary<int, FunctionInfo> funcDict)
			{
				Sample result = new Sample();

				result.ThreadId = Utilities.Read7BitEncodedInt64(reader);
				int count = Utilities.Read7BitEncodedInt(reader);
				result.Functions = new List<int>(count);
				for(int i = 0; i < count; ++i)
				{
					int id = Utilities.Read7BitEncodedInt(reader);
					/*if(funcDict.ContainsKey(id))
					{
						FunctionInfo info = funcDict[id];
						result.Functions.Add(info.Class + "." + info.Name);
					}
					else
					{
						result.Functions.Add("{Unknown}");
					}*/
					result.Functions.Add(id);
				}

				return result;
			}
		}
	}

	namespace Requests
	{
		struct GetFunctionMapping
		{
			public int FunctionId;
		};
	}
}
