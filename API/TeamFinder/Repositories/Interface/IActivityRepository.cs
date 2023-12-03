using TeamFinder.Models;

namespace TeamFinder.Repositories.Interface;

public interface IActivityRepository
{
    Task<Activity> CreateAsync(Activity activity);

    Task<Activity?> GetActivityAsync(Guid id);

    Task<IEnumerable<Activity>> GetAllActivities();

    Task<Activity?> EditActivity(Activity activity);

    Task<Activity?> DeleteActivity(Guid id);
}