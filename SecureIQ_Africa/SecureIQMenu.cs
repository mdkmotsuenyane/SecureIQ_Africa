using SecureIQ_Africa;
using System;
using System.Speech.Synthesis;
using System.Threading;
using static System.Console;

public class SecureIQMenu
{
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
    static void Divider(char symbol = '=', int length = 50)
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine(new string(symbol, length));
        ResetColor();
    }

    // header
    static void Header(string title)
    {
        ForegroundColor = ConsoleColor.Cyan;
        Divider();
        WriteLine(title.ToUpper());
        Divider();
        ResetColor();
    }

    // Welcome Screen
    public static void ShowWelcomeScreen()
    {
        Clear();
        //prints logo ascii art
        WriteLine("  █████████                                                  █████    ██████         █████████      ██████             ███                    \r\n ███░░░░░███                                                ░░███   ███░░░░███      ███░░░░░███    ███░░███           ░░░                     \r\n░███    ░░░   ██████   ██████  █████ ████ ████████   ██████  ░███  ███    ░░███    ░███    ░███   ░███ ░░░  ████████  ████   ██████   ██████  \r\n░░█████████  ███░░███ ███░░███░░███ ░███ ░░███░░███ ███░░███ ░███ ░███     ░███    ░███████████  ███████   ░░███░░███░░███  ███░░███ ░░░░░███ \r\n ░░░░░░░░███░███████ ░███ ░░░  ░███ ░███  ░███ ░░░ ░███████  ░███ ░███   ██░███    ░███░░░░░███ ░░░███░     ░███ ░░░  ░███ ░███ ░░░   ███████ \r\n ███    ░███░███░░░  ░███  ███ ░███ ░███  ░███     ░███░░░   ░███ ░░███ ░░████     ░███    ░███   ░███      ░███      ░███ ░███  ███ ███░░███ \r\n░░█████████ ░░██████ ░░██████  ░░████████ █████    ░░██████  █████ ░░░██████░██    █████   █████  █████     █████     █████░░██████ ░░████████\r\n ░░░░░░░░░   ░░░░░░   ░░░░░░    ░░░░░░░░ ░░░░░      ░░░░░░  ░░░░░    ░░░░░░ ░░    ░░░░░   ░░░░░  ░░░░░     ░░░░░     ░░░░░  ░░░░░░   ░░░░░░░░ \r\n                                                                                                                                              \r\n                                                                                                                                              \r\n                                                                                                                                              ");
        WriteLine("                       .,,uod8B8bou,,.\r\n              ..,uod8BBBBBBBBBBBBBBBBRPFT?l!i:.\r\n         ,=m8BBBBBBBBBBBBBBBRPFT?!||||||||||||||\r\n         !...:!TVBBBRPFT||||||||||!!^^\"\"'   ||||\r\n         !.......:!?|||||!!^^\"\"'            ||||\r\n         !.........||||                     ||||\r\n         !.........||||  ##                 ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         !.........||||                     ||||\r\n         `.........||||                    ,||||\r\n          .;.......||||               _.-!!|||||\r\n   .,uodWBBBBb.....||||       _.-!!|||||||||!:'\r\n!YBBBBBBBBBBBBBBb..!|||:..-!!|||||||!iof68BBBBBb....\r\n!..YBBBBBBBBBBBBBBb!!||||||||!iof68BBBBBBRPFT?!::   `.\r\n!....YBBBBBBBBBBBBBBbaaitf68BBBBBBRPFT?!:::::::::     `.\r\n!......YBBBBBBBBBBBBBBBBBBBRPFT?!::::::;:!^\"`;:::       `.\r\n!........YBBBBBBBBBBRPFT?!::::::::::^''...::::::;         iBBbo.\r\n`..........YBRPFT?!::::::::::::::::::::::::;iof68bo.      WBBBBbo.\r\n  `..........:::::::::::::::::::::::;iof688888888888b.     `YBBBP^'\r\n    `........::::::::::::::::;iof688888888888888888888b.     `\r\n      `......:::::::::;iof688888888888888888888888888888b.\r\n        `....:::;iof688888888888888888888888888888888899fT!\r\n          `..::!8888888888888888888888888888888899fT|!^\"'\r\n            `' !!988888888888888888888888899fT|!^\"'\r\n                `!!8888888888888888899fT|!^\"'\r\n                  `!988888888899fT|!^\"'\r\n                    `!9899fT|!^\"'\r\n                      `!^\"'\r\n");
        WriteLine("*********** SecureIQ Africa ***********");

        TypeText("Welcome to SecureIQ Africa");

        string message =
            "Welcome to SecureIQ Africa, your trusted cybersecurity assistant.";

        //generates voice that welcomes the user
        SpeechSynthesizer voice = new SpeechSynthesizer();
        voice.Speak(message);

        Write("What is your name: ");
        string name = ReadLine();

        // store name in Response class
        respond.name = name;

        TypeText($"Hi {name}, welcome to SecureIQ Africa!");

        WriteLine("********************************************");
        ResetColor();
        ShowMainMenu();
    }

    // Main Menu 
    public static void ShowMainMenu()
    {
        Clear();
        Header("Ask me anything about cybersecurity (type 'exit' to quit)");

        string input;

        do
        {
            Write("> ");
            input = ReadLine();

            if (input.ToLower() != "exit")
            {
                respond.GetResponse(input);
                respond.Ask(input);
            }

        } while (input.ToLower() != "exit");

        ExitApp();
    }

    // Exit app
    static void ExitApp()
    {
        Header("Good Bye");
        TypeText($"Thanks for using SecureIQ Africa, {respond.name} ^-^", 50);

        Thread.Sleep(1000);
        Environment.Exit(0);
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
}