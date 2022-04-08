using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradeWebApp.Data
{
    public class Grade
    {
        public int GradeID { get; set; }
        public int? FirstTermGrade { get; set; }
        public int? SecondTermGrade { get; set; }
        public int? FinalGrade { get; set; }
        
        public   Student Student { get; set; }
        public  Subject Subject { get; set; }
        public  List<GradeDetail> GradeDetails { get; set; }
    }
}
