using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;

namespace PsychologicalTestsApp
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void TeacherButton_Click(object sender, RoutedEventArgs e)
        {
            InputPanel.Visibility = Visibility.Visible;
            GroupLabel.Visibility = Visibility.Collapsed;
            GroupComboBox.Visibility = Visibility.Collapsed;
            UsernameLabel.Text = "Имя пользователя";
            UsernameLabel.Visibility = Visibility.Visible;
            UsernameTextBox.Visibility = Visibility.Visible;
            UsernameComboBox.Visibility = Visibility.Collapsed;
            PasswordLabel.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Visible;
            LoginButton.Content = "Войти как преподаватель";
        }

        private void StudentButton_Click(object sender, RoutedEventArgs e)
        {
            InputPanel.Visibility = Visibility.Visible;
            GroupLabel.Visibility = Visibility.Visible;
            GroupComboBox.Visibility = Visibility.Visible;
            UsernameLabel.Text = "ФИО студента";
            UsernameLabel.Visibility = Visibility.Visible;
            UsernameComboBox.Visibility = Visibility.Visible;
            UsernameTextBox.Visibility = Visibility.Collapsed;
            PasswordLabel.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Collapsed;
            LoginButton.Content = "Войти как студент";

            // Загружаем список групп и студентов
            LoadGroups();
            if (GroupComboBox.SelectedItem is ComboBoxItem c)
                LoadStudents(c.Content.ToString());
        }

        private void LoadGroups()
        {
            GroupComboBox.Items.Clear();
            var groups = dbHelper.GetGroups();
            if (groups.Count == 0)
            {
                MessageBox.Show("Группы не найдены. Обратитесь к администратору.");
                LoginButton.IsEnabled = false;
                return;
            }
            foreach (var group in groups)
            {
                GroupComboBox.Items.Add(new ComboBoxItem { Content = group, Tag = group });
            }
            if (GroupComboBox.Items.Count > 0)
            {
                GroupComboBox.SelectedIndex = 0;
            }
        }

        private void LoadStudents(string findGroup = "")
        {
            UsernameComboBox.Items.Clear();
            var groups = dbHelper.GetStudents(findGroup);
            if (groups.Count == 0)
            {
                MessageBox.Show("Студенты не найдены. Обратитесь к администратору.");
                LoginButton.IsEnabled = false;
                return;
            }
            foreach (var group in groups)
            {
                UsernameComboBox.Items.Add(new ComboBoxItem { Content = group.Name, Tag = group.Name });
            }
            if (UsernameComboBox.Items.Count > 0)
            {
                UsernameComboBox.SelectedIndex = 0;
            }
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupComboBox.SelectedItem is ComboBoxItem)
            {
                UsernameTextBox.Text = "";
                if (GroupComboBox.SelectedItem is ComboBoxItem c)
                    LoadStudents(c.Content.ToString());
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;

            if (string.IsNullOrWhiteSpace(username) && UsernameComboBox.Visibility == Visibility.Collapsed)
            {
                MessageBox.Show("Введите ФИО или имя пользователя.");
                return;
            }

            if (LoginButton.Content.ToString() == "Войти как студент")
            {
                if (!(GroupComboBox.SelectedItem is ComboBoxItem selectedGroup))
                {
                    MessageBox.Show("Выберите группу.");
                    return;
                }

                string groupName = selectedGroup.Tag.ToString();

                // Проверяем, существует ли студент в базе
                var student = dbHelper.GetStudent(UsernameComboBox.Text, groupName);
                if (student != null)
                {
                    // Сохраняем данные в Session
                    Session.StudentId = student.Id;
                    Session.StudentName = student.Name;

                    // Открываем окно выбора тем
                    ThemesWindow themesWindow = new ThemesWindow();
                    themesWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Студент с таким ФИО не найден в выбранной группе.");
                }
            }
            else // Вход как преподаватель
            {
                string password = PasswordBox.Password;
                

                var user = dbHelper.GetUser(username, password);
                if (user != null)
                {
                    // Открыть интерфейс преподавателя
                    MessageBox.Show("Добро пожаловать, преподаватель!");
                    TeacherWindow teacherWindow = new TeacherWindow();
                    teacherWindow.Show();
                    this.Close(); // Закрыть текущее окно
                }
                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль.");
                }
            }
        }
    }
}