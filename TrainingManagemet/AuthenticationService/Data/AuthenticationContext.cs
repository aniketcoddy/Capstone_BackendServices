using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;
using System.Collections.Generic;

namespace AuthenticationService.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}