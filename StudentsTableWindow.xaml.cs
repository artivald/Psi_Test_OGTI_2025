using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;
using System.Linq;

namespace PsychologicalTestsApp
{
    public partial class StudentsTableWindow : Window
    {
        private DatabaseHelper dbHelper;
        private int editingStudentId = -1;
        private List<StudentDisplay> allStudents;
        private List<string> groups;

        public StudentsTableWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            allStudents = new List<StudentDisplay>();
            groups = new List<string>();
            LoadGroups();
            LoadStudents();
        }

        private void LoadGroups()
        {
            groups = dbHelper.GetGroups();
            GroupComboBox.ItemsSource = groups;
            if (GroupComboBox.Items.Count > 0)
                GroupComboBox.SelectedIndex = 0;

            // Загрузка групп для фильтра
            GroupFilterComboBox.Items.Add("Все группы");
            foreach (var group in groups)
            {
                GroupFilterComboBox.Items.Add(group);
            }
            GroupFilterComboBox.SelectedIndex = 0;
        }

        private void LoadStudents()
        {
            allStudents = dbHelper.GetStudentsWithGroupNames();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string selectedGroup = GroupFilterComboBox.SelectedItem?.ToString();
            string nameFilter = NameFilterTextBox.Text.Trim().ToLower();

            var filteredStudents = allStudents.Where(s =>
                (selectedGroup == "Все группы" || s.GroupName == selectedGroup) &&
                (string.IsNullOrEmpty(nameFilter) || s.Name.ToLower().Contains(nameFilter))
            ).ToList();

            StudentsDataGrid.ItemsSource = filteredStudents;
        }

        private void GroupFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            editingStudentId = -1;
            StudentNameTextBox.Text = string.Empty;
            LoadGroups();
            StudentFormPanel.Visibility = Visibility.Visible;
        }

        private void EditStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is StudentDisplay student)
            {
                editingStudentId = student.Id;
                StudentNameTextBox.Text = student.Name;
                LoadGroups();
                GroupComboBox.SelectedItem = student.GroupName;
                StudentFormPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите студента для редактирования.");
            }
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is StudentDisplay student)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить студента '{student.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (dbHelper.DeleteStudent(student.Id))
                    {
                        MessageBox.Show("Студент успешно удалён.");
                        LoadStudents();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении студента. Возможно, он связан с другими данными.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите студента для удаления.");
            }
        }

        private void SaveStudentButton_Click(object sender, RoutedEventArgs e)
        {
            string studentName = StudentNameTextBox.Text.Trim();
            string groupName = GroupComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(groupName))
            {
                MessageBox.Show("Заполните все обязательные поля.");
                return;
            }

            if (editingStudentId == -1)
            {
                if (dbHelper.AddStudent(studentName, groupName))
                {
                    MessageBox.Show("Студент добавлен успешно.");
                    ResetForm();
                    LoadStudents();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении студента.");
                }
            }
            else
            {
                if (dbHelper.UpdateStudent(editingStudentId, studentName, groupName))
                {
                    MessageBox.Show("Студент обновлён успешно.");
                    ResetForm();
                    LoadStudents();
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении студента.");
                }
            }
        }

        private void CancelStudentButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResetForm()
        {
            editingStudentId = -1;
            StudentNameTextBox.Text = string.Empty;
            StudentFormPanel.Visibility = Visibility.Collapsed;
            if (GroupComboBox.Items.Count > 0)
                GroupComboBox.SelectedIndex = 0;
        }
    }
}