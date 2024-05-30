using TeamFinder.Models;

namespace TeamFinder.Repositories.Interface;

public interface IActivityRepository
{
    Task<Activity> CreateAsync(Activity activity);

    Task<Activity?> GetActivityAsync(Guid id);

    Task<IEnumerable<Activity>> GetAllActivities(string? query = null, string? filter = null);

    Task<Activity?> EditActivity(Activity activity);

    Task<Activity?> DeleteActivity(Guid id);
}