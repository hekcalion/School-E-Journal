using SchoolGradeWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradeWebApp.Models
{
    public class TimetableViewModel
    {
        public List<Timetable> Mondey { get; set; }
        public List<Timetable> Tuesday { get; set; }
        public List<Timetable> Wednesday { get; set; }
        public List<Timetable> Thursday { get; set; }
        public List<Timetable> Friday { get; set; }
    }
}
