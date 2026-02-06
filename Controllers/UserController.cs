using DatabasFirst_Collegedb.Data;
using DatabasFirst_Collegedb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    public class UserController : Controller
    {
        private readonly MyContext _context;   // class variable


        // constructor
        public UserController(MyContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("user_id");
            var role_id = Convert.ToInt32(HttpContext.Session.GetString("role_id"));
            if (user != null && role_id == 2)  // in your case 1 for admin
            {
                // Cards
                ViewBag.TotalFaculty = _context.Faculties.Count();
                ViewBag.TotalStudents = _context.Students.Count();
                ViewBag.TotalCourses = _context.Courses.Count();
                ViewBag.TotalEnrollments = _context.Enrollments.Count();

               

                // Table
                var tableData = _context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.Course)
                    .Include(e => e.Course.Faculty)
                    .Select(e => new {
                        Student = e.Student.Name,
                        Faculty = e.Course.Faculty.Name,
                        Course = e.Course.CourseName,
                        Present = _context.Attendances.Count(a => a.StudentId == e.StudentId && a.CourseId == e.CourseId && a.Status == "Present"),
                        Absent = _context.Attendances.Count(a => a.StudentId == e.StudentId && a.CourseId == e.CourseId && a.Status == "Absent"),
                        EnrollDate = e.EnrollmentDate
                    }).ToList();

                ViewBag.TableData = tableData;

                return View();
            }
            else if(user != null && role_id == 3)
            {
                return RedirectToAction("FacultyIndex", "Faculty");
               
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // for hashing password
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);


            user.RoleId = 3;


            _context.Users.Add(user);  // insert into users (name, password, passwword) values ()
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string log_username, string log_password)
        {
            var row = _context.Users.FirstOrDefault(u => u.Username == log_username);


            if (row != null)
            {
                // verifying hash password - which comes from db
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(row, row.Password, log_password);

                if (result == PasswordVerificationResult.Success)
                {
                    // storing data in session variables
                    HttpContext.Session.SetString("user_id", row.UserId.ToString());
                    HttpContext.Session.SetString("role_id", row.RoleId.ToString());
                    HttpContext.Session.SetString("user_name", row.Username);
                    

                    return RedirectToAction("Index");
                }

            }

            //else part
            ViewBag.error = "Invalid Credentials";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
          //  HttpContext.Session.Remove("user_id");
            return RedirectToAction("Login");
        }



  

    }

}
