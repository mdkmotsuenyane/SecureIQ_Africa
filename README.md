# 🛡️ SecureIQ Africa

SecureIQ Africa is a **C# console-based cybersecurity chatbot** designed to educate users about basic cybersecurity concepts such as phishing, malware, passwords, and safe internet practices.

It features a simple AI-style keyword response system, typing effects, speech synthesis, and chat logging.

---

## 🚀 Features

- 💬 Interactive cybersecurity chatbot
- 🧠 Keyword-based response engine
- 🗣️ Speech synthesis welcome message
- ⌨️ Typing animation effect
- 📁 Automatic chat logging to text file
- 👤 Personalized user experience (uses user name)
- 🎨 Colored console interface
- 🚪 Exit command support

---

## 🏗️ Project Structure
SecureIQ_Africa/
- │
- ├── SecureIQMenu.cs # Main UI, menu system, and program flow
- ├── Response.cs # Chatbot logic and response handling
- ├── SecureData.cs # Stores cybersecurity keywords & responses
- ├── Program.cs # Application entry point
- └── chat.txt # Auto-generated chat log file depend ingon the name of the user maybe john.txt 

---

## ▶️ How to Run

### Requirements
- Visual Studio 2019 / 2022
- .NET Framework or .NET Console App support
- Windows OS (required for System.Speech.Synthesis)

## 💡 How It Works
- ASCII logo is printed
- User enters their name
- System creates a personalized chat log file
- Welcome message is displayed with speech + animation
- User types cybersecurity questions
- Bot matches keywords and responds
- All chats are saved in a .txt file
- Type exit to quit the application
- 
### Steps

```bash
# Clone the repository
git clone https://github.com/your-username/SecureIQ_Africa.git

# Open the project in Visual Studio
# Build and Run (F5)
```
## 👨‍💻 Author
- Mokadi Motsuenyane ST10480772 DIS2 Group 3

## 📜 License

- This project is for educational purposes for IIE Rosebank College. 
