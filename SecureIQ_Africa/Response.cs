using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Console;

namespace SecureIQ_Africa
{
    internal class Response
    {
        public string name { get; set; }

        // create a new intsance of ResponseData class
        private SecureData data = new SecureData();

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please ask me something about cybersecurity!";

            userInput = userInput.ToLower();

            foreach (var item in data.secureData)
            {
                foreach (var keyword in item.Value.keywords)
                {
                    if (userInput.Contains(keyword))
                    {
                        return item.Value.response;
                    }
                }
            }

            return "Sorry, I don't understand that yet. Try asking about passwords, phishing, malware, or Wi-Fi security.";
        }

        public void Ask(string question)
        {
            string response = GetResponse(question);
            WriteLine(name + " ," + response);
            WriteLine("");
        }

        public void Welcome()
        {
            string message;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;

            message = " Welcome to SecureIQ Africa, your trusted partner in cybersecurity awareness. How can we help you today : ";
            WriteLine("");
            WriteLine(message + " " + name);

            ResetColor();
        }
    }
}