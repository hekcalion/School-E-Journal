using SchoolGradeWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradeWebApp.Models
{
    public class GradeViewModel
    {
        public Student Student { get; set; }
        public string SubjectName { get; set; }
    }
}
