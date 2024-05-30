﻿using TeamFinder.Data;
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
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Activity>> GetAllActivities(string? query = null, string? filter = null)
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

        // Pagination

        return await activities
            .Include(c => c.Categories)
            .Include(x => x.Updates)
            .Include(x => x.CreatedBy)
            .Include(x => x.Teams)
            .ToListAsync();
    }

    public async Task<Activity?> EditActivity(Activity activity)
    {
        var existingActivity = await _dbContext.Activities.Include(c => c.Categories)
            .FirstOrDefaultAsync(act => act.Id == activity.Id);

        if (existingActivity != null)
        {
            existingActivity.Categories = activity.Categories;
            _dbContext.Entry(existingActivity).CurrentValues.SetValues(activity);
            await _dbContext.SaveChangesAsync();
            return activity;
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