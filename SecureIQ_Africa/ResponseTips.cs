using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureIQ_Africa
{
    public class ResponseTips
    {
        private static readonly Random random = new Random();

        private Dictionary<string, List<List<string>>> topicTips;

        public ResponseTips()
        {
            InitializeTips();
        }

        private void InitializeTips()
        {
            topicTips = new Dictionary<string, List<List<string>>>()
            {
                {
                    "password", new List<List<string>>
                    {
                        new List<string>
                        {
                            "Tip 1: Use a passphrase instead of a password - combine 4-6 random words like 'Correct-Horse-Battery-Staple'!",
                            "Here's a great tip: Create passphrases using random words - they're easier to remember and harder to crack than complex passwords!",
                            "Pro tip: A passphrase like 'Coffee-Rainbow-Elephant-Pizza' is both strong and memorable. Avoid common phrases though!",
                            "Password tip: Think of a sentence like 'My first car was a red Toyota!' and use MfCw@rT! - strong and personal!"
                        },
                        new List<string>
                        {
                            "Tip 2: Enable password expiration reminders every 90 days to regularly update your credentials.",
                            "Security reminder: Change your passwords every 3 months - set calendar reminders to stay on top of this!",
                            "Pro tip: Don't just rotate the same password - create entirely new ones every 90 days!",
                            "Important: Password aging is crucial - mark your calendar for regular password updates!"
                        },
                        new List<string>
                        {
                            "Tip 3: Never use personal information like birthdays or pet names in your passwords - hackers can easily find this!",
                            "Avoid using: Birthdays, anniversaries, pet names, or favorite sports teams in your passwords!",
                            "Hackers check social media for personal info - keep your passwords unrelated to your public life!",
                            "Your mother's maiden name? Too easy to find. Use random words instead!"
                        },
                        new List<string>
                        {
                            "Tip 4: Use different passwords for every account - a password manager can help with this!",
                            "One password to rule them all? No! Each account needs its own unique key to your digital kingdom!",
                            "Password managers generate and store unique passwords - you only need to remember one master password!",
                            "Never reuse passwords - if one account gets hacked, all your accounts become vulnerable!"
                        },
                        new List<string>
                        {
                            "Tip 5: Enable 2FA whenever possible - it adds an extra layer of security even if your password is compromised!",
                            "Supercharge your security: Password + 2FA = Fort Knox level protection!",
                            "Get a text or use an authenticator app - that second step blocks 99.9% of account attacks!",
                            "Even if hackers steal your password, they can't get in without that second factor - enable 2FA today!"
                        }
                    }
                },
                {
                    "phishing", new List<List<string>>
                    {
                        new List<string>
                        {
                            "Tip 1: Always hover over links before clicking to see the actual URL destination!",
                            "Pro tip: Hover, don't hurry! Check where links really go before clicking anything suspicious!",
                            "Mouse-over magic: Let your cursor reveal the true destination of any link before you click!",
                            "That link might say 'paypal.com' but hover shows 'paypa1.com' - always check first!"
                        },
                        new List<string>
                        {
                            "Tip 2: Check the sender's email address carefully - scammers use addresses that look similar to real ones!",
                            "Don't trust the display name - check the actual email address behind it!",
                            "Scammers use @paypa1.com instead of @paypal.com - look for these tricks!",
                            "One character difference can fool you - examine sender addresses with eagle eyes!"
                        },
                        new List<string>
                        {
                            "Tip 3: Look for poor grammar, spelling mistakes, and urgent language demanding immediate action!",
                            "'Your account will be CLOSED!' - urgency and fear are phishing red flags!",
                            "Spelling errors and weird capitalization = almost always a scam!",
                            "Real companies don't threaten you or demand immediate action via email!"
                        },
                        new List<string>
                        {
                            "Tip 4: Never share personal info via email - legitimate companies won't ask for passwords or credit cards this way!",
                            "Your bank will NEVER email asking for your password or PIN - that's always a scam!",
                            "When in doubt, call the company directly using their official number - not the one in the email!",
                            "No real company needs your password - they already have it (hashed and secured)!"
                        },
                        new List<string>
                        {
                            "Tip 5: When in doubt, contact the company directly using their official website or phone number!",
                            "Don't use links in suspicious emails - open a new browser window and type the real website address!",
                            "Got a suspicious text from your 'bank'? Call them using the number on your card, not the text!",
                            "Verify independently - always use official channels, never contact info provided in suspicious messages!"
                        }
                    }
                },
                {
                    "malware", new List<List<string>>
                    {
                        new List<string>
                        {
                            "Tip 1: Keep your operating system and all software updated with the latest security patches!",
                            "Update now, hack later! Security patches fix holes that malware exploits!",
                            "Those annoying update notifications? They're protecting you from known vulnerabilities!",
                            "Enable automatic updates - your future self will thank you when you avoid malware!"
                        },
                        new List<string>
                        {
                            "Tip 2: Only download software from official websites or trusted app stores!",
                            "Stick to official stores - pirated software is a Trojan horse for malware!",
                            "'Free' cracks and keygens almost always contain hidden malware!",
                            "Verify publisher signatures - only download from sources you trust completely!"
                        },
                        new List<string>
                        {
                            "Tip 3: Be cautious of email attachments, even from people you know - their account might be compromised!",
                            "Trust no one! Friends send malware when hacked - verify unexpected attachments!",
                            "'Is this you in the video?' links are classic malware delivery - don't click!",
                            "Call or text the sender directly to verify - one minute of verification saves hours of cleanup!"
                        },
                        new List<string>
                        {
                            "Tip 4: Use a reputable antivirus program and keep it updated regularly!",
                            "Think of antivirus as a vaccine for your computer - regular updates keep it effective!",
                            "Free antivirus is better than none, but paid versions offer more comprehensive protection!",
                            "Real-time protection + weekly full scans = best defense against malware!"
                        },
                        new List<string>
                        {
                            "Tip 5: Backup your important files to an external drive or cloud storage regularly!",
                            "3-2-1 backup rule: 3 copies, 2 different media, 1 off-site backup!",
                            "Ransomware can't hold your files hostage if you have backups!",
                            "Automate your backups - memory fails, automated systems don't!"
                        }
                    }
                },
                {
                    "vpn", new List<List<string>>
                    {
                        new List<string>
                        {
                            "Tip 1: Always use a VPN on public Wi-Fi networks like coffee shops or airports!",
                            "Public WiFi + No VPN = Strangers can see everything you do!",
                            "Airport Wi-Fi is convenient but dangerous - VPN makes it safe!",
                            "That free coffee shop WiFi? Use a VPN or save your browsing for home!"
                        },
                        new List<string>
                        {
                            "Tip 2: Choose a VPN provider that has a strict no-logs policy!",
                            "If a VPN keeps logs, they're not protecting your privacy - they're collecting it!",
                            "No-logs means the VPN can't sell your data even if subpoenaed!",
                            "Read the privacy policy - 'we collect minimal data' is NOT no-logs!"
                        },
                        new List<string>
                        {
                            "Tip 3: Look for VPNs with kill switch feature - it blocks internet if VPN drops!",
                            "Kill switch saves you: If VPN disconnects, your internet stops - no data leaks!",
                            "Without kill switch, you're exposed during VPN reconnections - always enable it!",
                            "Kill switch = Guaranteed protection, even when connections fail!"
                        },
                        new List<string>
                        {
                            "Tip 4: Free VPNs often sell your data - invest in a reputable paid VPN service!",
                            "If you're not paying for the product, YOU are the product - free VPNs sell your data!",
                            "Paid VPNs have business incentives to protect you - free ones need to make money somehow!",
                            "'Free' VPNs often have slower speeds, data caps, and privacy concerns - you get what you pay for!"
                        },
                        new List<string>
                        {
                            "Tip 5: Use VPN protocols like WireGuard or OpenVPN for better security!",
                            "WireGuard is faster and more secure than old protocols like PPTP!",
                            "OpenVPN is the gold standard - WireGuard is the new champion!",
                            "Avoid PPTP and L2TP - they're outdated and vulnerable. Use OpenVPN or WireGuard!"
                        }
                    }
                },
                {
                    "2fa", new List<List<string>>
                    {
                        new List<string>
                        {
                            "Tip 1: Use authenticator apps (Google Authenticator, Authy) instead of SMS when possible!",
                            "SMS can be intercepted - authenticator apps are much more secure!",
                            "Authy and Google Authenticator work without cell signal - perfect for travel!",
                            "App-based 2FA > SMS 2FA > No 2FA - upgrade your security today!"
                        },
                        new List<string>
                        {
                            "Tip 2: Save your backup codes in a secure place - you'll need them if you lose your phone!",
                            "Those backup codes are your lifeline - print them and store safely!",
                            "Lost your phone? Those backup codes save your accounts - don't ignore them!",
                            "Password managers can store backup codes - keep them accessible but secure!"
                        },
                        new List<string>
                        {
                            "Tip 3: Enable 2FA on your email account first - it's the gateway to your other accounts!",
                            "Your email is the master key - secure it with 2FA before everything else!",
                            "Password reset links go to your email - protect this account the most!",
                            "Email is your digital identity - 2FA here protects ALL your other accounts!"
                        },
                        new List<string>
                        {
                            "Tip 4: Consider hardware security keys like YubiKey for maximum protection!",
                            "YubiKey is phishing-proof - even if you click a fake link, your key won't authorize it!",
                            "Hardware keys are the strongest 2FA - they can't be hacked remotely!",
                            "Plug and tap - YubiKey makes 2FA both more secure AND easier to use!"
                        },
                        new List<string>
                        {
                            "Tip 5: Some password managers can store 2FA codes, but keep them separate from your passwords!",
                            "Convenience vs security: Storing 2FA with passwords is risky if your vault is compromised!",
                            "Use separate apps: Password manager + Authenticator app = defense in depth!",
                            "If someone steals your password vault, you want 2FA to be a separate barrier - don't store them together!"
                        }
                    }
                }
            };
        }

        public string GetRandomTip(string topic, int tipIndex)
        {
            if (!topicTips.ContainsKey(topic))
                return null;

            var tips = topicTips[topic];

            // If tipIndex is within range and we want a specific tip category
            if (tipIndex >= 0 && tipIndex < tips.Count)
            {
                var tipVariations = tips[tipIndex];
                int randomIndex = random.Next(tipVariations.Count);
                return tipVariations[randomIndex];
            }

            // Otherwise get random tip from all tips for this topic
            int randomTipIndex = random.Next(tips.Count);
            var randomTipVariations = tips[randomTipIndex];
            int randomVariationIndex = random.Next(randomTipVariations.Count);
            return randomTipVariations[randomVariationIndex];
        }

        public string GetRandomTipByCategory(string topic)
        {
            if (!topicTips.ContainsKey(topic))
                return null;

            var tips = topicTips[topic];
            int randomTipIndex = random.Next(tips.Count);
            var tipVariations = tips[randomTipIndex];
            int randomVariationIndex = random.Next(tipVariations.Count);
            return tipVariations[randomVariationIndex];
        }
    }
}