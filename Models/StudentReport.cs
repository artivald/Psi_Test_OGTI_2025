namespace PsychologicalTestsApp.Models
{
    public class StudentReport
    {
        public string GroupName { get; set; }
        public string FullName { get; set; }
        public System.DateTime LastDate { get; set; }

        public string LastDateFormatted => LastDate.ToString("dd.MM.yyyy");
    }
}