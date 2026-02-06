using DatabasFirst_Collegedb.Data;
using DatabasFirst_Collegedb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabasFirst_Collegedb.Controllers
{
    public class AdminController : Controller
    {

        private readonly MyContext _context;   // class variable
        private IWebHostEnvironment _env;

        public AdminController(MyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        // ------------------------------------------------------------------------------

        // Faculty CRUD

        public IActionResult FetchFaculty()
        {
            var faculties = _context.Faculties.ToList();   // selec t * from Faculties
            return View(faculties);
        }


        public IActionResult CreateFaculty()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult CreateFaculty(Faculty f, IFormFile image)
        {
            // get logged-in user id from session
            string userid = HttpContext.Session.GetString("user_id");

            if (userid == null)
                return RedirectToAction("Login");

            f.UserId = Convert.ToInt32(userid);   //  IMPORTANT LINE

            string fileExtension = Path.GetExtension(image.FileName).ToLower();   // .jpg  .png  .jfif  .webp

            // validation for file extension
            if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jfif")
            {
             
                 string imagePath = Path.Combine(_env.WebRootPath,"admin", "images", "faculty", image.FileName);  // wwwroot/admin/images/student/

                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(fs);
                }

                f.Image = image.FileName; // property in Faculty model

                _context.Faculties.Add(f);
                _context.SaveChanges();

           
                 ViewBag.message = "File uploaded successfully";    // on success message
            }

            else
            {
                ViewBag.message = "Only JPG or PNG allowed";   // on failure message
            }

            return RedirectToAction("FetchFaculty");
        }


        public IActionResult UpdateFaculty(int id)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty == null)
                return NotFound();

            return View(faculty);
        }



        [HttpPost]
        public IActionResult UpdateFaculty(Faculty f, IFormFile image)
        {
            var faculty = _context.Faculties.Find(f.FacultyId);
            if (faculty == null)
                return NotFound();

            faculty.Name = f.Name;
            faculty.Email = f.Email;
            faculty.Experience = f.Experience;
            faculty.Department = f.Department;

            if (image != null)
            {
                string path = Path.Combine(_env.WebRootPath,
                    "admin", "images", "faculty", image.FileName);

                using var fs = new FileStream(path, FileMode.Create);
                image.CopyTo(fs);

                faculty.Image = image.FileName;
            }

            _context.SaveChanges();
            return RedirectToAction("FetchFaculty");
        }


        public IActionResult DeleteFaculty(int id)
        {
            var faculty = _context.Faculties.Find(id);   // select * from Faculties where FacultyId = id

            if (faculty == null)
                return NotFound();

            _context.Faculties.Remove(faculty);
            _context.SaveChanges();

            return RedirectToAction("FetchFaculty");
        }

        // ------------------------------------------------------------------------------

        // Student CRUD

        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateStudent(Student s, IFormFile image)
        {
            // get logged-in user id from session
            string userid = HttpContext.Session.GetString("user_id");

            if (userid == null)
                return RedirectToAction("Login");

            s.UserId = Convert.ToInt32(userid);   //  IMPORTANT LINE

            string fileExtension = Path.GetExtension(image.FileName).ToLower();   // .jpg  .png  .jfif  

            // validation for file extension
            if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jfif")
            {

                string imagePath = Path.Combine(_env.WebRootPath, "admin", "images", "student", image.FileName);

                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(fs);
                }

                s.Image = image.FileName; // property in Faculty model

                _context.Students.Add(s);
                _context.SaveChanges();


                ViewBag.message = "File uploaded successfully";    // on success message
            }

            else
            {
                ViewBag.message = "Only JPG or PNG allowed";   // on failure message
            }

            return RedirectToAction("FetchStudent");
        }

        public IActionResult FetchStudent()
        {
            var students = _context.Students.ToList();
            return View(students);
        }

        public IActionResult UpdateStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        
        [HttpPost]
        public IActionResult UpdateStudent(Student s, IFormFile image)
        {
            var student = _context.Students.Find(s.StudentId);
            if (student == null)
                return NotFound();

            student.Name = s.Name;
            student.Email = s.Email;
            student.Age = s.Age;
            student.Batch = s.Batch;

            if (image != null)
            {
                string path = Path.Combine(_env.WebRootPath,
                    "admin", "images", "student", image.FileName);

                using var fs = new FileStream(path, FileMode.Create);
                image.CopyTo(fs);

                student.Image = image.FileName;
            }

            _context.SaveChanges();
            return RedirectToAction("FetchStudent");
        }


        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);

            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction("FetchStudent");
        }


        // ------------------------------------------------------------------------------

        // ===========================

        // Courses CRUD - Admin Only
        public IActionResult FetchCourses()
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2") // 1 = Admin
                return RedirectToAction("AccessDenied");

            var courses = _context.Courses.Include(c => c.Faculty).ToList();
            return View(courses);
        }

        public IActionResult CreateCourse()
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            ViewBag.Faculties = _context.Faculties.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateCourse(Course c)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            _context.Courses.Add(c);
            _context.SaveChanges();
            return RedirectToAction("FetchCourses");
        }

        public IActionResult UpdateCourse(int id)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            ViewBag.Faculties = _context.Faculties.ToList();
            return View(course);
        }

        [HttpPost]
        public IActionResult UpdateCourse(Course c)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            var course = _context.Courses.Find(c.CourseId);
            if (course == null)
                return NotFound();

            course.CourseName = c.CourseName;
            course.FacultyId = c.FacultyId;
            course.Duration = c.Duration;
            course.AvailableSeats = c.AvailableSeats;

            _context.SaveChanges();
            return RedirectToAction("FetchCourses");
        }

        public IActionResult DeleteCourse(int id)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("Courses");
        }

        // ------------------------------------------------------------------------------
        // Attendance CRUD

        public IActionResult FetchAttendance()
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" && role != "3") // 1 = Admin and 2 = Faculty
                return RedirectToAction("AccessDenied");

            var attendace = _context.Attendances.Include(a => a.Student).Include(c => c.Course).ToList();
            return View(attendace);
        }

        public IActionResult CreateAttendance()
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" && role != "3")
                return RedirectToAction("AccessDenied");

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();
          
            return View();
        }

        [HttpPost]
        public IActionResult CreateAttendance(Attendance a)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" && role != "3")
                return RedirectToAction("AccessDenied");

            _context.Attendances.Add(a);
            _context.SaveChanges();
            return RedirectToAction("FetchAttendance");
        }

        public IActionResult UpdateAttendance(int id)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" && role != "3")
                return RedirectToAction("AccessDenied");

            //var attendance = _context.Attendances
            //    .Include(a => a.Student)
            //    .Include(a => a.Course)
            //    .FirstOrDefault(a => a.AttendanceId == id);
            var attendance = _context.Attendances.Find(id);

            if (attendance == null)
                return NotFound();

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(attendance);
        }


        [HttpPost]
        public IActionResult UpdateAttendance(Attendance a)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" && role != "3")
                return RedirectToAction("AccessDenied");

            var attendance = _context.Attendances.Find(a.AttendanceId);
            if (attendance == null)
                return NotFound();

            attendance.StudentId = a.StudentId;
            attendance.CourseId = a.CourseId;
            attendance.AttendanceDate = a.AttendanceDate;
            attendance.Status = a.Status;

            _context.SaveChanges();
            return RedirectToAction("FetchAttendance");
        }

    
        public IActionResult DeleteAttendance(int id)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" && role != "3")
                return RedirectToAction("AccessDenied");

            var attendance = _context.Attendances.Find(id);
            if (attendance == null)
                return NotFound();

            _context.Attendances.Remove(attendance);
            _context.SaveChanges();
            return RedirectToAction("FetchAttendance");
        }

        // ------------------------------------------------------------------------------
        // Enrollement CRUD
        public IActionResult FetchEnrollment()
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" ) // 1 = Admin 
                return RedirectToAction("AccessDenied");

            var enrollments = _context.Enrollments.Include(a => a.Student).Include(c => c.Course).ToList();
            return View(enrollments);
        }


        public IActionResult CreateEnrollment()
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateEnrollment(Enrollment e)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            _context.Enrollments.Add(e);
            _context.SaveChanges();
            return RedirectToAction("FetchEnrollment");
        }

        public IActionResult UpdateEnrollment(int id)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2" )
                return RedirectToAction("AccessDenied");

            //var enrollment = _context.Enrollments
            //    .Include(a => a.Student)
            //    .Include(a => a.Course)
            //    .FirstOrDefault(a => a.EnrollmentId == id);


            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
                return NotFound();

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(enrollment);
        }


        [HttpPost]
        public IActionResult UpdateEnrollment(Enrollment e)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            var enrollment = _context.Enrollments.Find(e.EnrollmentId);
            if (enrollment == null)
                return NotFound();

            enrollment.StudentId = e.StudentId;
            enrollment.CourseId = e.CourseId;
            enrollment.EnrollmentDate = e.EnrollmentDate;


            _context.SaveChanges();
            return RedirectToAction("FetchEnrollment");
        }


        public IActionResult DeleteEnrollment(int id)
        {
            var role = HttpContext.Session.GetString("role_id");
            if (role != "2")
                return RedirectToAction("AccessDenied");

            var enrollment = _context.Enrollments.Find(id);
            if (enrollment == null)
                return NotFound();

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
            return RedirectToAction("FetchEnrollment");
        }


        // ------------------------------------------------------------------------------
    }
}
