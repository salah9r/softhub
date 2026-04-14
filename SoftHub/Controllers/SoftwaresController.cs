using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftHub.Data;
using SoftHub.Models;
using System.IO;
using System.Linq;

namespace SoftHub.Controllers
{
    public class SoftwaresController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        public SoftwaresController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // عرض كل البرامج
        public IActionResult Index()
        {
            var softwares = _context.Softwares.ToList();
            return View(softwares);
        }
        [Authorize] // 🔥 فقط للمسجلين
        public IActionResult Download(int id)
        {
            var software = _context.Softwares.FirstOrDefault(s => s.Id == id);

            if (software == null || string.IsNullOrEmpty(software.FilePath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_env.WebRootPath, software.FilePath.TrimStart('/'));

            var fileName = Path.GetFileName(filePath);

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }

        // تفاصيل البرنامج
        public IActionResult Details(int id)
        {
            var software = _context.Softwares.FirstOrDefault(s => s.Id == id);

            if (software == null)
                return NotFound();

            return View(software);
        }

        // صفحة الإضافة (Admin فقط)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Software software)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                // 🖼 رفع الصورة
                if (software.ImageFile != null)
                {
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(software.ImageFile.FileName);
                    string imagePath = Path.Combine(uploadsFolder, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        software.ImageFile.CopyTo(stream);
                    }

                    software.ImagePath = "/uploads/" + imageName;
                }

                // 📁 رفع الملف
                if (software.UploadFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(software.UploadFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        software.UploadFile.CopyTo(stream);
                    }

                    software.FilePath = "/uploads/" + fileName;
                }

                _context.Softwares.Add(software);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(software);
        }

        // حذف
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var software = _context.Softwares.Find(id);
            if (software != null)
            {
                _context.Softwares.Remove(software);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}