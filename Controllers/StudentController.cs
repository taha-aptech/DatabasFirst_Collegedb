using Microsoft.AspNetCore.Mvc;

namespace DatabasFirst_Collegedb.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
