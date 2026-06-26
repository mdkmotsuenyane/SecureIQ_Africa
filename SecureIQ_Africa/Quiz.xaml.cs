using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SecureIQ_Africa
{
    public partial class Quiz : Window
    {
        private QuizData quizData = new QuizData();
        private List<QuestionItem> questions;
        private int currentIndex = 0;
        private int score = 0;
        private RadioButton selectedOption;
        private List<AnswerState> answerStates;
        private string _topicFilter = null;

        private class QuestionItem
        {
            public string QuestionText { get; set; }
            public string CorrectAnswer { get; set; }
            public List<string> Options { get; set; }
        }

        private class AnswerState
        {
            public string SelectedOption { get; set; }
            public bool IsAnswered { get; set; }
            public bool IsCorrect { get; set; }
        }

        public Quiz(string topicFilter = null)
        {
            InitializeComponent();
            _topicFilter = topicFilter;
            LoadQuestions();

            // Log quiz start using ActivityLogService
            if (!string.IsNullOrEmpty(_topicFilter))
                ActivityLogService.AddEntry($"Quiz started on topic: '{_topicFilter}'");
            else
                ActivityLogService.AddEntry("Quiz started");

            ShowQuestion(currentIndex);
            UpdateScoreAndProgress();
            UpdateNavigationButtons();
        }

        private void LoadQuestions()
        {
            var allQuestions = quizData.Questions;

            var excludedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "greeting", "howareyou", "whatIsYourName", "whatCanYouDo",
                "purpose", "help"
            };

            var genericPhrases = new List<string>
            {
                "i can answer", "hello", "hi", "how are you", "what is your name",
                "my name is", "i am a", "can you help", "help me", "what can you do",
                "how can i help", "good morning", "good afternoon", "good evening",
                "nice to meet", "thanks", "thank you", "secureiq africa chatbot",
                "my purpose is to educate"
            };

            var filtered = allQuestions
                .Where(q => !excludedKeys.Contains(q.Topic))
                .Where(q => !genericPhrases.Any(phrase =>
                    q.QuestionText.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();

            if (!string.IsNullOrEmpty(_topicFilter))
            {
                filtered = filtered.Where(q => q.Topic.Equals(_topicFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (filtered.Count < 3)
            {
                filtered = allQuestions
                    .Where(q => !excludedKeys.Contains(q.Topic))
                    .Where(q => !genericPhrases.Any(phrase =>
                        q.QuestionText.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            }

            var selected = filtered.OrderBy(x => Guid.NewGuid()).Take(10).ToList();

            questions = new List<QuestionItem>();
            answerStates = new List<AnswerState>();

            var allTopics = quizData.Questions
                .Where(q => !excludedKeys.Contains(q.Topic))
                .Select(q => q.Topic)
                .ToArray();

            var rnd = new Random();

            foreach (var q in selected)
            {
                var wrongs = allTopics
                    .Where(t => !t.Equals(q.Topic, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => rnd.Next())
                    .Take(3)
                    .ToList();

                var options = new List<string> { q.Topic };
                options.AddRange(wrongs);
                options = options.OrderBy(x => rnd.Next()).ToList();

                questions.Add(new QuestionItem
                {
                    QuestionText = q.QuestionText,
                    CorrectAnswer = q.Topic,
                    Options = options
                });

                answerStates.Add(new AnswerState());
            }
        }

        private void ShowQuestion(int index)
        {
            if (index >= questions.Count)
            {
                ShowFinalScore();
                return;
            }

            var q = questions[index];
            QuestionTextBlock.Text = q.QuestionText;

            OptionsPanel.Children.Clear();
            OptionsPanel.Visibility = Visibility.Visible;
            selectedOption = null;

            foreach (var opt in q.Options)
            {
                var rb = new RadioButton
                {
                    Content = opt,
                    FontSize = 18,
                    Foreground = System.Windows.Media.Brushes.White,
                    Margin = new Thickness(0, 5, 0, 5),
                    GroupName = "OptionsGroup"
                };
                rb.Checked += (s, e) => selectedOption = rb;
                OptionsPanel.Children.Add(rb);
            }

            var state = answerStates[index];
            if (state.IsAnswered)
            {
                foreach (RadioButton rb in OptionsPanel.Children)
                {
                    if (rb.Content.ToString() == state.SelectedOption)
                    {
                        rb.IsChecked = true;
                        selectedOption = rb;
                        break;
                    }
                }

                FeedbackTextBlock.Text = state.IsCorrect ? "Correct!" : $"Incorrect. The correct keyword is: {q.CorrectAnswer}";
                FeedbackTextBlock.Foreground = state.IsCorrect ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.OrangeRed;
                FooterNextButton.Visibility = Visibility.Visible;
                EnterButton.IsEnabled = false;

                foreach (RadioButton rb in OptionsPanel.Children)
                {
                    rb.Checked += (s, e) =>
                    {
                        // Only enable Enter if the newly selected option is different from the stored one
                        if (rb.Content.ToString() != state.SelectedOption)
                            EnterButton.IsEnabled = true;
                    };
                }
            }
            else
            {
                FeedbackTextBlock.Text = "";
                FooterNextButton.Visibility = Visibility.Collapsed;
                EnterButton.IsEnabled = true;
            }

            UpdateScoreAndProgress();
            UpdateNavigationButtons();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            var state = answerStates[currentIndex];
            string userAnswer = GetUserAnswer();

            if (string.IsNullOrWhiteSpace(userAnswer))
            {
                FeedbackTextBlock.Text = "Please select an answer.";
                FeedbackTextBlock.Foreground = System.Windows.Media.Brushes.Yellow;
                return;
            }

            var q = questions[currentIndex];
            bool isCorrect = string.Equals(userAnswer.Trim(), q.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            // Prevent score changes if the same answer is re-submitted
            if (state.IsAnswered && string.Equals(state.SelectedOption, userAnswer.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                // No change – just show feedback again
                FeedbackTextBlock.Text = state.IsCorrect ? "Correct!" : $"Incorrect. The correct keyword is: {q.CorrectAnswer}";
                FeedbackTextBlock.Foreground = state.IsCorrect ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.OrangeRed;
                return;
            }

            // Adjust score if already answered
            if (state.IsAnswered)
            {
                if (state.IsCorrect)
                    score--;
                if (isCorrect)
                    score++;
            }
            else
            {
                if (isCorrect)
                    score++;
                state.IsAnswered = true;
            }

            state.SelectedOption = userAnswer.Trim();
            state.IsCorrect = isCorrect;

            FeedbackTextBlock.Text = isCorrect ? "Correct!" : $"Incorrect. The correct keyword is: {q.CorrectAnswer}";
            FeedbackTextBlock.Foreground = isCorrect ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.OrangeRed;

            // Log the answer attempt using ActivityLogService
            string result = isCorrect ? "correct" : "incorrect";
            ActivityLogService.AddEntry($"Quiz answer: '{userAnswer.Trim()}' was {result} for question: '{q.QuestionText}'");

            UpdateScoreAndProgress();
            FooterNextButton.Visibility = Visibility.Visible;
            EnterButton.IsEnabled = false;
            UpdateNavigationButtons();
        }

        private string GetUserAnswer()
        {
            return selectedOption?.Content.ToString();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;
            if (currentIndex < questions.Count)
                ShowQuestion(currentIndex);
            else
                ShowFinalScore();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                ShowQuestion(currentIndex);
            }
        }

        private void ShowFinalScore()
        {
            string rating;
            if (score < 5)
                rating = "Poor";
            else if (score <= 7)
                rating = "Good";
            else if (score <= 9)
                rating = "Excellent";
            else
                rating = "Perfect";

            QuestionTextBlock.Text = $"Quiz complete! Your score: {score} out of {questions.Count} – {rating}";
            OptionsPanel.Visibility = Visibility.Collapsed;
            FeedbackTextBlock.Text = "";
            FooterNextButton.Visibility = Visibility.Collapsed;
            EnterButton.Visibility = Visibility.Collapsed;
            PreviousButton.Visibility = Visibility.Collapsed;
            ScoreTextBlock.Text = $"Final Score: {score}";
            ProgressTextBlock.Text = "Finished";

            // Log the quiz completion using ActivityLogExtensions 
            ActivityLogExtensions.LogQuizCompleted(score, questions.Count);
        }

        private void UpdateScoreAndProgress()
        {
            ScoreTextBlock.Text = $"Score: {score}";
            ProgressTextBlock.Text = $"Question {currentIndex + 1} of {questions.Count}";
        }

        private void UpdateNavigationButtons()
        {
            PreviousButton.IsEnabled = currentIndex > 0;
        }
    }
}