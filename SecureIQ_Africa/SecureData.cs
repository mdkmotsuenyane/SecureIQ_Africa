using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Console;

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
                "password is a string of characters that must be keyed to gain access to a computer, network, or service or to a phone or similar device." +
                   "\n Please use strong, unique passwords for every account. Combine uppercase, lowercase, numbers, and symbols.")
            },
            {
                "phishing",
                (new[] { "phishing", "scam", "fake email", "suspicious link" },
                "phishin is the fraudulent practice of sending emails or other messages purporting to be from reputable companies in order to induce individuals to reveal personal information, such as passwords and credit card numbers." +
                   "\nWatch out for emails or messages asking for personal info. When in doubt, don't click links.")
            },
            {
                "malware",
                (new[] { "malware", "virus", "trojan", "spyware" },
                "malware software that is specifically designed to disrupt, damage, or gain unauthorized access to a computer system." +
                   "\nKeep your antivirus updated and avoid downloading files from unknown sources.")
            },
            {
                "wifi",
                (new[] { "wifi", "public wifi", "network", "hotspot" },
                "\nPublic Wi-Fi can be risky. Avoid online banking or shopping, or use a VPN.")
            },
            {
                "vpn",
                (new[] { "vpn", "virtual private network" },
                "A VPN, which stands for virtual private network, establishes a digital connection between your computer and a remote server owned by a VPN provider, creating a point-to-point tunnel that encrypts your personal data, masks your IP address, and lets you sidestep website blocks and firewalls on the internet." +
                   "\nA VPN keeps your online activity private and protects your data on public networks.")
            },
            {
                "2fa",
                (new[] { "2fa", "two factor", "authentication" },
                "Two-factor authentication (2FA) is a way of verifying a user's identity by asking for exactly two pieces of proof" +
                   "\nEnable two-factor authentication for an extra layer of security on your accounts.")
            },
            {
                "socialengineering",
                (new[] { "social engineering", "trick", "manipulation" },
                "Social engineering is the use of deception to manipulate individuals into divulging confidential or personal information that may be used for fraudulent purposes." +
                   "\nNever share personal info with strangers or unsolicited contacts. Think before you click.")
            },
            {
                "safeBrowsing",
                (new[] { "safe browsing", "secure browsing", "website safety" },
                "Safe Browsing is a Google service that lets client applications check URLs against Google's constantly updated lists of unsafe web resources" +
                   "\nStick to trusted websites and check for HTTPS before entering any info.")
            },
            {
                "updates",
                (new[] { "update", "patch", "software update" },
                "Keep your apps and devices updated to protect against security threats.")
            },
            {
                "backup",
                (new[] { "backup", "data backup", "restore" },
                "Regularly backup your files so you don’t lose important data if something goes wrong.")
            },
            {
                "dataPrivacy",
                (new[] { "privacy", "personal data", "data protection" },
                "Be careful what you share online. Review privacy settings and think before posting.")
            },
            {
                "ransomware",
                (new[] { "ransomware", "encrypt", "file lock" },
                "Don’t open suspicious attachments. Keep backups and antivirus ready in case of attacks.")
            },
            {
                "iotSecurity",
                (new[] { "iot", "smart devices", "home devices", "security" },
                "Change default passwords on smart devices and update their firmware regularly.")
            },
            {
                "deepfake",
                (new[] { "deepfake", "fake video", "manipulated media" },
                "Verify videos or messages from unknown sources. Don’t trust everything you see online.")
            },
            {
                "shoppingOnline",
                (new[] { "shopping", "online shopping", "payment", "card" },
                "Use secure payment methods and trusted websites when shopping online. Avoid saving card info unnecessarily.")
            },
            {
                "emailsafety",
                (new[] { "email", "spam", "suspicious email" },
                "Check sender addresses carefully and don’t download attachments from unknown contacts.")
            },
            {
                "passwordmanager",
                (new[] { "password manager", "store passwords", "vault" },
                "Use a password manager to safely store and generate strong, unique passwords for all accounts.")
            }
       };
    }
}