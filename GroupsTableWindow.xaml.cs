using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PsychologicalTestsApp.Models;
using System.Linq;

namespace PsychologicalTestsApp
{
    public partial class GroupsTableWindow : Window
    {
        private DatabaseHelper dbHelper;
        private List<GroupDisplay> allGroups;
        private List<GroupDisplay> filteredGroups;

        public GroupsTableWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            allGroups = new List<GroupDisplay>();
            filteredGroups = new List<GroupDisplay>();
            LoadGroups();
        }

        private void LoadGroups()
        {
            allGroups = dbHelper.FetchGroups1();
            filteredGroups = allGroups.ToList();
            GroupsDataGrid.ItemsSource = filteredGroups;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filterText))
            {
                filteredGroups = allGroups.ToList();
            }
            else
            {
                filteredGroups = allGroups
                    .Where(g => g.Name.ToLower().Contains(filterText))
                    .ToList();
            }
            GroupsDataGrid.ItemsSource = filteredGroups;
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = Prompt("Введите название группы:");
            if (!string.IsNullOrEmpty(groupName))
            {
                int groupId = dbHelper.InsertGroup1(groupName.Trim());
                if (groupId != -1)
                {
                    allGroups.Add(new GroupDisplay { Id = groupId, Name = groupName.Trim() });
                    FilterTextBox_TextChanged(null, null); // Обновить фильтр
                    MessageBox.Show("Группа добавлена успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении группы.");
                }
            }
        }

        private void EditGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу для редактирования.");
                System.Diagnostics.Debug.WriteLine("EditGroupButton_Click: SelectedItem is null");
                return;
            }

            var selectedGroup = GroupsDataGrid.SelectedItem as GroupDisplay;
            if (selectedGroup == null)
            {
                MessageBox.Show("Недопустимый выбор группы.");
                System.Diagnostics.Debug.WriteLine("EditGroupButton_Click: Invalid item type");
                return;
            }

            string newName = Prompt("Введите новое название группы:", selectedGroup.Name);
            if (!string.IsNullOrEmpty(newName))
            {
                if (dbHelper.UpdateGroup1(selectedGroup.Id, newName.Trim()))
                {
                    selectedGroup.Name = newName.Trim();
                    FilterTextBox_TextChanged(null, null); // Обновить фильтр
                    MessageBox.Show("Группа обновлена успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении группы.");
                }
            }
        }

        private void DeleteGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу для удаления.");
                System.Diagnostics.Debug.WriteLine("DeleteGroupButton_Click: SelectedItem is null");
                return;
            }

            var selectedGroup = GroupsDataGrid.SelectedItem as GroupDisplay;
            if (selectedGroup == null)
            {
                MessageBox.Show("Недопустимый выбор группы.");
                System.Diagnostics.Debug.WriteLine("DeleteGroupButton_Click: Invalid item type");
                return;
            }

            if (MessageBox.Show($"Удалить группу '{selectedGroup.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (dbHelper.DeleteGroup1(selectedGroup.Id))
                {
                    allGroups.Remove(selectedGroup);
                    FilterTextBox_TextChanged(null, null); // Обновить фильтр
                    MessageBox.Show("Группа удалена успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении группы.");
                }
            }
        }

        private void CancelGroupButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private string Prompt(string message, string defaultText = "")
        {
            var inputBox = new Window
            {
                Title = "Ввод",
                Width = 400,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            stackPanel.Children.Add(new TextBlock { Text = message, Margin = new Thickness(0, 0, 0, 10) });
            var textBox = new TextBox { Width = 350, Text = defaultText };
            stackPanel.Children.Add(textBox);
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };
            var okButton = new Button { Content = "OK", Width = 80, Margin = new Thickness(0, 0, 10, 0) };
            var cancelButton = new Button { Content = "Отмена", Width = 80 };
            okButton.Click += (s, args) => { inputBox.DialogResult = true; inputBox.Close(); };
            cancelButton.Click += (s, args) => { inputBox.DialogResult = false; inputBox.Close(); };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonPanel);
            inputBox.Content = stackPanel;
            return inputBox.ShowDialog() == true ? textBox.Text : null;
        }
    }
}