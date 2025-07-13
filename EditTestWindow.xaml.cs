
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;
using MySql.Data.MySqlClient;

namespace PsychologicalTestsApp
{
    public partial class EditTestWindow : Window
    {
        private DatabaseHelper dbHelper;
        private int editingTestId = -1;
        private List<QuestionDisplay> questions = new List<QuestionDisplay>();
        private List<RangeDisplay> ranges = new List<RangeDisplay>();

        public EditTestWindow(TestDisplay test = null)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadThemes();

            if (test != null)
            {
                editingTestId = test.Id;
                TestNameTextBox.Text = test.Name;
                ThemeComboBox.SelectedItem = test.ThemeName;
                LoadQuestions(test.Id);
                LoadRanges(test.Id);
                Title = "Редактирование теста";
            }
            else
            {
                Title = "Добавление теста";
            }
        }

        private void LoadThemes()
        {
            ThemeComboBox.ItemsSource = dbHelper.GetThemes();
            if (ThemeComboBox.Items.Count > 0)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void LoadQuestions(int testId)
        {
            questions = dbHelper.GetQuestionsByTestId(testId);
            RefreshQuestionsList();
        }

        private void RefreshQuestionsList()
        {
            QuestionsListBox.Items.Clear();
            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                QuestionsListBox.Items.Add(new ListBoxQuestionItem
                {
                    DisplayText = $"Вопрос {i + 1}: {question.Text}",
                    Question = question,
                    QuestionListIndex = i,
                    IsAnswer = false
                });
                for (int j = 0; j < question.Answers.Count; j++)
                {
                    var answer = question.Answers[j];
                    QuestionsListBox.Items.Add(new ListBoxQuestionItem
                    {
                        DisplayText = $"  Ответ {j + 1}: {answer.Text} {(answer.IsCorrect ? "(правильный)" : "")}",
                        Question = question,
                        Answer = answer,
                        QuestionListIndex = i,
                        AnswerListIndex = j,
                        IsAnswer = true
                    });
                }
            }
        }

        private void LoadRanges(int testId)
        {
            ranges = dbHelper.GetRangesByTestId(testId);
            RefreshRangesList();
        }

        private void RefreshRangesList()
        {
            RangesListBox.Items.Clear();
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                RangesListBox.Items.Add(new ListBoxRangeItem
                {
                    DisplayText = $"[{range.MinScore}-{range.MaxScore}] {range.Description}",
                    Range = range,
                    RangeListIndex = i
                });
            }
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            string questionText = QuestionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(questionText))
            {
                MessageBox.Show("Введите текст вопроса.");
                return;
            }

            var question = new QuestionDisplay { Text = questionText };
            questions.Add(question);
            RefreshQuestionsList();

            string answerText;
            bool isCorrect;
            do
            {
                var answerDialog = new AnswerEditDialog();
                if (answerDialog.ShowDialog() != true)
                    break;

                answerText = answerDialog.AnswerText.Trim();
                isCorrect = answerDialog.IsCorrect;

                if (isCorrect && question.Answers.Exists(a => a.IsCorrect))
                {
                    MessageBox.Show("В вопросе уже есть правильный ответ. Выберите другой или отредактируйте существующий.");
                    continue;
                }

                if (isCorrect)
                {
                    foreach (var answer in question.Answers)
                        answer.IsCorrect = false;
                }

                question.Answers.Add(new AnswerDisplay { Text = answerText, IsCorrect = isCorrect });
                RefreshQuestionsList();
            } while (MessageBox.Show("Добавить ещё ответ?", "Продолжить", MessageBoxButton.YesNo) == MessageBoxResult.Yes);

            QuestionTextBox.Clear();
        }

        private void AddAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите вопрос, к которому добавить ответ.");
                return;
            }

            var selectedItem = QuestionsListBox.SelectedItem as ListBoxQuestionItem;
            if (selectedItem == null || selectedItem.IsAnswer)
            {
                MessageBox.Show("Выберите строку с вопросом (начинается с 'Вопрос').");
                return;
            }

            var question = selectedItem.Question;
            var answerDialog = new AnswerEditDialog();
            if (answerDialog.ShowDialog() == true)
            {
                if (answerDialog.IsCorrect && question.Answers.Exists(a => a.IsCorrect))
                {
                    MessageBox.Show("В вопросе уже есть правильный ответ. Выберите другой или отредактируйте существующий.");
                    return;
                }

                if (answerDialog.IsCorrect)
                {
                    foreach (var answer in question.Answers)
                        answer.IsCorrect = false;
                }

                question.Answers.Add(new AnswerDisplay
                {
                    Text = answerDialog.AnswerText.Trim(),
                    IsCorrect = answerDialog.IsCorrect
                });
                RefreshQuestionsList();
            }
        }

        private void EditQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("EditQuestionButton_Click: Button clicked");
            if (QuestionsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите вопрос для редактирования.");
                System.Diagnostics.Debug.WriteLine("EditQuestionButton_Click: SelectedItem is null");
                return;
            }

            var selectedItem = QuestionsListBox.SelectedItem as ListBoxQuestionItem;
            if (selectedItem == null || selectedItem.IsAnswer)
            {
                MessageBox.Show("Выберите строку с вопросом (начинается с 'Вопрос').");
                return;
            }

            var question = selectedItem.Question;
            string newText = Prompt("Введите новый текст вопроса:", question.Text);
            if (!string.IsNullOrEmpty(newText))
            {
                question.Text = newText.Trim();
                RefreshQuestionsList();
            }
        }

        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DeleteQuestionButton_Click: Button clicked");
            if (QuestionsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите вопрос для удаления.");
                System.Diagnostics.Debug.WriteLine("DeleteQuestionButton_Click: SelectedItem is null");
                return;
            }

            var selectedItem = QuestionsListBox.SelectedItem as ListBoxQuestionItem;
            if (selectedItem == null || selectedItem.IsAnswer)
            {
                MessageBox.Show("Выберите строку с вопросом (начинается с 'Вопрос').");
                return;
            }

            var question = selectedItem.Question;
            if (MessageBox.Show($"Удалить вопрос '{question.Text}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                questions.RemoveAt(selectedItem.QuestionListIndex);
                RefreshQuestionsList();
            }
        }

        private void EditAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("EditAnswerButton_Click: Button clicked");
            if (QuestionsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите ответ для редактирования.");
                System.Diagnostics.Debug.WriteLine("EditAnswerButton_Click: SelectedItem is null");
                return;
            }

            var selectedItem = QuestionsListBox.SelectedItem as ListBoxQuestionItem;
            if (selectedItem == null || !selectedItem.IsAnswer)
            {
                MessageBox.Show("Выберите строку с ответом (начинается с 'Ответ').");
                return;
            }

            var question = selectedItem.Question;
            var answer = selectedItem.Answer;
            var answerDialog = new AnswerEditDialog(answer.Text, answer.IsCorrect);
            if (answerDialog.ShowDialog() == true)
            {
                if (answerDialog.IsCorrect && question.Answers.Exists(a => a.IsCorrect && a != answer))
                {
                    MessageBox.Show("В вопросе уже есть правильный ответ. Сначала снимите отметку с другого ответа.");
                    return;
                }

                if (answerDialog.IsCorrect)
                {
                    foreach (var a in question.Answers)
                        a.IsCorrect = false;
                }

                answer.Text = answerDialog.AnswerText.Trim();
                answer.IsCorrect = answerDialog.IsCorrect;
                RefreshQuestionsList();
            }
        }

        private void DeleteAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DeleteAnswerButton_Click: Button clicked");
            if (QuestionsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите ответ для удаления.");
                System.Diagnostics.Debug.WriteLine("DeleteAnswerButton_Click: SelectedItem is null");
                return;
            }

            var selectedItem = QuestionsListBox.SelectedItem as ListBoxQuestionItem;
            if (selectedItem == null || !selectedItem.IsAnswer)
            {
                MessageBox.Show("Выберите строку с ответом (начинается с 'Ответ').");
                return;
            }

            var question = selectedItem.Question;
            var answer = selectedItem.Answer;
            if (MessageBox.Show($"Удалить ответ '{answer.Text}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                question.Answers.RemoveAt(selectedItem.AnswerListIndex);
                RefreshQuestionsList();
            }
        }

        private void AddRangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MinScoreTextBox.Text, out int minScore) ||
                !int.TryParse(MaxScoreTextBox.Text, out int maxScore) ||
                string.IsNullOrEmpty(DescriptionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля диапазона корректно.");
                return;
            }

            var range = new RangeDisplay
            {
                MinScore = minScore,
                MaxScore = maxScore,
                Description = DescriptionTextBox.Text.Trim()
            };
            ranges.Add(range);
            RefreshRangesList();

            MinScoreTextBox.Text = "0";
            MaxScoreTextBox.Text = "0";
            DescriptionTextBox.Clear();
        }

        private void EditRangeButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("EditRangeButton_Click: Button clicked");
            if (RangesListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите диапазон для редактирования.");
                System.Diagnostics.Debug.WriteLine("EditRangeButton_Click: SelectedItem is null");
                return;
            }

            var selectedItem = RangesListBox.SelectedItem as ListBoxRangeItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Недопустимый выбор диапазона.");
                System.Diagnostics.Debug.WriteLine("EditRangeButton_Click: Invalid item type");
                return;
            }

            var range = selectedItem.Range;
            System.Diagnostics.Debug.WriteLine($"EditRangeButton_Click: Opening RangeEditDialog for range [{range.MinScore}-{range.MaxScore}]");
            var dialog = new RangeEditDialog(range.MinScore, range.MaxScore, range.Description);
            if (dialog.ShowDialog() == true)
            {
                System.Diagnostics.Debug.WriteLine($"EditRangeButton_Click: Dialog saved with MinScore={dialog.MinScore}, MaxScore={dialog.MaxScore}, Description={dialog.Description}");
                range.MinScore = dialog.MinScore;
                range.MaxScore = dialog.MaxScore;
                range.Description = dialog.Description.Trim();
                RefreshRangesList();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("EditRangeButton_Click: Dialog cancelled");
            }
        }

        private void DeleteRangeButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DeleteRangeButton_Click: Button clicked");
            if (RangesListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите диапазон для удаления.");
                System.Diagnostics.Debug.WriteLine("DeleteRangeButton_Click: SelectedItem is null");
                return;
            }

            var selectedItem = RangesListBox.SelectedItem as ListBoxRangeItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Недопустимый выбор диапазона.");
                System.Diagnostics.Debug.WriteLine("DeleteRangeButton_Click: Invalid item type");
                return;
            }

            var range = selectedItem.Range;
            if (MessageBox.Show($"Удалить диапазон [{range.MinScore}-{range.MaxScore}]?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteRangeButton_Click: Deleting range [{range.MinScore}-{range.MaxScore}]");
                ranges.RemoveAt(selectedItem.RangeListIndex);
                RefreshRangesList();
            }
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
                MessageBox.Show("Выбранная тема не найдена.");
                return;
            }

            if (editingTestId == -1)
            {
                int testId = dbHelper.AddTest(testName, themeId);
                if (testId == -1)
                {
                    MessageBox.Show("Ошибка при добавлении теста.");
                    return;
                }

                SaveQuestionsAndRanges(testId);
                MessageBox.Show("Тест добавлен успешно.");
            }
            else
            {
                if (dbHelper.UpdateTest(editingTestId, testName, themeId))
                {
                    SaveQuestionsAndRanges(editingTestId);
                    MessageBox.Show("Тест обновлён успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении теста.");
                    return;
                }
            }

            DialogResult = true;
            Close();
        }

        private void SaveQuestionsAndRanges(int testId)
        {
            dbHelper.DeleteQuestionsByTestId(testId);
            dbHelper.DeleteRangesByTestId(testId);

            foreach (var question in questions)
            {
                int questionId = dbHelper.AddQuestion(question.Text, testId);
                if (questionId != -1)
                {
                    foreach (var answer in question.Answers)
                    {
                        int answerId = dbHelper.AddAnswer(answer.Text, questionId);
                        if (answerId != -1 && answer.IsCorrect)
                        {
                            dbHelper.AddKey(answerId);
                        }
                    }
                }
            }

            foreach (var range in ranges)
            {
                dbHelper.AddTestResultDescription(testId, range.MinScore, range.MaxScore, range.Description);
            }
        }

        private void CancelTestButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private string Prompt(string message, string defaultText = "")
        {
            var inputBox = new Window
            {
                Title = "Ввод",
                Width = 400,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            stackPanel.Children.Add(new TextBlock { Text = message, Margin = new Thickness(0, 0, 0, 10) });
            var textBox = new TextBox { Width = 350, Text = defaultText };
            stackPanel.Children.Add(textBox);
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };
            var okButton = new Button { Content = "OK", Width = 80, Margin = new Thickness(0, 0, 10, 0) };
            var cancelButton = new Button { Content = "Отмена", Width = 80 };
            okButton.Click += (s, args) => { inputBox.DialogResult = true; inputBox.Close(); };
            cancelButton.Click += (s, args) => { inputBox.DialogResult = false; inputBox.Close(); };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonPanel);
            inputBox.Content = stackPanel;
            return inputBox.ShowDialog() == true ? textBox.Text : null;
        }

        private class ListBoxQuestionItem
        {
            public string DisplayText { get; set; }
            public QuestionDisplay Question { get; set; }
            public AnswerDisplay Answer { get; set; }
            public int QuestionListIndex { get; set; }
            public int AnswerListIndex { get; set; }
            public bool IsAnswer { get; set; }
        }

        private class ListBoxRangeItem
        {
            public string DisplayText { get; set; }
            public RangeDisplay Range { get; set; }
            public int RangeListIndex { get; set; }
        }

        private class AnswerEditDialog : Window
        {
            public string AnswerText { get; private set; }
            public bool IsCorrect { get; private set; }

            public AnswerEditDialog(string text = "", bool isCorrect = false)
            {
                Title = "Редактирование ответа";
                Width = 400;
                Height = 200;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var stackPanel = new StackPanel { Margin = new Thickness(10) };
                stackPanel.Children.Add(new TextBlock { Text = "Текст ответа:", Margin = new Thickness(0, 0, 0, 5) });
                var textBox = new TextBox { Text = text, Width = 350 };
                stackPanel.Children.Add(textBox);
                var correctCheckBox = new CheckBox { Content = "Правильный ответ (даёт 1 балл)", IsChecked = isCorrect, Margin = new Thickness(0, 10, 0, 0) };
                stackPanel.Children.Add(correctCheckBox);

                var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };
                var okButton = new Button { Content = "Сохранить", Width = 80, Margin = new Thickness(0, 0, 10, 0) };
                var cancelButton = new Button { Content = "Отмена", Width = 80 };
                okButton.Click += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        AnswerText = textBox.Text;
                        IsCorrect = correctCheckBox.IsChecked == true;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Введите текст ответа.");
                    }
                };
                cancelButton.Click += (s, args) => { DialogResult = false; Close(); };
                buttonPanel.Children.Add(okButton);
                buttonPanel.Children.Add(cancelButton);
                stackPanel.Children.Add(buttonPanel);

                Content = stackPanel;
            }
        }

        private class RangeEditDialog : Window
        {
            public int MinScore { get; private set; }
            public int MaxScore { get; private set; }
            public string Description { get; private set; }

            public RangeEditDialog(int minScore, int maxScore, string description)
            {
                Title = "Редактирование диапазона";
                Width = 400;
                Height = 275;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var stackPanel = new StackPanel { Margin = new Thickness(10) };
                stackPanel.Children.Add(new TextBlock { Text = "Минимальный балл:", Margin = new Thickness(0, 0, 0, 5) });
                var minScoreBox = new TextBox { Text = minScore.ToString(), Width = 350, Margin = new Thickness(0, 0, 0, 10) };
                stackPanel.Children.Add(minScoreBox);
                stackPanel.Children.Add(new TextBlock { Text = "Максимальный балл:", Margin = new Thickness(0, 0, 0, 5) });
                var maxScoreBox = new TextBox { Text = maxScore.ToString(), Width = 350, Margin = new Thickness(0, 0, 0, 10) };
                stackPanel.Children.Add(maxScoreBox);
                stackPanel.Children.Add(new TextBlock { Text = "Описание:", Margin = new Thickness(0, 0, 0, 5) });
                var descriptionBox = new TextBox { Text = description, Width = 350, Height = 50, AcceptsReturn = true, Margin = new Thickness(0, 0, 0, 10) };
                stackPanel.Children.Add(descriptionBox);

                var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };
                var saveButton = new Button { Content = "Сохранить", Width = 80, Margin = new Thickness(0, 0, 10, 0) };
                var cancelButton = new Button { Content = "Отмена", Width = 80 };
                saveButton.Click += (s, args) =>
                {
                    if (int.TryParse(minScoreBox.Text, out int min) && int.TryParse(maxScoreBox.Text, out int max) && !string.IsNullOrEmpty(descriptionBox.Text))
                    {
                        MinScore = min;
                        MaxScore = max;
                        Description = descriptionBox.Text;
                        DialogResult = true;
                        Close();
                        System.Diagnostics.Debug.WriteLine("RangeEditDialog: Save button clicked, dialog closed with success");
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля корректно.");
                        System.Diagnostics.Debug.WriteLine("RangeEditDialog: Save button clicked, invalid input");
                    }
                };
                cancelButton.Click += (s, args) =>
                {
                    DialogResult = false;
                    Close();
                    System.Diagnostics.Debug.WriteLine("RangeEditDialog: Cancel button clicked, dialog closed");
                };
                buttonPanel.Children.Add(saveButton);
                buttonPanel.Children.Add(cancelButton);
                stackPanel.Children.Add(buttonPanel);

                Content = stackPanel;
            }
        }
    }
}
