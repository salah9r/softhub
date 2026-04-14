using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SoftHub.Data;
using System.Linq;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Dashboard()
    {
        ViewBag.Users = _context.Users.Count();
        ViewBag.Softwares = _context.Softwares.Count();
        ViewBag.Downloads = _context.Softwares.Sum(s => s.DownloadCount);
        ViewBag.Visits = _context.Visits.Count();

        var visitsData = _context.Visits
            .ToList()
            .Where(v => v.Date >= DateTime.Now.AddDays(-7))
            .GroupBy(v => v.Date.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToList();

        ViewBag.Dates = visitsData.Select(x => x.Date.ToString("MM-dd")).ToList();
        ViewBag.Counts = visitsData.Select(x => x.Count).ToList();

        return View();
    }

}