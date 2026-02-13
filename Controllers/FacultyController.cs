using DatabasFirst_Collegedb.Data;
using Microsoft.AspNetCore.Mvc;

namespace DatabasFirst_Collegedb.Controllers
{
    public class FacultyController : Controller
    {
        private readonly MyContext _context;

        public FacultyController(MyContext context)
        {
            _context = context;
        }

        public IActionResult FacultyIndex()
        {
            return View();
        }
    }
}
