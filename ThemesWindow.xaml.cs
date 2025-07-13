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
            List<string> themes = dbHelper.GetThemes();
            if (themes.Count == 0)
            {
                MessageBox.Show("Темы не найдены. Обратитесь к администратору.");
                return;
            }
            ThemesListBox.ItemsSource = themes;
        }

        private void SelectThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemesListBox.SelectedItem is string selectedTheme)
            {
                TestSelectionWindow testSelectionWindow = new TestSelectionWindow(selectedTheme);
                testSelectionWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тему.");
            }
        }

        private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Дополнительная логика, если нужна
        }
    }
}