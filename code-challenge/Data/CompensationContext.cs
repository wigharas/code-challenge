using challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace challenge.Data
{
    public class CompensationContext : DbContext
    {
        public CompensationContext(DbContextOptions<CompensationContext> options): base(options)
        { }
        
        public DbSet<Compensation> Compensations { get; set; }
    }
}
