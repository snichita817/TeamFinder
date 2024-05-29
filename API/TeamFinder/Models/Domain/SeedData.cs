using Mailjet.Client.Resources;
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
                var organizerId = "ac6d5c77-33fd-4793-a45a-2f7654cada69";
                var userId = "8340a2c0-e5b1-4917-b6f5-34e432e25446";

                // Create an Admin
                var admin = new ApplicationUser
                {
                    Id = adminId,
                    UserName = "admin@teamfinder.com",
                    Email = "admin@teamfinder.com",
                    NormalizedEmail = "admin@teamfinder.com".ToUpper(),
                    NormalizedUserName = "admin@teamfinder.com".ToUpper(),
                    Categories = new List<Category>(),
                    EmailConfirmed = true,
                };
                admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin@123");

                // Seed the Admin
                context.ApplicationUsers.Add(admin);

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

                context.UserRoles.AddRange(adminRoles);

                // Create an Organizer
                var organizer = new ApplicationUser
                {
                    Id = organizerId,
                    UserName = "organizer@teamfinder.com",
                    Email = "organizer@teamfinder.com",
                    NormalizedEmail = "organizer@teamfinder.com".ToUpper(),
                    NormalizedUserName = "organizer@teamfinder.com".ToUpper(),
                    Categories = new List<Category>(),
                    EmailConfirmed = true,
                };
                organizer.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(organizer, "Organizer@123");

                // Seed the Organizer
                context.ApplicationUsers.Add(organizer);

                // Give roles to the Organizer
                var organizerRoles = new List<IdentityUserRole<string>>
                {
                    new()
                    {
                        UserId = organizerId,
                        RoleId = userRoleId
                    },
                    new()
                    {
                        UserId = organizerId,
                        RoleId = organizerRoleId
                    }
                };

                // Seed the Organizer Roles
                context.UserRoles.AddRange(organizerRoles);

                // Create an User
                var user = new ApplicationUser
                {
                    Id = userId,
                    UserName = "user@teamfinder.com",
                    Email = "user@teamfinder.com",
                    NormalizedEmail = "user@teamfinder.com".ToUpper(),
                    NormalizedUserName = "user@teamfinder.com".ToUpper(),
                    Categories = new List<Category>(),
                    EmailConfirmed = true,
                };
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "User@123");

                // Seed the User
                context.ApplicationUsers.Add(user);

                // Give roles to the User
                var userRoles = new List<IdentityUserRole<string>>
                {
                    new()
                    {
                        UserId = userId,
                        RoleId = userRoleId
                    }
                };

                // Seed the Organizer Roles
                context.UserRoles.AddRange(userRoles);

                context.SaveChanges();
                // builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
            }
        }
    }
}
