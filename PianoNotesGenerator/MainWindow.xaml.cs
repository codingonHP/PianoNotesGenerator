using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
namespace PianoNotesGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] _notes = new string[] { "A", "B", "C", "D", "E", "F", "G", "A#", "B#", "C#", "D#", "E#", "F#", "G#" };
        private const int _defaultDifficulty = 1000;
        private int _difficulty = _defaultDifficulty;
        private string _baseWindowTitle = "Random Notes Generator";

        Random random = new Random();
        Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            txtCustomDifficulty.IsEnabled = false;
            Title = _baseWindowTitle + " " + "new note in " + _difficulty + " ms";

            Task task = new Task(() =>
            {
                _timer = new Timer(Callback, this, 0, _difficulty);
            });

            task.Start();
        }

        private static void Callback(object state)
        {
            var mainWindow = state as MainWindow;
            GenerateNotes(state as MainWindow);
        }

        public static void GenerateNotes(MainWindow window)
        {
            var newRandom = window.random.Next(0, 13);
            try
            {
                window.Dispatcher.Invoke(() =>
                {
                    window.lblNewNote.Content = window._notes[newRandom];
                });
            }
            catch (Exception)
            {
                if (window._timer != null)
                {
                    window._timer.Dispose();
                    window._timer = null;
                }
            }

        }

        private void txtCustomDifficulty_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCustomDifficulty.Text))
                {
                    _difficulty = Convert.ToInt32(txtCustomDifficulty.Text.Trim());
                    Refresh();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Only numbers like 1000 please.", "Don't write stories here...", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
            }

        }

        private void sldDifficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _difficulty = Convert.ToInt32(sldDifficulty.Value);
            Refresh();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                if (_difficulty == 0)
                {
                    _difficulty = _defaultDifficulty;
                }
                _timer = new Timer(Callback, this, _difficulty, _difficulty);
                Title = _baseWindowTitle + " " + "new note in " + _difficulty + " ms";
            }
        }

        private void chkWantCustomDifficulty_Clicked(object sender, RoutedEventArgs e)
        {
            if (chkWantCustomDifficulty.IsChecked == true)
            {
                txtCustomDifficulty.IsEnabled = true;
            }
            else
            {
                txtCustomDifficulty.IsEnabled = false;
            }

            txtCustomDifficulty.Text = string.Empty;
        }

    }
}
