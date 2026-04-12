using SecureIQ_Africa;
using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using static System.Console;

public class SecureIQMenu
{
    // create new filepath 
    static string filePath;

    // creates a new instance for Response Class
    static Response respond = new Response();

    // typing effect
    static void TypeText(string text, int delay = 30)
    {
        foreach (char c in text)
        {
            Write(c);
            Thread.Sleep(delay);
        }
        WriteLine();
    }

    // divider line
    static void Divider(char symbol = '=', int length = 70)
    {
        ForegroundColor = ConsoleColor.Blue;
        WriteLine(new string(symbol, length));
        ResetColor();
    }

    // header
    static void Header(string title)
    {
        ForegroundColor = ConsoleColor.Yellow;
        Divider();
        WriteLine(title.ToUpper());
        Divider();
        ResetColor();
    }

    // Welcome Screen
    public static void ShowWelcomeScreen()
    {
        Clear();
        ForegroundColor = ConsoleColor.DarkYellow;
        //prints logo ascii art
        WriteLine("  █████████                                                  █████    ██████         █████████      ██████             ███                    \r\n ███░░░░░███                                                ░░███   ███░░░░███      ███░░░░░███    ███░░███           ░░░                     \r\n░███    ░░░   ██████   ██████  █████ ████ ████████   ██████  ░███  ███    ░░███    ░███    ░███   ░███ ░░░  ████████  ████   ██████   ██████  \r\n░░█████████  ███░░███ ███░░███░░███ ░███ ░░███░░███ ███░░███ ░███ ░███     ░███    ░███████████  ███████   ░░███░░███░░███  ███░░███ ░░░░░███ \r\n ░░░░░░░░███░███████ ░███ ░░░  ░███ ░███  ░███ ░░░ ░███████  ░███ ░███   ██░███    ░███░░░░░███ ░░░███░     ░███ ░░░  ░███ ░███ ░░░   ███████ \r\n ███    ░███░███░░░  ░███  ███ ░███ ░███  ░███     ░███░░░   ░███ ░░███ ░░████     ░███    ░███   ░███      ░███      ░███ ░███  ███ ███░░███ \r\n░░█████████ ░░██████ ░░██████  ░░████████ █████    ░░██████  █████ ░░░██████░██    █████   █████  █████     █████     █████░░██████ ░░████████\r\n ░░░░░░░░░   ░░░░░░   ░░░░░░    ░░░░░░░░ ░░░░░      ░░░░░░  ░░░░░    ░░░░░░ ░░    ░░░░░   ░░░░░  ░░░░░     ░░░░░     ░░░░░  ░░░░░░   ░░░░░░░░ \r\n                                                                                                                                              \r\n                                                                                                                                              \r\n                                                                                                                                              ");
        WriteLine("                       .,,uod8B8bou,,.\r\n              ..,uod8BBBBBBBBBBBBBBBBRPFT?l!i:.\r\n         ,=m8BBBBBBBBBBBBBBBRPFT?!||||||||||||||\r\n         !...:!TVBBBRPFT||||||||||!!^^\"\"'   ||||\r\n         !.......:!?|||||!!^^\"\"'            ||||\r\n         !.........||||                     ||||\r\n         !.........||||  ##                 ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         `.........||||                    ,||||\r\n          .;.......||||               _.-!!|||||\r\n   .,uodWBBBBb.....||||       _.-!!|||||||||!:'\r\n!YBBBBBBBBBBBBBBb..!|||:..-!!|||||||!iof68BBBBBb....\r\n!..YBBBBBBBBBBBBBBb!!||||||||!iof68BBBBBBRPFT?!::   `.\r\n!....YBBBBBBBBBBBBBBbaaitf68BBBBBBRPFT?!:::::::::     `.\r\n!......YBBBBBBBBBBBBBBBBBBBRPFT?!::::::;:!^\"`;:::       `.\r\n!........YBBBBBBBBBBRPFT?!::::::::::^''...::::::;         iBBbo.\r\n`..........YBRPFT?!::::::::::::::::::::::::;iof68bo.      WBBBBbo.\r\n  `..........:::::::::::::::::::::::;iof688888888888b.     `YBBBP^'\r\n    `........::::::::::::::::;iof688888888888888888888b.     `\r\n      `......:::::::::;iof688888888888888888888888888888b.\r\n        `....:::;iof688888888888888888888888888888888899fT!\r\n          `..::!8888888888888888888888888888888899fT|!^\"'\r\n            `' !!988888888888888888888888899fT|!^\"'\r\n                `!!8888888888888888899fT|!^\"'\r\n                  `!988888888899fT|!^\"'\r\n                    `!9899fT|!^\"'\r\n                      `!^\"'\r\n");
        //resets colour
        ResetColor();
        ForegroundColor = ConsoleColor.DarkBlue;
        WriteLine("*********** SecureIQ Africa ***********");
        ResetColor();
        TypeText("Welcome to SecureIQ Africa");

        //message
        string message =
              "Welcome to SecureIQ Africa, your trusted cybersecurity assistant.";

        //system voice the welcomes the user
        SpeechSynthesizer voice = new SpeechSynthesizer();
        voice.Speak(message);

        // declaration
        string name;

        do
        {
            //prompt for name
            ForegroundColor = ConsoleColor.DarkYellow;
            Write("What is your name: ");
            ResetColor();

            name = ReadLine();

            //validates the name
            if (string.IsNullOrWhiteSpace(name))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Name cannot be empty. Please enter a valid name.");
                ResetColor();
            }

        } while (string.IsNullOrWhiteSpace(name));
        //instance name from Response classe
        respond.name = name;

        // safe filename
        string cleanName = string.Concat(name.Split(Path.GetInvalidFileNameChars()));
        if (string.IsNullOrWhiteSpace(cleanName))
            cleanName = "User";

        filePath = $"{cleanName}_chatlog.txt";

        File.AppendAllText(filePath, $"\n===== New Session: {DateTime.Now} =====\n");

        TypeText($"Hi {name}, welcome to SecureIQ Africa!");

        ForegroundColor = ConsoleColor.DarkBlue;
        WriteLine("********************************************");
        ResetColor();

        ShowMainMenu();
    }

    // Main Menu 
    public static void ShowMainMenu()
    {
        Clear();
        //calls methood
        Header("Ask me anything about cybersecurity (type 'exit' to quit)");

        string input;

        //do while loop to ensure that the program keeps running until the user types exit
        do
        {
            ForegroundColor = ConsoleColor.DarkYellow;
            Write("> ");
            ResetColor();

            input = ReadLine();

            // validates the input
            if (string.IsNullOrWhiteSpace(input))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Please enter a valid question.");
                ResetColor();
                continue;
            }

            if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                string botResponse = respond.Ask(input);
                SaveChat(input, botResponse);
            }

        } while (!input.Equals("exit", StringComparison.OrdinalIgnoreCase));

        ExitApp();
    }

    // Exit app
    static void ExitApp()
    {
        string choice;
        Clear();
        do
        {
            //do while loop to ensure thatb the program keeps running until the user types exit
            Header("Do you really want to exit or return to menu?");
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("1. Exit");
            WriteLine("2. Return to Menu");
            ResetColor();

            ForegroundColor = ConsoleColor.DarkYellow;
            Write("Choose option (1 or 2): ");
            ResetColor();

            choice = ReadLine();

            // validates the choices
            if (choice != "1" && choice != "2")
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Invalid choice. Please enter 1 or 2.");
                ResetColor();
            }

        } while (choice != "1" && choice != "2");

        //if 2 is chosen the method is called
        if (choice == "2")
        {
            ReturnToMenu();
        }
        else
        {
            //closes console application
            Header("Good Bye");
            ForegroundColor = ConsoleColor.DarkYellow;
            TypeText($"Thanks for using SecureIQ Africa, {respond.name} ^-^", 50);
            ResetColor();
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }

    // return to menu helper 
    static void ReturnToMenu()
    {
        WriteLine();
        ForegroundColor = ConsoleColor.DarkGreen;
        WriteLine("Press any key to return to menu...");
        ResetColor();
        ReadKey();
        Clear();

        ShowMainMenu();
    }

    // save chat to text file
    static void SaveChat(string userInput, string response)
    {
        try
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string log =
                 $"[{time}] {respond.name}: {userInput}\n" +
                 $"[{time}] SecureIQ Bot: {response}\n\n";

            File.AppendAllText(filePath, log);
        }
        catch (Exception ex)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("Failed to save chat log: " + ex.Message);
            ResetColor();
        }
    }
}