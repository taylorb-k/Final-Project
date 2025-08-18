using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Models;
using LMS_FinalProject.Models;

namespace LMS_FinalProject.Services
{
    public class SessionService
    {
        public User CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUser != null;

        public bool IsInstructor => CurrentUser?.Role == UserRole.Instructor;

        public bool IsAdmin => CurrentUser?.Role == UserRole.Administrator;

        public bool IsStudent => CurrentUser?.Role == UserRole.Student;

        public void Login(User user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
