using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SecureIQ_Africa
{
    public partial class ActivityLog : Window
    {
        public ActivityLog()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += ActivityLog_Loaded;
        }

        private void ActivityLog_Loaded(object sender, RoutedEventArgs e) => RefreshLog();

        public int LogCount => ActivityLogService.Log.Count;

        private void RefreshLog()
        {
            var recent = ActivityLogService.Log
                .Skip(Math.Max(0, ActivityLogService.Log.Count - 10))
                .ToList();

            for (int i = 0; i < recent.Count; i++)
                recent[i].IsAlternate = (i % 2 == 1);

            LogListView.ItemsSource = recent;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => RefreshLog();
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }

    public class ActivityLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        public bool IsAlternate { get; set; }
        public string FormattedTimestamp => Timestamp.ToString("HH:mm:ss  dd/MM/yyyy");

        public ActivityLogEntry(string description)
        {
            Timestamp = DateTime.Now;
            Description = description;
        }
    }

    public static class ActivityLogService
    {
        private static readonly ObservableCollection<ActivityLogEntry> _log = new ObservableCollection<ActivityLogEntry>();

        public static ObservableCollection<ActivityLogEntry> Log => _log;

        public static void AddEntry(string description)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                _log.Add(new ActivityLogEntry(description));
                while (_log.Count > 100) _log.RemoveAt(0);
            });
        }

        public static List<ActivityLogEntry> GetRecentEntries(int count = 10)
            => _log.Skip(Math.Max(0, _log.Count - count)).ToList();
    }

    public static class ActivityLogExtensions
    {
        public static void LogTaskAdded(string taskName)
            => ActivityLogService.AddEntry($"Task added: \"{taskName}\"");

        public static void LogTaskUpdated(string taskName, string oldStatus = null, string newStatus = null)
        {
            if (!string.IsNullOrEmpty(oldStatus) && !string.IsNullOrEmpty(newStatus))
                ActivityLogService.AddEntry($"Task updated: \"{taskName}\" ({oldStatus} → {newStatus})");
            else
                ActivityLogService.AddEntry($"Task updated: \"{taskName}\"");
        }

        public static void LogTaskCompleted(string taskName)
            => ActivityLogService.AddEntry($"Task completed: \"{taskName}\"");

        public static void LogReminderSet(string taskName, DateTime reminderTime)
            => ActivityLogService.AddEntry($"Reminder set for \"{taskName}\" at {reminderTime:HH:mm dd/MM/yyyy}");

        public static void LogQuizStarted()
            => ActivityLogService.AddEntry("Quiz started");

        public static void LogQuizCompleted(int score, int total)
            => ActivityLogService.AddEntry($"Quiz completed: {score}/{total} correct");

        public static void LogNlpInteraction(string command, string response = null)
        {
            if (!string.IsNullOrEmpty(response))
                ActivityLogService.AddEntry($"NLP: \"{command}\" → {response}");
            else
                ActivityLogService.AddEntry($"NLP: \"{command}\"");
        }

        public static void LogCustom(string message)
            => ActivityLogService.AddEntry(message);
    }
}