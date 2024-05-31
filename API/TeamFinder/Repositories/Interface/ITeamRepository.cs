using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface ITeamRepository
    {
        Task<Team> CreateAsync(Team team);

        Task<Team?> GetTeamByIdAsync(Guid id);

        Task<IEnumerable<Team>> GetAllTeamsAsync(string? query = null);

        Task<IEnumerable<Team>> GetTeamsByActivityId(Guid activityId, string? query = null);

        Task<IEnumerable<Team>> GetActivityUreviewedTeams(Guid activityId, string? query = null);

        Task<IEnumerable<Team>> GetTeamsByUserId(string userId);

        Task<Team> AcceptTeam(Guid teamId);

        Task<Team> RejectTeam(Guid teamId);

        Task<Team?> EditTeam(Team team);

        Task<Team?> DeleteTeam(Guid id);

        Task<Team?> GetUserTeam(Guid activityId, string userId);
    }
}
