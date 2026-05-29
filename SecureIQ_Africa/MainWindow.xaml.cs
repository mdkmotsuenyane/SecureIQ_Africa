using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a valid name!");
                return;
            }
            else
            {
                Response respond = new Response();
                respond.name = name;
            }

            // Close the MainWindow and open the SecureIQChatWindow
            SecureIQChatWindow chatWindow = new SecureIQChatWindow(name);
            chatWindow.Show();
            this.Close();
        }
    }
}
