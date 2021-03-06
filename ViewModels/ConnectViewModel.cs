﻿using MultiType.Models;
using System.ComponentModel;

namespace MultiType.ViewModels
{
	class ConnectViewModel: INotifyPropertyChanged
	{
		private  ConnectModel _model;
		private string _ipAddr;
		private string _portNumber;
		private bool _connectionEstablished;
		private string _inputError;
		internal SocketsAPI.AsyncTcpClient asyncSocket;

		public string InputError
		{
			get { return _inputError; }
			set
			{
				_inputError = value;
				NotifyPropertyChanged("InputError");
			}
		}

		public bool ConnectionEstablished
		{
			get { return _connectionEstablished; }
			set
			{
				_connectionEstablished = value;
				NotifyPropertyChanged("ConnectionEstablished");
			}
		}

		public string IPAddress
		{
			get { return _ipAddr; }
			set	{_ipAddr = value;}
		}
		public string PortNumber
		{
			get{return _portNumber;}
			set{ _portNumber = value; }
		}

		internal ConnectViewModel()
		{
			_model = new ConnectModel(this);
			IPAddress = "";
			_portNumber = "";
		}

		internal void ConnectToServer()
		{
			_model.ConnectToServer(_ipAddr, _portNumber);
		}

		#region NPC Implementation
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}
		#endregion NPC Implementation
	}
}
