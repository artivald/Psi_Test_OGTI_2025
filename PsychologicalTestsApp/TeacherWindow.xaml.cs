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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PsychologicalTestsApp
{
    public partial class TeacherWindow : Window
    {
        private DatabaseHelper dbHelper;

        public TeacherWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void AddTestButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления теста
            MessageBox.Show("Добавление теста...");
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow();
            addStudentWindow.ShowDialog(); // Открывает окно как модальное
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            AddGroupWindow addGroupWindow = new AddGroupWindow();
            addGroupWindow.ShowDialog(); // Открывает окно как модальное
        }
    }
}