﻿using MahApps.Metro.Controls;
using System.Windows;

namespace MultiType
{
	/// <summary>
	/// Interaction logic for Menu.xaml
	/// </summary>
	public partial class Menu : MetroWindow
	{
		public Menu()
		{
			InitializeComponent();
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		/// <summary>
		/// Open the lesson select window to allow the user to host a game
		/// </summary>
		private void Host_Click(object sender, RoutedEventArgs e)
		{
			var lessonSelect = new LessonSelect();
			lessonSelect.Show();
			this.Close();
		}

		/// <summary>
		/// Open the client connect window to allow the user to connect to a host
		/// </summary>
		private void Join_Click(object sender, RoutedEventArgs e)
		{
			var window = new ClientConnect();
			this.Close();
			window.Show();
		}

		/// <summary>
		/// Open the lesson select window to allow the user to begin a single player game
		/// </summary>
		private void SinglePlayer_Click(object sender, RoutedEventArgs e)
		{
			var window = new LessonSelect(isSinglePlayer:true);
			window.Show();
			this.Close();
		}
	}
}
