using System;
using System.Collections.Generic;

namespace SecureIQ_Africa
{
    public class ResponseTips
    {
        private Dictionary<string, List<string>> tipsByTopic;
        private static readonly Random random = new Random();

        public ResponseTips()
        {
            InitializeTips();
        }

        private void InitializeTips()
        {
            tipsByTopic = new Dictionary<string, List<string>>
            {
                // Password Tips
                {
                    "password", new List<string>
                    {
                        "Use a password manager to generate and store unique, complex passwords for each account.",
                        "Create passphrases using 4-6 random words (e.g., 'PurpleTurtleDanceMountain') - they're strong AND memorable!",
                        "Never reuse passwords across different accounts - if one gets hacked, all are vulnerable.",
                        "Enable biometric login (fingerprint/face ID) where available for extra security.",
                        "Change default passwords on all devices immediately - hackers know the defaults!",
                        "A strong password should be at least 12 characters long with mixed case, numbers, and symbols.",
                        "Avoid using personal info like birthdays, pet names, or addresses in passwords.",
                        "Set up password recovery options (email/phone) in case you forget your passwords."
                    }
                },
                // Phishing Tips
                {
                    "phishing", new List<string>
                    {
                        "Never click links in suspicious emails - hover over them first to see the real URL.",
                        "Check the sender's email address carefully - scammers use addresses that look almost real.",
                        "Legitimate companies never ask for passwords or personal info via email.",
                        "Look for urgent language like 'act now' or 'your account will be closed' - that's a red flag!",
                        "When in doubt, go directly to the website by typing the URL yourself, not clicking the link.",
                        "Watch for spelling and grammar mistakes - many phishing emails come from non-native speakers.",
                        "Don't download attachments from unknown senders - they could contain malware.",
                        "Report phishing attempts to the company being impersonated and to your IT department."
                    }
                },
                // Malware Tips
                {
                    "malware", new List<string>
                    {
                        "Keep your antivirus software updated and run regular scans.",
                        "Don't download software from untrusted websites - use official sources only.",
                        "Be careful with email attachments, even from people you know - their account might be hacked.",
                        "Enable automatic updates for your operating system and all applications.",
                        "Use ad-blockers to avoid malicious ads that can install malware.",
                        "Back up your important files regularly to an external drive or cloud storage.",
                        "Be wary of pop-ups saying your computer is infected - they're often scams.",
                        "Consider using a standard user account instead of an administrator account for daily tasks."
                    }
                },
                // WiFi Tips
                {
                    "wifi", new List<string>
                    {
                        "Always use WPA2 or WPA3 encryption on your home Wi-Fi network - never use WEP or open networks.",
                        "Change your router's default admin password to something strong and unique.",
                        "Disable WPS (Wi-Fi Protected Setup) on your router as it has known security vulnerabilities.",
                        "Use a VPN when connecting to public Wi-Fi networks like in cafes or airports.",
                        "Hide your network SSID so it doesn't broadcast to everyone nearby.",
                        "Keep your router's firmware updated to protect against known exploits.",
                        "Create a guest network for visitors so they can't access your main devices.",
                        "Turn off Wi-Fi on your devices when not in use to avoid automatic connections to unsafe networks."
                    }
                },
                // VPN Tips
                {
                    "vpn", new List<string>
                    {
                        "Choose a VPN provider with a strict no-logs policy to protect your privacy.",
                        "Always connect to your VPN before accessing sensitive accounts on public Wi-Fi.",
                        "Look for VPNs that offer kill switch features - they block internet if VPN drops.",
                        "Free VPNs often sell your data - invest in a reputable paid service.",
                        "Use VPN protocols like WireGuard or OpenVPN for better security.",
                        "Connect to VPN servers in your own country for faster speeds when you don't need geo-spoofing.",
                        "Test your VPN for DNS and IP leaks using online leak test tools.",
                        "Remember that VPN protects your connection, not your device from malware."
                    }
                },
                // 2FA Tips
                {
                    "2fa", new List<string>
                    {
                        "Enable 2FA on all accounts that offer it - especially email, banking, and social media.",
                        "Use an authenticator app like Google Authenticator or Authy instead of SMS when possible.",
                        "Save your backup codes in a safe place - you'll need them if you lose your phone.",
                        "Consider using hardware security keys like YubiKey for the strongest 2FA protection.",
                        "Don't use security questions as your only 2FA method - they can often be guessed.",
                        "Set up multiple 2FA methods (app, backup codes, hardware key) in case one fails.",
                        "Be wary of 2FA phishing - never enter your code on a site you didn't navigate to yourself.",
                        "Review your active 2FA devices regularly and remove any you don't recognize."
                    }
                },
                // Privacy Tips
                {
                    "privacy", new List<string>
                    {
                        "Review privacy settings on social media apps at least once every few months.",
                        "Use browser extensions that block trackers and third-party cookies.",
                        "Be careful what personal information you share in public posts or profiles.",
                        "Use a search engine that doesn't track you, like DuckDuckGo, for private browsing.",
                        "Check which apps have access to your location, camera, and microphone on your phone.",
                        "Use encrypted messaging apps like Signal for sensitive conversations.",
                        "Regularly clear your browser history, cookies, and cache to remove tracking data.",
                        "Opt out of data collection where possible - look for privacy settings in each service."
                    }
                },
                // Backup Tips
                {
                    "backup", new List<string>
                    {
                        "Follow the 3-2-1 backup rule: 3 copies, 2 different media types, 1 off-site copy.",
                        "Automate your backups so you don't forget to do them manually.",
                        "Test your backups regularly by restoring a file to make sure they work.",
                        "Encrypt your backups, especially cloud backups, to protect sensitive data.",
                        "Keep a local backup on an external drive that isn't always connected to your computer.",
                        "Use versioning in your backup solution so you can recover older versions of files.",
                        "Don't forget to backup your phone photos and contacts - they're often irreplaceable.",
                        "Store critical documents (IDs, passwords) on encrypted USB drives in a safe place."
                    }
                }
            };
        }

        public string GetRandomTip(string topic, int offset = 0)
        {
            if (!tipsByTopic.ContainsKey(topic))
            {
                return GetGeneralTip();
            }

            var tips = tipsByTopic[topic];

            if (tips == null || tips.Count == 0)
            {
                return GetGeneralTip();
            }

            int index = (offset + random.Next(tips.Count)) % tips.Count;
            return tips[index];
        }

        private string GetGeneralTip()
        {
            string[] generalTips = new string[]
            {
                "Keep all your software and apps updated to protect against known vulnerabilities.",
                "Use unique passwords for every account to prevent credential stuffing attacks.",
                "Enable automatic updates on your devices for critical security patches.",
                "Think before you click - suspicious links are the #1 cause of security breaches.",
                "Regularly review your bank and credit card statements for unauthorized charges.",
                "Use a password manager to create and store strong, unique passwords.",
                "Enable two-factor authentication wherever possible for an extra layer of security.",
                "Back up your important data regularly to protect against ransomware and hardware failure.",
                "Be careful what you share on social media - scammers use this information.",
                "Lock your computer and phone when you step away from them, even at home."
            };

            return generalTips[random.Next(generalTips.Length)];
        }

        public string GetTipByIndex(string topic, int index)
        {
            if (!tipsByTopic.ContainsKey(topic))
                return GetGeneralTip();

            var tips = tipsByTopic[topic];

            if (tips == null || tips.Count == 0 || index >= tips.Count)
                return GetGeneralTip();

            return tips[index];
        }

        public List<string> GetAllTips(string topic)
        {
            if (!tipsByTopic.ContainsKey(topic))
                return new List<string>();

            return new List<string>(tipsByTopic[topic]);
        }

        public int GetTipCount(string topic)
        {
            if (!tipsByTopic.ContainsKey(topic))
                return 0;

            return tipsByTopic[topic].Count;
        }

        public bool HasTopic(string topic)
        {
            return tipsByTopic.ContainsKey(topic);
        }
    }
}