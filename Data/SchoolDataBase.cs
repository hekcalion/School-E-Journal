using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolGradeWebApp.Data
{
    public class SchoolDataBase : DbContext
    {
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeDetail> GradeDetails { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student_Subject> Student_Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Teacher_Subject> Teacher_Subjects { get; set; }
        public DbSet<TeacherWorkload> TeacherWorkloads { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<User> Users { get; set; }

        public SchoolDataBase(DbContextOptions<SchoolDataBase> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Grade>().Property(g => g.GradeID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<GradeDetail>().Property(g => g.GradeID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<GradeDetail>().Property(g => g.Mark).IsRequired();
            modelBuilder.Entity<GradeDetail>().Property(g => g.Date).IsRequired();
            modelBuilder.Entity<Group>().Property(g => g.GroupID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Group>().Property(g => g.GroupName).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Student>().Property(s => s.StudentID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Student>().Property(s => s.Name).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Student>().Property(s => s.Surname).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Student>().Property(s => s.YearOfEntry).IsRequired();
            modelBuilder.Entity<Subject>().Property(s => s.SubjectID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Subject>().Property(s => s.SubjectName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Teacher>().Property(t => t.TeacherID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Teacher>().Property(t => t.Name).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Teacher>().Property(t => t.Surname).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Timetable>().Property(t => t.TimetableID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<GradeDetail>().HasKey(g =>  g.GradeDetailID  );
            modelBuilder.Entity<TeacherWorkload>().HasKey(t => t.TeacherWorkloadID);
            modelBuilder.Entity<GradeDetail>().Property(g => g.GradeDetailID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<GradeDetail>().Property(g => g.Date).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<TeacherWorkload>().Property(t => t.TeacherWorkloadID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>().HasMaxLength(10);


            modelBuilder.Entity<Group>().HasMany(s => s.Students).WithOne(g => g.Group).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Teacher>().HasMany(t => t.TeacherWorkloads).WithOne(t => t.Teacher);
            modelBuilder.Entity<TeacherWorkload>().HasOne(t => t.Teacher).WithMany(t => t.TeacherWorkloads);
            modelBuilder.Entity<TeacherWorkload>().HasOne(s => s.Subject).WithMany(t => t.TeacherWorkloads);
            modelBuilder.Entity<TeacherWorkload>().HasOne(g => g.Group).WithMany(t => t.TeacherWorkloads);
            modelBuilder.Entity<Student>().HasOne(g => g.Group).WithMany(s => s.Students);
            modelBuilder.Entity<Student>().HasMany(s => s.Grades).WithOne(s => s.Student);
            modelBuilder.Entity<Grade>().HasOne(s => s.Student).WithMany(g => g.Grades);
            modelBuilder.Entity<Grade>().HasMany(g => g.GradeDetails).WithOne(g => g.Grade);
            modelBuilder.Entity<GradeDetail>().HasOne(g => g.Grade).WithMany(g => g.GradeDetails);

            modelBuilder.Entity<User>().HasKey(u => u.LoginID);
            modelBuilder.Entity<User>().HasOne(u => u.Student).WithOne(s => s.User).HasForeignKey<Student>(s => s.LoginID);
            modelBuilder.Entity<User>().HasOne(u => u.Teacher).WithOne(t => t.User).HasForeignKey<Teacher>(t => t.LoginID);

            modelBuilder.Entity<Student_Subject>().HasOne(s => s.Student).WithMany(ss => ss.Student_Subjects).HasForeignKey(s => s.StudentID);
            modelBuilder.Entity<Student_Subject>().HasOne(s => s.Subject).WithMany(ss => ss.Student_Subjects).HasForeignKey(s => s.SubjectID);
            modelBuilder.Entity<Student_Subject>().Property(ss => ss.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Teacher_Subject>().HasOne(t => t.Teacher).WithMany(ts => ts.Teacher_Subjects).HasForeignKey(t => t.TeacherID);
            modelBuilder.Entity<Teacher_Subject>().HasOne(s => s.Subject).WithMany(ts => ts.Teacher_Subjects).HasForeignKey(s => s.SubjectID);
            modelBuilder.Entity<Teacher_Subject>().Property(ts => ts.id).ValueGeneratedOnAdd();
        }
    }
}
