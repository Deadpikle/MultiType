﻿using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MultiType.ViewModels;

namespace MultiType
{
	/// <summary>
	/// Interaction logic for ClientConnect.xaml
	/// </summary>
	public partial class ClientConnect : MetroWindow
	{
		ConnectViewModel _viewModel;
		public ClientConnect()
		{
			// set up the window and its data context
			InitializeComponent();
			_viewModel = new ConnectViewModel();
			this.DataContext = _viewModel;
		}

		/// <summary>
		/// return to the main menu
		/// </summary>
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			var menu = new Menu();
			menu.Show();
			this.Close();
		}

		/// <summary>
		/// Attempt to connect to the host
		/// </summary>
		private void Connect_Click(object sender, RoutedEventArgs e)
		{
			if (_viewModel == null) return;
			_viewModel.ConnectToServer();
		}

		/// <summary>
		/// A connection to the host has been established.
		/// Open the main window, passing in an empty lesson string (we don't know what the lesson is yet) and the socket connected to the host.
		/// </summary>
		private void ConnectionEstablished_Checked(object sender, RoutedEventArgs e)
		{
			if (_viewModel == null && _viewModel.asyncSocket!=null) return;
			// connection has been established, open the primary window, passing in the peer socket
			// we don't know what the lesson is yet, so pass an empty string to the view model
			const string lessonString = "";
			var window = new MainWindow(_viewModel.asyncSocket, lessonString);
            window.LessonTitle = "";
            window.Show();
            this.Close();
		}

		/// <summary>
		/// Block improper input to the IP address text box
		/// </summary>
		private void IP_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			var pattern = @"^\d$";
			if (!Regex.IsMatch(e.Text, pattern) && !e.Text.Equals("."))
				e.Handled = true;
			if (((TextBox)sender).Text.Length >= 15)
				e.Handled = true;
		}

		/// <summary>
		/// Block improper input to the Port # input text box
		/// </summary>
		private void Port_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!Regex.IsMatch(e.Text, @"^\d$"))
				e.Handled = true;
			if (((TextBox)sender).Text.Length >= 5)
				e.Handled = true;
		}

        private void Port_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_viewModel == null) return;
                _viewModel.ConnectToServer();
            }
        }
    }
}
