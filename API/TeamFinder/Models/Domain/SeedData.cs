using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;

namespace TeamFinder.Models.Domain
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService
                <DbContextOptions<ApplicationDbContext>>()))
            {
                // If there are roles in the database, don't seed
                if(context.Roles.Any())
                {
                    return;
                }

                var userRoleId = "298f9218-3e3a-4b21-9198-3cf08a40f191";
                var organizerRoleId = "5a0f7a7e-c62f-49db-bc0f-9b0061917e77";
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
                        Id = organizerRoleId,
                        Name = "Organizer",
                        NormalizedName = "Organizer".ToUpper(),
                        ConcurrencyStamp = organizerRoleId
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
                context.Roles.AddRange(roles);
                //builder.Entity<IdentityRole>().HasData(roles);

                var adminId = "66a83869-d054-4385-8f6f-2ad64ba78e3c";

                // Create an Admin
                var admin = new ApplicationUser
                {
                    Id = adminId,
                    UserName = "admin@teamfinder.com",
                    Email = "admin@teamfinder.com",
                    NormalizedEmail = "admin@teamfinder.com".ToUpper(),
                    NormalizedUserName = "admin@teamfinder.com".ToUpper(),
                    Categories = new List<Category>()
                };
                admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin@123");

                // Seed the Admin
                context.ApplicationUsers.Add(admin);

                // builder.Entity<ApplicationUser>().HasData(admin);

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

                // Seed the Admin Roles
                context.UserRoles.AddRange(adminRoles);
                context.SaveChanges();
                // builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
            }
        }
    }
}
