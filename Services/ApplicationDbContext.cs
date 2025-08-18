using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.Models;
using LMS_FinalProject.Models;

namespace LMS_FinalProject.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.EnrolledCourses)
                .WithMany(c => c.Students);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TaughtCourses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed users
            var adminUser = new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin123", // In production, use password hashing
                Email = "admin@lms.com",
                FirstName = "Admin",
                LastName = "User",
                Role = UserRole.Administrator
            };

            var instructor = new User
            {
                Id = 2,
                Username = "instructor",
                Password = "instructor123",
                Email = "instructor@lms.com",
                FirstName = "John",
                LastName = "Doe",
                Role = UserRole.Instructor
            };

            var student = new User
            {
                Id = 3,
                Username = "student",
                Password = "student123",
                Email = "student@lms.com",
                FirstName = "Jane",
                LastName = "Smith",
                Role = UserRole.Student
            };

            modelBuilder.Entity<User>().HasData(adminUser, instructor, student);

            // Seed courses
            var course = new Course
            {
                Id = 1,
                CourseCode = "CS101",
                Title = "Introduction to Programming",
                Description = "A beginner-friendly course to programming concepts using C#.",
                InstructorId = instructor.Id
            };

            modelBuilder.Entity<Course>().HasData(course);
        }
    }
}
