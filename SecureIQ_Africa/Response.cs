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
            //checks if the line is empty
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please ask me something about cybersecurity!";

            //get the user input and turns into lower case
            userInput = userInput.ToLower().Trim();

            var sortedData = data.secureData
                .OrderByDescending(x => x.Value.keywords.Max(k => k.Length));

            //slipt thespaces in between
            var words = userInput.Split(' ');

            foreach (var item in sortedData)
            {
                foreach (var keyword in item.Value.keywords)
                {
                    if (words.Contains(keyword) || userInput.Contains(keyword))
                    {
                        return item.Value.response;
                    }
                }
            }
            //error message
            ForegroundColor = ConsoleColor.Red;
            return "Sorry, I don't understand that yet. Try asking about passwords, phishing, malware, or Wi-Fi security.";
        }

        public string Ask(string question)
        {
            string response = GetResponse(question);
            WriteLine($"{name}, {response}");
            WriteLine();
            return response;
        }


    }
}