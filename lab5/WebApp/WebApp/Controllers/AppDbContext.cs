using DrinksApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Drink> Drinks { get; set; }  // Таблица для напитков
    }
}
