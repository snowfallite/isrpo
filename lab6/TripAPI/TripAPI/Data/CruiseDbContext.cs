using System.Collections.Generic;
using System.Reflection.Emit;
using CruiseCompany.Models;
using Microsoft.EntityFrameworkCore;


namespace CruiseCompany.Data;

public class CruiseDbContext : DbContext
{
    public CruiseDbContext(DbContextOptions<CruiseDbContext> options) : base(options) { }

    public DbSet<CruiseRoute> CruiseRoutes => Set<CruiseRoute>();
    public DbSet<Liner> Liners => Set<Liner>();
    public DbSet<RouteLiner> RouteLiners => Set<RouteLiner>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RouteLiner>()
            .HasKey(rl => new { rl.RouteId, rl.LinerId });

        modelBuilder.Entity<RouteLiner>()
            .HasOne(rl => rl.Route)
            .WithMany(r => r.RouteLiners)
            .HasForeignKey(rl => rl.RouteId);

        modelBuilder.Entity<RouteLiner>()
            .HasOne(rl => rl.Liner)
            .WithMany(l => l.RouteLiners)
            .HasForeignKey(rl => rl.LinerId);
    }
}
