using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecureIQ_Africa
{
    /// <summary>
    /// Interaction logic for SecureIQChatWindow.xaml
    /// </summary>
    public partial class SecureIQChatWindow : Window
    {
        private string username;
        private bool isStored = false;
        private Response responseHandler;
        private string historyFile = "History/chat_history.txt";

        // constructor that accepts username 
        public SecureIQChatWindow(string userName)
        {
            InitializeComponent();
            this.username = userName ?? "Guest"; // Handle null username

            // Initialize the response handler
            responseHandler = new Response();

            // Set the username properly using the method
            responseHandler.SetUserName(username);

            Greeting();
        }

        //voice greeting system 
        public void Greeting()
        {
            try
            {
                // Check if file exists before trying to play
                string soundPath = "C:\\Users\\Lenovo\\source\\repos\\SecureIQ-Africa\\SecureIQ-Africa\\SecureIQ.wav";
                if (File.Exists(soundPath))
                {
                    SoundPlayer player = new SoundPlayer(soundPath);
                    player.Play();
                }
                else
                {
                    // Handle missing sound file gracefully
                    Console.WriteLine("Sound file not found: " + soundPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }

            // Get personalized greeting from the Response class
            string greeting = responseHandler.GetPersonalizedGreeting();
            if (string.IsNullOrEmpty(greeting))
            {
                greeting = $"Welcome, {username}! We are here to answer your cybersecurity-related questions.";
            }

            BotMessage(greeting);
        }

        //events that happen after the button is clicked 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userMessage = userInput.Text.Trim();
                if (string.IsNullOrEmpty(userMessage))
                {
                    BotMessage("Please type your cybersecurity question.");
                    return;
                }

                // Display user message
                ChatMessage(userMessage);

                // Save to history
                SaveMessage(username, userMessage);
                isStored = true;

                // Get and display bot response
                string response = responseHandler.GetResponse(userMessage);
                BotMessage(response);
                SaveMessage("Bot", response);

                // Clear input
                userInput.Clear();
                // Return focus to input field
                userInput.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                BotMessage("An error occurred. Please try again.");
            }
        }

        //Bot Message 
        public void BotMessage(string message)
        {
            
            Dispatcher.Invoke(() =>
            {
                StackPanel stack = new StackPanel();

                TextBlock time = new TextBlock()
                {
                    Text = DateTime.Now.ToString("HH:mm:ss"),
                    Foreground = Brushes.White,
                    FontSize = 11
                };

                Border border = new Border()
                {
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(10),
                    Margin = new Thickness(5, 2, 5, 5),
                    MaxWidth = 400,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };

                TextBlock text = new TextBlock()
                {
                    Text = "🤖: " + message,
                    Foreground = Brushes.Black,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap
                };

                border.Child = text;
                stack.Children.Add(time);
                stack.Children.Add(border);

                ChatPanel.Children.Add(stack);

                // Auto-scroll to bottom
                ScrollViewer scrollViewer = FindScrollViewer(ChatPanel);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            });
        }

        //Chat Message 
        public void ChatMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                StackPanel stack = new StackPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                TextBlock time = new TextBlock()
                {
                    Text = DateTime.Now.ToString("HH:mm:ss"),
                    Foreground = Brushes.White,
                    FontSize = 11,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Border border = new Border()
                {
                    Background = Brushes.DarkGray,
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(10),
                    Margin = new Thickness(10, 2, 5, 5),
                    MaxWidth = 400,
                    HorizontalAlignment = HorizontalAlignment.Right,
                };

                TextBlock text = new TextBlock()
                {
                    Text = "👤: " + message,
                    Foreground = Brushes.White,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap
                };

                border.Child = text;
                stack.Children.Add(time);
                stack.Children.Add(border);

                ChatPanel.Children.Add(stack);

                // Auto-scroll to bottom
                ScrollViewer scrollViewer = FindScrollViewer(ChatPanel);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            });
        }

        // Helper method to find ScrollViewer in the visual tree
        private ScrollViewer FindScrollViewer(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is ScrollViewer scrollViewer)
                    return scrollViewer;

                var result = FindScrollViewer(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        //saving history 
        private void SaveMessage(string user, string message)
        {
            try
            {
                Directory.CreateDirectory("History");
                string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {user}: {message}";
                File.AppendAllText(historyFile, line + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
            }
        }

        // Handle Enter key press in textbox
        private void userInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        // TYPING EFFECT - Added this method
        private async Task TypingAnimation()
        {
            Border typingBorder = new Border()
            {
                Background = Brushes.Gray,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock typingText = new TextBlock()
            {
                Text = username + " is Typing...",
                Foreground = Brushes.Black
            };

            typingBorder.Child = typingText;

            // Add timestamp
            StackPanel messageStack = new StackPanel();
            TextBlock time = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Foreground = Brushes.Gray,
                FontSize = 11
            };
            messageStack.Children.Add(time);
            messageStack.Children.Add(typingBorder);

            // Add to chat panel
            Dispatcher.Invoke(() =>
            {
                ChatPanel.Children.Add(messageStack);
                ScrollViewer scrollViewer = FindScrollViewer(ChatPanel);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            });

            // Show typing indicator for 2 seconds
            await Task.Delay(2000);

            // Remove typing indicator
            Dispatcher.Invoke(() =>
            {
                ChatPanel.Children.Remove(messageStack);
            });
        }
    }
}