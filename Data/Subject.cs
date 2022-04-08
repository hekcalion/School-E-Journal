using System.Collections.Generic;

namespace SchoolGradeWebApp.Data
{
    public class Subject
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }

        public  List<Student_Subject> Student_Subjects { get; set; }
        public  List<Teacher_Subject> Teacher_Subjects { get; set; }
        public List<TeacherWorkload> TeacherWorkloads { get; set; }
    }
}
