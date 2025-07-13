namespace PsychologicalTestsApp
{
    public static class Session
    {
        public static int? StudentId { get; set; }
        public static string StudentName { get; set; }

        public static void Clear()
        {
            StudentId = null;
            StudentName = null;
        }
    }
}
