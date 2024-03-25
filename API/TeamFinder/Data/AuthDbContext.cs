using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TeamFinder.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext() {}

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userRoleId = "298f9218-3e3a-4b21-9198-3cf08a40f191";
            var adminRoleId = "a84035d4-a82f-4d10-a979-7a40f209256c";

            // Creater User and Admin Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                },
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            var adminId = "66a83869-d054-4385-8f6f-2ad64ba78e3c";

            // Create an Admin
            var admin = new IdentityUser
            {
                Id = adminId,
                UserName = "admin@teamfinder.com",
                Email = "admin@teamfinder.com",
                NormalizedEmail = "admin@teamfinder.com".ToUpper(),
                NormalizedUserName = "admin@teamfinder.com".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give roles to the Admin
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new()
                {
                    UserId = adminId,
                    RoleId = userRoleId
                },
                new()
                {
                    UserId = adminId,
                    RoleId = adminRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
