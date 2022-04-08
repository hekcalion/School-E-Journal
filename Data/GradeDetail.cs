using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolGradeWebApp.Data
{
    public class GradeDetail
    {
        public int GradeDetailID { get; set; }     
        public int Mark { get; set; }
        public DateTime Date { get; set; } 
        public string Description { get; set; }


        public int GradeID { get; set; }
        public  Grade Grade { get; set; }
    }
}
