using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.Models;

namespace UniversityApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}
