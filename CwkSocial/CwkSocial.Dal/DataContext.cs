using Cwk.Domain.Aggregates.PostAggregate;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Dal.Configurations;
using CwkSocial.Dal.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Dal
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)    
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
            base.OnModelCreating(modelBuilder);
        }
    }
}
