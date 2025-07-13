using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychologicalTestsApp.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}