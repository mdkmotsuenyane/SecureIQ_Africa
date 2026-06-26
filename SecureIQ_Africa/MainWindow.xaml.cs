using System;
using System.IO;
using System.Media;
using System.Windows;

namespace SecureIQ_Africa
{
    public partial class MainWindow : Window
    {
        private readonly string soundPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "SecureIQ.wav"
        );

        private SoundPlayer _greetingPlayer;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => Greeting();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(
                    "Please enter a valid name!",
                    "Input Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // ✅ Log using the ActivityLogService (instead of ActivityLogger)
            ActivityLogService.AddEntry($"User '{name}' submitted their name.");

            Menu menu = new Menu(name);
            menu.Show();
            this.Close();
        }

        private void Greeting()
        {
            try
            {
                if (File.Exists(soundPath))
                {
                    _greetingPlayer = new SoundPlayer(soundPath);
                    _greetingPlayer.Play();
                }
                else
                {
                    MessageBox.Show(
                        "Welcome sound not found.",
                        "Info",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
            catch (Exception ex)
            {
                // Also log errors via the service
                ActivityLogService.AddEntry($"Error playing greeting sound: {ex.Message}");

                MessageBox.Show(
                    $"Error playing sound: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _greetingPlayer?.Dispose();
            base.OnClosed(e);
        }
    }
}