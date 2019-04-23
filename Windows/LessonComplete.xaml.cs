using MahApps.Metro.Controls;
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

        public LessonComplete(string wpm, int userErrors)
        {
            InitializeComponent();
            RateRun.Text = wpm;
            if (userErrors < 2) // allow one error
            {
                Stats.Visibility = Visibility.Visible;
                UserErrors.Visibility = Visibility.Collapsed;
            }
            else
            {
                Stats.Visibility = Visibility.Collapsed;
                UserErrors.Visibility = Visibility.Visible;
                ErrorsRun.Text = userErrors.ToString();
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

