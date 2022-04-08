using System.Collections.Generic;

namespace SchoolGradeWebApp.Data
{
    public class Timetable
    {
        public int TimetableID { get; set; }
        public string DayOfWeek { get; set; }
        public int LessonNumber { get; set; }

        public Subject Subject { get; set; }
        public   Group Group { get; set; }
    }
}
