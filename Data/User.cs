using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradeWebApp.Data
{
    public enum Role
    {
        Student,
        Teacher
    }
    public class User
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}
