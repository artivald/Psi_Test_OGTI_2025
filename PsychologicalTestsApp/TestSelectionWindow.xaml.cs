using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models; // Убедитесь, что это пространство имен правильно указано

namespace PsychologicalTestsApp
{
    public partial class TestSelectionWindow : Window
    {
        private DatabaseHelper dbHelper;
        private string selectedTheme;

        public TestSelectionWindow(string theme)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            selectedTheme = theme; // Сохраняем выбранную тему
            LoadTests(); // Загружаем тесты, связанные с темой
        }

        private void LoadTests()
        {
            // Получаем тесты, связанные с выбранной темой
            var tests = dbHelper.GetTestsByTheme(selectedTheme);
            if (tests.Count == 0)
            {
                MessageBox.Show("Нет доступных тестов для выбранной темы.");
                return;
            }

            foreach (var test in tests)
            {
                TestsListBox.Items.Add(test.Name); // Добавляем тесты в ListBox
            }

            // Если тест с id = 1 существует, можно добавить его в ListBox
            var testId = dbHelper.GetTestIdByName("Тест типы личности по Фрейду");
            if (testId != -1)
            {
                TestsListBox.Items.Add("Тест типы личности по Фрейду");
            }
        }


        private void TakeTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedItem != null)
            {
                string selectedTest = TestsListBox.SelectedItem.ToString();
                // Открыть окно теста
                TestWindow testWindow = new TestWindow(selectedTest); // Передаем выбранный тест
                testWindow.Show();
                this.Close(); // Закрываем текущее окно
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тест."); // Сообщение, если тест не выбран
            }
        }
    }
}
