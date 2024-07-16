using Microsoft.EntityFrameworkCore;
using BatchService.Models;
using System.Collections.Generic;

namespace BatchService.Data
{
    public class BatchContext : DbContext
    {
        public BatchContext(DbContextOptions<BatchContext> options) : base(options) { }

        public DbSet<Batch> Batches { get; set; }
    }
}
