# 🛡️ SecureIQ Africa

SecureIQ Africa is a C# WPF-based cybersecurity chatbot designed to educate users about basic cybersecurity concepts such as phishing, malware, passwords, and safe internet practices.

It features a modern graphical user interface, AI‑style keyword response system, typing animations, voice greeting, automatic chat logging, **sentiment detection**, **memory & recall**, **empathetic responses**, **NLP intent parsing**, **interactive quiz system**, **task management with SQL database**, and a comprehensive **activity log**.

## 🚀 Features

### Core Features
- 💬 Interactive cybersecurity chatbot with modern GUI
- 👌 Improved Input Validation
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
- 😊 **Sentiment Detection** – Detects user emotions (angry, worried, frustrated, sad, happy, curious, confident)
- 🧠 **Memory & Recall** – Remembers user's name and favorite cybersecurity topics
- 💝 **Empathetic Responses** – Responds appropriately based on detected sentiment
- 🔄 **Follow-up Questions** – Handles "yes/no" responses and offers alternative topics
- 📚 **Topic Suggestions** – Provides list of available cybersecurity topics
- 💡 **Cybersecurity Tips** – Shares practical security tips for each topic
- 🗣️ **Natural Conversation Flow** – Handles farewells, gratitude, and dismissals

### ✨ New Features (v3.0)
- 🧠 **NLP Engine** – Natural language understanding for task/reminder creation, quiz requests, and more
- 📋 **Task Manager** – Create, complete, and delete tasks with SQLite (LocalDB) persistence
- ❓ **Interactive Quiz** – Test your cybersecurity knowledge with multiple‑choice questions
- 🗂️ **Topic‑based Quizzes** – Filter quiz questions by cybersecurity topic
- 📝 **Activity Log** – Track every user action, quiz attempt, task operation, and NLP interpretation
- 💾 **SQL Database Integration** – Persistent task storage using LocalDB
- 📊 **Real‑time Statistics** – Track task completion rates and quiz scores

## 📁 Project Structure

```
SecureIQ_Africa/
│
├── SecureIQChatWindow.xaml          # Main chat window UI
├── SecureIQChatWindow.xaml.cs       # Chat window logic & typing animation
├── Response.cs                       # Chatbot response handling (sentiment, memory, tips)
├── Memory.cs                         # User memory storage (name, favorite topics)
├── ResponseTips.cs                   # Cybersecurity tips database
├── SecureData.cs                     # Cybersecurity keywords & responses storage
│
├── NLPEngine.cs                      # Natural language processing for intent parsing
│
├── Quiz.xaml                         # Quiz window UI
├── Quiz.xaml.cs                      # Quiz logic (questions, scoring, navigation)
├── QuizData.cs                       # Cybersecurity quiz question database
│
├── TaskManagerWindow.xaml            # Task manager UI
├── TaskManagerWindow.xaml.cs         # Task management logic with SQL
│
├── ActivityLog.xaml                  # Activity log viewer UI
├── ActivityLog.xaml.cs               # Activity log display logic
├── ActivityLogService.cs             # Centralised activity logging service
├── ActivityLogExtensions.cs          # Extension methods for specialised logging
│
├── History/                          # Auto‑generated chat history folder
│   └── [username]_chat_history.txt   # Individual chat logs
│
├── SecureIQ.wav                      # Voice greeting sound file
└── icon.png                          # Application icon
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

### Example 2: User shares favourite topic
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

### Example 5: User creates a task
```
User: "remind me to change my passwords"
Bot: [Task added to Task Manager]
```

### Example 6: User requests a quiz
```
User: "quiz me on password security"
Bot: [Opens quiz window filtered by "password" topic]
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
4. If emotion has no topic → Bot asks follow‑up question ("What's wrong?")

### NLP Intent Parsing Flow
1. User types a message
2. `NLPEngine` parses the intent:
   - **Task/Reminder**: "remind me to...", "add a task..."
   - **List Tasks**: "list tasks", "show reminders"
   - **Delete Task**: "delete task #3", "clear all tasks"
   - **Quiz**: "quiz me", "give me a quiz", "test me"
3. System executes the appropriate action (opens Task Manager or Quiz)

### Quiz System Flow
1. User types "quiz me" or similar trigger
2. System loads 10 random questions (filtered by topic if specified)
3. User selects answers via radio buttons
4. System tracks score and provides immediate feedback
5. Final score displayed with a rating (Poor/Good/Excellent/Perfect)
6. All quiz attempts are logged to the Activity Log

### Task Manager Flow
1. User types "remind me to..." or opens the Task Manager
2. Tasks are stored in a SQL Server LocalDB database
3. Users can:
   - Add new tasks
   - Mark tasks as complete/incomplete
   - Delete tasks
4. All operations are logged to the Activity Log
5. Tasks persist between sessions

### Activity Log
- **Centralised logging** via `ActivityLogService` – every significant action is recorded.
- **Real‑time display** – the log viewer shows the 10 most recent entries.
- **Auto‑trimming** – the log keeps at most 100 entries to avoid memory bloat.
- **Specialised logging** – helper methods (`LogTaskAdded`, `LogQuizCompleted`, `LogNlpInteraction`, etc.) simplify logging from different parts of the application.
- **Persistent?** – The log is in‑memory per session; it resets when the application restarts (but you can easily extend it to write to a file).

### Memory & Recall Flow
1. User shares their name → Bot stores it
2. User says "my favorite topic is X" → Bot stores preference
3. User asks "what is my name?" → Bot recalls stored name
4. User asks "what is my favorite topic?" → Bot recalls stored preference

### Chat Logging
- All conversations are automatically saved to `History/chat_history.txt`
- Each message includes a timestamp and sender name

### Exit Application
- Close the window or type "exit" to quit
- Chat history is preserved for the session
- Tasks are automatically saved to the database

## 🎮 Controls

### Chat Window
| Action | Method |
|--------|--------|
| Send message | Click "Send" button OR press Enter key |
| Clear input | Automatically after sending |
| Scroll chat | Mouse wheel or auto‑scroll |
| Exit | Close window or type "exit" |

### Quiz Window
| Action | Method |
|--------|--------|
| Select answer | Click radio button |
| Submit answer | Click "Enter" button |
| Next question | Click "Next" button |
| Previous question | Click "Previous" button |

### Task Manager
| Action | Method |
|--------|--------|
| Add task | Type task + Enter OR click "Create" |
| Complete task | Click checkbox |
| Delete task | Click "✕" button |

### Activity Log Viewer
| Action | Method |
|--------|--------|
| Refresh | Click "🔄 Refresh" button |
| Close | Click "✕ Close" button |

## 🔧 Technical Implementation

| Component | Technology |
|-----------|------------|
| UI Framework | Windows Presentation Foundation (WPF) |
| Programming Language | C# |
| Database | SQL Server LocalDB (SQL Server Express) |
| Animation | Async/await for typing effect |
| Audio | System.Media.SoundPlayer for voice greeting |
| File Handling | StreamWriter / File.AppendAllText for chat logs |
| Message Display | Dynamic StackPanel with Borders for chat bubbles |
| Sentiment Detection | Keyword‑based pattern matching |
| NLP Parsing | Regular expression and keyword‑based intent recognition |
| Memory Storage | In‑memory `Dictionary` with session persistence |
| MVVM | `INotifyPropertyChanged` for data binding |

## 📝 Chat Log Format

```
2025-05-29 14:30:15 - John: What is phishing?
2025-05-29 14:30:17 - Bot: Phishing is a cyber attack where attackers trick you into revealing sensitive information...
2025-05-29 14:30:25 - John: I'm worried about online scams
2025-05-29 14:30:27 - Bot: It's completely understandable to feel worried about phishing. Your concern shows you care about your security!
```

## 📊 Activity Log Format

The Activity Log captures system events with timestamps:

```
2025-05-29 14:30:15 - Parsed: "What is phishing?" -> Intent: None, Sentiment: curious
2025-05-29 14:30:25 - Parsed: "I'm worried about online scams" -> Intent: None, Sentiment: worried
2025-05-29 14:35:00 - Quiz started on topic: 'password'
2025-05-29 14:35:12 - Quiz answer: 'password' was correct for question: 'Which of these is the strongest password?'
2025-05-29 14:36:00 - Quiz completed: 8/10 correct
2025-05-29 14:40:00 - Task added: "Change passwords monthly"
2025-05-29 14:45:00 - Task completed: "Change passwords monthly"
```

## 🎨 UI Features

- **Chat Bubbles**: Left‑aligned (bot) and right‑aligned (user)
- **Timestamps**: Every message shows send time
- **Typing Indicator**: Shows "🤖 is typing..." when bot is "thinking"
- **Auto‑scroll**: Automatically scrolls to newest message
- **Focus Management**: Textbox automatically focused after sending
- **Dark Theme**: Consistent dark colour scheme throughout the application
- **ASCII Art Logo**: Custom SecureIQ branding in all windows
- **Responsive Layouts**: Windows resize gracefully

## ▶️ How to Run

### Requirements
- Visual Studio 2019 / 2022 or later
- .NET Framework 4.7.2 or .NET Core/.NET 5+ with WPF support
- Windows OS (required for WPF and System.Media)
- SQL Server LocalDB (for Task Manager)

### Database Setup
The Task Manager uses SQL Server LocalDB. The database will be created automatically when the application first runs. If needed, you can create it manually:

```sql
CREATE DATABASE TaskChat;
GO
USE TaskChat;
GO
CREATE TABLE [Task] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Remainder NVARCHAR(255),
    IsCompleted BIT NOT NULL DEFAULT 0
);
```

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
https://youtu.be/6o-vuC7huxc
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
- Review the Activity Log for troubleshooting

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| v3.0| June 2026 | Added NLP Engine, Quiz System, Task Manager with SQL, Activity Log |
| v2.0 | May 2025 | WPF GUI version with typing animations, sentiment detection, memory & recall, empathetic responses |
| v1.0 | April 2025 | Console‑based version with speech synthesis |

## 🧪 Supported Cybersecurity Topics

| Topic | Description |
|-------|-------------|
| Password Security | Strong passwords, password managers, passphrases |
| Phishing Detection | Spotting scams, fake emails, suspicious links |
| Malware Protection | Viruses, ransomware, trojans, antivirus |
| WiFi Security | Public WiFi risks, home network protection |
| VPN & Privacy | Virtual Private Networks, data encryption |
| Two‑Factor Authentication | 2FA setup, authenticator apps, security keys |
| Data Backup | 3‑2‑1 backup rule, cloud storage |
| Online Privacy | Personal data protection, social media safety |
| Social Engineering | Manipulation tactics, pretexting, baiting |
| IoT Security | Smart device vulnerabilities, default passwords |

## 📋 NLP Intent Triggers

| Intent | Trigger Phrases |
|--------|-----------------|
| Task Creation | "remind me to", "add a task", "create a task", "I need to" |
| List Tasks | "list tasks", "show tasks", "my tasks", "what are my tasks" |
| Delete Task | "delete task #3", "remove task", "clear all tasks" |
| Quiz | "quiz me", "give me a quiz", "test me", "take a quiz" |
