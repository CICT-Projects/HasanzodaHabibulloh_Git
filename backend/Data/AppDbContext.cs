using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Tire> Tires { get; set; }
    public DbSet<Oil> Oils { get; set; }
    public DbSet<Battery> Batteries { get; set; }
    public DbSet<Filter> Filters { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table-per-hierarchy: все типы запчастей в одной таблице с дискриминатором
        modelBuilder.Entity<BasePart>()
            .HasDiscriminator<string>("part_type")
            .HasValue<Tire>("tire")
            .HasValue<Oil>("oil")
            .HasValue<Battery>("battery")
            .HasValue<Filter>("filter");
    }
}
