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
    }
}
