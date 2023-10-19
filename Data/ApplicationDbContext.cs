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
    }
}