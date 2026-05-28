using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Track conversation context for better recall
        private string lastUserMessage = "";
        private string lastBotResponse = "";
        private DateTime lastInteraction = DateTime.Now;
        private int interactionCount = 0;

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
                { "2fa", new List<string> { "authentication", "codes", "backup", "apps", "security keys" } }
            };
        }

        // Set user name
        public void SetUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                userName = name;
                AddToConversation($"User name set to: {name}");
            }
        }

        // Get user name
        public string GetUserName()
        {
            return userName;
        }

        // Get personalized greeting based on memory
        public string GetPersonalizedGreeting()
        {
            if (!string.IsNullOrEmpty(userName))
            {
                // Check if we have previous interests
                if (topicInterests.Count > 0)
                {
                    var topTopics = topicInterests.OrderByDescending(x => x.Value).Take(2).ToList();
                    if (topTopics.Any())
                    {
                        string topics = string.Join(" and ", topTopics.Select(t => t.Key));
                        return $"Hello {userName}! I'm your cybersecurity awareness bot. How can I help you stay safe online today?";
                    }
                }

                // Check if we have conversation history
                if (conversationHistory.Count > 0)
                {
                    return $"Hello {userName}! I'm your cybersecurity awareness bot. How can I help you stay safe online today?";
                }

                return $"Hello {userName}! I'm your cybersecurity awareness bot. How can I help you stay safe online today?";
            }

            return "Hello! I'm your cybersecurity awareness bot. What's your name?";
        }

        // Add to conversation history
        public void AddToConversation(string message)
        {
            conversationHistory.Add($"{DateTime.Now:HH:mm:ss} - {message}");

            // Keep history manageable (last 100 messages)
            if (conversationHistory.Count > 100)
            {
                conversationHistory.RemoveAt(0);
            }
        }

        // Detect and track user interests
        public void DetectInterest(string userInput)
        {
            string normalizedInput = userInput.ToLower();
            lastUserMessage = userInput;
            lastInteraction = DateTime.Now;
            interactionCount++;

            // Define topic keywords
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

            // Increment interest counts
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

                        // Track when topic was last discussed
                        lastTopicDiscussed[topic.Key] = DateTime.Now;
                        break;
                    }
                }
            }

            // Store user question
            if (normalizedInput.Contains("?") || normalizedInput.Contains("how") || normalizedInput.Contains("what"))
            {
                userQuestions.Add(userInput);
                if (userQuestions.Count > 20) userQuestions.RemoveAt(0);
            }

            AddToConversation($"User asked: {userInput}");
        }

        // Check if user is asking for repetition
        public bool IsAskingAgain(string userInput)
        {
            string lowerInput = userInput.ToLower();
            return lowerInput.Contains("again") ||
                   lowerInput.Contains("repeat") ||
                   lowerInput.Contains("say that again") ||
                   lowerInput.Contains("what did you say");
        }

        // Get user's favorite topics based on interest count
        public List<string> GetFavoriteTopics()
        {
            return topicInterests
                .OrderByDescending(x => x.Value)
                .Take(3)
                .Select(x => x.Key)
                .ToList();
        }

        // Suggest a topic based on memory
        public string SuggestTopic()
        {
            // Suggest based on previous interests
            if (topicInterests.Count > 0)
            {
                var topTopic = topicInterests.OrderByDescending(x => x.Value).First();
                return $"Based on our previous conversations, you seemed interested in {topTopic.Key}. Would you like to learn more about {topTopic.Key} security?";
            }

            // Default suggestions for new users
            string[] suggestions = {
                "password security",
                "phishing prevention",
                "malware protection",
                "VPN usage",
                "2FA setup"
            };

            Random random = new Random();
            return $"How about learning about {suggestions[random.Next(suggestions.Length)]}? It's an important cybersecurity topic!";
        }

        // Recall previous conversation context
        public string RecallPreviousContext(string topic)
        {
            if (lastTopicDiscussed.ContainsKey(topic))
            {
                TimeSpan timeSince = DateTime.Now - lastTopicDiscussed[topic];
                if (timeSince.TotalMinutes < 30) // Recall within 30 minutes
                {
                    int interestLevel = topicInterests.ContainsKey(topic) ? topicInterests[topic] : 0;
                    return $"I remember you asked about {topic} before. You've shown interest in this topic {interestLevel} time(s). Would you like me to recap or share new information?";
                }
            }
            return null;
        }

        // Get conversation summary
        public string GetConversationSummary()
        {
            if (conversationHistory.Count == 0)
                return "We haven't had any conversations yet.";

            StringBuilder summary = new StringBuilder();
            summary.AppendLine($"=== Conversation Summary ===");
            summary.AppendLine($"Total interactions: {interactionCount}");
            summary.AppendLine($"Last interaction: {lastInteraction:yyyy-MM-dd HH:mm:ss}");

            if (topicInterests.Count > 0)
            {
                summary.AppendLine($"\nTopics you're interested in:");
                foreach (var topic in topicInterests.OrderByDescending(x => x.Value))
                {
                    summary.AppendLine($"  - {topic.Key}: mentioned {topic.Value} time(s)");
                }
            }

            if (!string.IsNullOrEmpty(userName))
            {
                summary.AppendLine($"\nUser: {userName}");
            }

            return summary.ToString();
        }

        // Recall specific information from memory
        public string Recall(string query)
        {
            string lowerQuery = query.ToLower();

            // Check for topic recall
            foreach (var topic in topicContext.Keys)
            {
                if (lowerQuery.Contains(topic))
                {
                    return RecallPreviousContext(topic) ?? $"I can help you with {topic}. What specific aspect would you like to know about?";
                }
            }

            // Check for conversation recall
            if (lowerQuery.Contains("last") && lowerQuery.Contains("question"))
            {
                if (userQuestions.Count > 0)
                {
                    return $"Your last question was: \"{userQuestions.Last()}\". Would you like me to answer it again?";
                }
            }

            // Recall favorite topics
            if (lowerQuery.Contains("favorite") || lowerQuery.Contains("interested in"))
            {
                var favorites = GetFavoriteTopics();
                if (favorites.Any())
                {
                    return $"Based on our conversations, you've shown interest in: {string.Join(", ", favorites)}. Would you like to explore any of these topics further?";
                }
                return "I haven't noticed any specific topics you're interested in yet. Feel free to ask me about cybersecurity!";
            }

            return null;
        }

        // Set bot response for context
        public void SetLastBotResponse(string response)
        {
            lastBotResponse = response;
            AddToConversation($"Bot responded: {response.Substring(0, Math.Min(50, response.Length))}...");
        }

        // Get recent conversation history
        public List<string> GetRecentHistory(int count = 5)
        {
            return conversationHistory.Skip(Math.Max(0, conversationHistory.Count - count)).ToList();
        }

        // Clear memory (for privacy/reset)
        public void ClearMemory()
        {
            userName = null;
            conversationHistory.Clear();
            topicInterests.Clear();
            lastTopicDiscussed.Clear();
            userQuestions.Clear();
            lastUserMessage = "";
            lastBotResponse = "";
            interactionCount = 0;
        }

        // Check if user is asking to recall something
        public bool IsAskingToRecall(string userInput)
        {
            string lowerInput = userInput.ToLower();
            return lowerInput.Contains("remember") ||
                   lowerInput.Contains("recall") ||
                   lowerInput.Contains("do you remember") ||
                   lowerInput.Contains("what did i ask") ||
                   lowerInput.Contains("previous") ||
                   lowerInput.Contains("before");
        }
    }
}