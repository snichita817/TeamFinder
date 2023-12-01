using Microsoft.EntityFrameworkCore;
using TeamFinder.Models;

namespace TeamFinder.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
}