using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Repositories.Implementation
{
    public class WinnerResultRepository : IWinnerResultRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WinnerResultRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WinnerResult> CreateAsync(WinnerResult winnerResult)
        {
            await _dbContext.WinnerResults.AddAsync(winnerResult);
            await _dbContext.SaveChangesAsync();
            return winnerResult;
        }
    }
}
