using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SecureIQ_Africa
{
    public class NLPEngine
    {
        // NESTED ENUM 
        public enum IntentType
        {
            None,
            Task,
            Reminder,
            ListTasks,
            DeleteTask,
            Quiz
        }

        // NESTED INTENT CLASS
        public class Intent
        {
            public IntentType Type { get; set; }
            public string Action { get; set; }
            public string Topic { get; set; }
            public double Confidence { get; set; } = 0.0;
            public string Sentiment { get; set; } = "neutral";
        }

        // TRIGGERS
        private readonly string[] taskTriggers = {
            "remind me to", "remind me about", "add a task", "add task",
            "create a task", "create task", "set a reminder", "set reminder",
            "add a reminder", "add reminder", "task to", "reminder to",
            "i need to", "i have to", "don't forget to", "remember to"
        };

        private readonly string[] listTriggers = {
            "list tasks", "show tasks", "my tasks", "what are my tasks",
            "what tasks do i have", "show reminders", "list reminders", "my reminders"
        };

        private readonly string[] deleteTriggers = {
            "delete task", "remove task", "delete reminder", "remove reminder",
            "clear task", "clear reminder"
        };

        // UPDATED: Expanded quiz triggers to catch more natural variations
        private readonly string[] quizTriggers = {
            "quiz me", "give me a quiz", "test me", "ask me a question",
            "take a quiz", "i want a quiz", "i would like a quiz",
            "start a quiz", "let's do a quiz", "i want to take a quiz",
            "do a quiz", "give me a test", "i need a quiz", "ready for a quiz",
            "let me take a quiz", "can i have a quiz", "i'd like a quiz"
        };

        private string ExtractAction(string input, string trigger)
        {
            int index = input.IndexOf(trigger, StringComparison.OrdinalIgnoreCase);
            if (index < 0) return null;

            string after = input.Substring(index + trigger.Length).Trim();
            after = Regex.Replace(after, @"[.!?]+$", "");
            return string.IsNullOrWhiteSpace(after) ? null : after;
        }

        private string DetectTopic(string text)
        {
            var topics = Response.GetStaticTopics();
            string lowerText = text.ToLower();
            foreach (string topic in topics)
                if (lowerText.Contains(topic))
                    return topic;
            return null;
        }

        private string DetectSentiment(string input)
        {
            return SentimentDetector.Detect(input);
        }

        public Intent ParseIntent(string input)
        {
            // Handle empty input and log it immediately
            if (string.IsNullOrWhiteSpace(input))
            {
                var emptyIntent = new Intent { Type = IntentType.None, Sentiment = "neutral" };
                ActivityLogger.Instance.AddLog($"Parsed empty/null input -> Intent: None");
                return emptyIntent;
            }

            string lowerInput = input.ToLower().Trim();
            Intent intent = new Intent { Type = IntentType.None };

            // List tasks
            if (listTriggers.Any(t => lowerInput.Contains(t)))
            {
                intent.Type = IntentType.ListTasks;
            }
            // Delete task 
            else if (deleteTriggers.Any(t => lowerInput.Contains(t)))
            {
                var match = Regex.Match(lowerInput, @"(?:delete|remove|clear)\s+(?:task|reminder)\s*#?\s*(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int idx))
                {
                    intent.Type = IntentType.DeleteTask;
                    intent.Action = idx.ToString();
                }
                else
                {
                    intent.Type = IntentType.DeleteTask;
                    intent.Action = "all";
                }
            }
            // Quiz – now recognises the expanded triggers
            else if (quizTriggers.Any(t => lowerInput.Contains(t)))
            {
                intent.Type = IntentType.Quiz;
                intent.Topic = DetectTopic(lowerInput);
                intent.Action = "quiz";
            }
            // Task / Reminder creation
            else
            {
                foreach (string trigger in taskTriggers)
                {
                    if (lowerInput.Contains(trigger))
                    {
                        string action = ExtractAction(input, trigger);
                        if (!string.IsNullOrEmpty(action))
                        {
                            intent.Type = IntentType.Task;
                            intent.Action = action;
                            intent.Topic = DetectTopic(action);
                            break;
                        }
                    }
                }
            }

            // Always detect sentiment
            intent.Sentiment = DetectSentiment(input);

            // ***** INTEGRATION WITH ACTIVITY LOGGER *****
            // Build a descriptive log message
            string logMsg = $"Parsed: \"{input}\" -> Intent: {intent.Type}";
            if (!string.IsNullOrEmpty(intent.Action))
                logMsg += $", Action: {intent.Action}";
            if (!string.IsNullOrEmpty(intent.Topic))
                logMsg += $", Topic: {intent.Topic}";
            logMsg += $", Sentiment: {intent.Sentiment}";

            ActivityLogger.Instance.AddLog(logMsg);

            return intent;
        }

        // IMPROVED SENTIMENT DETECTOR
        private static class SentimentDetector
        {
            private static readonly Dictionary<string, (int score, string sentiment)> sentimentWords =
                new Dictionary<string, (int, string)>(StringComparer.OrdinalIgnoreCase)
                {
                    { "angry", (-3, "angry") }, { "frustrated", (-3, "frustrated") },
                    { "annoying", (-2, "angry") }, { "hate", (-3, "angry") },
                    { "mad", (-3, "angry") }, { "pissed", (-3, "angry") },
                    { "worried", (-3, "worried") }, { "anxious", (-3, "worried") },
                    { "nervous", (-2, "worried") }, { "scared", (-3, "worried") },
                    { "fear", (-3, "worried") }, { "stressed", (-2, "worried") },
                    { "overwhelmed", (-2, "worried") }, { "concerned", (-2, "worried") },
                    { "sad", (-3, "sad") }, { "depressed", (-3, "sad") },
                    { "upset", (-2, "sad") }, { "unhappy", (-2, "sad") },
                    { "terrible", (-3, "sad") }, { "awful", (-3, "sad") },
                    { "miserable", (-3, "sad") },
                    { "happy", (3, "happy") }, { "excited", (3, "happy") },
                    { "great", (3, "happy") }, { "awesome", (3, "happy") },
                    { "wonderful", (3, "happy") }, { "fantastic", (3, "happy") },
                    { "good", (2, "happy") }, { "amazing", (3, "happy") },
                    { "curious", (2, "curious") }, { "interesting", (2, "curious") },
                    { "learn", (1, "curious") }, { "tell me", (1, "curious") },
                    { "explain", (1, "curious") },
                    { "understand", (2, "confident") }, { "got it", (2, "confident") },
                    { "clear", (2, "confident") }, { "makes sense", (2, "confident") },
                    { "i see", (1, "confident") }
                };

            private static readonly HashSet<string> negations = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "not", "no", "never", "don't", "dont", "isn't", "arent", "wasn't" };

            private static readonly HashSet<string> intensifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "very", "really", "extremely", "so", "too", "quite" };

            public static string Detect(string input)
            {
                if (string.IsNullOrWhiteSpace(input)) return "neutral";

                string[] tokens = Regex.Split(input.ToLowerInvariant(), @"\W+")
                                       .Where(t => !string.IsNullOrEmpty(t))
                                       .ToArray();

                var sentimentScores = new Dictionary<string, int>();
                bool negateNext = false;
                int intensifierMultiplier = 1;

                for (int i = 0; i < tokens.Length; i++)
                {
                    string word = tokens[i];

                    if (negations.Contains(word))
                    {
                        negateNext = !negateNext;
                        continue;
                    }

                    if (intensifiers.Contains(word))
                    {
                        intensifierMultiplier *= 2;
                        continue;
                    }

                    if (sentimentWords.TryGetValue(word, out var entry))
                    {
                        int score = entry.score;
                        string sentiment = entry.sentiment;

                        if (negateNext)
                        {
                            score = -score;
                            negateNext = false;
                        }
                        score *= intensifierMultiplier;

                        if (!sentimentScores.ContainsKey(sentiment))
                            sentimentScores[sentiment] = 0;
                        sentimentScores[sentiment] += score;

                        intensifierMultiplier = 1;
                    }
                    else
                    {
                        // Reset intensifier if not used, but keep negation alive for the next sentiment word
                        intensifierMultiplier = 1;
                    }
                }

                if (sentimentScores.Count == 0)
                    return "neutral";

                return sentimentScores.OrderByDescending(kvp => kvp.Value).First().Key;
            }
        }
    }
}