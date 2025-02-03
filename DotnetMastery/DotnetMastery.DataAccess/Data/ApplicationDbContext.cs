using Dotnet.Models;

using Microsoft.EntityFrameworkCore;

namespace DotnetMastery.DataAccess.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product> Products { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "action", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Sci", DisplayOrder = 1 },
            new Category { Id = 3, Name = "Physics", DisplayOrder = 3 }
            );
            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Title = "Box of Stone",
                Description = "Best Seller",
                ISBN = "4002",
                Author = "Dil bahadur",
                ListPrice = 99,
                Price=90,
                Price50=84,
                Price100=80

            });

             
        }
    }

}
