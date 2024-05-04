using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.Models;

namespace AuthorizationService.Data
{
    public class DataContext: IdentityDbContext<ApplicationUser, 
        AppRole, 
        string, 
        IdentityUserClaim<string>, 
        AppUserRole, 
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(userRoles => userRoles.UserRoles)
                .WithOne(user => user.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(userRoles => userRoles.UserRoles)
                .WithOne(user => user.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();
        }

            public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        }
}
