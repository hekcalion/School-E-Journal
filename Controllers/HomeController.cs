using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolGradeWebApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using SchoolGradeWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolGradeWebApp.Controllers
{
    public class HomeController : Controller
    {        
        private SchoolDataBase db;
        public static Student CurrentStudent { get; set; }
        public static List<TeacherWorkload> CurrentTeacherWorkloads { get; set; }
        public static TimetableViewModel CurrentTeacherTimeTable { get; set; }
        public static GroupViewModel CurrentGroup { get; set; }
        public static StudentGradeViewModel CurrentStudentGrade { get; set; }
        public HomeController(SchoolDataBase db)
        {
            this.db = db;                       
        }

        
        public IActionResult Index()
        {
            return View();
        }


        // ///////////////// Login ///////////////////
        public IActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public  IActionResult Login(LoginViewModel model)
        {
            if(string.IsNullOrEmpty(model.Login))
            {               
                model.ErrorMassage = "Ви не ввели логін";
                return View("Login",model);
            }
            if (string.IsNullOrEmpty(model.Password))
            {               
                model.ErrorMassage = "Ви не ввели пароль";
                return View("Login", model);
            }
            User user = db.Users
                .Include(s => s.Student)
                .Include(t => t.Teacher)
                .Where(u => u.LoginID == model.Login)
                .First();
            if(user == null)
            {              
                model.ErrorMassage = "Неправильний логін";
                return View("Login",model);
            }
            if(user.Password != model.Password)
            {
                model.Login = "";
                model.ErrorMassage = "Неправильний пароль";
                return View("Login", model);
            }
            if(user.Role.ToString() == "Student")
            {
                Student student = db.Students.Include(g => g.Grades)
                    .ThenInclude(gd => gd.GradeDetails)
                    .Include(ss => ss.Student_Subjects)
                    .ThenInclude(sub => sub.Subject)
                    .Include(g => g.Group)
                    .Where(student => student.StudentID == user.Student.StudentID)
                    .First();
                CurrentStudent = student;
                return View("Student",student);
            }
            else
            {

                List<TeacherWorkload> TeacherWorkloads = db.TeacherWorkloads
                    .Include(t => t.Group)
                    .ThenInclude(g => g.Students)
                    .ThenInclude(s => s.Grades)
                    .ThenInclude(g => g.GradeDetails)
                    .Include(t => t.Subject)
                    .Include(t => t.Teacher)
                    .Where(t => t.TeacherID == user.Teacher.TeacherID)
                    .ToList();                         
                CurrentTeacherWorkloads = TeacherWorkloads;
                return View("Teacher", CurrentTeacherWorkloads);
            }         
        }


        // ///////////////// Student -> Subject ///////////////////
        public IActionResult Student()
        {           
            return View("Student",CurrentStudent);
        }



        // ///////////////// Subject -> Grade ///////////////////
        public IActionResult Grade(string id)
        {
            GradeViewModel model = new GradeViewModel
            {
                Student = CurrentStudent,
                SubjectName = id
            };
            return View("Grade", model);
        }


        // ///////////////// Student -> Timetable ///////////////////
        public IActionResult Timetable (int id)
        {
            List<Timetable> Timetable = db.Timetables
                .Include(g => g.Group)
                .Include(s => s.Subject)
                .Where(group => group.Group.GroupID == id)
                .ToList();
            TimetableViewModel model = new TimetableViewModel
            {
                Mondey = Timetable.Where(t => t.DayOfWeek == "Понеділок")
                .OrderBy(t => t.LessonNumber).ToList(),
                Tuesday = Timetable.Where(t => t.DayOfWeek == "Вівторок")
                .OrderBy(t => t.LessonNumber).ToList(),
                Wednesday = Timetable.Where(t => t.DayOfWeek == "Середа")
                .OrderBy(t => t.LessonNumber).ToList(),
                Thursday = Timetable.Where(t => t.DayOfWeek == "Четвер")
                .OrderBy(t => t.LessonNumber).ToList(),
                Friday = Timetable.Where(t => t.DayOfWeek == "П'ятниця")
                .OrderBy(t => t.LessonNumber).ToList()
            };
            return View("Timetable", model);
        }



        // ///////////////// Teacher -> TeacherWorkLoad ///////////////////
        public IActionResult Teacher()
        {
            return View("Teacher", CurrentTeacherWorkloads);
        }

        public IActionResult TeacherUpdate()
        {
            int TeacherID = CurrentTeacherWorkloads.First().TeacherID;
            CurrentTeacherWorkloads = db.TeacherWorkloads
                    .Include(t => t.Group)
                    .ThenInclude(g => g.Students)
                    .ThenInclude(s => s.Grades)
                    .ThenInclude(g => g.GradeDetails)
                    .Include(t => t.Subject)
                    .Include(t => t.Teacher)
                    .Where(t => t.TeacherID == TeacherID)
                    .ToList();
            return View("Teacher", CurrentTeacherWorkloads);
        }

        // ///////////////// TeacherTimetable ///////////////////
        public IActionResult TeacherTimetable()
        {
            if(CurrentTeacherTimeTable == null)
            {
                List<TeacherWorkload> teacherWorkloads = CurrentTeacherWorkloads;
                List<Timetable> Timetable = db.Timetables
                    .Include(t => t.Group)
                    .Include(t => t.Subject)
                    .ToList();
                List<Timetable> teacherTimetable = new List<Timetable>();
                foreach(TeacherWorkload tw in teacherWorkloads)
                {
                    foreach(Timetable tab in Timetable)
                    {
                        if(tw.GroupID == tab.Group.GroupID && tw.SubjectID == tab.Subject.SubjectID)
                        {
                            teacherTimetable.Add(tab);
                        }
                    }
                }
                TimetableViewModel CurrentTeacherTimeTable = new TimetableViewModel
                {
                    Mondey = teacherTimetable.Where(t => t.DayOfWeek == "Понеділок")
                    .OrderBy(t => t.LessonNumber).ToList(),
                    Tuesday = teacherTimetable.Where(t => t.DayOfWeek == "Вівторок")
                    .OrderBy(t => t.LessonNumber).ToList(),
                    Wednesday = teacherTimetable.Where(t => t.DayOfWeek == "Середа")
                    .OrderBy(t => t.LessonNumber).ToList(),
                    Thursday = teacherTimetable.Where(t => t.DayOfWeek == "Четвер")
                    .OrderBy(t => t.LessonNumber).ToList(),
                    Friday = teacherTimetable.Where(t => t.DayOfWeek == "П'ятниця")
                    .OrderBy(t => t.LessonNumber).ToList()
                };
                return View("TeacherTimetable", CurrentTeacherTimeTable);
            }
            else
            {
                return View("TeacherTimetable", CurrentTeacherTimeTable);
            }      
        }

        // ///////////////// TeacherWorkLoad -> Group ///////////////////
        public IActionResult Group (int GroupID, int SubjectID)
        {
            if((GroupID <=0) || (SubjectID <= 0))
            {
                return NotFound();
            }
            else
            {
                Group group = db.Groups
                    .Include(g => g.Students)
                    .ThenInclude(s => s.Grades)
                    .ThenInclude(g => g.Subject)
                    .Where(g => g.GroupID == GroupID)
                    .FirstOrDefault();
                Subject subject = db.Subjects
                    .Where(sub => sub.SubjectID == SubjectID)
                    .FirstOrDefault();
                if ((group == null) || (subject == null))
                {
                    return NotFound();
                }
                else
                {
                    CurrentGroup = new GroupViewModel
                    {
                        Group = group,
                        Subject = subject
                    };
                    return View("Group", CurrentGroup);
                }
            }
        }

        public IActionResult GroupUpdate()
        {
            int GroupID = CurrentGroup.Group.GroupID;
            int SubjectID = CurrentGroup.Subject.SubjectID;
            Group group = db.Groups
                .Include(g => g.Students)
                .ThenInclude(s => s.Grades)
                .ThenInclude(g => g.Subject)
                .Where(g => g.GroupID == GroupID)
                .FirstOrDefault();
            Subject subject = db.Subjects
                .Where(sub => sub.SubjectID == SubjectID)
                .FirstOrDefault();
            CurrentGroup = new GroupViewModel
            {
                Group = group,
                Subject = subject
            };
            return View("Group", CurrentGroup);
        }

        // ///////////////// Group -> Student///////////////////
        public IActionResult StudentGrade(int StudentID , int SubjectID)
        {
            Student student = db.Students
                .Where(s => s.StudentID == StudentID)
                .First();
            Grade grade = db.Grades
               .Include(g => g.GradeDetails)
               .Include(g => g.Subject)
               .Include(g => g.Student)
               .Where(s => s.Student.StudentID == StudentID)
               .Where(s => s.Subject.SubjectID == SubjectID)
               .First();
            CurrentStudentGrade = new StudentGradeViewModel
            {
                Student = student,
                Grade = grade
            };
            return View("StudentGrade", CurrentStudentGrade);
        }

        public IActionResult StudentGradeUpdate()
        {
            int? GradeID = CurrentStudentGrade.Grade.GradeID;
            if(GradeID == null)
            {
                return NotFound();
            }
            else
            {
                if(GradeID == 0)
                {
                    return NotFound();
                }
                else
                {
                    Grade grade = db.Grades
                        .Include(g => g.GradeDetails)
                        .Include(g => g.Subject)
                        .Include(g => g.Student)
                        .Where(g => g.GradeID == GradeID)
                        .First();
                    CurrentStudentGrade.Grade = grade;
                    return View("StudentGrade", CurrentStudentGrade);
                }
            }
        }

        // ///////////////// EDIT FOR GRADE ///////////////////
                   
        
        public async Task<IActionResult> GradeEdit(int GradeID)
        {
            if (GradeID <=0)
            {
                return NotFound();
            }

            var grade = await db.Grades.FindAsync(GradeID);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeEdit(int id, [Bind("GradeID,FirstTermGrade,SecondTermGrade,FinalGrade")] Grade grade)
        {
            if (id != grade.GradeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(grade);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.GradeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return StudentGradeUpdate();
            }
            return View(grade);
        }

        private bool GradeExists(int id)
        {
            return db.Grades.Any(e => e.GradeID == id);
        }



        // ///////////////// CREATE EDIT DELETE FOR GRADEDETAIL ///////////////////

        public async Task<IActionResult> GradeDetailAddEdit(int GradeDetailID)
        {
            if (GradeDetailID == 0)
            {
                return NotFound();
            }
            if(GradeDetailID == -200)
            {
                return View(new GradeDetail { Date = System.DateTime.Now});
            }
            var gradeDetail = await db.GradeDetails.FindAsync(GradeDetailID);
            if (gradeDetail == null)
            {
                return NotFound();
            }
            ViewData["GradeID"] = new SelectList(db.Grades, "GradeID", "GradeID", gradeDetail.GradeID);
            return View(gradeDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeDetailAddEdit(int GradeDetailID, [Bind("GradeDetailID,GradeID,Mark,Date,Description")] GradeDetail gradeDetail)
        {
            if (ModelState.IsValid)
            {
                if(GradeDetailID == -200)
                {
                    gradeDetail.GradeID = CurrentStudentGrade.Grade.GradeID;
                    gradeDetail.GradeDetailID = db.GradeDetails.Count() + 1;
                    db.Add(gradeDetail);
                    await db.SaveChangesAsync();
                    return StudentGradeUpdate();
                }
                try
                {
                    db.Update(gradeDetail);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeDetailExists(gradeDetail.GradeDetailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return StudentGradeUpdate();
            }
            ViewData["GradeID"] = new SelectList(db.Grades, "GradeID", "GradeID", gradeDetail.GradeID);
            return View(gradeDetail);
        }

        public async Task<IActionResult> GradeDetailDelete(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            var gradeDetail = await db.GradeDetails
                .FirstOrDefaultAsync(m => m.GradeDetailID == id);
            if (gradeDetail == null)
            {
                return NotFound();
            }
            return View(gradeDetail);
        }

        [HttpPost, ActionName("GradeDetailDelete")]
        [ValidateAntiForgeryToken]
        public  IActionResult GradeDetailDeleteConfirmed(int id)
        {
            var gradeDetail = db.GradeDetails.Find(id);
            if(gradeDetail == null)
            {
                return StudentGradeUpdate();
            }        
            db.GradeDetails.Remove(gradeDetail);
            db.SaveChanges();           
            return StudentGradeUpdate();
        }

        private bool GradeDetailExists(int id)
        {
            return db.GradeDetails.Any(e => e.GradeDetailID == id);
        }

        // ////////////////////////////////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
