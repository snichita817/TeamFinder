using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface IUpdateRepository
    {
        Task<Update> CreateAsync(Update update);

        Task<IEnumerable<Update>> GetAllAsync();

        Task<Update?> DeleteAsync(Guid id);
    }
}
