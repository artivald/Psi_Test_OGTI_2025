using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;

namespace PsychologicalTestsApp
{
    public partial class ReportWindow : Window
    {
        private readonly DatabaseHelper _dbHelper;

        public ReportWindow()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            LoadThemes();
        }

        private void LoadThemes()
        {
            var themes = _dbHelper.GetThemes();
            ThemeComboBox.ItemsSource = themes;
            if (themes.Count > 0)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is string themeName)
            {
                var tests = _dbHelper.GetTestsByTheme(themeName);
                Debug.WriteLine($"Loaded {tests.Count} tests for theme: '{themeName}'");
                TestComboBox.ItemsSource = tests;
                TestComboBox.SelectedIndex = tests.Count > 0 ? 0 : -1;
                ReportDataGrid.ItemsSource = null; // Очистить таблицу
            }
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestComboBox.SelectedItem is string testName)
            {
                Debug.WriteLine($"Generating report for test: '{testName}'");
                var report = _dbHelper.GetTestCompletionReport(testName);
                Debug.WriteLine($"Report contains {report.Count} students");
                ReportDataGrid.ItemsSource = report;
                if (report.Count == 0)
                    MessageBox.Show("Нет студентов, прошедших этот тест.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите тест.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}