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
                        "A strong password should be at least 12 characters long with mixed case, numbers, and symbols."
                    }
                },
                // Phishing Tips
                {
                    "phishing", new List<string>
                    {
                        "Never click links in suspicious emails - hover over them first to see the real URL.",
                        "Check the sender's email address carefully - scammers use addresses that look almost real.",
                        "Look for urgent language like 'act now' or 'your account will be closed' - that's a red flag!",
                        "When in doubt, go directly to the website by typing the URL yourself, not clicking the link."
                    }
                },
                // Malware Tips
                {
                    "malware", new List<string>
                    {
                        "Keep your antivirus software updated and run regular scans.",
                        "Don't download software from untrusted websites - use official sources only.",
                        "Be careful with email attachments, even from people you know - their account might be hacked.",
                        "Back up your important files regularly to protect against ransomware and malware."
                    }
                },
                // WiFi Tips
                {
                    "wifi", new List<string>
                    {
                        "Always use WPA2 or WPA3 encryption on your home Wi-Fi network - never use WEP or open networks.",
                        "Change your router's default admin password to something strong and unique.",
                        "Use a VPN when connecting to public Wi-Fi networks like in cafes or airports.",
                        "Turn off Wi-Fi on your devices when not in use to avoid automatic connections to unsafe networks."
                    }
                },
                // VPN Tips
                {
                    "vpn", new List<string>
                    {
                        "Choose a VPN provider with a strict no-logs policy to protect your privacy.",
                        "Always connect to your VPN before accessing sensitive accounts on public Wi-Fi.",
                        "Free VPNs often sell your data - invest in a reputable paid service.",
                        "Test your VPN for DNS and IP leaks using online leak test tools."
                    }
                },
                // 2FA Tips
                {
                    "2fa", new List<string>
                    {
                        "Enable 2FA on all accounts that offer it - especially email, banking, and social media.",
                        "Use an authenticator app like Google Authenticator or Authy instead of SMS when possible.",
                        "Save your backup codes in a safe place - you'll need them if you lose your phone.",
                        "Consider using hardware security keys like YubiKey for the strongest 2FA protection."
                    }
                },
                // Privacy/Data Privacy Tips
                {
                    "dataPrivacy", new List<string>
                    {
                        "Review privacy settings on social media apps at least once every few months.",
                        "Use browser extensions that block trackers and third-party cookies.",
                        "Check which apps have access to your location, camera, and microphone on your phone.",
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
                        "Encrypt your backups, especially cloud backups, to protect sensitive data."
                    }
                },
                // Social Engineering Tips
                {
                    "socialengineering", new List<string>
                    {
                        "Never share sensitive information over the phone unless you initiated the call.",
                        "Verify requests for money or data through a second communication channel.",
                        "Be suspicious of anyone asking for passwords, even if they claim to be IT support.",
                        "Take your time - scammers create false urgency to rush your decision making."
                    }
                },
                // Safe Browsing Tips
                {
                    "safeBrowsing", new List<string>
                    {
                        "Always check for HTTPS (padlock icon) before entering passwords or payment info.",
                        "Avoid clicking on pop-up ads - close them using task manager if needed.",
                        "Use different browsers for different activities (e.g., Firefox for banking, Chrome for browsing).",
                        "Clear your browsing data regularly including cache, cookies, and history."
                    }
                },
                // Updates & Patching Tips
                {
                    "updates", new List<string>
                    {
                        "Enable automatic updates for your operating system, browser, and critical apps.",
                        "Don't postpone security updates - they often fix known vulnerabilities being exploited.",
                        "Update firmware on routers, printers, and smart devices too - not just computers.",
                        "Restart your devices after updates to ensure patches are fully applied."
                    }
                },
                // Ransomware Tips
                {
                    "ransomware", new List<string>
                    {
                        "Never pay the ransom - it encourages criminals and doesn't guarantee data recovery.",
                        "Keep offline backups that aren't connected to your network to resist ransomware.",
                        "Disable macros in Office documents - ransomware often spreads via infected macros.",
                        "Use application whitelisting to prevent unauthorized programs from running."
                    }
                },
                // IoT Security Tips
                {
                    "iotSecurity", new List<string>
                    {
                        "Change default passwords on ALL smart devices before connecting them to Wi-Fi.",
                        "Create a separate guest network for IoT devices away from your main computers.",
                        "Turn off features you don't use (microphones, cameras, remote access).",
                        "Check for and install firmware updates monthly on all smart devices."
                    }
                },
                // Deepfake Tips
                {
                    "deepfake", new List<string>
                    {
                        "Verify suspicious video/audio calls through a trusted second channel (e.g., call back on known number).",
                        "Look for inconsistencies: unnatural blinking, mismatched audio, strange lighting.",
                        "Be skeptical of emotional or urgent messages even if they appear from someone you know.",
                        "Establish a family/corporate code word to verify identity in video calls."
                    }
                },
                // Online Shopping Tips
                {
                    "shoppingOnline", new List<string>
                    {
                        "Use virtual credit cards or payment services like PayPal for an extra layer of security.",
                        "Never save payment information on shopping sites unless absolutely necessary.",
                        "Verify website legitimacy by checking contact info, return policies, and trust seals.",
                        "Monitor your credit card statements weekly for unauthorized charges."
                    }
                },
                // Email Safety Tips
                {
                    "emailsafety", new List<string>
                    {
                        "Enable two-factor authentication on your email account - it's your digital identity hub.",
                        "Check the 'Reply-To' address - it may differ from the 'From' address in phishing emails.",
                        "Never click unsubscribe links in suspicious emails - it confirms your address is valid.",
                        "Verify unexpected attachments by calling the sender on a known number before opening."
                    }
                },
                // Password Manager Tips
                {
                    "passwordmanager", new List<string>
                    {
                        "Choose a password manager with end-to-end encryption and zero-knowledge architecture.",
                        "Use a strong, memorable master password for your password manager - you'll need to remember only this one!",
                        "Enable 2FA for your password manager account itself.",
                        "Regularly export encrypted backups of your password vault."
                    }
                },
                // Firewall Tips
                {
                    "firewall", new List<string>
                    {
                        "Never disable your firewall for convenience - it's your first line of defense.",
                        "Configure your firewall to block all incoming connections by default.",
                        "Use application-aware firewalls to control which programs can access the internet.",
                        "Review firewall logs monthly to spot unusual connection attempts."
                    }
                },
                // Encryption Tips
                {
                    "encryption", new List<string>
                    {
                        "Encrypt your entire hard drive using BitLocker (Windows), FileVault (Mac), or LUKS (Linux).",
                        "Use end-to-end encrypted messaging apps like Signal, WhatsApp, or Telegram secret chats.",
                        "Encrypt sensitive files before uploading to cloud storage - use tools like VeraCrypt.",
                        "Look for the padlock icon in your browser - it means your connection is encrypted."
                    }
                },
                // Antivirus Tips
                {
                    "antivirus", new List<string>
                    {
                        "Run full system scans weekly, not just quick scans.",
                        "Keep antivirus definitions updated automatically - often daily.",
                        "Enable real-time protection to catch threats before they execute.",
                        "Test your antivirus with the EICAR test file to ensure it's working."
                    }
                },
                // IDS Tips
                {
                    "ids", new List<string>
                    {
                        "Place IDS sensors at key network junctions to monitor traffic effectively.",
                        "Tune your IDS to reduce false alarms - customize rules for your environment.",
                        "Review IDS alerts daily - unmonitored detection is useless.",
                        "Use both signature-based and anomaly-based detection for comprehensive coverage."
                    }
                },
                // IPS Tips
                {
                    "ips", new List<string>
                    {
                        "Configure IPS in 'block' mode once you've tuned out false positives.",
                        "Keep IPS signature databases updated automatically.",
                        "Log all IPS actions for compliance and forensic analysis.",
                        "Test IPS rules in monitor mode first before enabling blocking."
                    }
                },
                // Zero Trust Tips
                {
                    "zero trust", new List<string>
                    {
                        "Implement micro-segmentation - don't trust any network segment by default.",
                        "Verify every access request, regardless of source (internal or external).",
                        "Use least privilege access - users get only the permissions they need.",
                        "Use multi-factor authentication for EVERY access request, not just logins."
                    }
                },
                // Social Media Safety Tips
                {
                    "socialmedia", new List<string>
                    {
                        "Set all social media profiles to private by default.",
                        "Never post vacation plans in real-time - wait until you return home.",
                        "Remove geotags from photos before posting - they reveal your location.",
                        "Don't share your birthday, phone number, or address on social media."
                    }
                },
                // Strong Authentication Tips
                {
                    "strongauthentication", new List<string>
                    {
                        "Use biometric authentication (fingerprint, face ID) with a strong PIN backup.",
                        "Implement risk-based authentication that challenges unusual access attempts.",
                        "Use FIDO2/WebAuthn security keys for phishing-resistant authentication.",
                        "Avoid SMS-based 2FA when possible - SIM swapping is a real threat."
                    }
                },
                // Data Breach Tips
                {
                    "dataBreach", new List<string>
                    {
                        "Monitor your accounts using breach notification services like HaveIBeenPwned.",
                        "Change passwords immediately when you learn of a breach affecting you.",
                        "Freeze your credit if your financial data was exposed in a breach.",
                        "Monitor your bank statements closely for 6+ months after a financial breach."
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