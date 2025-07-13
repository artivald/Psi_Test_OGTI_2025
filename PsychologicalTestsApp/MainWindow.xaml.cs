using System.Windows;
using System.Windows.Controls;


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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Преподаватель")
                {
                    // Логика для преподавателя
                    string username = UsernameTextBox.Text.Trim();
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
                else if (selectedItem.Content.ToString() == "Студент")
                {
                    // Логика для студента
                    string fullName = UsernameTextBox.Text.Trim(); // Используем UsernameTextBox для ФИО
                    string groupName = PasswordBox.Password; // Используем PasswordBox для группы

                    var student = dbHelper.GetStudent(fullName, groupName);
                    if (student != null)
                    {
                        // Открыть окно для списка тем
                        ThemesWindow themesWindow = new ThemesWindow();
                        themesWindow.Show();
                        this.Close(); // Закрыть текущее окно
                    }
                    else
                    {
                        MessageBox.Show("Неверное ФИО или группа.");
                    }
                }
            }
        }

    }
}
