using MahApps.Metro.Controls;
using MultiType.Models;
using System.Windows;

namespace MultiType
{

    /// <summary>
    /// Interaction logic for LessonComplete.xaml
    /// </summary>
    public partial class LessonComplete : MetroWindow
    {
        internal Miscellaneous.DialogResult Result { get; set; }

        public LessonComplete()
        {
            InitializeComponent();
            Stats.Visibility = Visibility.Collapsed;
        }

        public LessonComplete(string wpm)
        {
            InitializeComponent();
            RateRun.Text = wpm;
            Stats.Visibility = Visibility.Visible;
            UserErrors.Visibility = Visibility.Collapsed;
        }

        public LessonComplete(LessonCompleteModel lessonComplete)
        {
            InitializeComponent();
            RateRun.Text = lessonComplete.WPM;
            if (lessonComplete.DidSucceed) // allow one error
            {
                Stats.Visibility = Visibility.Visible;
                UserErrors.Visibility = Visibility.Collapsed;
            }
            else
            {
                Stats.Visibility = Visibility.Collapsed;
                UserErrors.Visibility = Visibility.Visible;
                ErrorsRun.Text = lessonComplete.UserErrors.ToString();
            }
            if (string.IsNullOrWhiteSpace(lessonComplete.LessonName))
            {
                LessonTitle.Visibility = Visibility.Collapsed;
            }
            else
            {
                LessonTitle.Text = lessonComplete.LessonName;
                LessonTitle.Visibility = Visibility.Visible;
            }
        }

        private void SelectNew_Click(object sender, RoutedEventArgs e)
	    {
            Result = Miscellaneous.DialogResult.New;
            DialogResult = true;
	    }

	    private void Repeat_Click(object sender, RoutedEventArgs e)
	    {
            Result = Miscellaneous.DialogResult.Repeat;
            DialogResult = true;
	    }

	    private void Quit_Click(object sender, RoutedEventArgs e)
	    {
            DialogResult = false;
	    }
	}
}

