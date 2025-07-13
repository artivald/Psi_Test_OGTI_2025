using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;

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
            selectedTheme = theme;
            LoadTests();
        }

        private void LoadTests()
        {
            var tests = dbHelper.GetTestsByTheme(selectedTheme);
            if (tests.Count == 0)
            {
                MessageBox.Show("Нет доступных тестов для выбранной темы.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            TestsListBox.ItemsSource = tests; // Устанавливаем список строк как источник данных
        }

        private void TakeTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestsListBox.SelectedItem is string selectedTestName)
            {
                TestWindow testWindow = new TestWindow(selectedTestName);
                testWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тест.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}