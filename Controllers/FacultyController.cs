using Microsoft.AspNetCore.Mvc;

namespace DatabasFirst_Collegedb.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult FacultyIndex()
        {
            return View();
        }
    }
}
