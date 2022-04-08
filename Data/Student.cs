using System.Collections.Generic;

namespace SchoolGradeWebApp.Data
{
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int YearOfEntry { get; set; }
    
        public   Group  Group { get; set; }
        public  List<Student_Subject> Student_Subjects { get; set; }
        public  List<Grade> Grades { get; set; }
        public string LoginID { get; set; }
        public User User { get; set; }
    }
}
