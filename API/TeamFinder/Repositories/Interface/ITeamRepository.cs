using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface ITeamRepository
    {
        Task<Team> CreateAsync(Team team);

        Task<Team?> GetTeamByIdAsync(Guid id);

        Task<IEnumerable<Team>> GetAllTeamsAsync();

        Task<IEnumerable<Team>> GetTeamsByActivityId(Guid activityId);

        Task<Team?> EditTeam(Team team);

        Task<Team?> DeleteTeam(Guid id);
    }
}
