using Microsoft.EntityFrameworkCore;
using Task_3.Models;

namespace Task_3.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
