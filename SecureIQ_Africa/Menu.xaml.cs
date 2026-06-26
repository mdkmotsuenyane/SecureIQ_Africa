using System.Windows;
using System.Windows.Controls;

namespace SecureIQ_Africa
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private string name;

        public Menu(string userName)
        {
            InitializeComponent();
            name = userName;
            TextBox1.Text = userName; // Displays the user's name in the welcome area
        }

        // Opens the Quiz window
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogger.Instance.AddLog("Quiz started by user.");
            Quiz quiz = new Quiz();
            quiz.Show();
        }

        // Opens the Chat window
        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogger.Instance.AddLog("Chat window opened.");
            SecureIQChatWindow chat = new SecureIQChatWindow(name);
            chat.Show();
        }

        // Opens the Task Manager window
        private void TaskManagerButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogger.Instance.AddLog("Task Manager opened.");
            TaskManagerWindow taskPage = new TaskManagerWindow();
            taskPage.Show();
        }

        // Opens the Activity Log window
        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogger.Instance.AddLog("Activity log viewed.");
            ActivityLog logWindow = new ActivityLog();
            logWindow.Owner = this;
            logWindow.ShowDialog();
        }

        // Exits the application
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogger.Instance.AddLog("Application closed.");
            Application.Current.Shutdown();
        }
    }
}