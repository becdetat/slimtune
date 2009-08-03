using System;
using System.Threading;
using System.ComponentModel;

namespace SlimTuneUI
{
	/// <summary>
	/// A Connection combines two objects: a ProfilerClient and an IStorageEngine.
	/// The storage engine is valid for the lifetime of the Connection.
	/// The ProfilerClient may not be, or may not exist to begin with.
	/// </summary>
	/// <remarks>
	/// Connection is responsible for the receive thread driving a ProfilerClient.
	/// </remarks>
	public class Connection : IDisposable, INotifyPropertyChanged
	{
		public event EventHandler Connected;
		public event EventHandler Disconnected;

		Thread m_recvThread;
		volatile bool m_receive = false;

		public string Name
		{
			get { return Utilities.GetStandardCaption(this); }
		}

		public string Executable { get; set; }
		public string HostName
		{
			get { return Client != null ? Client.HostName : string.Empty; }
		}

		public int Port
		{
			get { return Client != null ? Client.Port : 0; }
		}

		public IStorageEngine StorageEngine { get; private set; }
		public ProfilerClient Client { get; private set; }
		public bool IsConnected { get; private set; }

		public Connection(IStorageEngine storageEngine)
		{
			if(storageEngine == null)
				throw new ArgumentNullException("storageEngine");
			this.StorageEngine = storageEngine;
		}

		/// <summary>
		/// Creates a thread to run the specified client, receiving data.
		/// </summary>
		/// <remarks>This method takes ownership of the client.</remarks>
		/// <param name="client">The client to send and receive data with.</param>
		public void RunClient(ProfilerClient client)
		{
			if(client == null)
				throw new ArgumentNullException("client");
			if(this.Client != null)
				throw new InvalidOperationException();
			this.Client = client;

			m_recvThread = new Thread(new ParameterizedThreadStart(ReceiveThread));
			m_receive = true;
			m_recvThread.Start(Client);
		}

		/// <summary>
		/// Terminates receiving from the target and closes the profiler client.
		/// </summary>
		public void DisconnectClient()
		{
			m_receive = false;
			if(m_recvThread != null)
			{
				//this usually won't fail
				if(!m_recvThread.Join(3000))
				{
					//but if it does, we're blocked on IO -- pull the rug out from under the thread
					m_recvThread.Abort();
				}
				m_recvThread = null;
			}
		}

		void ReceiveThread(object data)
		{
			var client = (ProfilerClient) data;
			IsConnected = true;
			if(Connected != null)
				Connected(this, EventArgs.Empty);

			try
			{
				while(m_receive)
				{
					try
					{
						string text = client.Receive();
						if(text == null)
							break;
					}
					catch(System.Net.Sockets.SocketException)
					{
						m_receive = false;
					}
#if DEBUG
					catch(Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.Message);
						System.Diagnostics.Debugger.Break();
					}
#endif
				}
			}
			finally
			{
				IsConnected = false;
				Client.Dispose();
				Client = null;
				Executable = string.Empty;

				if(PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("IsConnected"));
					PropertyChanged(this, new PropertyChangedEventArgs("Client"));
					PropertyChanged(this, new PropertyChangedEventArgs("Name"));
				}

				if(Disconnected != null)
				{
					Disconnected(this, EventArgs.Empty);
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#region IDisposable Members

		public void Dispose()
		{
			DisconnectClient();
			StorageEngine.Dispose();
		}

		#endregion
	}
}
