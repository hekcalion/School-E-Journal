using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradeWebApp.Data
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public List<Teacher_Subject> Teacher_Subjects { get; set; }
        public List<TeacherWorkload> TeacherWorkloads { get; set; }
        public string LoginID { get; set; }
        public User User { get; set; }
    }
}
