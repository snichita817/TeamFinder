using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Repositories.Implementation
{
    public class UpdateRepository : IUpdateRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UpdateRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Update> CreateAsync(Update update)
        {
            await _dbContext.Updates.AddAsync(update);
            await _dbContext.SaveChangesAsync();

            return update;
        }

        public async Task<IEnumerable<Update>> GetAllAsync()
        {
            return await _dbContext.Updates.ToListAsync();
        }

        public async Task<Update?> DeleteAsync(Guid id)
        {
            var existingUpdate = await _dbContext.Updates.FirstOrDefaultAsync(u => u.Id == id);
            
            if (existingUpdate == null) {
                return null;
            }
            
            _dbContext.Updates.Remove(existingUpdate);
            await _dbContext.SaveChangesAsync();
            return existingUpdate;
        }
    }
}
