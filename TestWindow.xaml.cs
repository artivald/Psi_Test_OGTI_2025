using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;

namespace PsychologicalTestsApp
{
    public partial class TestWindow : Window
    {
        private DatabaseHelper dbHelper;
        private string testName;
        private List<Question> questions;
        private Dictionary<int, int> answers;
        private int currentQuestionIndex;

        public TestWindow(string testName)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.testName = testName;
            answers = new Dictionary<int, int>();
            currentQuestionIndex = 0;
            LoadTest();
        }

        private void LoadTest()
        {
            TestNameTextBlock.Text = testName;
            questions = dbHelper.GetQuestionsByTest(testName);

            if (questions.Count == 0)
            {
                MessageBox.Show("Вопросы для теста не найдены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            DisplayCurrentQuestion();
            UpdateButtonStates();
        }

        private void DisplayCurrentQuestion()
        {
            QuestionTextBlock.Text = questions[currentQuestionIndex].Text;
            AnswersPanel.Children.Clear();

            var answers = dbHelper.GetAnswersByQuestion(questions[currentQuestionIndex].Id);
            var groupName = $"question_{questions[currentQuestionIndex].Id}";
            foreach (var answer in answers)
            {
                var radioButton = new RadioButton
                {
                    Content = answer.Text,
                    Tag = answer.Id,
                    GroupName = groupName,
                    Margin = new Thickness(0, 5, 0, 5),
                    FontSize = 14
                };
                if (this.answers.ContainsKey(questions[currentQuestionIndex].Id) &&
                    this.answers[questions[currentQuestionIndex].Id] == answer.Id)
                {
                    radioButton.IsChecked = true;
                }
                AnswersPanel.Children.Add(radioButton);
            }
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRadioButton = AnswersPanel.Children.Cast<UIElement>().OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            if (selectedRadioButton == null)
            {
                MessageBox.Show("Пожалуйста, выберите ответ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int questionId = questions[currentQuestionIndex].Id;
            int answerId = (int)selectedRadioButton.Tag;
            answers[questionId] = answerId;

            if (currentQuestionIndex < questions.Count - 1)
            {
                currentQuestionIndex++;
                DisplayCurrentQuestion();
                UpdateButtonStates();
            }
            else
            {
                FinishTest();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!answers.ContainsKey(questions[currentQuestionIndex].Id))
            {
                MessageBox.Show("Пожалуйста, ответьте на текущий вопрос.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentQuestionIndex < questions.Count - 1)
            {
                currentQuestionIndex++;
                DisplayCurrentQuestion();
                UpdateButtonStates();
            }
            else
            {
                FinishTest();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                DisplayCurrentQuestion();
                UpdateButtonStates();
            }
        }

        private void UpdateButtonStates()
        {
            BackButton.IsEnabled = currentQuestionIndex > 0;
            NextButton.Content = currentQuestionIndex == questions.Count - 1 ? "Завершить" : "Далее";
        }

        private void FinishTest()
        {
            if (answers.Count < questions.Count)
            {
                MessageBox.Show("Пожалуйста, ответьте на все вопросы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(Session.StudentId is int studentId))
            {
                MessageBox.Show("Ошибка: студент не авторизован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int score = CalculateScore();
            int testId = dbHelper.GetTestIdByName(testName);

            if (testId == -1)
            {
                MessageBox.Show("Тест не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dbHelper.SaveTestResult(studentId, testId, score))
            {
                string resultDescription = dbHelper.GetResultDescription(score, testId);
                ResultWindow resultWindow = new ResultWindow(resultDescription);
                resultWindow.ShowDialog();
                Session.Clear();
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении результата.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CalculateScore()
        {
            int score = 0;
            foreach (var answer in answers)
            {
                if (dbHelper.IsCorrectAnswer(answer.Value))
                {
                    score++;
                }
            }
            return score;
        }
    }
}