using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;

namespace PsychologicalTestsApp
{
    public partial class TestsTableWindow : Window
    {
        private DatabaseHelper dbHelper;
        private int editingTestId = -1;
        private List<QuestionDisplay> questions = new List<QuestionDisplay>();
        private List<RangeDisplay> ranges = new List<RangeDisplay>();

        public TestsTableWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadTests();
        }

        private void LoadTests()
        {
            TestsDataGrid.ItemsSource = dbHelper.GetTestsWithThemes();
        }

        private void LoadThemes()
        {
            ThemeComboBox.ItemsSource = dbHelper.GetThemes();
            if (ThemeComboBox.Items.Count > 0)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void AddTestButton_Click(object sender, RoutedEventArgs e)
        {
            AddTestWindow addWindow = new AddTestWindow();
            addWindow.ShowDialog();
            LoadTests();
        }

        private void EditTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestsDataGrid.SelectedItem is TestDisplay test)
            {
                EditTestWindow editWindow = new EditTestWindow(test);
                editWindow.ShowDialog();
                LoadTests(); // Обновление таблицы после редактирования
            }
            else
            {
                MessageBox.Show("Выберите тест для редактирования.");
            }
        }

        private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestsDataGrid.SelectedItem is TestDisplay test)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить тест '{test.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (dbHelper.DeleteTest(test.Id))
                    {
                        MessageBox.Show("Тест успешно удалён.");
                        LoadTests();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении теста. Возможно, он связан с другими данными.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите тест для удаления.");
            }
        }

        private void AddThemeButton_Click(object sender, RoutedEventArgs e)
        {
            NewThemePanel.Visibility = Visibility.Visible;
        }

        private void SaveThemeButton_Click(object sender, RoutedEventArgs e)
        {
            string themeName = NewThemeTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(themeName))
            {
                int themeId = dbHelper.AddTheme(themeName);
                if (themeId != -1)
                {
                    LoadThemes();
                    ThemeComboBox.SelectedItem = themeName;
                    NewThemePanel.Visibility = Visibility.Collapsed;
                    NewThemeTextBox.Text = string.Empty;
                    MessageBox.Show("Тема добавлена успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении темы.");
                }
            }
            else
            {
                MessageBox.Show("Введите название темы.");
            }
        }

        private void CancelThemeButton_Click(object sender, RoutedEventArgs e)
        {
            NewThemePanel.Visibility = Visibility.Collapsed;
            NewThemeTextBox.Text = string.Empty;
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var question = new QuestionDisplay { Text = "Новый вопрос" };
            question.Answers.Add(new AnswerDisplay { Text = "Ответ 1", IsCorrect = false });
            question.Answers.Add(new AnswerDisplay { Text = "Ответ 2", IsCorrect = false });
            questions.Add(question);
            AddQuestionToPanel(question);
        }

        private void AddQuestionToPanel(QuestionDisplay question)
        {
            StackPanel questionPanel = new StackPanel { Margin = new Thickness(0, 5, 0, 5) };
            TextBlock questionText = new TextBlock { Text = question.Text, FontWeight = FontWeights.Bold };
            questionPanel.Children.Add(questionText);
            foreach (var answer in question.Answers)
            {
                CheckBox answerCheckBox = new CheckBox { Content = answer.Text, IsChecked = answer.IsCorrect, Margin = new Thickness(10, 0, 0, 0) };
                questionPanel.Children.Add(answerCheckBox);
            }
            QuestionsPanel.Children.Add(questionPanel);
        }

        private void AddRangeButton_Click(object sender, RoutedEventArgs e)
        {
            var rangePanel = new StackPanel { Margin = new Thickness(0, 5, 0, 5) };
            TextBlock rangeText = new TextBlock { Text = "Новый диапазон" };
            TextBox minScoreBox = new TextBox { Width = 100, Margin = new Thickness(0, 5, 5, 5), Text = "0" };
            TextBox maxScoreBox = new TextBox { Width = 100, Margin = new Thickness(0, 5, 5, 5), Text = "0" };
            TextBox descriptionBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 5, 5), AcceptsReturn = true };
            StackPanel inputPanel = new StackPanel { Orientation = Orientation.Horizontal };
            inputPanel.Children.Add(new TextBlock { Text = "Мин. балл:", Margin = new Thickness(0, 0, 5, 0) });
            inputPanel.Children.Add(minScoreBox);
            inputPanel.Children.Add(new TextBlock { Text = "Макс. балл:", Margin = new Thickness(10, 0, 5, 0) });
            inputPanel.Children.Add(maxScoreBox);
            rangePanel.Children.Add(rangeText);
            rangePanel.Children.Add(inputPanel);
            rangePanel.Children.Add(new TextBlock { Text = "Описание:" });
            rangePanel.Children.Add(descriptionBox);
            ranges.Add(new RangeDisplay
            {
                MinScore = 0,
                MaxScore = 0,
                Description = string.Empty,
                MinScoreBox = minScoreBox,
                MaxScoreBox = maxScoreBox,
                DescriptionBox = descriptionBox
            });
            RangesPanel.Children.Add(rangePanel);
        }

        private void AddRangeToPanel(RangeDisplay range)
        {
            var rangePanel = new StackPanel { Margin = new Thickness(0, 5, 0, 5) };
            TextBlock rangeText = new TextBlock { Text = $"Диапазон: {range.MinScore} - {range.MaxScore}" };
            TextBox minScoreBox = new TextBox { Width = 100, Margin = new Thickness(0, 5, 5, 5), Text = range.MinScore.ToString() };
            TextBox maxScoreBox = new TextBox { Width = 100, Margin = new Thickness(0, 5, 5, 5), Text = range.MaxScore.ToString() };
            TextBox descriptionBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 5, 5), Text = range.Description, AcceptsReturn = true };
            StackPanel inputPanel = new StackPanel { Orientation = Orientation.Horizontal };
            inputPanel.Children.Add(new TextBlock { Text = "Мин. балл:", Margin = new Thickness(0, 0, 5, 0) });
            inputPanel.Children.Add(minScoreBox);
            inputPanel.Children.Add(new TextBlock { Text = "Макс. балл:", Margin = new Thickness(10, 0, 5, 0) });
            inputPanel.Children.Add(maxScoreBox);
            rangePanel.Children.Add(rangeText);
            rangePanel.Children.Add(inputPanel);
            rangePanel.Children.Add(new TextBlock { Text = "Описание:" });
            rangePanel.Children.Add(descriptionBox);
            range.MinScoreBox = minScoreBox;
            range.MaxScoreBox = maxScoreBox;
            range.DescriptionBox = descriptionBox;
            RangesPanel.Children.Add(rangePanel);
        }

        private void SaveTestButton_Click(object sender, RoutedEventArgs e)
        {
            string testName = TestNameTextBox.Text.Trim();
            string themeName = ThemeComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(testName) || string.IsNullOrEmpty(themeName))
            {
                MessageBox.Show("Заполните все обязательные поля.");
                return;
            }

            int themeId = dbHelper.GetThemeIdByName(themeName);
            if (themeId == -1)
            {
                MessageBox.Show("Тема не найдена.");
                return;
            }

            // Валидация диапазонов
            foreach (var range in ranges)
            {
                if (!int.TryParse(range.MinScoreBox.Text, out int minScore) || !int.TryParse(range.MaxScoreBox.Text, out int maxScore))
                {
                    MessageBox.Show("Минимальный и максимальный баллы должны быть числами.");
                    return;
                }
                if (minScore > maxScore)
                {
                    MessageBox.Show("Минимальный балл не может быть больше максимального.");
                    return;
                }
                range.MinScore = minScore;
                range.MaxScore = maxScore;
                range.Description = range.DescriptionBox.Text.Trim();
                if (string.IsNullOrEmpty(range.Description))
                {
                    MessageBox.Show("Описание диапазона не может быть пустым.");
                    return;
                }
            }

            if (editingTestId == -1)
            {
                int testId = dbHelper.AddTest(testName, themeId);
                if (testId != -1)
                {
                    // Сохранение вопросов
                    foreach (var question in questions)
                    {
                        int questionId = dbHelper.AddQuestion(question.Text, testId);
                        if (questionId != -1)
                        {
                            foreach (var answer in question.Answers)
                            {
                                int answerId = dbHelper.AddAnswer(answer.Text, questionId);
                                if (answer.IsCorrect)
                                    dbHelper.AddKey(answerId);
                            }
                        }
                    }
                    // Сохранение диапазонов
                    foreach (var range in ranges)
                    {
                        dbHelper.AddTestResultDescription(testId, range.MinScore, range.MaxScore, range.Description);
                    }
                    MessageBox.Show("Тест добавлен успешно.");
                    ResetForm();
                    LoadTests();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении теста.");
                }
            }
            else
            {
                if (dbHelper.UpdateTest(editingTestId, testName, themeId))
                {
                    // Обновление вопросов и диапазонов (заглушка, нужно реализовать обновление/удаление в БД)
                    MessageBox.Show("Тест обновлен успешно.");
                    ResetForm();
                    LoadTests();
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении теста.");
                }
            }
        }

        private void CancelTestButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            editingTestId = -1;
            TestNameTextBox.Text = string.Empty;
            QuestionsPanel.Children.Clear();
            RangesPanel.Children.Clear();
            questions.Clear();
            ranges.Clear();
            TestFormPanel.Visibility = Visibility.Collapsed;
            if (ThemeComboBox.Items.Count > 0)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}