using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetAsync(Guid id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(category is null)
            {
                return null;
            }

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category is null)
            {
                return null;
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
    
        public async Task<Category?> EditCategory(Category category)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if(existingCategory is null)
            {
                return null;
            }

            _dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
    }
}
