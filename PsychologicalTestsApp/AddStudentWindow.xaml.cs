using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PsychologicalTestsApp
{
    public partial class AddStudentWindow : Window
    {
        private DatabaseHelper dbHelper;

        public AddStudentWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadGroups();
        }

        private void LoadGroups()
        {
            var groups = dbHelper.GetGroups(); // Предполагается, что у вас есть метод GetGroups в DatabaseHelper
            GroupComboBox.ItemsSource = groups;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            string groupName = GroupComboBox.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(groupName))
            {
                if (dbHelper.AddStudent(fullName, groupName)) // Предполагается, что у вас есть метод AddStudent в DatabaseHelper
                {
                    MessageBox.Show("Студент добавлен успешно.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении студента.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }
    }
}