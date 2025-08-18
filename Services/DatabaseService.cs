using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearningManagementSystem.Models;
using LMS_FinalProject.Models;
using System.IO;

namespace LMS_FinalProject.Services
{
    public class DatabaseService
    {
        private readonly ApplicationDbContext _context;

        public DatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeDatabaseAsync()
        {
            await _context.Database.MigrateAsync();
        }

        // User related methods
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            // In production, use proper password hashing
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Course related methods
        public async Task<List<Course>> GetCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<List<Course>> GetEnrolledCoursesAsync(int studentId)
        {
            var student = await _context.Users
                .Include(u => u.EnrolledCourses)
                    .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            return student?.EnrolledCourses.ToList() ?? new List<Course>();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Students)
                .Include(c => c.Modules)
                    .ThenInclude(m => m.ContentItems)
                .Include(c => c.Assignments)
                .Include(c => c.Announcements)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<bool> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return true;
        }

        // Assignment related methods
        public async Task<List<Assignment>> GetUpcomingAssignmentsAsync(int studentId)
        {
            var student = await _context.Users
                .Include(u => u.EnrolledCourses)
                    .ThenInclude(c => c.Assignments)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            if (student == null)
                return new List<Assignment>();

            var assignments = new List<Assignment>();
            foreach (var course in student.EnrolledCourses)
            {
                assignments.AddRange(course.Assignments);
            }

            return assignments
                .Where(a => a.DueDate > DateTime.Now)
                .OrderBy(a => a.DueDate)
                .ToList();
        }

        // Announcement related methods
        public async Task<List<Announcement>> GetRecentAnnouncementsAsync(int studentId)
        {
            var student = await _context.Users
                .Include(u => u.EnrolledCourses)
                    .ThenInclude(c => c.Announcements)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            if (student == null)
                return new List<Announcement>();

            var announcements = new List<Announcement>();
            foreach (var course in student.EnrolledCourses)
            {
                announcements.AddRange(course.Announcements);
            }

            return announcements
                .OrderByDescending(a => a.Date)
                .Take(5)
                .ToList();
        }
    }
}
