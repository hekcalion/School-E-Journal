using System.Collections.Generic;


namespace SchoolGradeWebApp.Data
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }

        public  List<Student> Students { get;  set; }
        public List<TeacherWorkload> TeacherWorkloads { get; set; }
    }
}
