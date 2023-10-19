using GroupProjectDeployment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace GroupProjectDeployment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroupProjectDeployment.Models.Product> Products { get; set; }
        public DbSet<GroupProjectDeployment.Models.Review> Reviews { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Review>().HasKey(r => r.Id);

            modelBuilder.Entity<Product>()
            .HasMany(p => p.Reviews)
            .WithOne(r => r.Product);

            modelBuilder.Entity<ApplicationUser>().HasKey(p => p.Id);
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Review>().HasKey(r => r.Id);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId); // Assuming the foreign key property in Review is named ProductId
        }
    }
}