using Microsoft.EntityFrameworkCore;
using EnrollmentService.Models;

namespace EnrollmentService.Data
{
    public class EnrollmentsContext : DbContext
    {
        public EnrollmentsContext(DbContextOptions<EnrollmentsContext> options)
            : base(options)
        {
        }

        public DbSet<Enrollment> Enrollments { get; set; }
    }
}
