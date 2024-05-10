using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface ITeamRepository
    {
        Task<Team> CreateAsync(Team team);
    }
}
