using System;
using System.Collections.Generic;
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
        private Dictionary<int, int> answers; // questionId -> answerId

        public TestWindow(string testName)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.testName = testName;
            answers = new Dictionary<int, int>();
            LoadTest();
        }

        private void LoadTest()
        {
            TestNameTextBlock.Text = testName;
            questions = dbHelper.GetQuestionsByTest(testName);

            foreach (var question in questions)
            {
                // Добавляем вопрос
                var questionText = new TextBlock
                {
                    Text = question.Text,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5)
                };
                QuestionsPanel.Children.Add(questionText);

                // Получаем варианты ответов
                var answers = dbHelper.GetAnswersByQuestion(question.Id);

                // Добавляем варианты ответов как RadioButton
                var answersStack = new StackPanel { Margin = new Thickness(20, 0, 0, 10) };
                foreach (var answer in answers)
                {
                    var radioButton = new RadioButton
                    {
                        Content = answer.Text,
                        Tag = answer.Id,
                        GroupName = $"question_{question.Id}",
                        Margin = new Thickness(0, 2, 0, 2)
                    };
                    radioButton.Checked += (s, e) =>
                    {
                        this.answers[question.Id] = answer.Id;
                    };
                    answersStack.Children.Add(radioButton);
                }
                QuestionsPanel.Children.Add(answersStack);
            }
        }

        private void FinishTestButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что на все вопросы даны ответы
            if (answers.Count < questions.Count)
            {
                MessageBox.Show("Пожалуйста, ответьте на все вопросы.");
                return;
            }

            // Рассчитываем результат
            int score = CalculateScore();

            // Сохраняем результат
            int studentId = 1; // Здесь нужно получить ID текущего студента
            int testId = dbHelper.GetTestIdByName(testName);

            if (dbHelper.SaveTestResult(studentId, testId, score))
            {
                // Получаем описание результата
                string resultDescription = GetResultDescription(score);

                // Открываем окно с результатом
                ResultWindow resultWindow = new ResultWindow(resultDescription);
                resultWindow.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении результата.");
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

        private string GetResultDescription(int score)
        {
            return dbHelper.GetResultDescription(score);
        }
    }
}
