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
        private Window menuWindow;

        public SecureIQChatWindow(string userName, Window menuWin = null)
        {
            InitializeComponent();
            this.username = userName ?? "Guest";
            this.menuWindow = menuWin;

            responseHandler = new Response();
            responseHandler.SetUserName(username);

            try
            {
                Directory.CreateDirectory("History");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating History directory: {ex.Message}");
            }

            Greeting();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Log session closure
            ActivityLogExtensions.LogCustom($"Chat session closed for {username}");

            this.Close();

            if (menuWindow != null)
            {
                menuWindow.Show();
                menuWindow.Activate();
            }
            else
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win is Menu && win != this)
                    {
                        win.Show();
                        win.Activate();
                        break;
                    }
                }
            }
        }

        private async void Greeting()
        {
            string greeting = responseHandler.GetPersonalizedGreeting();
            if (string.IsNullOrEmpty(greeting))
            {
                greeting = $"Welcome, {username}! We are here to answer your cybersecurity-related questions.";
            }

            // Log chat start
            ActivityLogExtensions.LogCustom($"Chat session started for {username}");

            await TypingAnimation();
            BotMessage(greeting);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userMessage = userInput.Text.Trim();
                if (string.IsNullOrEmpty(userMessage))
                {
                    BotMessage("Please type your cybersecurity question.");
                    return;
                }

                // Log user message
                ActivityLogExtensions.LogCustom($"User ({username}): {userMessage}");

                ChatMessage(userMessage);
                SaveMessage(username, userMessage);
                isStored = true;

                await TypingAnimation();

                string response = responseHandler.GetResponse(userMessage);
                BotMessage(response);
                SaveMessage("Bot", response);

                // Log bot response
                ActivityLogExtensions.LogCustom($"Bot: {response}");

                userInput.Clear();
                userInput.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                BotMessage("An error occurred. Please try again.");
            }
        }

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

                ScrollViewer scrollViewer = FindScrollViewer(ChatPanel);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            });
        }

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

                ScrollViewer scrollViewer = FindScrollViewer(ChatPanel);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            });
        }

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
                try
                {
                    string fallbackFile = "chat_history_backup.txt";
                    string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {user}: {message}";
                    File.AppendAllText(fallbackFile, line + Environment.NewLine);
                }
                catch
                {
                    Console.WriteLine("Saving failed");
                }
            }
        }

        private void userInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

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
                Text = "🤖 is Typing...",
                Foreground = Brushes.Black
            };

            typingBorder.Child = typingText;

            StackPanel messageStack = new StackPanel();
            TextBlock time = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Foreground = Brushes.Gray,
                FontSize = 11
            };
            messageStack.Children.Add(time);
            messageStack.Children.Add(typingBorder);

            Dispatcher.Invoke(() =>
            {
                ChatPanel.Children.Add(messageStack);
                ScrollViewer scrollViewer = FindScrollViewer(ChatPanel);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            });

            await Task.Delay(2000);

            Dispatcher.Invoke(() =>
            {
                ChatPanel.Children.Remove(messageStack);
            });
        }
    }
}