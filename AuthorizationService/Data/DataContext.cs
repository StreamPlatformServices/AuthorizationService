using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.Models;
using AuthorizationService.Entity;

namespace AuthorizationService.Data
{
    public class DataContext: IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().Ignore(e => e.AccessFailedCount);
            builder.Entity<AppUser>().Ignore(e => e.EmailConfirmed);
            builder.Entity<AppUser>().Ignore(e => e.LockoutEnabled);
            builder.Entity<AppUser>().Ignore(e => e.LockoutEnd);
            builder.Entity<AppUser>().Ignore(e => e.PhoneNumberConfirmed);
            builder.Entity<AppUser>().Ignore(e => e.SecurityStamp);
            builder.Entity<AppUser>().Ignore(e => e.TwoFactorEnabled);
            builder.Entity<AppUser>().Ignore(e => e.UserRoleEnum);

            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();
            builder.Ignore<IdentityUserRole<string>>();
            builder.Ignore<IdentityRole>();
            builder.Entity<AppUser>().ToTable("Users");
        }

        public DbSet<AppUser> ApplicationUsers { get; set; }
        }
}
