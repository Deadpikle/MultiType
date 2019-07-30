using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MultiType.Models;
using MultiType.ViewModels;

namespace MultiType
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private PrimaryViewModel _viewModel;
		private int _contentLength;
		private bool _isSinglePlayer;
		private bool _isServer;

		internal MainWindow(string lessonString, int racerSpeed)
		{
			InitializeComponent();
			PeerStatsGrid.Visibility = Visibility.Hidden;
			//adjust the placement of the local stats grid to account for a single player
			LocalStatsGrid.Margin = new Thickness(LocalStatsGrid.Margin.Left, LocalStatsGrid.Margin.Top+20,
				LocalStatsGrid.Margin.Right, LocalStatsGrid.Margin.Bottom);
			_viewModel = new PrimaryViewModel(lessonString, UserInput, LessonContent, racerSpeed: racerSpeed);
			_isSinglePlayer = true;
			this.DataContext = _viewModel;
			//_contentLength = 0;
			UserInput.Focus();
            LessonTitle = "";
        }

        internal MainWindow(SocketsAPI.AsyncTcpClient socket, string lessonString, bool isServer = false)
        {
			InitializeComponent();
			this.DataContext = new PrimaryViewModel(lessonString, UserInput, LessonContent, socket, isServer );
			_viewModel = (PrimaryViewModel)DataContext;
			_isSinglePlayer = false;
			ChangeLesson.Visibility = Visibility.Collapsed;
			_isServer=isServer;			
        }

        public string LessonTitle { get; set; }

		internal void Window_Loaded(object sender, RoutedEventArgs e)
		{
			OpenStartGameDialog();
		}

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Pause)
                _viewModel.TogglePause();
        }

		private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
			var content = new TextRange(UserInput.Document.ContentStart, UserInput.Document.ContentEnd).Text;
			if (_contentLength == content.Length)
                return;
			_contentLength = content.Length;
			if (content.Length < 2)
                return;
			content = content.Substring(0, content.Length - 2);
            Console.WriteLine("Content is {0}", content);
			_viewModel?.CharacterTyped(content);
        }
		
		private void RTBScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			var scroller = (ScrollViewer)sender;
			scroller.ScrollToBottom();
			var offset = scroller.VerticalOffset;
			PeerContentScroll.ScrollToVerticalOffset(offset);
			LessonContentScroll.ScrollToVerticalOffset(offset);
		}

		private void LessonSelect_Click(object sender, RoutedEventArgs e)
		{
			if (_isSinglePlayer == false) return;
			var window = new LessonSelect(_isSinglePlayer);
			window.Show();
			this.Close();
		}

		private void Menu_Click(object sender, RoutedEventArgs e)
		{
			var window = new Menu();
			window.Show();
			this.Close();
		}

		private void PopupOpen_Unchecked(object sender, RoutedEventArgs e)
		{
			var checkbox = (CheckBox)sender;
			if (checkbox.IsChecked == false)
				UserInput.Focus();
		}
		
		private void GameComplete_Checked(object sender, RoutedEventArgs e)
		{
			var checkbox = (CheckBox)sender;
			if (_isSinglePlayer || _isServer)
			{
                var model = new LessonCompleteModel(User1WPM.Content.ToString(), _viewModel.UserErrors, LessonTitle);
                var completeWindow = new LessonComplete(model);
				if (completeWindow.ShowDialog() == false)
				{
					completeWindow.Close();
                    App.Current.Shutdown();
				}
				else if (completeWindow.Result == Miscellaneous.DialogResult.Repeat)
				{
					completeWindow.Close();
					_viewModel.RepeatLesson();
					OpenStartGameDialog();
					return;
				}
                else if (completeWindow.Result == Miscellaneous.DialogResult.New)
				{
					completeWindow.Close();
					var window = new MiniLessonSelect();
					if (window.ShowDialog() == true)
					{
						var lessonString = window.LessonString;
						_viewModel.NewLesson(lessonString);
                        LessonTitle = window.LessonTitle; // ohhh the evil hackssss and ugly codddeeee
                        OpenStartGameDialog();
					}
					else
					{
						// should we notify the peer in this case?
						var menu = new Menu();
						menu.Show();
					}
					window.Close();
					//this.Close();
					return;
				}
			}
			else
			{
				_viewModel.ShowPopup = true;
				_viewModel.StaticPopupText = "Waiting for the host to select a lesson and start a new game...";
                return;
			}
		}

		internal void OpenStartGameDialog()
		{
			if (_isSinglePlayer)
				return;
			else if (!_isServer)
			{
				_viewModel.ShowPopup = true;
				_viewModel.StaticPopupText = "Waiting for host to start...";
			}
			else // isServer
			{
				string message = "Click OK to start the game.";
				string caption = "Prompt";
				MessageBoxButton buttons = MessageBoxButton.OK;
				if (MessageBox.Show(message, caption, buttons) == MessageBoxResult.OK)
				{
					_viewModel.StartGame();
				}
				else
				{
					var window = new Menu();
					window.Show();
					this.Close();
				}
			}
		}

		private void ClearRTB_Checked(object sender, RoutedEventArgs e)
		{
			var checkbox = (CheckBox)sender;
			UserInput.Document.Blocks.Clear();
			var paragraph = new Paragraph();
			paragraph.Foreground = Brushes.Blue;
			paragraph.LineHeight = 48;
			UserInput.Document.Blocks.Add(paragraph);
			checkbox.IsChecked = false;
            _viewModel.PeerTypedContent = "";
        }

        private void UserInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Keyboard.Focus(UserInput);
        }

        private void UserInput_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        // http://www.devx.com/dotnet/Article/34644
        protected void BlockCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void UserInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up ||
                e.Key == Key.Down ||
                e.Key == Key.Left ||
                e.Key == Key.Right ||
                e.Key == Key.Tab ||
                e.Key == Key.OemBackTab)
            {
                e.Handled = true;
            }
        }
    }
}
