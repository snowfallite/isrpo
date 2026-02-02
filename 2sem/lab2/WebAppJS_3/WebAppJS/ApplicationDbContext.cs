namespace WebAppJS
{
    using Microsoft.EntityFrameworkCore;
    using WebAppJS.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<SceneObject> SceneObjects { get; set; }
    }

}