using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradeWebApp.Data
{
    public class TeacherWorkload
    {
        public int TeacherWorkloadID { get; set; }
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }
        public int GroupID { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public Group Group { get; set; }
    }
}
