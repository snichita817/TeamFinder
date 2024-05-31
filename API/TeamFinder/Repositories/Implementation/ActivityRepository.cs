using TeamFinder.Data;
using TeamFinder.Models;
using TeamFinder.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace TeamFinder.Repositories.Implementation;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ActivityRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task<Activity> CreateAsync(Activity activity)
    {
        await _dbContext.Activities.AddAsync(activity);
        await _dbContext.SaveChangesAsync();

        return activity;
    }

    public async Task<Activity?> GetActivityAsync(Guid id)
    {
        return await _dbContext.Activities
            .Include(c => c.Categories)
            .Include(x => x.Updates)
            .Include(x => x.CreatedBy)
            .Include(x => x.Teams)
            .Include(x => x.WinnerResult)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Activity>> GetAllActivities(string? query = null, string? filter = null, string? organizerId = null)
    {
        // Query the database
        var activities = _dbContext.Activities.AsQueryable();

        // Filtering
        if(string.IsNullOrWhiteSpace(query) == false)
        {
            activities = activities.Where(a => a.Title.Contains(query));
        }

        var now = DateTime.Now;
        if (filter == "upcoming")
        {
            activities = activities.Where(a => a.StartDate > now);
        }
        else if (filter == "ended")
        {
            activities = activities.Where(a => a.EndDate < now);
        }

        if(string.IsNullOrWhiteSpace(organizerId) == false)
        {
            activities = activities
                .Where(a => a.CreatedBy.Id == organizerId)
                .OrderByDescending(x => x.EndDate);
        }

        return await activities
            .Include(c => c.Categories)
            .Include(x => x.Updates)
            .Include(x => x.CreatedBy)
            .Include(x => x.Teams)
            .ToListAsync();
    }

    public async Task<Activity?> EditActivity(Activity activity)
    {
        var existingActivity = await _dbContext.Activities
        .Include(c => c.Categories)
        .Include(t => t.Teams)  // Assuming Teams is the navigation property for the list of teams
        .FirstOrDefaultAsync(act => act.Id == activity.Id);

        if (existingActivity != null)
        {
            existingActivity.Categories = activity.Categories;
            _dbContext.Entry(existingActivity).CurrentValues.SetValues(activity);

            // Update the MinParticipant and MaxParticipant values of each Team in Activities
            if (existingActivity.Teams != null)
            {
                foreach (var team in existingActivity.Teams)
                {
                    var updatedTeam = existingActivity.Teams.FirstOrDefault(t => t.Id == team.Id);
                    if (updatedTeam != null)
                    {
                        updatedTeam.MinParticipant = existingActivity.MinParticipant;
                        updatedTeam.MaxParticipant = existingActivity.MaxParticipant;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return existingActivity;
        }

        return null;
    }

    public async Task<Activity?> DeleteActivity(Guid id)
    {
        var existingActivity = await _dbContext.Activities.Include(x => x.CreatedBy).Include(x => x.Teams).FirstOrDefaultAsync(a => a.Id == id);

        if (existingActivity == null)
        {
            return null;
        }

        _dbContext.Activities.Remove(existingActivity);
        await _dbContext.SaveChangesAsync();
        return existingActivity;
    }
}