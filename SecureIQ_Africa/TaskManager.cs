using System.Collections.Generic;
using System.Linq;

namespace SecureIQ_Africa
{
    public class TaskManager
    {
        private List<string> tasks = new List<string>();

        public void AddTask(string description) => tasks.Add(description);

        public List<string> GetAllTasks() => tasks;

        public bool DeleteTask(int index)
        {
            if (index >= 0 && index < tasks.Count)
            {
                tasks.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void ClearTasks() => tasks.Clear();

        public string FormatTaskList()
        {
            if (tasks.Count == 0)
                return "You have no tasks or reminders.";

            return "📋 Your tasks:\n" + string.Join("\n", tasks.Select((t, i) => $"{i + 1}) {t}"));
        }
    }
}