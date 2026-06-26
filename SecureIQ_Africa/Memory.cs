using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecureIQ_Africa
{
    public class Memory
    {
        private string userName = null;
        private List<string> conversationHistory = new List<string>();
        private Dictionary<string, int> topicInterests = new Dictionary<string, int>();
        private Dictionary<string, DateTime> lastTopicDiscussed = new Dictionary<string, DateTime>();
        private List<string> userQuestions = new List<string>();
        private Dictionary<string, List<string>> topicContext = new Dictionary<string, List<string>>();
        private Dictionary<string, string> userPreferences = new Dictionary<string, string>();

        private string lastUserMessage = "";
        private string lastBotResponse = "";
        private DateTime lastInteraction = DateTime.Now;
        private int interactionCount = 0;
        private static readonly Random random = new Random();

        // NEW: sentiment history
        private List<string> sentimentHistory = new List<string>();
        private const int MaxSentimentHistory = 10;

        public Memory()
        {
            InitializeTopicContext();
        }

        private void InitializeTopicContext()
        {
            topicContext = new Dictionary<string, List<string>>
            {
                { "password", new List<string> { "strength", "management", "creation", "storage", "expiry" } },
                { "phishing", new List<string> { "email", "links", "scams", "prevention", "reporting" } },
                { "malware", new List<string> { "viruses", "ransomware", "trojan", "antivirus", "removal" } },
                { "vpn", new List<string> { "privacy", "encryption", "protocols", "public wifi", "providers" } },
                { "2fa", new List<string> { "authentication", "codes", "backup", "apps", "security keys" } },
                { "privacy", new List<string> { "data protection", "personal info", "tracking", "cookies", "gdpr" } },
                { "wifi", new List<string> { "public wifi", "home network", "security", "encryption", "hotspot" } },
                { "backup", new List<string> { "data backup", "cloud storage", "external drive", "recovery" } }
            };
        }

        public void SetUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                userName = name;
                AddToConversation($"User name set to: {name}");
            }
        }

        public string GetUserName()
        {
            return userName;
        }

        public void StoreUserPreference(string category, string value)
        {
            if (!string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(value))
            {
                userPreferences[category] = value;
                AddToConversation($"User preference stored: {category} = {value}");
            }
        }

        public string GetUserPreference(string category)
        {
            if (userPreferences.ContainsKey(category))
                return userPreferences[category];
            return null;
        }

        public Dictionary<string, string> GetAllUserPreferences()
        {
            return new Dictionary<string, string>(userPreferences);
        }

        public string GetPersonalizedGreeting()
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (userPreferences.ContainsKey("favorite_topic"))
                {
                    string favTopic = userPreferences["favorite_topic"];
                    return $"Hello {userName}! I remember you're interested in {favTopic}. How can I help you with cybersecurity today?";
                }

                if (topicInterests.Count > 0)
                {
                    var topTopics = topicInterests.OrderByDescending(x => x.Value).Take(2).ToList();
                    if (topTopics.Any())
                    {
                        string topics = string.Join(" and ", topTopics.Select(t => t.Key));
                        return $"Hello {userName}! As someone interested in {topics}, I have some relevant cybersecurity tips for you today. How can I help?";
                    }
                }

                if (conversationHistory.Count > 0)
                {
                    return $"Hello {userName}! I'm your cybersecurity awareness bot. How can I help you stay safe online today?";
                }

                return $"Hello {userName}! I'm your cybersecurity awareness bot. How can I help you stay safe online today?";
            }

            return "Hello! I'm your cybersecurity awareness bot. What's your name?";
        }

        public void AddToConversation(string message)
        {
            conversationHistory.Add($"{DateTime.Now:HH:mm:ss} - {message}");

            if (conversationHistory.Count > 100)
            {
                conversationHistory.RemoveAt(0);
            }
        }

        public void DetectInterest(string userInput)
        {
            string normalizedInput = userInput.ToLower();
            lastUserMessage = userInput;
            lastInteraction = DateTime.Now;
            interactionCount++;

            var topicKeywords = new Dictionary<string, string[]>
            {
                { "password", new[] { "password", "passphrase", "login", "credential", "account access" } },
                { "phishing", new[] { "phishing", "scam", "fake email", "suspicious link", "fraud" } },
                { "malware", new[] { "malware", "virus", "trojan", "ransomware", "spyware" } },
                { "vpn", new[] { "vpn", "virtual private network", "encryption", "privacy" } },
                { "2fa", new[] { "2fa", "two factor", "authentication", "mfa", "multi factor" } },
                { "wifi", new[] { "wifi", "wireless", "network", "hotspot" } },
                { "backup", new[] { "backup", "restore", "data loss", "recovery" } },
                { "privacy", new[] { "privacy", "personal data", "data protection", "gdpr" } }
            };

            foreach (var topic in topicKeywords)
            {
                foreach (var keyword in topic.Value)
                {
                    if (normalizedInput.Contains(keyword))
                    {
                        if (topicInterests.ContainsKey(topic.Key))
                            topicInterests[topic.Key]++;
                        else
                            topicInterests[topic.Key] = 1;

                        lastTopicDiscussed[topic.Key] = DateTime.Now;
                        break;
                    }
                }
            }

            if (normalizedInput.Contains("?") || normalizedInput.Contains("how") || normalizedInput.Contains("what"))
            {
                userQuestions.Add(userInput);
                if (userQuestions.Count > 20) userQuestions.RemoveAt(0);
            }

            AddToConversation($"User asked: {userInput}");
        }

        public bool IsAskingAgain(string userInput)
        {
            string lowerInput = userInput.ToLower();
            return lowerInput.Contains("again") ||
                   lowerInput.Contains("repeat") ||
                   lowerInput.Contains("say that again") ||
                   lowerInput.Contains("what did you say");
        }

        public List<string> GetFavoriteTopics()
        {
            return topicInterests
                .OrderByDescending(x => x.Value)
                .Take(3)
                .Select(x => x.Key)
                .ToList();
        }

        public string GetPersonalizedResponse(string topic, string baseResponse)
        {
            string favoriteTopic = GetUserPreference("favorite_topic");

            if (!string.IsNullOrEmpty(favoriteTopic) && favoriteTopic == topic)
            {
                return $"As someone who's interested in {topic}, you'll find this particularly useful:\n{baseResponse}";
            }

            if (topicInterests.ContainsKey(topic) && topicInterests[topic] > 1)
            {
                return $"Since you've asked about {topic} before, here's additional insight:\n{baseResponse}";
            }

            return baseResponse;
        }

        public string SuggestTopic()
        {
            string favoriteTopic = GetUserPreference("favorite_topic");
            if (!string.IsNullOrEmpty(favoriteTopic))
            {
                return $"Since you're interested in {favoriteTopic}, would you like to learn more about {favoriteTopic} security today? I have some great tips to share!";
            }

            if (topicInterests.Count > 0)
            {
                var topTopic = topicInterests.OrderByDescending(x => x.Value).First();
                return $"Based on our previous conversations, you seemed interested in {topTopic.Key}. Would you like to learn more about {topTopic.Key} security?";
            }

            if (!string.IsNullOrEmpty(userName))
            {
                string[] suggestions = {
                    "password security",
                    "phishing prevention",
                    "malware protection",
                    "VPN usage",
                    "2FA setup",
                    "data privacy"
                };
                return $"{userName}, how about learning about {suggestions[random.Next(suggestions.Length)]}? It's an important cybersecurity topic!";
            }

            string[] defaultSuggestions = {
                "password security",
                "phishing prevention",
                "malware protection",
                "VPN usage",
                "2FA setup"
            };
            return $"How about learning about {defaultSuggestions[random.Next(defaultSuggestions.Length)]}? It's an important cybersecurity topic!";
        }

        public string RecallPreviousContext(string topic)
        {
            if (lastTopicDiscussed.ContainsKey(topic))
            {
                TimeSpan timeSince = DateTime.Now - lastTopicDiscussed[topic];
                int interestLevel = topicInterests.ContainsKey(topic) ? topicInterests[topic] : 0;

                if (interestLevel > 0)
                {
                    if (timeSince.TotalMinutes < 30)
                    {
                        return $"I remember you asked about {topic} before. You've shown interest in this topic {interestLevel} time(s). Would you like me to recap or share new information?";
                    }
                    else
                    {
                        return $"Earlier, you showed interest in {topic}. Would you like to continue learning about this topic?";
                    }
                }
            }
            return null;
        }

        public string GetConversationSummary()
        {
            if (conversationHistory.Count == 0)
                return "We haven't had any conversations yet.";

            StringBuilder summary = new StringBuilder();
            summary.AppendLine("=== Conversation Summary ===");
            summary.AppendLine($"Total interactions: {interactionCount}");
            summary.AppendLine($"Last interaction: {lastInteraction:yyyy-MM-dd HH:mm:ss}");

            if (!string.IsNullOrEmpty(userName))
            {
                summary.AppendLine($"\nUser: {userName}");
            }

            if (userPreferences.Count > 0)
            {
                summary.AppendLine("\nUser Preferences:");
                foreach (var pref in userPreferences)
                {
                    if (pref.Key == "favorite_topic")
                        summary.AppendLine($"  - Favorite topic: {pref.Value}");
                    else
                        summary.AppendLine($"  - {pref.Key}: {pref.Value}");
                }
            }

            if (topicInterests.Count > 0)
            {
                summary.AppendLine("\nTopics you're interested in:");
                foreach (var topic in topicInterests.OrderByDescending(x => x.Value).Take(5))
                {
                    summary.AppendLine($"  - {topic.Key}: mentioned {topic.Value} time(s)");
                }
            }

            return summary.ToString();
        }

        public string Recall(string query)
        {
            string lowerQuery = query.ToLower();

            if (lowerQuery.Contains("favorite") && lowerQuery.Contains("topic"))
            {
                string favTopic = GetUserPreference("favorite_topic");
                if (!string.IsNullOrEmpty(favTopic))
                {
                    return $"You told me you're interested in {favTopic}. Would you like to learn more about {favTopic} today?";
                }
                return "You haven't told me your favorite cybersecurity topic yet. What topics interest you?";
            }

            if (lowerQuery.Contains("my name") || (lowerQuery.Contains("what") && lowerQuery.Contains("name")))
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    return $"Your name is {userName}! I remember you told me earlier.";
                }
                return "I don't know your name yet. Could you tell me?";
            }

            foreach (var topic in topicContext.Keys)
            {
                if (lowerQuery.Contains(topic))
                {
                    return RecallPreviousContext(topic) ?? $"I can help you with {topic}. What specific aspect would you like to know about?";
                }
            }

            if (lowerQuery.Contains("last") && lowerQuery.Contains("question"))
            {
                if (userQuestions.Count > 0)
                {
                    return $"Your last question was: \"{userQuestions.Last()}\". Would you like me to answer it again?";
                }
            }

            if (lowerQuery.Contains("favorite") || lowerQuery.Contains("interested in") || lowerQuery.Contains("what do i like"))
            {
                var favorites = GetFavoriteTopics();
                string favTopic = GetUserPreference("favorite_topic");

                if (!string.IsNullOrEmpty(favTopic))
                {
                    return $"I remember you're particularly interested in {favTopic}. You've also shown interest in {string.Join(", ", favorites.Where(f => f != favTopic).Take(2))}. Would you like to explore any of these topics further?";
                }
                else if (favorites.Any())
                {
                    return $"Based on our conversations, you've shown interest in: {string.Join(", ", favorites)}. Would you like to learn more about any of these topics?";
                }
                return "I haven't noticed any specific topics you're interested in yet. Feel free to ask me about cybersecurity!";
            }

            if (lowerQuery.Contains("what did we talk about") || lowerQuery.Contains("conversation so far"))
            {
                return GetConversationSummary();
            }

            return null;
        }

        public void SetLastBotResponse(string response)
        {
            lastBotResponse = response;
            AddToConversation($"Bot responded: {response.Substring(0, Math.Min(50, response.Length))}...");
        }

        public List<string> GetRecentHistory(int count = 5)
        {
            return conversationHistory.Skip(Math.Max(0, conversationHistory.Count - count)).ToList();
        }

        public void ClearMemory()
        {
            userName = null;
            conversationHistory.Clear();
            topicInterests.Clear();
            lastTopicDiscussed.Clear();
            userQuestions.Clear();
            userPreferences.Clear();
            lastUserMessage = "";
            lastBotResponse = "";
            interactionCount = 0;
            sentimentHistory.Clear();   // added
        }

        public bool IsAskingToRecall(string userInput)
        {
            string lowerInput = userInput.ToLower();
            return lowerInput.Contains("remember") ||
                   lowerInput.Contains("recall") ||
                   lowerInput.Contains("do you remember") ||
                   lowerInput.Contains("what did i ask") ||
                   lowerInput.Contains("previous") ||
                   lowerInput.Contains("before") ||
                   lowerInput.Contains("what do i like") ||
                   lowerInput.Contains("my favorite") ||
                   lowerInput.Contains("what is my name");
        }

        public string GetLastUserMessage()
        {
            return lastUserMessage;
        }

        public string GetLastBotResponse()
        {
            return lastBotResponse;
        }

        public int GetInteractionCount()
        {
            return interactionCount;
        }

        // ---- NEW SENTIMENT METHODS ----
        public void StoreSentiment(string sentiment)
        {
            if (string.IsNullOrEmpty(sentiment) || sentiment == "neutral")
                return;
            sentimentHistory.Add(sentiment);
            if (sentimentHistory.Count > MaxSentimentHistory)
                sentimentHistory.RemoveAt(0);
        }

        public string GetDominantRecentSentiment()
        {
            if (sentimentHistory.Count == 0) return "neutral";
            return sentimentHistory.GroupBy(s => s)
                                   .OrderByDescending(g => g.Count())
                                   .First().Key;
        }

        public string GetSentimentSummary()
        {
            if (sentimentHistory.Count == 0) return "No emotional data yet.";
            var dominant = GetDominantRecentSentiment();
            return $"You've been feeling {dominant} recently.";
        }

        public void ClearSentimentHistory() => sentimentHistory.Clear();
    }
}