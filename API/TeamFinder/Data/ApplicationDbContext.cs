using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TeamFinder.Models;
using TeamFinder.Models.Domain;

namespace TeamFinder.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Update> Updates { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Team> Teams { get; set; }
}