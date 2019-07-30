using System.Windows;
using MahApps.Metro.Controls;
using MultiType.ViewModels;

namespace MultiType
{
	/// <summary>
	/// Interaction logic for MiniLessonSelect.xaml
	/// Allows a single player or the host of a multiplayer game to choose a new lesson after completing their current lesson.
	/// </summary>
	public partial class MiniLessonSelect : MetroWindow
	{
		public string LessonString { get; set; }
        public string LessonTitle { get; set; }
        private LessonViewModel _viewModel;

		public MiniLessonSelect()
		{
			InitializeComponent();
			_viewModel = new LessonViewModel();
			this.DataContext = _viewModel;
		}

		private void Choose_Click(object sender, RoutedEventArgs e)
		{
			// todo must ensure that the user has actually selected a lesson
			if (_viewModel == null) return;
			if (_viewModel.SelectedLessonIndex.Equals("0")) return;
			LessonString = _viewModel.LessonString;
            LessonTitle = _viewModel.LessonNames[int.Parse(_viewModel.SelectedLessonIndex)];
			DialogResult = true;
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
