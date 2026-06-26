using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SecureIQ_Africa
{
    public partial class TaskManagerWindow : Window, INotifyPropertyChanged
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TaskChat;Trusted_Connection=True";
        private ObservableCollection<TaskItem> _tasks;
        private bool _isLoading = false;  // Prevents logging during initial load

        public ObservableCollection<TaskItem> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
                UpdateCounts();
            }
        }

        public TaskManagerWindow()
        {
            InitializeComponent();
            DataContext = this;

            this.Closing += Window_Closing;

            // Placeholder handling
            NewTaskTextBox.GotFocus += (s, e) =>
            {
                if (NewTaskTextBox.Text == "Enter a new task...")
                    NewTaskTextBox.Text = "";
            };
            NewTaskTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(NewTaskTextBox.Text))
                    NewTaskTextBox.Text = "Enter a new task...";
            };

            // Load data
            LoadTasks();
            UpdateCounts();
        }

        // ------------------------- DATABASE OPERATIONS -------------------------

        private void LoadTasks()
        {
            _isLoading = true;
            var taskList = new ObservableCollection<TaskItem>();

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT Id, Title, Description, Remainder, IsCompleted FROM [Task] ORDER BY Id", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new TaskItem
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Remainder = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                IsCompleted = reader.GetBoolean(4)
                            };
                            // Subscribe to property changes for live logging
                            task.PropertyChanged += Task_PropertyChanged;
                            taskList.Add(task);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Tasks = taskList;
            _isLoading = false;
        }

        private void SaveTasks()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var task in Tasks)
                    {
                        if (task.Id == 0) // New task – INSERT
                        {
                            using (var cmd = new SqlCommand(
                                "INSERT INTO [Task] (Title, Description, Remainder, IsCompleted) VALUES (@title, @desc, @rem, @comp); SELECT SCOPE_IDENTITY();",
                                conn))
                            {
                                cmd.Parameters.AddWithValue("@title", task.Title ?? "");
                                cmd.Parameters.AddWithValue("@desc", task.Description ?? "");
                                cmd.Parameters.AddWithValue("@rem", string.IsNullOrEmpty(task.Remainder) ? (object)DBNull.Value : task.Remainder);
                                cmd.Parameters.AddWithValue("@comp", task.IsCompleted);
                                task.Id = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }
                        else // Update existing
                        {
                            using (var cmd = new SqlCommand(
                                "UPDATE [Task] SET Title = @title, Description = @desc, Remainder = @rem, IsCompleted = @comp WHERE Id = @id",
                                conn))
                            {
                                cmd.Parameters.AddWithValue("@title", task.Title ?? "");
                                cmd.Parameters.AddWithValue("@desc", task.Description ?? "");
                                cmd.Parameters.AddWithValue("@rem", string.IsNullOrEmpty(task.Remainder) ? (object)DBNull.Value : task.Remainder);
                                cmd.Parameters.AddWithValue("@comp", task.IsCompleted);
                                cmd.Parameters.AddWithValue("@id", task.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving tasks: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTaskFromDatabase(int id)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM [Task] WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ------------------------- EVENT HANDLERS -------------------------

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveTasks();
        }

        // Checkbox toggle – logs completion/uncompletion
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk && chk.DataContext is TaskItem task)
            {
                bool newStatus = task.IsCompleted; // already updated by binding
                // Log the change
                if (newStatus)
                    ActivityLogExtensions.LogTaskCompleted(task.Title);
                else
                    ActivityLogExtensions.LogCustom($"Task uncompleted: \"{task.Title}\"");

                UpdateCounts();
                SaveTasks(); // save immediately
            }
        }

        // Delete button – logs deletion
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is TaskItem task)
            {
                // Log before removal
                ActivityLogExtensions.LogCustom($"Task deleted: \"{task.Title}\"");

                if (task.Id > 0)
                    DeleteTaskFromDatabase(task.Id);

                // Unsubscribe to avoid memory leak
                task.PropertyChanged -= Task_PropertyChanged;
                Tasks.Remove(task);
                UpdateCounts();
            }
        }

        // Enter key in textbox – adds new task
        private void NewTaskTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewTask();
        }

        // Add button click
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewTask();
        }

        // Helper to add a new task
        private void AddNewTask()
        {
            string title = NewTaskTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(title) && title != "Enter a new task...")
            {
                var newTask = new TaskItem
                {
                    Title = title,
                    Description = "",
                    Remainder = "",
                    IsCompleted = false
                };
                newTask.PropertyChanged += Task_PropertyChanged; // subscribe for future edits
                Tasks.Add(newTask);
                NewTaskTextBox.Text = "Enter a new task...";

                // Log the addition
                ActivityLogExtensions.LogTaskAdded(title);

                UpdateCounts();
                SaveTasks(); // save immediately
            }
        }

        // Updates the active/completed counts
        private void UpdateCounts()
        {
            if (Tasks == null) return;
            TaskCount.Text = Tasks.Count(t => !t.IsCompleted).ToString();
            CompletedCount.Text = Tasks.Count(t => t.IsCompleted).ToString();
        }

        // ------------------------- LOGGING FOR EDITS (Title, Description, Remainder) -------------------------

        // This event is fired whenever any property of a TaskItem changes.
        // We log only user-driven changes (not during load) and skip IsCompleted (handled separately).
        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_isLoading) return;
            if (sender is TaskItem task)
            {
                // Only log changes to Title, Description, Remainder
                if (e.PropertyName == nameof(TaskItem.Title) ||
                    e.PropertyName == nameof(TaskItem.Description) ||
                    e.PropertyName == nameof(TaskItem.Remainder))
                {
                    // Use a generic update log; you can add more detail if needed.
                    ActivityLogExtensions.LogTaskUpdated(task.Title);
                    // Save immediately to persist changes
                    SaveTasks();
                }
            }
        }

        // Optional: attach LostFocus handlers to textboxes in XAML to avoid logging on every keystroke
        // But since we subscribe to PropertyChanged, we already log only when the property actually changes.
        // To prevent logging when programmatically setting (e.g., loading), we use the _isLoading flag.
        // Also, we save on each change – adjust if performance is an issue.

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // - TASK ITEM CLASS (unchanged, but remains) 
    public class TaskItem : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _description;
        private string _remainder;
        private bool _isCompleted;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public string Remainder
        {
            get => _remainder;
            set { _remainder = value; OnPropertyChanged(nameof(Remainder)); }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}