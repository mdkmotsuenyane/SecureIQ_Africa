using SecureIQ_Africa;
using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using static System.Console;

public class SecureIQMenu
{
    //crete new filepath 
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
    static void Divider(char symbol = '=', int length = 50)
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

        string message =
            "Welcome to SecureIQ Africa, your trusted cybersecurity assistant.";

        //generates voice that welcomes the user
        SpeechSynthesizer voice = new SpeechSynthesizer();
        voice.Speak(message);
        ForegroundColor = ConsoleColor.DarkYellow;
        Write("What is your name: ");
        ResetColor();
        string name = ReadLine();

        // store name in Response class
        respond.name = name;

        // create safe file name
        string cleanName = string.Concat(name.Split(Path.GetInvalidFileNameChars()));
        filePath = $"{cleanName}_chatlog.txt";

        // start session
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

        Header("Ask me anything about cybersecurity (type 'exit' to quit)");

        string input;
        ResetColor();
        //give user input
        do
        {
            ResetColor();
            ForegroundColor= ConsoleColor.DarkYellow;
            Write("> ");
            ResetColor();
            input = ReadLine();

            if (input.ToLower() != "exit")
            {
                respond.GetResponse(input);

                string botResponse = respond.Ask(input);
                SaveChat(input, botResponse);
                
            }

        } while (input.ToLower() != "exit");

        ExitApp();
    }

    // Exit app
    static void ExitApp()
    {
        Header("Good Bye");
        ForegroundColor = ConsoleColor.DarkYellow;
        TypeText($"Thanks for using SecureIQ Africa, {respond.name} ^-^", 50);
        ResetColor() ;
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

    //save that chat to textfile with
    static void SaveChat(string userInput, string response)
    {
        //exeption handeling
        try
        {
            //time of the interaction
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //textfile data
            string log =
                 $"[{time}] {respond.name}: {userInput}\n" +
                 $"[{time}] SecureIQ Bot: {response}\n\n";

            //append the file path and the logo
            File.AppendAllText(filePath, log);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save chat log: " + ex.Message);
        }
    }
}