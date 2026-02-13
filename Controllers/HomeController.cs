using DatabasFirst_Collegedb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabasFirst_Collegedb.Controllers
{
    public class HomeController : Controller
    {

        private readonly MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new
            {
                Courses = _context.Courses
                                   .Select(c => c.CourseName)
                                   .ToList(),

                TotalCourses = _context.Courses.Count(),
                TotalStudents = _context.Students.Count(),
                TotalFaculty = _context.Faculties.Count(),
                TotalEnrollments = _context.Enrollments.Count()
            };

            return View(model);
        }


        public IActionResult Courses()
        {
            var courses = _context.Courses
                .Include(c => c.Faculty)
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    FacultyName = c.Faculty.Name,
                    c.Duration,
                    c.AvailableSeats,
                 //   ImagePath = "~/img/courses-" + ((c.CourseId % 6) + 1) + ".jpg"
                })
                .ToList();

            var model = new
            {
                Courses = courses,
                TotalCourses = _context.Courses.Count(),
                TotalStudents = _context.Students.Count(),
                TotalFaculty = _context.Faculties.Count(),
                TotalEnrollments = _context.Enrollments.Count()
            };

            return View(model);
        }
    }
}
