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
    internal class Program
    {
        static void Main(string[] args)
        {
            //declaration
            string name, message;

            message = " Welcome to SecureIQ Africa, your trusted partner in cybersecurity awareness. How can we help you today?";


            //prints logo
            WriteLine("  █████████                                                  █████    ██████         █████████      ██████             ███                    \r\n ███░░░░░███                                                ░░███   ███░░░░███      ███░░░░░███    ███░░███           ░░░                     \r\n░███    ░░░   ██████   ██████  █████ ████ ████████   ██████  ░███  ███    ░░███    ░███    ░███   ░███ ░░░  ████████  ████   ██████   ██████  \r\n░░█████████  ███░░███ ███░░███░░███ ░███ ░░███░░███ ███░░███ ░███ ░███     ░███    ░███████████  ███████   ░░███░░███░░███  ███░░███ ░░░░░███ \r\n ░░░░░░░░███░███████ ░███ ░░░  ░███ ░███  ░███ ░░░ ░███████  ░███ ░███   ██░███    ░███░░░░░███ ░░░███░     ░███ ░░░  ░███ ░███ ░░░   ███████ \r\n ███    ░███░███░░░  ░███  ███ ░███ ░███  ░███     ░███░░░   ░███ ░░███ ░░████     ░███    ░███   ░███      ░███      ░███ ░███  ███ ███░░███ \r\n░░█████████ ░░██████ ░░██████  ░░████████ █████    ░░██████  █████ ░░░██████░██    █████   █████  █████     █████     █████░░██████ ░░████████\r\n ░░░░░░░░░   ░░░░░░   ░░░░░░    ░░░░░░░░ ░░░░░      ░░░░░░  ░░░░░    ░░░░░░ ░░    ░░░░░   ░░░░░  ░░░░░     ░░░░░     ░░░░░  ░░░░░░   ░░░░░░░░ \r\n                                                                                                                                              \r\n                                                                                                                                              \r\n                                                                                                                                              ");
            WriteLine("                       .,,uod8B8bou,,.\r\n              ..,uod8BBBBBBBBBBBBBBBBRPFT?l!i:.\r\n         ,=m8BBBBBBBBBBBBBBBRPFT?!||||||||||||||\r\n         !...:!TVBBBRPFT||||||||||!!^^\"\"'   ||||\r\n         !.......:!?|||||!!^^\"\"'            ||||\r\n         !.........||||                     ||||\r\n         !.........||||  ##                 ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         `.........||||                    ,||||\r\n          .;.......||||               _.-!!|||||\r\n   .,uodWBBBBb.....||||       _.-!!|||||||||!:'\r\n!YBBBBBBBBBBBBBBb..!|||:..-!!|||||||!iof68BBBBBb....\r\n!..YBBBBBBBBBBBBBBb!!||||||||!iof68BBBBBBRPFT?!::   `.\r\n!....YBBBBBBBBBBBBBBbaaitf68BBBBBBRPFT?!:::::::::     `.\r\n!......YBBBBBBBBBBBBBBBBBBBRPFT?!::::::;:!^\"`;:::       `.\r\n!........YBBBBBBBBBBRPFT?!::::::::::^''...::::::;         iBBbo.\r\n`..........YBRPFT?!::::::::::::::::::::::::;iof68bo.      WBBBBbo.\r\n  `..........:::::::::::::::::::::::;iof688888888888b.     `YBBBP^'\r\n    `........::::::::::::::::;iof688888888888888888888b.     `\r\n      `......:::::::::;iof688888888888888888888888888888b.\r\n        `....:::;iof688888888888888888888888888888888899fT!\r\n          `..::!8888888888888888888888888888888899fT|!^\"'\r\n            `' !!988888888888888888888888899fT|!^\"'\r\n                `!!8888888888888888899fT|!^\"'\r\n                  `!988888888899fT|!^\"'\r\n                    `!9899fT|!^\"'\r\n                      `!^\"'\r\n");
            //prompt the user 
            Write("What is your name : ");
            name = ReadLine();

            //System voice
            SpeechSynthesizer voice = new SpeechSynthesizer();
            voice.Speak(message);

            //Creates a new instance for the response class
            Response respond = new Response();
            respond.name = name;

            WriteLine(message + " " + name);


        }
        public string Validate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                WriteLine("Please enter a valid question.");
                // Return null for invalid input
                return null;
            }

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                WriteLine("Goodbye! Stay safe online.");
                // closes the program
                Environment.Exit(0);
            }
            // Return the valid input
            return input;
        }



    }
}