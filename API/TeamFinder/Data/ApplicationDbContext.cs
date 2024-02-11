using Microsoft.EntityFrameworkCore;
using TeamFinder.Models;
using TeamFinder.Models.Domain;

namespace TeamFinder.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Update> Updates { get; set; }
}