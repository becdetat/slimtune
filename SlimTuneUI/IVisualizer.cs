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
}
