using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface IWinnerResultRepository
    {
        Task<WinnerResult> CreateAsync(WinnerResult winnerResult);
    }
}
