using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace SecureIQ_Africa
{
    internal class Response
    {
        //get and set name
        public string name { get; set; }


        //dictionary is used to store all the posssible questions
        private Dictionary<string, (string[] keywords, string response)> secureData =
            new Dictionary<string, (string[], string)>()
        {
            {
                "password",
                (new[] { "password", "pass", "login", "credential" },
                "Avoid using personal information and use strong passwords with uppercase, lowercase, numbers, and symbols.")
            },
            {
                "phishing",
                (new[] { "phishing", "scam", "fake email", "suspicious link" },
                "Do not click suspicious links. Always verify the sender before responding.")
            },
            {
                "malware",
                (new[] { "malware", "virus", "trojan", "spyware" },
                "Install antivirus software and avoid downloading from untrusted sources.")
            },
            {
                "wifi",
                (new[] { "wifi", "public wifi", "network", "hotspot" },
                "Avoid sensitive transactions on public Wi-Fi. Use a VPN if possible.")
            },
            {
                "greeting",
                (new[] { "hello", "hi", "hey" },
                "Hello! I'm your cybersecurity awareness bot.")
            },
            {
                "help",
                (new[] { "help", "what can i ask", "what do you know" },
                "You can ask me about passwords, phishing, malware, safe browsing, and more.")
            }
        };

        //used to get response
        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please ask me something about cybersecurity!";

            userInput = userInput.ToLower();

            foreach (var item in secureData)
            {
                foreach (var keyword in item.Value.keywords)
                {
                    if (userInput.Contains(keyword))
                    {
                        return item.Value.response;
                    }
                }
            }

            return "Sorry, I don't understand that yet.";
        }

        //  prints response
        public void Ask(string question)
        {
            string response;
            response = GetResponse(question);
            WriteLine(name + " " + response);
        }



        public void Welcome()
        {
            string message = " Welcome to SecureIQ Africa, your trusted partner in cybersecurity awareness. How can we help you today : ";
            WriteLine(" █████   ███   █████          ████                                            \r\n░░███   ░███  ░░███          ░░███                                            \r\n ░███   ░███   ░███   ██████  ░███   ██████   ██████  █████████████    ██████ \r\n ░███   ░███   ░███  ███░░███ ░███  ███░░███ ███░░███░░███░░███░░███  ███░░███\r\n ░░███  █████  ███  ░███████  ░███ ░███ ░░░ ░███ ░███ ░███ ░███ ░███ ░███████ \r\n  ░░░█████░█████░   ░███░░░   ░███ ░███  ███░███ ░███ ░███ ░███ ░███ ░███░░░  \r\n    ░░███ ░░███     ░░██████  █████░░██████ ░░██████  █████░███ █████░░██████ \r\n     ░░░   ░░░       ░░░░░░  ░░░░░  ░░░░░░   ░░░░░░  ░░░░░ ░░░ ░░░░░  ░░░░░░  \r\n                                                                              \r\n                                                                              \r\n                                                                              ");
            WriteLine(message + " " + name);
        }

    }
}