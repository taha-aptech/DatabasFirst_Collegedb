using DatabasFirst_Collegedb.Data;
using DatabasFirst_Collegedb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using System;

namespace Auth.Controllers
{
    public class UserController : Controller
    {
        private readonly MyContext _context;

        public UserController(MyContext context)
        {
            _context = context;
        }

        // Home Page
        //public IActionResult Index()
        //{
        //    return View();
        //}


        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("user_id");
            if (user != null)
            {
                ViewBag.user_name = HttpContext.Session.GetString("user_name");
                return View();
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


            user.RoleId = 2;


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

            //if(row != null && row.password == log_password)
            //{
            //    HttpContext.Session.SetString("user_session", row.id.ToString());
            //    return RedirectToAction("Index");
            //}

            if (row != null)
            {
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(row, row.Password, log_password);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("user_id", row.UserId.ToString());
                    HttpContext.Session.SetString("user_id", row.RoleId.ToString());
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




        public IActionResult FacultyPanel()
        {
            var role = HttpContext.Session.GetString("user_role");

            if (role != "1")
            {
                return RedirectToAction("AccessDenied");
            }


            return View();
        }


        public IActionResult StudentPanel()
        {
            var role = HttpContext.Session.GetString("user_role");

            if (role != "2")
            {
                return RedirectToAction("AccessDenied");
            }

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }

}
