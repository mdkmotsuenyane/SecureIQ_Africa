using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SecureIQ_Africa
{
    public class Response
    {
        //attributes with a getter and setter
        public string name { get; set; }

        //attributes
        private SecureData data = new SecureData();
        private Memory memory = new Memory();
        private ResponseTips tips = new ResponseTips();
        private string currentTopic = null;
        private int followUpCount = 0;
        private string expectedResponse = null;
        private string lastQuestion = null;
        private int consecutiveUnknownResponses = 0;
        private int consecutiveNoCount = 0;
        private bool conversationEnded = false;
        private string askingWhatsWrongFor = null;
        private Dictionary<string, string> responseCache = new Dictionary<string, string>();
        private static readonly Random random = new Random();

        // Validation constants
        private const int MAX_INPUT_LENGTH = 1000;
        private const int MAX_CONSECUTIVE_INVALID = 5;
        private int consecutiveInvalidInputs = 0;

        // Synonym mapping for topics
        private Dictionary<string, string[]> topicSynonyms = new Dictionary<string, string[]>()
        {
            { "password", new[] { "password", "pass", "login", "credential", "account access", "passwords", "password safety" } },
            { "phishing", new[] { "phishing", "scam", "scams", "fake email", "suspicious link", "fraud", "fraudulent", "spoof", "phish" } },
            { "malware", new[] { "malware", "virus", "viruses", "trojan", "ransomware", "spyware", "malicious" } },
            { "wifi", new[] { "wifi", "wi-fi", "wireless", "network", "hotspot", "router", "internet" } },
            { "vpn", new[] { "vpn", "virtual private network", "encryption", "private network" } },
            { "2fa", new[] { "2fa", "two factor", "two-factor", "authentication", "mfa", "multi factor", "2 factor" } },
            { "privacy", new[] { "privacy", "personal data", "data protection", "gdpr", "private" } },
            { "backup", new[] { "backup", "restore", "data loss", "recovery", "back up" } }
        };

        private string[] fallbackResponses = new[]
        {
            "I want to help you learn about cybersecurity. Could you ask me about specific topics like passwords, phishing, malware, or WiFi security?",
            "I'm here to help with cybersecurity questions. Try asking me about creating strong passwords, spotting phishing emails, or protecting your home WiFi.",
            "Let me help you stay safe online. What would you like to know about - password security, online privacy, or how to avoid scams?",
            "I specialize in cybersecurity awareness. You can ask me about 2FA, VPNs, safe browsing, or data backups. What interests you?"
        };

        private string[] confusionPhrases = new[]
        {
            "i don't understand", "confused", "explain simply", "too technical",
            "simpler terms", "i'm lost", "not clear", "what do you mean", "huh", "what", "come again"
        };

        private string[] dismissalPhrases = new[]
        {
            "it is fine", "it's fine", "that's fine", "nevermind", "never mind",
            "forget it", "skip it", "don't worry", "it's okay", "its okay",
            "that's okay", "no worries", "all good"
        };

        private string[] farewellPhrases = new[]
        {
            "goodbye", "bye", "quit", "exit", "i quit", "i'm done", "im done",
            "that's all", "no more", "stop", "end", "bye bye", "see you", "later"
        };

        private string[] gratitudePhrases = new[]
        {
            "thank you", "thanks", "thx", "appreciate it", "thank", "ty", "thanks a lot"
        };

        private string[] acknowledgmentAfterGoodbye = new[]
        {
            "okay", "ok", "k", "got it", "alright", "fine", "sure", "yeah", "yes"
        };

        private string[] questionWords = new[]
        {
            "why", "how", "what", "when", "where", "who", "which", "can you", "could you", "please explain"
        };

        public Response()
        {
            name = null;
        }

        public void SetUserName(string userName)
        {
            // Validate username
            if (!string.IsNullOrWhiteSpace(userName))
            {
                string validatedName = ValidateAndSanitizeInput(userName);
                if (!string.IsNullOrEmpty(validatedName) && validatedName.Length <= 50)
                {
                    name = validatedName;
                    memory.SetUserName(userName);
                }
            }
        }

        public string GetUserName()
        {
            return name ?? memory.GetUserName();
        }

        public bool HasUserName()
        {
            return GetUserName() != null;
        }

        public string GetPersonalizedGreeting()
        {
            return memory.GetPersonalizedGreeting();
        }

        // New validation method
        private string ValidateAndSanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Trim whitespace
            input = input.Trim();

            // Check length
            if (input.Length > MAX_INPUT_LENGTH)
                input = input.Substring(0, MAX_INPUT_LENGTH);

            // Remove control characters
            input = Regex.Replace(input, @"[\x00-\x1F\x7F]", "");


            // Normalize whitespace
            input = Regex.Replace(input, @"\s+", " ");

            return input.Trim();
        }

        // New method to check if input is gibberish
        private bool IsGibberishInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            // Check for excessive repetition of same character
            if (Regex.IsMatch(input, @"(.)\1{10,}"))
                return true;

            // Check for keyboard mashing patterns
            string lowerInput = input.ToLower();
            if (Regex.IsMatch(lowerInput, @"[asdfghjkl]{8,}") ||
                Regex.IsMatch(lowerInput, @"[qwertyuiop]{8,}") ||
                Regex.IsMatch(lowerInput, @"[zxcvbnm]{8,}"))
                return true;

            // Check if input has no vowels and is longer than 10 chars (likely gibberish)
            if (input.Length > 10 && !Regex.IsMatch(input, @"[aeiouAEIOU]"))
                return true;

            return false;
        }

        // New method to check for harmful patterns
        private bool ContainsHarmfulPatterns(string input)
        {
            string lowerInput = input.ToLower();

            // Check for SQL injection patterns
            string[] sqlPatterns = { "select ", "insert ", "update ", "delete ", "drop ", "create ", "alter ", "exec ", "execute " };
            foreach (string pattern in sqlPatterns)
            {
                if (lowerInput.Contains(pattern))
                    return true;
            }

            // Check for script tags
            if (lowerInput.Contains("<script") || lowerInput.Contains("javascript:"))
                return true;

            return false;
        }

        private bool IsWholeWord(string text, string word)
        {
            int index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            if (index < 0) return false;

            bool beforeOk = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
            bool afterOk = index + word.Length >= text.Length ||
                           !char.IsLetterOrDigit(text[index + word.Length]);
            return beforeOk && afterOk;
        }

        private string[] SplitWords(string input)
        {
            return input.Split(new char[] { ' ', '\t', '\r', '\n' },
                               StringSplitOptions.RemoveEmptyEntries);
        }

        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
            return input.Trim();
        }

        private bool IsUserConfused(string input)
        {
            string lowerInput = input.ToLower();
            return confusionPhrases.Any(phrase => lowerInput.Contains(phrase));
        }

        private bool IsDismissal(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return dismissalPhrases.Any(phrase => lowerInput.Contains(phrase));
        }

        private bool IsFarewell(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return farewellPhrases.Any(phrase => lowerInput.Contains(phrase));
        }

        private bool IsGratitude(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return gratitudePhrases.Any(phrase => lowerInput.Contains(phrase));
        }

        private bool IsAcknowledgmentAfterGoodbye(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return acknowledgmentAfterGoodbye.Any(phrase => lowerInput == phrase || lowerInput.Contains(phrase));
        }

        private bool IsQuestionWord(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return questionWords.Any(phrase => lowerInput == phrase || lowerInput.StartsWith(phrase));
        }

        private bool WantsAnotherTip(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("another tip") || lowerInput.Contains("more tips") ||
                   lowerInput.Contains("give me another") || lowerInput.Contains("another one") ||
                   lowerInput.Contains("more advice") || lowerInput.Contains("another suggestion");
        }

        private bool IsAffirmativeResponse(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return lowerInput == "yes" || lowerInput == "yeah" || lowerInput == "yep" ||
                   lowerInput == "sure" || lowerInput == "ok" || lowerInput == "okay" ||
                   lowerInput == "please" || lowerInput == "yeah sure" || lowerInput == "y" ||
                   lowerInput == "sure thing" || lowerInput == "definitely" || lowerInput == "absolutely" ||
                   lowerInput == "yes!" || lowerInput == "yeah!" || lowerInput == "sure!" ||
                   lowerInput == "ok!" || lowerInput == "k" || lowerInput == "kk" ||
                   lowerInput == "alright" || lowerInput == "alrighty" || lowerInput == "fine" ||
                   lowerInput == "got it" || lowerInput == "understood" || lowerInput == "correct";
        }

        private bool IsNegativeResponse(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return lowerInput == "no" || lowerInput == "nope" || lowerInput == "not really" ||
                   lowerInput == "no thanks" || lowerInput == "nah" || lowerInput == "no!" ||
                   lowerInput == "nope!" || lowerInput == "not interested";
        }

        private bool IsConversationEnding(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return lowerInput == "maybe later" || lowerInput == "not now" || lowerInput == "pass" ||
                   lowerInput == "no way" || lowerInput == "negative" || lowerInput == "nevermind" ||
                   lowerInput == "never mind" || lowerInput.Contains("don't want") ||
                   lowerInput == "i'm done" || lowerInput == "im done" || lowerInput == "that's all";
        }

        private bool IsGreeting(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return lowerInput == "hi" || lowerInput == "hello" || lowerInput == "hey" ||
                   lowerInput == "good morning" || lowerInput == "good afternoon" || lowerInput == "good evening" ||
                   lowerInput == "hi there" || lowerInput == "hello there";
        }

        private bool IsHelpRequest(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput == "help" || lowerInput == "what can you do" ||
                   lowerInput.Contains("what can i ask") || lowerInput.Contains("how do you work") ||
                   lowerInput == "commands";
        }

        private bool IsNameQuestion(string input)
        {
            string lowerInput = input.ToLower().Trim();
            return lowerInput.Contains("what is my name") || lowerInput.Contains("do you know my name") ||
                   lowerInput.Contains("my name") || lowerInput == "what's my name";
        }

        private bool IsEmptyOrGibberish(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true;
            string trimmed = input.Trim();
            if (trimmed.Length <= 2 && !IsAffirmativeResponse(trimmed) && !IsNegativeResponse(trimmed))
                return true;

            // Additional gibberish check
            if (IsGibberishInput(trimmed))
                return true;

            return false;
        }

        private string DetectTopicFromSynonyms(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput)) return null;

            string lowerInput = userInput.ToLower().Trim();

            foreach (var topic in topicSynonyms)
            {
                foreach (string synonym in topic.Value)
                {
                    if (lowerInput.Contains(synonym))
                    {
                        return topic.Key;
                    }
                }
            }

            return null;
        }

        private string DetectSentiment(string userInput)
        {
            string lowerInput = userInput.ToLower();

            // Angry detection
            if (lowerInput.Contains("angry") || lowerInput.Contains("frustrated") ||
                lowerInput.Contains("annoying") || lowerInput.Contains("hate") ||
                lowerInput.Contains("stupid") || lowerInput.Contains("makes me angry") ||
                lowerInput.Contains("i'm angry") || lowerInput.Contains("im angry") ||
                lowerInput.Contains("pissed") || lowerInput.Contains("mad"))
            {
                return "angry";
            }

            // Worried detection
            if (lowerInput.Contains("worried") || lowerInput.Contains("anxious") ||
                lowerInput.Contains("nervous") || lowerInput.Contains("scared") ||
                lowerInput.Contains("fear") || lowerInput.Contains("unsafe") ||
                lowerInput.Contains("stressed") || lowerInput.Contains("overwhelmed") ||
                lowerInput.Contains("concerned") || lowerInput.Contains("afraid"))
            {
                return "worried";
            }

            // Frustrated detection
            if (lowerInput.Contains("frustrated") || lowerInput.Contains("confusing") ||
                lowerInput.Contains("too hard") || lowerInput.Contains("difficult") ||
                lowerInput.Contains("tired of") || lowerInput.Contains("exhausted") ||
                lowerInput.Contains("annoyed"))
            {
                return "frustrated";
            }

            // Sad detection
            if (lowerInput.Contains("sad") || lowerInput.Contains("depressed") ||
                lowerInput.Contains("upset") || lowerInput.Contains("unhappy") ||
                lowerInput.Contains("terrible") || lowerInput.Contains("awful") ||
                lowerInput.Contains("miserable"))
            {
                return "sad";
            }

            // Happy detection
            if (lowerInput.Contains("happy") || lowerInput.Contains("excited") ||
                lowerInput.Contains("great") || lowerInput.Contains("awesome") ||
                lowerInput.Contains("wonderful") || lowerInput.Contains("fantastic") ||
                lowerInput.Contains("good") || lowerInput.Contains("amazing"))
            {
                return "happy";
            }

            // Curious detection
            if (lowerInput.Contains("curious") || lowerInput.Contains("interesting") ||
                lowerInput.Contains("tell me more") || lowerInput.Contains("want to learn") ||
                lowerInput.Contains("how does") || lowerInput.Contains("why is") ||
                lowerInput.Contains("what is") || lowerInput.Contains("explain"))
            {
                return "curious";
            }

            // Confident detection
            if (lowerInput.Contains("got it") || lowerInput.Contains("understand") ||
                lowerInput.Contains("makes sense") || lowerInput.Contains("clear") ||
                lowerInput.Contains("i see") || lowerInput.Contains("thanks") ||
                lowerInput.Contains("thank you"))
            {
                return "confident";
            }

            return "neutral";
        }

        private string GetEmpatheticResponseForEmotion(string emotion, string topic = null)
        {
            string userName = GetUserName();
            string namePrefix = string.IsNullOrEmpty(userName) ? "" : $"{userName}, ";

            switch (emotion)
            {
                case "angry":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}I understand that dealing with {topic} can be really frustrating. Let me share something that might help.\n\n";
                    else
                        return $"{namePrefix}I'm sorry to hear that you're feeling angry. ";

                case "worried":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}It's completely understandable to feel worried about {topic}. Your concern shows you care about your security! Let me share some practical tips.\n\n";
                    else
                        return $"{namePrefix}I'm sorry you're feeling worried. ";

                case "frustrated":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}I hear your frustration - {topic} can definitely be complicated. You're not alone. Let me break this down simply.\n\n";
                    else
                        return $"{namePrefix}I understand cybersecurity can be frustrating. ";

                case "sad":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}I'm sorry {topic} is making you feel this way. Let me share something that might help improve the situation.\n\n";
                    else
                        return $"{namePrefix}I'm sorry you're feeling down. ";

                case "happy":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}That's great that you're excited about {topic}! Let me share something interesting.\n\n";
                    else
                        return $"{namePrefix}I'm glad you're feeling positive! ";

                case "curious":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}Great curiosity about {topic}! Let me share something interesting.\n\n";
                    else
                        return $"{namePrefix}Great curiosity! Cybersecurity is fascinating. ";

                case "confident":
                    if (!string.IsNullOrEmpty(topic))
                        return $"{namePrefix}Excellent! You're on the right track with {topic}. Here's something valuable to add.\n\n";
                    else
                        return $"{namePrefix}Excellent! You're doing great. ";

                default:
                    return "";
            }
        }

        private string GetFollowUpQuestionForEmotion(string emotion)
        {
            switch (emotion)
            {
                case "angry":
                    return "What's wrong? I'm here to help with any cybersecurity concerns you have.";
                case "worried":
                    return "What specific cybersecurity concern do you have? I'm here to help.";
                case "frustrated":
                    return "What specifically is bothering you? Let me try to help.";
                case "sad":
                    return "What's making you feel this way? I'd like to help if I can.";
                case "happy":
                    return "Would you like to learn more about cybersecurity today?";
                case "curious":
                    return "What would you like to learn about?";
                case "confident":
                    return "Would you like to test your knowledge or learn something new?";
                default:
                    return "How can I help you with cybersecurity today?";
            }
        }

        private string GetResponseForTopic(string topic)
        {
            string baseResponse = "";
            foreach (var item in data.secureData)
            {
                if (item.Key == topic)
                {
                    baseResponse = item.Value.response;
                    break;
                }
            }

            if (string.IsNullOrEmpty(baseResponse))
            {
                baseResponse = $"Let me help you learn about {topic} security.";
            }

            string tip = tips.GetRandomTip(topic, 0);
            string personalizedTip = memory.GetPersonalizedResponse(topic, tip);

            currentTopic = topic;
            followUpCount = 1;
            expectedResponse = "learn_more";
            consecutiveUnknownResponses = 0;
            consecutiveNoCount = 0;
            conversationEnded = false;

            return $"{baseResponse}\n\nHere's a helpful tip:\n{personalizedTip}\n\nWould you like to learn more about {topic}?";
        }

        private string HandleMemoryOperations(string userInput)
        {
            string normalizedInput = userInput.ToLower().Trim();

            // Check for name question FIRST - HIGHEST PRIORITY
            if (IsNameQuestion(userInput))
            {
                if (!string.IsNullOrEmpty(GetUserName()))
                {
                    return $"Your name is {GetUserName()}! I remember you told me earlier.";
                }
                return "I don't know your name yet. Could you please tell me?";
            }

            // Store favorite topic - IMPROVED DETECTION
            bool isFavoriteTopic = normalizedInput.Contains("my favorite topic") ||
                                   normalizedInput.Contains("favorite topic is") ||
                                   normalizedInput.Contains("is my favorite topic") ||
                                   normalizedInput.Contains("favorite topic") ||
                                   normalizedInput.Contains("i love") ||
                                   normalizedInput.Contains("i like") ||
                                   (normalizedInput.Contains("interested in") && !normalizedInput.Contains("what is"));

            if (isFavoriteTopic)
            {
                string topic = DetectTopicFromSynonyms(normalizedInput);

                // Manual checks for common topics
                if (topic == null && (normalizedInput.Contains("password safety") || normalizedInput.Contains("password")))
                {
                    topic = "password";
                }
                if (topic == null && (normalizedInput.Contains("phishing") || normalizedInput.Contains("scam")))
                {
                    topic = "phishing";
                }
                if (topic == null && (normalizedInput.Contains("wifi") || normalizedInput.Contains("wi-fi")))
                {
                    topic = "wifi";
                }
                if (topic == null && (normalizedInput.Contains("malware") || normalizedInput.Contains("virus")))
                {
                    topic = "malware";
                }
                if (topic == null && (normalizedInput.Contains("vpn") || normalizedInput.Contains("virtual private")))
                {
                    topic = "vpn";
                }
                if (topic == null && (normalizedInput.Contains("2fa") || normalizedInput.Contains("two factor")))
                {
                    topic = "2fa";
                }
                if (topic == null && normalizedInput.Contains("privacy"))
                {
                    topic = "privacy";
                }
                if (topic == null && normalizedInput.Contains("backup"))
                {
                    topic = "backup";
                }

                if (topic != null)
                {
                    memory.StoreUserPreference("favorite_topic", topic);
                    currentTopic = topic;
                    expectedResponse = "learn_more";
                    lastQuestion = $"Would you like to learn more about {topic}?";
                    consecutiveUnknownResponses = 0;
                    consecutiveNoCount = 0;
                    conversationEnded = false;
                    askingWhatsWrongFor = null;

                    string tip = tips.GetRandomTip(topic, 0);
                    if (string.IsNullOrWhiteSpace(tip))
                    {
                        tip = "Would you like me to share specific security tips about this topic?";
                    }

                    return $"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online.\n\n" +
                           $"As someone interested in {topic}, here's a helpful tip:\n{tip}\n\n" +
                           $"{lastQuestion}";
                }
            }

            // Recall from memory
            if (memory.IsAskingToRecall(userInput))
            {
                string recalledInfo = memory.Recall(userInput);
                if (recalledInfo != null)
                {
                    expectedResponse = null;
                    consecutiveUnknownResponses = 0;
                    consecutiveNoCount = 0;
                    conversationEnded = false;
                    return recalledInfo;
                }
                return "I remember our conversation! What specific information would you like me to recall?";
            }

            // Conversation summary
            if (normalizedInput.Contains("summary") || normalizedInput.Contains("conversation so far") ||
                normalizedInput.Contains("what did we talk about"))
            {
                expectedResponse = null;
                consecutiveUnknownResponses = 0;
                consecutiveNoCount = 0;
                conversationEnded = false;
                return memory.GetConversationSummary();
            }

            // Favorite topics recall
            if (normalizedInput.Contains("what do i like") || normalizedInput.Contains("my interests") ||
                normalizedInput.Contains("topics i ask about") || normalizedInput.Contains("what have i asked") ||
                normalizedInput.Contains("what is my favorite topic"))
            {
                var favorites = memory.GetFavoriteTopics();
                string favTopic = memory.GetUserPreference("favorite_topic");
                expectedResponse = null;
                consecutiveUnknownResponses = 0;
                consecutiveNoCount = 0;
                conversationEnded = false;

                if (!string.IsNullOrEmpty(favTopic))
                {
                    return $"You told me you're interested in {favTopic}. " +
                           (favorites.Any() ? $"You've also shown interest in {string.Join(", ", favorites.Where(f => f != favTopic).Take(2))}." : "") +
                           " Would you like to learn more about any of these topics?";
                }
                else if (favorites.Any())
                {
                    return $"You've shown interest in: {string.Join(", ", favorites)}. Would you like to learn more about any of these?";
                }
                return "You haven't asked about any specific topics yet. Try asking me about passwords, phishing, or malware!";
            }

            return null;
        }

        private string ProvideTip(string topic, bool askForAnother = true)
        {
            string tip = tips.GetRandomTip(topic, followUpCount);
            string personalizedTip = memory.GetPersonalizedResponse(topic, tip);
            followUpCount++;

            if (askForAnother)
            {
                expectedResponse = "another_tip";
                lastQuestion = "Would you like another tip?";
                consecutiveUnknownResponses = 0;
                consecutiveNoCount = 0;
                conversationEnded = false;
                return $"{personalizedTip}\n\n{lastQuestion}";
            }
            else
            {
                expectedResponse = null;
                consecutiveUnknownResponses = 0;
                consecutiveNoCount = 0;
                conversationEnded = false;
                return personalizedTip;
            }
        }

        private string GetTopicSuggestions()
        {
            string favTopic = memory.GetUserPreference("favorite_topic");
            expectedResponse = "topic_selection";
            consecutiveUnknownResponses = 0;
            conversationEnded = false;

            if (!string.IsNullOrEmpty(favTopic))
            {
                return $"Since you're interested in {favTopic}, would you like to learn more about {favTopic}? Or I can suggest another topic like passwords, phishing, or malware protection.";
            }

            return "What cybersecurity topic would you like to learn about? I can help with:\n" +
                   "- Password security\n" +
                   "- Phishing detection (spotting scams and fake emails)\n" +
                   "- Malware protection\n" +
                   "- WiFi security\n" +
                   "- VPNs and privacy\n" +
                   "- Two-factor authentication (2FA)\n\n" +
                   "Just ask me about any of these topics!";
        }

        private string GetFarewellResponse()
        {
            conversationEnded = true;
            expectedResponse = null;
            currentTopic = null;
            followUpCount = 0;
            lastQuestion = null;
            consecutiveNoCount = 0;

            string userName = GetUserName();
            if (!string.IsNullOrEmpty(userName))
            {
                string[] farewells = {
                    $"Goodbye {userName}! Stay safe online. Feel free to come back anytime you have cybersecurity questions.",
                    $"Take care {userName}! Remember to keep your software updated and use strong passwords.",
                    $"See you later {userName}! Stay vigilant against online threats."
                };
                return farewells[random.Next(farewells.Length)];
            }

            string[] genericFarewells = {
                "Goodbye! Stay safe online. Feel free to return anytime you have cybersecurity questions.",
                "Take care! Remember to use strong passwords and enable 2FA where possible.",
                "See you later! Stay vigilant against phishing emails and suspicious links."
            };
            return genericFarewells[random.Next(genericFarewells.Length)];
        }

        private string GetGratitudeResponse()
        {
            conversationEnded = false;
            string userName = GetUserName();
            if (!string.IsNullOrEmpty(userName))
            {
                string[] thanks = {
                    $"You're welcome {userName}! I'm glad I could help. Would you like to learn about another topic?",
                    $"Happy to help, {userName}! Is there another cybersecurity topic you'd like to explore?",
                    $"Anytime, {userName}! Feel free to ask if you have more cybersecurity questions."
                };
                return thanks[random.Next(thanks.Length)];
            }

            string[] genericThanks = {
                "You're welcome! I'm glad I could help. Would you like to learn about another topic?",
                "Happy to help! Is there another cybersecurity topic you'd like to explore?",
                "Anytime! Feel free to ask if you have more cybersecurity questions."
            };
            return genericThanks[random.Next(genericThanks.Length)];
        }

        private string GetFinalGoodbye()
        {
            conversationEnded = true;
            expectedResponse = null;
            currentTopic = null;
            followUpCount = 0;
            lastQuestion = null;
            consecutiveNoCount = 0;

            string userName = GetUserName();
            if (!string.IsNullOrEmpty(userName))
            {
                return $"Alright {userName}, no problem! I'll be here if you want to learn about cybersecurity later. Stay safe online!";
            }
            return "Alright, no problem! I'll be here if you want to learn about cybersecurity later. Stay safe online!";
        }

        private string GetPostGoodbyeAcknowledgment()
        {
            string[] responses = {
                "Take care!",
                "Stay safe online!",
                "Have a great day!",
                "I'll be here when you're ready to learn more about cybersecurity.",
                "Remember to stay vigilant online!"
            };
            return responses[random.Next(responses.Length)];
        }

        private string GetQuestionResponse(string question)
        {
            string lowerQuestion = question.ToLower();

            if (lowerQuestion == "why" || lowerQuestion.Contains("why"))
            {
                string[] whyResponses = {
                    "Cybersecurity is important because it protects your personal information, financial data, and privacy from criminals who want to steal or harm.",
                    "Staying safe online matters because hackers and scammers are constantly looking for ways to exploit vulnerabilities.",
                    "A single security breach could lead to identity theft, financial loss, or unauthorized access to your accounts."
                };
                return whyResponses[random.Next(whyResponses.Length)] + " Would you like to learn more about a specific security topic?";
            }

            if (lowerQuestion.Contains("how"))
            {
                return "You can protect yourself by using strong passwords, enabling two-factor authentication, keeping software updated, and being cautious with suspicious emails. Would you like tips on a specific area?";
            }

            if (lowerQuestion.Contains("what"))
            {
                return GetTopicSuggestions();
            }

            return "That's a great question! Could you tell me more specifically what you'd like to know about cybersecurity?";
        }

        public string GetResponse(string userInput)
        {
            // Validate input for harmful patterns
            if (ContainsHarmfulPatterns(userInput))
            {
                consecutiveInvalidInputs++;
                if (consecutiveInvalidInputs >= MAX_CONSECUTIVE_INVALID)
                {
                    consecutiveInvalidInputs = 0;
                    return "I've noticed some concerning patterns in your messages. Let's keep our conversation focused on cybersecurity learning. What topic would you like to discuss?";
                }
                return "I can't process that request. Please ask me about cybersecurity topics like passwords, phishing, or online safety.";
            }

            // Sanitize input
            userInput = ValidateAndSanitizeInput(userInput);

            if (string.IsNullOrWhiteSpace(userInput))
            {
                consecutiveInvalidInputs++;
                if (consecutiveInvalidInputs >= MAX_CONSECUTIVE_INVALID)
                {
                    consecutiveInvalidInputs = 0;
                    return "I notice you're not typing any messages. Is there a cybersecurity topic I can help you with? Just let me know what you'd like to learn about.";
                }
                return "I didn't catch that. Could you please type a message about cybersecurity?";
            }

            // Reset consecutive invalid counter for valid input
            consecutiveInvalidInputs = 0;

            userInput = SanitizeInput(userInput);

            // If conversation ended but user asks a question, restart conversation
            if (conversationEnded)
            {
                if (IsQuestionWord(userInput) || IsGreeting(userInput) || IsHelpRequest(userInput) ||
                    DetectTopicFromSynonyms(userInput) != null || IsNameQuestion(userInput))
                {
                    conversationEnded = false;
                }
                else if (IsAcknowledgmentAfterGoodbye(userInput))
                {
                    string response = GetPostGoodbyeAcknowledgment();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }
                else if (!string.IsNullOrWhiteSpace(userInput))
                {
                    string response = GetPostGoodbyeAcknowledgment();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }
                else
                {
                    string response = GetPostGoodbyeAcknowledgment();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }
            }

            // Handle farewells 
            if (IsFarewell(userInput))
            {
                string response = GetFarewellResponse();
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            // Handle gratitude
            if (IsGratitude(userInput))
            {
                expectedResponse = null;
                consecutiveNoCount = 0;
                string response = GetGratitudeResponse();
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            // Handle conversation ending phrases
            if (IsConversationEnding(userInput))
            {
                string response = GetFinalGoodbye();
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            // Proirity Memory operations including favorite topic and name recall
            string memoryResponse = HandleMemoryOperations(userInput);
            if (memoryResponse != null)
            {
                CacheResponse(userInput.ToLower(), memoryResponse);
                memory.SetLastBotResponse(memoryResponse);
                return memoryResponse;
            }

            // Detect sentiment and topic
            string sentiment = DetectSentiment(userInput);
            string detectedTopic = DetectTopicFromSynonyms(userInput);

            // HANDLE RESPONSE TO "WHAT'S WRONG?" 
            if (expectedResponse == "asking_whats_wrong" && !string.IsNullOrEmpty(askingWhatsWrongFor))
            {
                string userLower = userInput.ToLower();
                string detectedIssueTopic = DetectTopicFromSynonyms(userInput);
                string originalEmotion = askingWhatsWrongFor;

                if (detectedIssueTopic != null)
                {
                    currentTopic = detectedIssueTopic;
                    followUpCount = 0;
                    expectedResponse = "another_tip";
                    askingWhatsWrongFor = null;

                    string tip = tips.GetRandomTip(detectedIssueTopic, 0);
                    string personalizedTip = memory.GetPersonalizedResponse(detectedIssueTopic, tip);
                    string empatheticPrefix = GetEmpatheticResponseForEmotion(originalEmotion, detectedIssueTopic);

                    string response = $"{empatheticPrefix}{personalizedTip}\n\nWould you like another tip?";
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (userLower.Contains("just") || userLower.Contains("everything") || userLower.Length < 10)
                {
                    expectedResponse = null;
                    askingWhatsWrongFor = null;
                    string response = "I understand. Would you like me to suggest some cybersecurity topics that might help you feel more secure?";
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                expectedResponse = null;
                askingWhatsWrongFor = null;
                string defaultResponse = "I want to help. What cybersecurity topic would you like to learn about? I can help with passwords, phishing, malware, WiFi security, VPNs, or 2FA.";
                CacheResponse(userInput.ToLower(), defaultResponse);
                memory.SetLastBotResponse(defaultResponse);
                return defaultResponse;
            }

            //  HANDLE ALL EMOTIONS WITHOUT TOPIC 
            if ((sentiment == "angry" || sentiment == "worried" || sentiment == "frustrated" ||
                 sentiment == "sad") && detectedTopic == null)
            {
                expectedResponse = "asking_whats_wrong";
                askingWhatsWrongFor = sentiment;
                string empatheticPrefix = GetEmpatheticResponseForEmotion(sentiment);
                string followUpQuestion = GetFollowUpQuestionForEmotion(sentiment);
                string response = empatheticPrefix + followUpQuestion;
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (sentiment == "happy" && detectedTopic == null)
            {
                expectedResponse = "learning_interest";
                string empatheticPrefix = GetEmpatheticResponseForEmotion("happy");
                string response = empatheticPrefix + "Would you like to learn about cybersecurity today?";
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (sentiment == "curious" && detectedTopic == null)
            {
                expectedResponse = null;
                string response = GetTopicSuggestions();
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (sentiment == "confident" && detectedTopic == null)
            {
                expectedResponse = null;
                string[] confidentResponses = {
                    "That's great! Would you like to learn about another cybersecurity topic?",
                    "Excellent! Feel free to ask me about any cybersecurity topic you're interested in.",
                    "Awesome! I'm here if you have any cybersecurity questions."
                };
                string response = confidentResponses[random.Next(confidentResponses.Length)];
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            // HANDLE ALL EMOTIONS WITH TOPIC 
            if ((sentiment == "angry" || sentiment == "worried" || sentiment == "frustrated" ||
                 sentiment == "sad" || sentiment == "happy" || sentiment == "curious" ||
                 sentiment == "confident") && detectedTopic != null)
            {
                currentTopic = detectedTopic;
                followUpCount = 0;
                conversationEnded = false;

                string tip = tips.GetRandomTip(detectedTopic, 0);
                string personalizedTip = memory.GetPersonalizedResponse(detectedTopic, tip);
                string empatheticPrefix = GetEmpatheticResponseForEmotion(sentiment, detectedTopic);

                if (sentiment == "curious" || sentiment == "confident" || sentiment == "happy")
                {
                    expectedResponse = "learn_more";
                    string response = $"{empatheticPrefix}{personalizedTip}\n\nWould you like to learn more about {detectedTopic}?";
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }
                else
                {
                    expectedResponse = "another_tip";
                    string response = $"{empatheticPrefix}{personalizedTip}\n\nWould you like another tip?";
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }
            }

            // Handle question words
            if (IsQuestionWord(userInput) && currentTopic == null && expectedResponse == null)
            {
                string response = GetQuestionResponse(userInput);
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (IsEmptyOrGibberish(userInput))
            {
                if (expectedResponse != null && !string.IsNullOrEmpty(lastQuestion))
                {
                    return lastQuestion;
                }
                return "Please ask me a cybersecurity question, or type 'help' to see what topics I can teach you about.";
            }

            consecutiveUnknownResponses = 0;

            if (IsDismissal(userInput))
            {
                expectedResponse = null;
                consecutiveNoCount = 0;
                string response = "Alright! Feel free to ask me about cybersecurity topics whenever you're ready. I'm here to help!";
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            string singleWordTopic = DetectTopicFromSynonyms(userInput);

            if (singleWordTopic != null && userInput.Split(' ').Length <= 3)
            {
                string response = GetResponseForTopic(singleWordTopic);
                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if ((userInput.ToLower().Contains("protect me") || userInput.ToLower().Contains("help me")) && detectedTopic != null)
            {
                currentTopic = detectedTopic;
                followUpCount = 0;
                consecutiveNoCount = 0;
                conversationEnded = false;

                string tip = tips.GetRandomTip(detectedTopic, 0);
                string personalizedTip = memory.GetPersonalizedResponse(detectedTopic, tip);

                expectedResponse = "another_tip";
                string response = $"I can definitely help you stay safe from {detectedTopic}.\n\n{personalizedTip}\n\nWould you like another tip?";

                CacheResponse(userInput.ToLower(), response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (IsGreeting(userInput))
            {
                expectedResponse = "learning_interest";
                consecutiveNoCount = 0;
                conversationEnded = false;
                string greeting = GetPersonalizedGreeting();
                lastQuestion = "Ready to learn more about cybersecurity?";
                greeting += $"\n\n{lastQuestion}";
                CacheResponse(userInput.ToLower(), greeting);
                memory.SetLastBotResponse(greeting);
                return greeting;
            }

            if (IsHelpRequest(userInput))
            {
                expectedResponse = null;
                consecutiveNoCount = 0;
                conversationEnded = false;
                string helpResponse = GetTopicSuggestions();
                CacheResponse(userInput.ToLower(), helpResponse);
                memory.SetLastBotResponse(helpResponse);
                return helpResponse;
            }

            if (IsAffirmativeResponse(userInput))
            {
                consecutiveNoCount = 0;
                conversationEnded = false;

                if (expectedResponse == "learn_more" && currentTopic != null)
                {
                    string response = ProvideTip(currentTopic, true);
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == "learning_interest")
                {
                    string response = GetTopicSuggestions();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == "another_tip" && currentTopic != null)
                {
                    string response = ProvideTip(currentTopic, true);
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == null)
                {
                    string response = GetTopicSuggestions();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }
            }

            // Handle "no" response
            if (IsNegativeResponse(userInput))
            {
                consecutiveNoCount++;
                conversationEnded = false;

                if (consecutiveNoCount >= 2)
                {
                    string response = GetFinalGoodbye();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == "learn_more" && currentTopic != null)
                {
                    expectedResponse = "topic_selection";
                    currentTopic = null;
                    string response = $"No problem! What other cybersecurity topic would you like to learn about?\n\n{GetTopicSuggestions()}";
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == "another_tip" && currentTopic != null)
                {
                    expectedResponse = "topic_selection";
                    currentTopic = null;
                    string response = $"Okay! Would you like to learn about a different cybersecurity topic?\n\n{GetTopicSuggestions()}";
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == "learning_interest")
                {
                    string response = GetFinalGoodbye();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                if (expectedResponse == "topic_selection")
                {
                    string response = GetFinalGoodbye();
                    CacheResponse(userInput.ToLower(), response);
                    memory.SetLastBotResponse(response);
                    return response;
                }

                expectedResponse = "topic_selection";
                string genericResponse = $"No problem! What cybersecurity topic would you like to learn about?\n\n{GetTopicSuggestions()}";
                CacheResponse(userInput.ToLower(), genericResponse);
                memory.SetLastBotResponse(genericResponse);
                return genericResponse;
            }

            if (!IsNegativeResponse(userInput))
            {
                consecutiveNoCount = 0;
            }

            memory.DetectInterest(userInput);
            string normalizedInput = userInput.ToLower().Trim();

            var sortedData = data.secureData.OrderByDescending(x => x.Value.keywords.Max(k => k.Length));
            string[] words = SplitWords(normalizedInput);

            foreach (var item in sortedData)
            {
                foreach (var keyword in item.Value.keywords)
                {
                    bool matched = false;

                    if (keyword.Contains(' '))
                    {
                        matched = normalizedInput.Contains(keyword);
                    }
                    else
                    {
                        matched = words.Contains(keyword) || IsWholeWord(normalizedInput, keyword);
                    }

                    if (matched)
                    {
                        currentTopic = item.Key;
                        followUpCount = 0;
                        consecutiveNoCount = 0;
                        conversationEnded = false;

                        string baseResponse = item.Value.response;
                        string tip = tips.GetRandomTip(currentTopic, 0);
                        string personalizedTip = memory.GetPersonalizedResponse(currentTopic, tip);

                        string finalResponse;

                        if (sentiment == "curious")
                        {
                            expectedResponse = null;
                            finalResponse = $"Great curiosity! Here's information about {currentTopic}:\n\n{baseResponse}\n\n{personalizedTip}\n\nWhat else would you like to know?";
                        }
                        else
                        {
                            expectedResponse = "learn_more";
                            finalResponse = $"{baseResponse}\n\nWould you like me to share some tips about {currentTopic}?";
                        }

                        CacheResponse(normalizedInput, finalResponse);
                        memory.SetLastBotResponse(finalResponse);
                        return finalResponse;
                    }
                }
            }

            if (IsUserConfused(normalizedInput) && currentTopic != null)
            {
                expectedResponse = null;
                consecutiveNoCount = 0;
                string response = GetSimplifiedExplanation(currentTopic);
                memory.SetLastBotResponse(response);
                CacheResponse(normalizedInput, response);
                return response;
            }

            if (WantsAnotherTip(normalizedInput) && currentTopic != null)
            {
                string response = ProvideTip(currentTopic, true);
                CacheResponse(normalizedInput, response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (expectedResponse != null && !string.IsNullOrEmpty(lastQuestion))
            {
                CacheResponse(normalizedInput, lastQuestion);
                memory.SetLastBotResponse(lastQuestion);
                return lastQuestion;
            }

            if (currentTopic == null || expectedResponse == "topic_selection")
            {
                string response = GetTopicSuggestions();
                CacheResponse(normalizedInput, response);
                memory.SetLastBotResponse(response);
                return response;
            }

            if (currentTopic != null)
            {
                string offerResponse = $"Would you like to learn more about {currentTopic} or try a different topic?";
                expectedResponse = "learn_more";
                lastQuestion = offerResponse;
                CacheResponse(normalizedInput, offerResponse);
                memory.SetLastBotResponse(offerResponse);
                return offerResponse;
            }

            if (!IsAffirmativeResponse(normalizedInput) && !IsNegativeResponse(normalizedInput))
            {
                foreach (var emotion in data.emotions)
                {
                    foreach (var keyword in emotion.Value.keywords)
                    {
                        if (IsWholeWord(normalizedInput, keyword))
                        {
                            expectedResponse = null;
                            consecutiveNoCount = 0;
                            string emotionResponse = emotion.Value.response;
                            if (!string.IsNullOrEmpty(GetUserName()))
                            {
                                emotionResponse = $"{GetUserName()}, " + emotionResponse.ToLower();
                            }
                            CacheResponse(normalizedInput, emotionResponse);
                            memory.SetLastBotResponse(emotionResponse);
                            return emotionResponse;
                        }
                    }
                }
            }

            string fallback = fallbackResponses[random.Next(fallbackResponses.Length)];
            CacheResponse(normalizedInput, fallback);
            memory.SetLastBotResponse(fallback);
            return fallback;
        }

        private string GetSimplifiedExplanation(string topic)
        {
            switch (topic)
            {
                case "password":
                    return "Let me explain simply: A strong password is like a good lock on your front door. Make it long (12+ characters), mix letters and numbers, and never reuse passwords. Simple enough?";
                case "phishing":
                    return "Think of phishing like a fake phone call from someone pretending to be your bank. Scammers send fake emails trying to trick you. Always check who's really asking! Make sense?";
                case "malware":
                    return "Malware is like a computer virus - bad software that can damage your device. Avoid downloading suspicious files. Does that help?";
                case "wifi":
                    return "WiFi security is about protecting your wireless network. Think of it like locking your front door - make sure only trusted people can use your internet.";
                case "vpn":
                    return "A VPN is like a secret tunnel for your internet traffic. It hides what you're doing online from others, especially on public WiFi.";
                case "2fa":
                    return "2FA is like having both a key AND a code to open a safe. Even if someone steals your password, they still need the code from your phone.";
                default:
                    return $"Let me explain {topic} in simpler terms. What specific part confuses you?";
            }
        }

        private void CacheResponse(string input, string response)
        {
            if (responseCache.Count < 100 && !responseCache.ContainsKey(input))
            {
                responseCache[input] = response;
            }
        }

        public void ResetCache()
        {
            responseCache.Clear();
        }

        public void ResetConversation()
        {
            currentTopic = null;
            followUpCount = 0;
            expectedResponse = null;
            lastQuestion = null;
            consecutiveUnknownResponses = 0;
            consecutiveNoCount = 0;
            conversationEnded = false;
            askingWhatsWrongFor = null;
            responseCache.Clear();
            consecutiveInvalidInputs = 0;
        }

        public Memory GetMemory()
        {
            return memory;
        }

        public void ClearMemory()
        {
            memory.ClearMemory();
            ResetConversation();
        }

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

    public class ChatResponse
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public List<string> FavoriteTopics { get; set; }
        public bool HasInterests { get; set; }
        public string SuggestedTopic { get; set; }
    }
}