using BlogSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace BlogSite.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Tags> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var tags = new List<Tags> {
                new Tags { Id=Guid.NewGuid(), Title = "technology" },
                new Tags { Id=Guid.NewGuid(), Title = "data science" },
                new Tags { Id=Guid.NewGuid(), Title = "machine learning" }
            };
            builder.Entity<Tags>().HasData(tags);

            base.OnModelCreating(builder);
        }
    }
}
