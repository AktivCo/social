using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SocialPackage.Infrastructure
{
    public class EfDbContext : DbContext
    {
        public EfDbContext()
            : base("name=SocialPackage.Infrastructure.EfDbContext")
        {
            //  this.Configuration.ProxyCreationEnabled = false;

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Limit> Limit { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasOptional(e => e.Limit);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles).WithMany(r => r.Users)
                .Map(mc =>
                {
                    mc.MapLeftKey("UserId");
                    mc.MapRightKey("RoleId");
                    mc.ToTable("UsersRoles");
                });
        }
    }
}