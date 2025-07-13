using System.Windows;

namespace PsychologicalTestsApp
{
    public partial class TeacherWindow : Window
    {
        public TeacherWindow()
        {
            InitializeComponent();
        }

        private void ViewTestsButton_Click(object sender, RoutedEventArgs e)
        {
            TestsTableWindow testsTableWindow = new TestsTableWindow();
            testsTableWindow.ShowDialog();
        }

        private void ViewStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentsTableWindow studentsTableWindow = new StudentsTableWindow();
            studentsTableWindow.ShowDialog();
        }

        private void ViewGroupsButton_Click(object sender, RoutedEventArgs e)
        {
            GroupsTableWindow groupsTableWindow = new GroupsTableWindow();
            groupsTableWindow.ShowDialog();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
        }

        private void ReportStatsButton_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportStatisticsWindow();
            reportWindow.ShowDialog();
        }
    }
}