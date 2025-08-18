using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Models;
using LMS_FinalProject.Models;

namespace LMS_FinalProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In a real app, store password hash
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public List<Course> EnrolledCourses { get; set; } = new List<Course>();
        public List<Course> TaughtCourses { get; set; } = new List<Course>();
    }

    public enum UserRole
    {
        Student,
        Instructor,
        Administrator
    }
}

// Models/Course.cs
namespace LearningManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User Instructor { get; set; }
        public int InstructorId { get; set; }
        public List<User> Students { get; set; } = new List<User>();
        public List<Module> Modules { get; set; } = new List<Module>();
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
        public List<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}

// Models/Module.cs
namespace LearningManagementSystem.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
    }
}

// Models/ContentItem.cs
namespace LearningManagementSystem.Models
{
    public class ContentItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ContentType Type { get; set; }
        public string Content { get; set; }
        public string FilePath { get; set; }
        public int Order { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }

    public enum ContentType
    {
        Text,
        Video,
        Document,
        Link
    }
}

// Models/Assignment.cs
namespace LearningManagementSystem.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Points { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Submission> Submissions { get; set; } = new List<Submission>();
    }
}

// Models/Submission.cs
namespace LearningManagementSystem.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
        public string SubmissionContent { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmissionDate { get; set; }
        public float Grade { get; set; }
        public string Feedback { get; set; }
    }
}

// Models/Announcement.cs
namespace LearningManagementSystem.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
