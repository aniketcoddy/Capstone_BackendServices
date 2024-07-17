using CourseService.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Data
{
    public class CourseContext : DbContext
    {
        public CourseContext(DbContextOptions<CourseContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
    }
}
