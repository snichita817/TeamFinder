using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Categories)
            .WithMany(c => c.ApplicationUsers)
            .UsingEntity(j => j.ToTable("ApplicationUserCategory"));
    }*/
}