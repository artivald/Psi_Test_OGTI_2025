using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PsychologicalTestsApp.Models
{
    public class QuestionDisplay
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<AnswerDisplay> Answers { get; set; } = new List<AnswerDisplay>();
    }
}
