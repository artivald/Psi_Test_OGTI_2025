using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PsychologicalTestsApp.Models
{
    public class RangeDisplay
    {
        public int Id { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public string Description { get; set; }
        public TextBox MinScoreBox { get; set; }
        public TextBox MaxScoreBox { get; set; }
        public TextBox DescriptionBox { get; set; }
    }
}
