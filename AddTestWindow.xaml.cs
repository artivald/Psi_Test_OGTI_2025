using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PsychologicalTestsApp
{
    public partial class AddTestWindow : Window
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly List<QuestionInput> _questions;
        private readonly List<RangeInput> _ranges;

        public AddTestWindow()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _questions = new List<QuestionInput>();
            _ranges = new List<RangeInput>();
            LoadThemes();
        }

        private void LoadThemes()
        {
            var themes = _dbHelper.GetThemes();
            ThemeComboBox.ItemsSource = themes;
            if (themes.Count > 0)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void AddThemeButton_Click(object sender, RoutedEventArgs e)
        {
            NewThemePanel.Visibility = Visibility.Visible;
            AddThemeButton.IsEnabled = false;
            NewThemeTextBox.Focus();
        }

        private void SaveThemeButton_Click(object sender, RoutedEventArgs e)
        {
            var themeName = NewThemeTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(themeName))
            {
                MessageBox.Show("Введите название темы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_dbHelper.GetThemeIdByName(themeName) != -1)
            {
                MessageBox.Show("Тема с таким названием уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var themeId = _dbHelper.AddTheme(themeName);
            if (themeId == -1)
            {
                MessageBox.Show("Не удалось добавить тему.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadThemes();
            ThemeComboBox.SelectedItem = themeName;
            NewThemePanel.Visibility = Visibility.Collapsed;
            AddThemeButton.IsEnabled = true;
            NewThemeTextBox.Text = "";
        }

        private void CancelThemeButton_Click(object sender, RoutedEventArgs e)
        {
            NewThemePanel.Visibility = Visibility.Collapsed;
            AddThemeButton.IsEnabled = true;
            NewThemeTextBox.Text = "";
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var questionIndex = _questions.Count + 1;
            var questionPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 15) };
            var questionTextBox = new TextBox { Text = $"Вопрос {questionIndex}" };
            questionPanel.Children.Add(questionTextBox);

            var answersPanel = new StackPanel { Margin = new Thickness(20, 5, 0, 5) };
            var answers = new List<TextBox>();
            var radios = new List<RadioButton>();
            var answerPanels = new List<StackPanel>();

            for (int i = 0; i < 2; i++)
                AddAnswer(answersPanel.Children, answers, radios, answerPanels, answers.Count + 1, questionIndex - 1);

            var addAnswerButton = new Button
            {
                Content = "Добавить ответ",
                Style = FindResource("ActionButton") as Style,
                Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                Margin = new Thickness(0, 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            addAnswerButton.Click += (s, ev) =>
            {
                if (answers.Count >= 10)
                {
                    MessageBox.Show("Максимальное количество ответов: 10.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                AddAnswer(answersPanel.Children, answers, radios, answerPanels, answers.Count + 1, questionIndex - 1);
            };

            questionPanel.Children.Add(answersPanel);
            questionPanel.Children.Add(addAnswerButton);
            QuestionsPanel.Children.Add(questionPanel);
            _questions.Add(new QuestionInput
            {
                QuestionTextBox = questionTextBox,
                AnswerTextBoxes = answers,
                CorrectAnswerRadios = radios,
                AnswerPanels = answerPanels
            });
        }

        private void AddAnswer(UIElementCollection answersPanelChildren, List<TextBox> answers, List<RadioButton> radios, List<StackPanel> answerPanels, int answerIndex, int questionIndex)
        {
            var answerPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 5) };
            var radio = new RadioButton
            {
                GroupName = $"Question_{questionIndex}",
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            if (!radios.Any(r => r.IsChecked == true)) radio.IsChecked = true;
            var answerTextBox = new TextBox { Width = 300, Text = $"Ответ {answerIndex}" };
            var deleteButton = new Button
            {
                Content = "Удалить",
                Style = FindResource("ActionButton") as Style,
                Background = new SolidColorBrush(Colors.Red),
                Width = 80,
                Visibility = answers.Count >= 2 ? Visibility.Visible : Visibility.Collapsed
            };

            deleteButton.Click += (s, e) =>
            {
                if (answers.Count <= 2)
                {
                    MessageBox.Show("Вопрос должен содержать минимум два ответа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                answersPanelChildren.Remove(answerPanel);
                answers.Remove(answerTextBox);
                radios.Remove(radio);
                answerPanels.Remove(answerPanel);

                if (radio.IsChecked == true && radios.Any())
                    radios[0].IsChecked = true;

                UpdateDeleteButtonsVisibility(answerPanels);

                // Обновление нумерации ответов
                for (int i = 0; i < answers.Count; i++)
                    answers[i].Text = $"Ответ {i + 1}";
            };

            answerPanel.Children.Add(radio);
            answerPanel.Children.Add(answerTextBox);
            answerPanel.Children.Add(deleteButton);
            answersPanelChildren.Add(answerPanel);
            answers.Add(answerTextBox);
            radios.Add(radio);
            answerPanels.Add(answerPanel);

            UpdateDeleteButtonsVisibility(answerPanels);
        }

        private void UpdateDeleteButtonsVisibility(List<StackPanel> answerPanels)
        {
            var visible = answerPanels.Count > 2;
            foreach (var panel in answerPanels)
            {
                var button = panel.Children.Cast<UIElement>().OfType<Button>().FirstOrDefault();
                if (button != null)
                    button.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AddRangeButton_Click(object sender, RoutedEventArgs e)
        {
            var rangePanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            var minScoreTextBox = new TextBox { Width = 60, Text = "0" };
            var maxScoreTextBox = new TextBox { Width = 60, Margin = new Thickness(10, 0, 0, 0), Text = "0" };
            var descriptionTextBox = new TextBox { Width = 350, Margin = new Thickness(10, 0, 0, 0), Text = "Описание результата" };

            rangePanel.Children.Add(new TextBlock { Text = "От ", VerticalAlignment = VerticalAlignment.Center });
            rangePanel.Children.Add(minScoreTextBox);
            rangePanel.Children.Add(new TextBlock { Text = " до ", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5, 0, 0, 0) });
            rangePanel.Children.Add(maxScoreTextBox);
            rangePanel.Children.Add(descriptionTextBox);

            RangesPanel.Children.Add(rangePanel);
            _ranges.Add(new RangeInput
            {
                MinScoreTextBox = minScoreTextBox,
                MaxScoreTextBox = maxScoreTextBox,
                DescriptionTextBox = descriptionTextBox
            });
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var testName = TestNameTextBox.Text.Trim();
            var themeName = ThemeComboBox.SelectedItem?.ToString();

            // Валидация
            if (string.IsNullOrWhiteSpace(testName) || string.IsNullOrWhiteSpace(themeName))
            {
                MessageBox.Show("Введите название теста и выберите тему.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_questions.Any())
            {
                MessageBox.Show("Добавьте хотя бы один вопрос.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var q in _questions)
            {
                if (string.IsNullOrWhiteSpace(q.QuestionTextBox.Text) || q.AnswerTextBoxes.Any(a => string.IsNullOrWhiteSpace(a.Text)))
                {
                    MessageBox.Show("Заполните все вопросы и ответы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (q.AnswerTextBoxes.Count < 2)
                {
                    MessageBox.Show("Каждый вопрос должен содержать минимум два ответа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!q.CorrectAnswerRadios.Any(r => r.IsChecked == true))
                {
                    MessageBox.Show("Выберите правильный ответ для каждого вопроса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (!_ranges.Any())
            {
                MessageBox.Show("Добавьте хотя бы один диапазон результатов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var r in _ranges)
            {
                if (!int.TryParse(r.MinScoreTextBox.Text, out int minScore) || !int.TryParse(r.MaxScoreTextBox.Text, out int maxScore) || string.IsNullOrWhiteSpace(r.DescriptionTextBox.Text))
                {
                    MessageBox.Show("Введите корректные значения для диапазонов и описания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (minScore > maxScore)
                {
                    MessageBox.Show("Минимальный балл не может быть больше максимального.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            for (int i = 0; i < _ranges.Count; i++)
            {
                int min1 = int.Parse(_ranges[i].MinScoreTextBox.Text);
                int max1 = int.Parse(_ranges[i].MaxScoreTextBox.Text);
                for (int j = i + 1; j < _ranges.Count; j++)
                {
                    int min2 = int.Parse(_ranges[j].MinScoreTextBox.Text);
                    int max2 = int.Parse(_ranges[j].MaxScoreTextBox.Text);
                    if (min1 <= max2 && min2 <= max1)
                    {
                        MessageBox.Show("Диапазоны результатов не должны пересекаться.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            // Сохранение
            var themeId = _dbHelper.GetThemeIdByName(themeName);
            if (themeId == -1)
            {
                MessageBox.Show("Выбранная тема не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var testId = _dbHelper.AddTest(testName, themeId);
            if (testId == -1)
            {
                MessageBox.Show("Не удалось добавить тест.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var q in _questions)
            {
                var questionId = _dbHelper.AddQuestion(q.QuestionTextBox.Text, testId);
                if (questionId == -1)
                {
                    MessageBox.Show("Не удалось добавить вопрос.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var correctAnswerIndex = q.CorrectAnswerRadios.FindIndex(r => r.IsChecked == true);
                for (int i = 0; i < q.AnswerTextBoxes.Count; i++)
                {
                    var answerId = _dbHelper.AddAnswer(q.AnswerTextBoxes[i].Text, questionId);
                    if (answerId == -1)
                    {
                        MessageBox.Show("Не удалось добавить ответ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (i == correctAnswerIndex)
                    {
                        if (!_dbHelper.AddKey(answerId))
                        {
                            MessageBox.Show($"Не удалось добавить ключ для ответа ID {answerId}. Возможно, ответ не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }

            foreach (var r in _ranges)
            {
                var minScore = int.Parse(r.MinScoreTextBox.Text);
                var maxScore = int.Parse(r.MaxScoreTextBox.Text);
                var description = r.DescriptionTextBox.Text;
                if (!_dbHelper.AddTestResultDescription(testId, minScore, maxScore, description))
                {
                    MessageBox.Show("Не удалось добавить диапазон результатов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public class QuestionInput
    {
        public TextBox QuestionTextBox { get; set; }
        public List<TextBox> AnswerTextBoxes { get; set; }
        public List<RadioButton> CorrectAnswerRadios { get; set; }
        public List<StackPanel> AnswerPanels { get; set; }
    }

    public class RangeInput
    {
        public TextBox MinScoreTextBox { get; set; }
        public TextBox MaxScoreTextBox { get; set; }
        public TextBox DescriptionTextBox { get; set; }
    }
}