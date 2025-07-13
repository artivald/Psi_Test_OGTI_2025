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
    public partial class AddGroupWindow : Window
    {
        private DatabaseHelper dbHelper;

        public AddGroupWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = GroupNameTextBox.Text;

            if (!string.IsNullOrEmpty(groupName))
            {
                if (dbHelper.AddGroup(groupName)) // Предполагается, что у вас есть метод AddGroup в DatabaseHelper
                {
                    MessageBox.Show("Группа добавлена успешно.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении группы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название группы.");
            }
        }
    }
}
