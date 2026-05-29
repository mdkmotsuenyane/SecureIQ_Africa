# 🛡️ SecureIQ Africa

SecureIQ Africa is a C# WPF-based cybersecurity chatbot designed to educate users about basic cybersecurity concepts such as phishing, malware, passwords, and safe internet practices.

It features a modern graphical user interface, AI-style keyword response system, typing animations, voice greeting, automatic chat logging, **sentiment detection**, **memory & recall**, and **empathetic responses**.

## 🚀 Features

### Core Features
- 💬 Interactive cybersecurity chatbot with modern GUI
- 🧠 Keyword-based response engine for cybersecurity education
- 🗣️ Voice greeting using System.Media.SoundPlayer
- ⌨️ Typing animation effect for realistic bot responses
- 📁 Automatic chat logging to text file in History folder
- 👤 Personalized user experience (greets user by name)
- 🎨 Modern WPF interface with chat bubbles
- ⌨️ Enter key support for sending messages
- 📜 Auto-scrolling chat panel
- 🚪 Exit command support

### Advanced Features
- 😊 **Sentiment Detection** - Detects user emotions (angry, worried, frustrated, sad, happy, curious, confident)
- 🧠 **Memory & Recall** - Remembers user's name and favorite cybersecurity topics
- 💝 **Empathetic Responses** - Responds appropriately based on detected sentiment
- 🔄 **Follow-up Questions** - Handles "yes/no" responses and offers alternative topics
- 📚 **Topic Suggestions** - Provides list of available cybersecurity topics
- 💡 **Cybersecurity Tips** - Shares practical security tips for each topic
- 🗣️ **Natural Conversation Flow** - Handles farewells, gratitude, and dismissals

## 📁 Project Structure

```
SecureIQ_Africa/
│
├── SecureIQChatWindow.xaml          # Main chat window UI
├── SecureIQChatWindow.xaml.cs       # Chat window logic & typing animation
├── Response.cs                       # Chatbot response handling logic (sentiment, memory, tips)
├── Memory.cs                         # User memory storage (name, favorite topics)
├── ResponseTips.cs                   # Cybersecurity tips database
├── SecureData.cs                     # Cybersecurity keywords & responses storage
├── History/                          # Auto-generated chat history folder
│   └── [username]_chat_history.txt   # Individual chat logs
└── SecureIQ.wav                      # Voice greeting sound file
```

## 🎮 Conversation Examples

### Example 1: User expresses frustration
```
User: "I'm angry about wifi"
Bot: "I understand that dealing with wifi can be really frustrating. 
      Let me share something that might help.
      
      Create a guest network for visitors so they can't access your main devices.
      
      Would you like another tip?"
```

### Example 2: User shares favorite topic
```
User: "Password safety is my favorite topic"
Bot: "Great! I'll remember that you're interested in password. 
      It's a crucial part of staying safe online.
      
      As someone interested in password, here's a helpful tip:
      Use a password manager to generate and store unique, complex passwords for each account.
      
      Would you like to learn more about password?"
```

### Example 3: User asks about their name
```
User: "What is my name?"
Bot: "Your name is John! I remember you told me earlier."
```

### Example 4: User says goodbye
```
User: "goodbye"
Bot: "Goodbye John! Stay safe online. Feel free to come back anytime you have cybersecurity questions."
```

## 💡 How It Works

### Application Launch
1. User enters their name in the login window
2. System validates the name
3. Chat Window opens with voice greeting
4. Welcome message appears with typing animation
5. Bot introduces itself and offers cybersecurity help

### Sentiment Detection Flow
1. User types a message expressing emotion
2. System detects sentiment (angry, worried, frustrated, sad, happy, curious, confident)
3. If emotion has a topic → Bot provides empathetic response + tip
4. If emotion has no topic → Bot asks follow-up question ("What's wrong?")

### Memory & Recall Flow
1. User shares their name → Bot stores it
2. User says "my favorite topic is X" → Bot stores preference
3. User asks "what is my name?" → Bot recalls stored name
4. User asks "what is my favorite topic?" → Bot recalls stored preference

### Chat Logging
- All conversations are automatically saved to `History/chat_history.txt`
- Each message includes timestamp and sender name

### Exit Application
- Close the window or type "exit" to quit
- Chat history is preserved for the session

## 🎮 Controls

| Action | Method |
|--------|--------|
| Send message | Click "Send" button OR press Enter key |
| Clear input | Automatically after sending |
| Scroll chat | Mouse wheel or auto-scroll |
| Exit | Close window or type "exit" |

## 🔧 Technical Implementation

| Component | Technology |
|-----------|------------|
| UI Framework | Windows Presentation Foundation (WPF) |
| Programming Language | C# |
| Animation | Async/await for typing effect |
| Audio | System.Media.SoundPlayer for voice greeting |
| File Handling | StreamWriter/File.AppendAllText for chat logs |
| Message Display | Dynamic StackPanel with Borders for chat bubbles |
| Sentiment Detection | Keyword-based pattern matching |
| Memory Storage | In-memory Dictionary with session persistence |

## 📝 Chat Log Format

```
2025-05-29 14:30:15 - John: What is phishing?
2025-05-29 14:30:17 - Bot: Phishing is a cyber attack where attackers trick you into revealing sensitive information...
2025-05-29 14:30:25 - John: I'm worried about online scams
2025-05-29 14:30:27 - Bot: It's completely understandable to feel worried about phishing. Your concern shows you care about your security!
```

## 🎨 UI Features

- **Chat Bubbles**: Left-aligned (bot) and right-aligned (user)
- **Timestamps**: Every message shows send time
- **Typing Indicator**: Shows "🤖 is typing..." when bot is "thinking"
- **Auto-scroll**: Automatically scrolls to newest message
- **Focus Management**: Textbox automatically focused after sending

## ▶️ How to Run

### Requirements
- Visual Studio 2019 / 2022 or later
- .NET Framework 4.7.2 or .NET Core/.NET 5+ with WPF support
- Windows OS (required for WPF and System.Media)

### Steps
```bash
# Clone the repository
git clone https://github.com/mdkmotsuenyane/SecureIQ_Africa.git

# Open the project in Visual Studio
# Build the solution (Ctrl+Shift+B)
# Run the application (F5)
```

## 📺 YouTube Demo

```
https://youtu.be/G1AP2P_0bs4?si=v5UgQFcuTsKxug_s
```

## 👨‍💻 Author

**Mokadi Motsuenyane**  
ST10480772  
DIS2 Group 3  

## 📜 License

This project is for educational purposes at Rosebank International College.  
All rights reserved.

## 🤝 Support

For questions or support regarding this project:
- Refer to the YouTube demo video
- Check the project repository documentation

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| v2.1 | May 2025 | WPF GUI version with typing animations and modern UI with Added sentiment detection, memory & recall, empathetic responses |
| v1.0 | April 2025 | Console-based version with speech synthesis |

## 🧪 Supported Cybersecurity Topics

| Topic | Description |
|-------|-------------|
| Password Security | Strong passwords, password managers, passphrases |
| Phishing Detection | Spotting scams, fake emails, suspicious links |
| Malware Protection | Viruses, ransomware, trojans, antivirus |
| WiFi Security | Public WiFi risks, home network protection |
| VPN & Privacy | Virtual Private Networks, data encryption |
| Two-Factor Authentication | 2FA setup, authenticator apps, security keys |
| Data Backup | 3-2-1 backup rule, cloud storage |
| Online Privacy | Personal data protection, social media safety |
