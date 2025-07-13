using System;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;

namespace PsychologicalTestsApp
{
    public partial class ReportStatisticsWindow : Window
    {
        private readonly DatabaseHelper _dbHelper;

        public ReportStatisticsWindow()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            LoadThemes();
            DatePickerSecond.SelectedDate = DateTime.Today;
        }

        private void LoadThemes()
        {
            var themes = _dbHelper.GetThemes();
            ThemeComboBox.ItemsSource = themes;
            if (themes.Count > 0)
                ThemeComboBox.SelectedIndex = 0;
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is string themeName)
            {
                var testResults = _dbHelper.GetUniqueStudentsByTheme(themeName, DatePickerFirst.SelectedDate ?? DateTime.MinValue, DatePickerSecond.SelectedDate ?? DateTime.Today);
                ResultsGrid.ItemsSource = testResults;
            }
            else
            {
                MessageBox.Show("Выберите тему.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}