using BulkyWeb_Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb_Razor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Sci-Fi", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Fantasy", DisplayOrder = 2});
        }
    }
}
