using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SecureIQ_Africa
{
    public class ActivityLogger
    {
        private static ActivityLogger _instance;
        public static ActivityLogger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ActivityLogger();
                return _instance;
            }
        }

        private ObservableCollection<LogEntry> _logs = new ObservableCollection<LogEntry>();
        public ReadOnlyObservableCollection<LogEntry> Logs { get; }

        private ActivityLogger()
        {
            Logs = new ReadOnlyObservableCollection<LogEntry>(_logs);
        }

        public void AddLog(string description)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Description = description,
                IsAlternate = (_logs.Count % 2 == 1)
            };
            _logs.Insert(0, entry);
            // Recalculate alternating flag
            for (int i = 0; i < _logs.Count; i++)
                _logs[i].IsAlternate = (i % 2 == 1);
            // Keep only last 100 entries
            while (_logs.Count > 100)
                _logs.RemoveAt(_logs.Count - 1);
        }

        public void ClearLogs() => _logs.Clear();
    }

    public class LogEntry : INotifyPropertyChanged
    {
        private DateTime _timestamp;
        private string _description;
        private bool _isAlternate;

        public DateTime Timestamp
        {
            get => _timestamp;
            set { _timestamp = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public bool IsAlternate
        {
            get => _isAlternate;
            set { _isAlternate = value; OnPropertyChanged(); }
        }

        public string FormattedTimestamp => Timestamp.ToString("HH:mm:ss · dd MMM yyyy");

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}