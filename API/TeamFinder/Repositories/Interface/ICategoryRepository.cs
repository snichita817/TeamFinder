using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<Category?> GetAsync(Guid id);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> DeleteAsync(Guid id);

        Task<Category?> EditCategory(Category category);
    }
}
