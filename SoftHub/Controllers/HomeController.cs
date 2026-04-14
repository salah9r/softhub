using Microsoft.AspNetCore.Mvc;
using SoftHub.Data;
using SoftHub.Models;

namespace SoftHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; // 🔥 تعريف

        public HomeController(ApplicationDbContext context) // 🔥 Constructor
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // تسجيل زيارة
            _context.Visits.Add(new Visit());
            _context.SaveChanges();

            return View();
        }
    }
}