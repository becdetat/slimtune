using System;
using System.Data;

namespace SlimTuneUI
{
	public interface IStorageEngine : IDisposable
	{
		void MapFunction(FunctionInfo funcInfo);
		void ParseSample(Messages.Sample sample);
		void ClearSamples();
		void UpdateThread(int threadId, bool? alive, string name);
		void Flush();

		DataSet Query(string query);
	}
}
