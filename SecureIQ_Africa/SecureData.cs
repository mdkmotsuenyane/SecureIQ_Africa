using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureIQ_Africa
{
    internal class SecureData
    {
        public Dictionary<string, (string[] keywords, string response)> secureData =
           new Dictionary<string, (string[], string)>()
        {
            {
                "greeting",
                (new[] { "hello", "hi", "hey" },
                "Hi there! I'm your cybersecurity awareness bot, here to help you stay safe online.")
            },
            {
                "howareyou",
                (new[] { "how are you", "how's it going", "how do you feel" },
                "I'm just a bot, but I'm always ready to help you learn about online safety!")
            },
            {
                "whatIsYourName",
                (new[] { "what is your name", "your name", "who are you" },
                "SecureIQ Africa chatbot")
            },
            {
                "whatCanYouDo",
                (new[] { "what can you do", "what do you do", "capabilities" },
                "i can answer cyber security related questions")
            },
            {
                "purpose",
                (new[] { "purpose", "what is your purpose", "why are you here" },
                "My purpose is to educate and guide you on cybersecurity best practices.")
            },
            {
                "help",
                (new[] { "help", "what can i ask", "what do you know" },
                "You can ask me about passwords, phishing, malware, safe browsing, privacy, and more.")
            },
            {
                "password",
                (new[] { "password", "pass", "login", "credential" },
                "password is a string of characters that must be keyed to gain access to a computer, network, or service or to a phone or similar device.\nPlease use strong, unique passwords for every account. Combine uppercase, lowercase, numbers, and symbols.")
            },
            {
                "phishing",
                (new[] { "phishing", "scam", "fake email", "suspicious link" },
                "Phishing is the fraudulent practice of sending emails or other messages purporting to be from reputable companies in order to induce individuals to reveal personal information, such as passwords and credit card numbers.\nWatch out for emails or messages asking for personal info. When in doubt, don't click links.")
            },
            {
                "malware",
                (new[] { "malware", "virus", "trojan", "spyware" },
                "malware software that is specifically designed to disrupt, damage, or gain unauthorized access to a computer system.\nKeep your antivirus updated and avoid downloading files from unknown sources.")
            },
            {
                "wifi",
                (new[] { "wifi", "public wifi", "network", "hotspot" },
                "Wifi is a facility allowing computers, smartphones, or other devices to connect to the internet or communicate with one another wirelessly within a particular area.\nPublic Wi-Fi can be risky. Avoid online banking or shopping, or use a VPN.")
            },
            {
                "vpn",
                (new[] { "vpn", "virtual private network" },
                "A VPN, which stands for virtual private network, establishes a digital connection between your computer and a remote server owned by a VPN provider, creating a point-to-point tunnel that encrypts your personal data, masks your IP address, and lets you sidestep website blocks and firewalls on the internet.\nA VPN keeps your online activity private and protects your data on public networks.")
            },
            {
                "2fa",
                (new[] { "2fa", "two factor", "authentication" },
                "Two-factor authentication (2FA) is a way of verifying a user's identity by asking for exactly two pieces of proof\nEnable two-factor authentication for an extra layer of security on your accounts.")
            },
            {
                "socialengineering",
                (new[] { "social engineering", "trick", "manipulation" },
                "Social engineering is the use of deception to manipulate individuals into divulging confidential or personal information that may be used for fraudulent purposes.\nNever share personal info with strangers or unsolicited contacts. Think before you click.")
            },
            {
                "safeBrowsing",
                (new[] { "safe browsing", "secure browsing", "website safety" },
                "Safe Browsing is a Google service that lets client applications check URLs against Google's constantly updated lists of unsafe web resources\nStick to trusted websites and check for HTTPS before entering any info.")
            },
            {
                "updates",
                (new[] { "update", "patch", "software update" },
                "update refers to a modification or improvement to software or hardware that addresses known issues, enhances functionality, or improves security\nKeep your apps and devices updated to protect against security threats.")
            },
            {
                "backup",
                (new[] { "backup", "data backup", "restore" },
                "a copy of a file or other item of data made in case the original is lost or damaged is Called backups\nRegularly backup your files so you don't lose important data if something goes wrong.")
            },
            {
                "dataPrivacy",
                (new[] { "privacy", "personal data", "data protection" },
                "Be careful what you share online. Review privacy settings and think before posting.")
            },
            {
                "ransomware",
                (new[] { "ransomware", "encrypt", "file lock" },
                "Ransomware is a type of malicious software (malware) that encrypts a victim's data or locks them out of their devices, demanding payment—typically in cryptocurrency—to restore access.\nDon't open suspicious attachments. Keep backups and antivirus ready in case of attacks.")
            },
            {
                "iotSecurity",
                (new[] { "iot", "smart devices", "home devices", "security" },
                "IoT security is the specialized subset of cybersecurity focused on protecting internet-connected devices—such as sensors, cameras, and smart appliances—and their networks from cyberattacks\nChange default passwords on smart devices and update their firmware regularly.")
            },
            {
                "deepfake",
                (new[] { "deepfake", "fake video", "manipulated media" },
                "A deepfake is a type of synthetic media where artificial intelligence (AI), particularly deep learning techniques, is used to manipulate or generate audio, video, or images to make them appear real. The term comes from combining deep learning and fake.\nVerify videos or messages from unknown sources. Don't trust everything you see online.")
            },
            {
                "shoppingOnline",
                (new[] { "shopping", "online shopping", "payment", "card" },
                "Shopping Online is the act of purchasing goods or services directly from a seller over the internet using a website, app, or digital platform\nUse secure payment methods and trusted websites when shopping online. Avoid saving card info unnecessarily.")
            },
            {
                "emailsafety",
                (new[] { "email", "spam", "suspicious email" },
                "Email security (or email safety) is the practice of protecting email accounts, communications, and data from unauthorized access, loss, or compromise. It involves a combination of technologies, policies, and user behaviors designed to defend against cyberthreats such as phishing, malware, ransomware, and spam.\nCheck sender addresses carefully and don't download attachments from unknown contacts.")
            },
            {
                "passwordmanager",
                (new[] { "password manager", "store passwords", "vault" },
                "Use a password manager to safely store and generate strong, unique passwords for all accounts.")
            },
            {
                "firewall",
                (new[] { "firewall", "network protection", "block traffic", "security wall" },
                "A firewall is a network security system that monitors and controls incoming and outgoing network traffic based on security rules.\nIt acts as a barrier between your trusted internal network and untrusted external networks like the internet.")
            },
            {
                "encryption",
                (new[] { "encryption", "encrypt data", "secure data", "cipher" },
                "Encryption is the process of converting information into a code to prevent unauthorized access.\nIt ensures that only authorized users with the correct key can read the data.")
            },
            {
                "antivirus",
                (new[] { "antivirus", "anti-virus", "scan virus", "security software" },
                "Antivirus software is a program designed to detect, prevent, and remove malware from computers and devices.\nKeep antivirus software updated to protect against new threats.")
            },
            {
                "ids",
                (new[] { "intrusion detection", "ids", "network monitoring", "attack detection" },
                "An Intrusion Detection System (IDS) monitors network or system activity for malicious actions or policy violations.\nIt alerts administrators when suspicious activity is detected.")
            },
            {
                "ips",
                (new[] { "intrusion prevention", "ips", "block attacks", "prevent intrusion" },
                "An Intrusion Prevention System (IPS) detects and actively blocks potential security threats in real time.\nIt goes beyond IDS by automatically preventing attacks.")
            },
            {
                "zero trust",
                (new[] { "zero trust", "never trust", "verify always", "security model" },
                "Zero Trust is a cybersecurity model that assumes no user or device is trusted by default, even inside a network.\nEvery access request must be verified before being granted.")
            },
            {
                "socialmedia",
                (new[] { "social media safety", "facebook safety", "instagram security", "tiktok privacy" },
                "Social media safety involves protecting your personal information and being cautious about what you share online.\nUse privacy settings and avoid sharing sensitive personal details publicly.")
            },
            {
                "strongauthentication",
                (new[] { "authentication", "login security", "secure login", "biometric login" },
                "Strong authentication verifies a user's identity using secure methods like biometrics, OTPs, or authentication apps.\nIt reduces the risk of unauthorized account access.")
            },
            {
                "dataBreach",
                (new[] { "data breach", "leak data", "hacked database", "stolen information" },
                "A data breach occurs when unauthorized individuals access confidential data.\nAlways change passwords immediately if a breach is suspected.")
            }
        };

        public Dictionary<string, (string[] keywords, string response)> emotions =
           new Dictionary<string, (string[], string)>()
        {
            { "happy",
                (new[] { "good", "great", "awesome", "wonderful", "fantastic", "happy", "excited" },
                "I'm glad you're feeling positive! Remember to keep that energy while staying cyber-safe!") },
            { "sad",
                (new[] { "bad", "terrible", "awful", "sad", "depressed", "upset", "unhappy" },
                "I'm sorry you're feeling down. Let me help protect your online world - that might cheer you up!") },
            { "worried",
                (new[] { "worried", "anxious", "nervous", "scared", "fear", "unsafe" },
                "Feeling concerned about security? That's actually good awareness! Let me share some safety tips.") },
            { "curious",
                (new[] { "how", "what", "why", "learn", "teach", "explain", "understand", "curious" },
                "Great curiosity! Cybersecurity is fascinating. What would you like to learn about?") },
            { "angry",
                (new[] { "angry", "frustrated", "hate", "annoying", "stupid" },
                "I understand your frustration. Security can be annoying, but let's work through it calmly.") },
            { "acknowledgment",
                (new[] { "okay", "ok", "alright", "fine", "got it" },
                "Great! I'm glad that makes sense. Would you like to learn more about cybersecurity topics?") },
            { "agreement",
                (new[] { "yes", "yeah", "yep", "sure", "absolutely", "definitely", "correct" },
                "Excellent! I'm happy to help. What specific cybersecurity topic would you like to explore further?") },
            { "negative",
                (new[] { "no", "not", "don't", "doesn't", "isn't", "aren't", "never" },
                "I understand. Is there a specific cybersecurity topic you'd like to learn about instead? I'm here to help!") },
            { "dismissal",
                (new[] { "ignore", "nevermind", "forget it", "never mind", "skip", "dismiss" },
                "No problem! Let me know if you have any cybersecurity questions. I'm here whenever you're ready to learn.") }
        };
    }
}