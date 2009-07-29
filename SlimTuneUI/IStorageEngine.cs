using System;
using System.Data;

namespace SlimTuneUI
{
	public class FunctionInfo
	{
		public int FunctionId;
		public int ClassId;
		public bool IsNative;
		public string Name;
		public string Signature;

		public FunctionInfo()
		{
		}

		public FunctionInfo(int funcId, int classId, bool isNative, string name, string signature)
		{
			FunctionId = funcId;
			ClassId = classId;
			IsNative = isNative;
			Name = name;
			Signature = signature;
		}
	}

	public class ClassInfo
	{
		public int ClassId;
		public string Name;

		public ClassInfo()
		{
		}

		public ClassInfo(int classId, string name)
		{
			ClassId = classId;
			Name = name;
		}
	}

	public interface IStorageEngine : IDisposable
	{
		void MapFunction(FunctionInfo funcInfo);
		void MapClass(ClassInfo classInfo);

		void ParseSample(Messages.Sample sample);
		void ClearSamples();
		void UpdateThread(int threadId, bool? alive, string name);
		void Flush();

		DataSet Query(string query);
	}
}
