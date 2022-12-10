using Cwk.Domain.Aggregates.Friendships;
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
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BasicInfo>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
