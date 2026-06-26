using System;
using System.Collections.Generic;

namespace SecureIQ_Africa
{
    public class QuizQuestion
    {
        // Correct answer 
        public string Topic { get; set; }
        // Hints 
        public string[] Keywords { get; set; }
        // Natural‑language question 
        public string QuestionText { get; set; }   
    }

    internal class QuizData
    {
        public List<QuizQuestion> Questions { get; private set; }

        public QuizData()
        {
            Questions = new List<QuizQuestion>
            {
                //  26 questions
                new QuizQuestion
                {
                    Topic = "Password",
                    Keywords = new[] { "password", "pass", "login", "credential" },
                    QuestionText = "What is a string of characters used to gain access to a computer or service?"
                },
                new QuizQuestion
                {
                    Topic = "Phishing",
                    Keywords = new[] { "phishing", "scam", "fake email", "suspicious link" },
                    QuestionText = "What is the fraudulent practice of sending deceptive emails to steal personal information?"
                },
                new QuizQuestion
                {
                    Topic = "Malware",
                    Keywords = new[] { "malware", "virus", "trojan", "spyware" },
                    QuestionText = "What is software designed to damage or gain unauthorized access to a system?"
                },
                new QuizQuestion
                {
                    Topic = "WIFI",
                    Keywords = new[] { "wifi", "public wifi", "network", "hotspot" },
                    QuestionText = "What is a wireless networking technology that allows devices to connect to the internet?"
                },
                new QuizQuestion
                {
                    Topic = "VPN",
                    Keywords = new[] { "vpn", "virtual private network" },
                    QuestionText = "What is a service that encrypts your internet connection and hides your IP address?"
                },
                new QuizQuestion
                {
                    Topic = "2FA",
                    Keywords = new[] { "2fa", "two factor", "authentication" },
                    QuestionText = "What is a security method that requires two forms of verification?"
                },
                new QuizQuestion
                {
                    Topic = "Social Engineering",
                    Keywords = new[] { "social engineering", "trick", "manipulation" },
                    QuestionText = "What is the manipulation of people to divulge confidential information?"
                },
                new QuizQuestion
                {
                    Topic = "Safe Browsing",
                    Keywords = new[] { "safe browsing", "secure browsing", "website safety" },
                    QuestionText = "What is the practice of checking website safety before entering information?"
                },
                new QuizQuestion
                {
                    Topic = "Updates",
                    Keywords = new[] { "update", "patch", "software update" },
                    QuestionText = "What are modifications to software that fix issues and improve security?"
                },
                new QuizQuestion
                {
                    Topic = "Backup",
                    Keywords = new[] { "backup", "data backup", "restore" },
                    QuestionText = "What is a copy of data made to prevent loss?"
                },
                new QuizQuestion
                {
                    Topic = "Data Privacy",
                    Keywords = new[] { "privacy", "personal data", "data protection" },
                    QuestionText = "What is the practice of protecting personal information from misuse?"
                },
                new QuizQuestion
                {
                    Topic = "Ransomware",
                    Keywords = new[] { "ransomware", "encrypt", "file lock" },
                    QuestionText = "What is malicious software that encrypts files and demands payment?"
                },
                new QuizQuestion
                {
                    Topic = "IOT Security",
                    Keywords = new[] { "iot", "smart devices", "home devices", "security" },
                    QuestionText = "What is the protection of internet‑connected smart devices?"
                },
                new QuizQuestion
                {
                    Topic = "Deepfake",
                    Keywords = new[] { "deepfake", "fake video", "manipulated media" },
                    QuestionText = "What is AI‑generated synthetic media that manipulates audio or video?"
                },
                new QuizQuestion
                {
                    Topic = "Online Shopping",
                    Keywords = new[] { "shopping", "online shopping", "payment", "card" },
                    QuestionText = "What is the act of purchasing goods over the internet?"
                },
                new QuizQuestion
                {
                    Topic = "Email Safety",
                    Keywords = new[] { "email", "spam", "suspicious email" },
                    QuestionText = "What is the protection of email accounts from threats?"
                },
                new QuizQuestion
                {
                    Topic = "Password Manager",
                    Keywords = new[] { "password manager", "store passwords", "vault" },
                    QuestionText = "What is a tool that stores and generates strong passwords?"
                },
                new QuizQuestion
                {
                    Topic = "Firewall",
                    Keywords = new[] { "firewall", "network protection", "block traffic", "security wall" },
                    QuestionText = "What is a network security system that monitors and controls traffic?"
                },
                new QuizQuestion
                {
                    Topic = "Encryption",
                    Keywords = new[] { "encryption", "encrypt data", "secure data", "cipher" },
                    QuestionText = "What is the process of converting data into a code to prevent unauthorized access?"
                },
                new QuizQuestion
                {
                    Topic = "Antivirus",
                    Keywords = new[] { "antivirus", "anti-virus", "scan virus", "security software" },
                    QuestionText = "What is software designed to detect and remove malware?"
                },
                new QuizQuestion
                {
                    Topic = "IDS",
                    Keywords = new[] { "intrusion detection", "ids", "network monitoring", "attack detection" },
                    QuestionText = "What is a system that monitors network for suspicious activity?"
                },
                new QuizQuestion
                {
                    Topic = "IPS",
                    Keywords = new[] { "intrusion prevention", "ips", "block attacks", "prevent intrusion" },
                    QuestionText = "What is a system that actively blocks security threats?"
                },
                new QuizQuestion
                {
                    Topic = "Zero Trust",
                    Keywords = new[] { "zero trust", "never trust", "verify always", "security model" },
                    QuestionText = "What is a security model that assumes no user or device is trusted by default?"
                },
                new QuizQuestion
                {
                    Topic = "Social Media Safety",
                    Keywords = new[] { "social media safety", "facebook safety", "instagram security", "tiktok privacy" },
                    QuestionText = "What is the practice of protecting personal information on social platforms?"
                },
                new QuizQuestion
                {
                    Topic = "Strong Authentication",
                    Keywords = new[] { "authentication", "login security", "secure login", "biometric login" },
                    QuestionText = "What is a secure login method using biometrics or OTPs?"
                },
                new QuizQuestion
                {
                    Topic = "Data Breach",
                    Keywords = new[] { "data breach", "leak data", "hacked database", "stolen information" },
                    QuestionText = "What is an incident where unauthorized individuals access confidential data?"
                },
                new QuizQuestion
                {
                    Topic = "Spoofing",
                    Keywords = new[] { "spoofing", "impersonation", "fake identity" },
                    QuestionText = "What is an attack where a person or program pretends to be someone else to gain unauthorized access?"
                },
                new QuizQuestion
                {
                    Topic = "Malvertising",
                    Keywords = new[] { "malvertising", "malicious ads", "infected ads" },
                    QuestionText = "What is the use of online advertisements to spread malware?"
                },
                new QuizQuestion
                {
                    Topic = "Botnet",
                    Keywords = new[] { "botnet", "network of bots", "zombie computers" },
                    QuestionText = "What is a network of infected computers controlled remotely for malicious purposes?"
                },
                new QuizQuestion
                {
                    Topic = "Zero-Day",
                    Keywords = new[] { "zero-day", "unknown vulnerability", "unpatched flaw" },
                    QuestionText = "What is a security vulnerability that is unknown to the vendor and has no patch?"
                },
                new QuizQuestion
                {
                    Topic = "Insider threat",
                    Keywords = new[] { "insider threat", "internal risk", "employee threat" },
                    QuestionText = "What is a security risk originating from within an organization, often from employees?"
                }
            };
        }
    }
}