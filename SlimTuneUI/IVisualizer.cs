using System;

namespace SlimTuneUI
{
	/// <summary>
	/// The base interface for classes that provide visualization of profiler data.
	/// </summary>
	public interface IVisualizer
	{
		void Initialize(MainWindow mainWindow, Connection connection);
		void Show(WeifenLuo.WinFormsUI.Docking.DockPanel parent);
	}

	public class VisualizerEntry
	{
		public string Name { get; set; }
		public Type Type { get; set; }

		public VisualizerEntry(string name, Type type)
		{
			this.Name = name;
			this.Type = type;
		}
	}
}
