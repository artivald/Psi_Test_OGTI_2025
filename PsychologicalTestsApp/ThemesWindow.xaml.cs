using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PsychologicalTestsApp
{
    public partial class ThemesWindow : Window
    {
        private DatabaseHelper dbHelper;

        public ThemesWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadThemes();
        }

        private void LoadThemes()
        {
            List<string> themes = dbHelper.GetThemes(); // Получаем список тем из базы данных
            ThemesListBox.ItemsSource = themes; // Устанавливаем источник данных для ListBox
        }

        private void SelectThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemesListBox.SelectedItem != null)
            {
                string selectedTheme = ThemesListBox.SelectedItem.ToString();
                // Открываем окно выбора тестов, передавая выбранную тему
                TestSelectionWindow testSelectionWindow = new TestSelectionWindow(selectedTheme);
                testSelectionWindow.Show();
                this.Close(); // Закрываем текущее окно
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тему."); // Сообщение, если тема не выбрана
            }
        }

        private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Здесь можно добавить дополнительную логику, если нужно
        }
    }
}
