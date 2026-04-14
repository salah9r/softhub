using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftHub.Models;
namespace SoftHub.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}
