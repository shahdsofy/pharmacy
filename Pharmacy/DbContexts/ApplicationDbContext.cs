using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;

namespace Pharmacy.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

    }
}
