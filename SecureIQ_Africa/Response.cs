using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureIQ_Africa
{
    public class Response
    {
        // Properties
        public string name { get; set; }

        // Fields
        private SecureData data = new SecureData();
        private Memory memory = new Memory();
        private ResponseTips tips = new ResponseTips();
        private string lastTopic = null;
        private int followUpCount = 0;
        private Dictionary<string, string> responseCache = new Dictionary<string, string>();
        private static readonly Random random = new Random();

        // Fallback responses for variety
        private string[] fallbackResponses = new[]
        {
            "Sorry, I don't understand that yet. Try asking about passwords, phishing, malware, or Wi-Fi security.",
            "I'm still learning! Could you ask about cybersecurity topics like 2FA, VPNs, or safe browsing instead?",
            "Hmm, I don't know that one. Want to learn about password security or phishing prevention instead?",
            "I'm not sure about that. Feel free to ask me about online safety, data privacy, or secure browsing!",
            "That's outside my knowledge for now. Would you like to learn about protecting your online accounts instead?"
        };

        // Confusion detection phrases
        private string[] confusionPhrases = new[]
        {
            "i don't understand",
            "confused",
            "explain simply",
            "too technical",
            "simpler terms",
            "i'm lost",
            "not clear",
            "what do you mean",
            "huh",
            "what",
            "come again"
        };

        // Constructor
        public Response()
        {
            name = null;
        }

        // Set user name
        public void SetUserName(string userName)
        {
            name = userName;
            memory.SetUserName(userName);
        }

        // Get user name
        public string GetUserName()
        {
            return name ?? memory.GetUserName();
        }

        // Check if user name is set
        public bool HasUserName()
        {
            return GetUserName() != null;
        }

        // Get personalized greeting
        public string GetPersonalizedGreeting()
        {
            return memory.GetPersonalizedGreeting();
        }

        // Helper method to check if a word appears as a whole word
        private bool IsWholeWord(string text, string word)
        {
            int index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            if (index < 0) return false;

            bool beforeOk = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
            bool afterOk = index + word.Length >= text.Length ||
                           !char.IsLetterOrDigit(text[index + word.Length]);
            return beforeOk && afterOk;
        }

        // Split input into words
        private string[] SplitWords(string input)
        {
            return input.Split(new char[] { ' ', '\t', '\r', '\n' },
                               StringSplitOptions.RemoveEmptyEntries);
        }

        // Helper method to sanitize input
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
            return input.Trim();
        }

        // Check if user is expressing confusion
        private bool IsUserConfused(string input)
        {
            string lowerInput = input.ToLower();
            return confusionPhrases.Any(phrase => lowerInput.Contains(phrase));
        }

        // Check if user wants another tip
        private bool WantsAnotherTip(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("another tip") ||
                   lowerInput.Contains("more tips") ||
                   lowerInput.Contains("give me another") ||
                   lowerInput.Contains("another one") ||
                   lowerInput.Contains("more advice") ||
                   lowerInput.Contains("another suggestion");
        }

        // Check if user wants to continue current topic
        private bool WantsToContinue(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("tell me more") ||
                   lowerInput.Contains("elaborate") ||
                   lowerInput.Contains("explain further") ||
                   lowerInput.Contains("go on") ||
                   lowerInput.Contains("continue") ||
                   lowerInput.Contains("and then?") ||
                   lowerInput.Contains("what else") ||
                   lowerInput.Contains("more about") ||
                   lowerInput.Contains("keep going");
        }

        // Handle memory recall requests
        private string HandleMemoryRecall(string userInput)
        {
            string normalizedInput = userInput.ToLower();

            // Check if user wants to recall something from memory
            if (memory.IsAskingToRecall(userInput))
            {
                string recalledInfo = memory.Recall(userInput);
                if (recalledInfo != null)
                {
                    return recalledInfo;
                }
                return "I remember our conversation! What specific information would you like me to recall?";
            }

            // Check for conversation summary request
            if (normalizedInput.Contains("summary") ||
                normalizedInput.Contains("conversation so far") ||
                normalizedInput.Contains("what did we talk about"))
            {
                return memory.GetConversationSummary();
            }

            // Check for favorite topics recall
            if (normalizedInput.Contains("what do i like") ||
                normalizedInput.Contains("my interests") ||
                normalizedInput.Contains("topics i ask about") ||
                normalizedInput.Contains("what have i asked"))
            {
                var favorites = memory.GetFavoriteTopics();
                if (favorites.Any())
                {
                    return $"You've shown interest in: {string.Join(", ", favorites)}. Would you like to learn more about any of these?";
                }
                return "You haven't asked about any specific topics yet. Try asking me about passwords, phishing, or malware!";
            }

            // Check for last question recall
            if (normalizedInput.Contains("last question") ||
                normalizedInput.Contains("previous question") ||
                normalizedInput.Contains("what did i just ask"))
            {
                var recentHistory = memory.GetRecentHistory(2);
                if (recentHistory.Count > 0)
                {
                    return $"Your last question was about: {recentHistory.Last()}. Would you like me to elaborate?";
                }
            }

            return null;
        }

        // Get another random tip for the current topic
        private string GetAnotherTip()
        {
            if (string.IsNullOrEmpty(lastTopic))
                return null;

            string tip = tips.GetRandomTip(lastTopic, followUpCount);
            if (tip != null)
            {
                followUpCount++;
                return tip;
            }
            return null;
        }

        // Get simplified explanation for confused users
        private string GetSimplifiedExplanation(string topic)
        {
            switch (topic)
            {
                case "password":
                    return "Let me explain simply: A strong password is like a good lock on your front door. Make it long (12+ characters), mix letters and numbers, and never use the same lock for every door (don't reuse passwords). Simple enough?";
                case "phishing":
                    return "Think of phishing like a fake phone call from someone pretending to be your bank. Scammers send fake emails or messages trying to trick you into giving them your info. Always check who's really asking! Make sense?";
                case "malware":
                    return "Malware is like a computer virus - it's bad software that can damage your device or steal your info. Just like you avoid touching dirty things, avoid downloading suspicious files. Does that help explain it?";
                case "vpn":
                    return "A VPN is like a secret tunnel for your internet traffic. It hides what you're doing online from others, especially on public WiFi. Think of it as a privacy cloak for your computer. Is that clearer?";
                case "2fa":
                    return "2FA is like having both a key AND a code to open a safe. Even if someone steals your key (password), they still need the code (from your phone) to get in. It's an extra security step. Understand better now?";
                default:
                    return $"Let me explain {topic} in simpler terms. What specific part confuses you?";
            }
        }

        // Get response based on user input
        public string GetResponse(string userInput)
        {
            // Sanitize input
            userInput = SanitizeInput(userInput);

            // Check if input is empty
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please ask me something about cybersecurity!";

            // Check for memory recall first
            string memoryResponse = HandleMemoryRecall(userInput);
            if (memoryResponse != null)
            {
                CacheResponse(userInput.ToLower(), memoryResponse);
                return memoryResponse;
            }

            // Detect interests in the background
            memory.DetectInterest(userInput);

            // Normalize input early so all checks use it consistently
            string normalizedInput = userInput.ToLower().Trim();

            // Check if user wants another tip
            if (WantsAnotherTip(normalizedInput) && lastTopic != null)
            {
                string tip = GetAnotherTip();
                if (tip != null)
                {
                    string response = tip + "\n\nWould you like another tip?";
                    memory.SetLastBotResponse(response);
                    CacheResponse(normalizedInput, response);
                    return response;
                }
            }

            // Check if user wants to continue with current topic
            bool wantsToContinue = WantsToContinue(normalizedInput);

            if (wantsToContinue && lastTopic != null)
            {
                followUpCount++;
                string response = GetFollowUpResponse(lastTopic, followUpCount);
                memory.SetLastBotResponse(response);
                CacheResponse(normalizedInput, response);
                return response;
            }

            // Check if user is confused
            if (IsUserConfused(normalizedInput) && lastTopic != null)
            {
                string response = GetSimplifiedExplanation(lastTopic);
                memory.SetLastBotResponse(response);
                CacheResponse(normalizedInput, response);
                return response;
            }

            // Check if user is asking for suggestions
            if (normalizedInput.Contains("suggest") ||
                normalizedInput.Contains("recommend") ||
                normalizedInput.Contains("what should i learn"))
            {
                string response = memory.SuggestTopic();
                memory.SetLastBotResponse(response);
                CacheResponse(normalizedInput, response);
                return response;
            }

            // Check for name-related questions
            if (normalizedInput.Contains("what is my name") ||
                normalizedInput.Contains("do you know my name") ||
                normalizedInput.Contains("my name"))
            {
                string response = HasUserName()
                    ? $"Your name is {GetUserName()}!"
                    : "I don't know your name yet. Please tell me!";
                memory.SetLastBotResponse(response);
                CacheResponse(normalizedInput, response);
                return response;
            }

            // Check for repeat request
            if (memory.IsAskingAgain(userInput) && lastTopic != null)
            {
                followUpCount = 1;
                string response = "Let me repeat: " + GetFollowUpResponse(lastTopic, 0);
                memory.SetLastBotResponse(response);
                CacheResponse(normalizedInput, response);
                return response;
            }

            // Check cache first
            if (responseCache.TryGetValue(normalizedInput, out string cachedResponse))
            {
                memory.SetLastBotResponse(cachedResponse);
                return cachedResponse;
            }

            // ========== FIXED: CYBERSECURITY TOPICS FIRST (HIGHEST PRIORITY) ==========

            // Sort keywords by length (longest first) for better matching
            var sortedData = data.secureData
                .OrderByDescending(x => x.Value.keywords.Max(k => k.Length));

            // Split into words for matching
            string[] words = SplitWords(normalizedInput);

            // Check for keyword matches (Cybersecurity topics - HIGH PRIORITY)
            foreach (var item in sortedData)
            {
                foreach (var keyword in item.Value.keywords)
                {
                    // For multi-word keywords, use Contains on full string
                    if (keyword.Contains(' '))
                    {
                        if (normalizedInput.Contains(keyword))
                        {
                            lastTopic = item.Key;
                            followUpCount = 1;
                            string topicResponse = item.Value.response + "\n\nWould you like me to share some tips about this?";
                            CacheResponse(normalizedInput, topicResponse);
                            memory.SetLastBotResponse(topicResponse);
                            return topicResponse;
                        }
                    }
                    // For single-word keywords, check whole words only
                    else
                    {
                        bool matched = words.Contains(keyword) ||
                                       IsWholeWord(normalizedInput, keyword);
                        if (matched)
                        {
                            lastTopic = item.Key;
                            followUpCount = 1;
                            string topicResponse = item.Value.response + "\n\nWould you like me to share some tips about this?";
                            CacheResponse(normalizedInput, topicResponse);
                            memory.SetLastBotResponse(topicResponse);
                            return topicResponse;
                        }
                    }
                }
            }

            // ========== EMOTIONS CHECK SECOND (LOWER PRIORITY) ==========
            // Check for emotional responses - only if no cybersecurity topic matched
            foreach (var emotion in data.emotions)
            {
                foreach (var keyword in emotion.Value.keywords)
                {
                    if (IsWholeWord(normalizedInput, keyword))
                    {
                        string emotionResponse = emotion.Value.response;
                        CacheResponse(normalizedInput, emotionResponse);
                        memory.SetLastBotResponse(emotionResponse);
                        return emotionResponse;
                    }
                }
            }

            // Return random fallback response
            string fallback = fallbackResponses[random.Next(fallbackResponses.Length)];
            CacheResponse(normalizedInput, fallback);
            memory.SetLastBotResponse(fallback);
            return fallback;
        }

        // Get follow-up response based on last topic and follow-up count
        private string GetFollowUpResponse(string topic, int followNumber)
        {
            switch (topic)
            {
                case "password":
                    if (followNumber == 1)
                        return tips.GetRandomTip("password", 0) + "\n\nWould you like another password tip?";
                    else if (followNumber == 2)
                        return tips.GetRandomTip("password", 1) + "\n\nWant to hear more about password security?";
                    else
                        return tips.GetRandomTip("password", 2) + "\n\nWould you like to learn about 2FA?";

                case "phishing":
                    if (followNumber == 1)
                        return tips.GetRandomTip("phishing", 0) + "\n\nWant another phishing tip?";
                    else if (followNumber == 2)
                        return tips.GetRandomTip("phishing", 1) + "\n\nNeed more advice on avoiding scams?";
                    else
                        return tips.GetRandomTip("phishing", 2) + "\n\nWould you like more examples of phishing?";

                case "malware":
                    if (followNumber == 1)
                        return tips.GetRandomTip("malware", 0) + "\n\nWant another malware protection tip?";
                    else if (followNumber == 2)
                        return tips.GetRandomTip("malware", 1) + "\n\nNeed more protection advice?";
                    else
                        return tips.GetRandomTip("malware", 2) + "\n\nShall I continue with more security tips?";

                case "vpn":
                    if (followNumber == 1)
                        return tips.GetRandomTip("vpn", 0) + "\n\nWant another VPN tip?";
                    else if (followNumber == 2)
                        return tips.GetRandomTip("vpn", 1) + "\n\nNeed more VPN advice?";
                    else
                        return tips.GetRandomTip("vpn", 2) + "\n\nWould you like to learn more about VPN protocols?";

                case "2fa":
                    if (followNumber == 1)
                        return tips.GetRandomTip("2fa", 0) + "\n\nWant another 2FA tip?";
                    else if (followNumber == 2)
                        return tips.GetRandomTip("2fa", 1) + "\n\nNeed more security advice?";
                    else
                        return tips.GetRandomTip("2fa", 2) + "\n\nWould you like to learn about password managers next?";

                default:
                    return $"I can provide more detailed information about {topic}. What specific aspect would you like to know about?";
            }
        }

        // Cache response for future use
        private void CacheResponse(string input, string response)
        {
            if (responseCache.Count < 100 && !responseCache.ContainsKey(input))
            {
                responseCache[input] = response;
            }
        }

        // Reset response cache
        public void ResetCache()
        {
            responseCache.Clear();
        }

        // Reset conversation context
        public void ResetConversation()
        {
            lastTopic = null;
            followUpCount = 0;
            responseCache.Clear();
        }

        // Get memory instance for integration
        public Memory GetMemory()
        {
            return memory;
        }

        // Clear all memory
        public void ClearMemory()
        {
            memory.ClearMemory();
            ResetConversation();
        }

        // Process message with full context
        public ChatResponse ProcessMessage(string userMessage)
        {
            string responseMessage = GetResponse(userMessage);
            List<string> topics = memory.GetFavoriteTopics();

            return new ChatResponse
            {
                Message = responseMessage,
                UserName = GetUserName(),
                FavoriteTopics = topics,
                HasInterests = topics.Any(),
                SuggestedTopic = memory.SuggestTopic()
            };
        }
    }

    // Response model
    public class ChatResponse
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public List<string> FavoriteTopics { get; set; }
        public bool HasInterests { get; set; }
        public string SuggestedTopic { get; set; }
    }
}