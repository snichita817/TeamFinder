using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Repositories.Implementation
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TeamRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Team> CreateAsync(Team team)
        {
            await _dbContext.Teams.AddAsync(team);
            await _dbContext.SaveChangesAsync();

            return team;
        }

        public async Task<Team?> GetTeamByIdAsync(Guid id)
        {
            return await _dbContext.Teams.Include(x => x.Members).Include(x=>x.TeamMembershipRequests).Include(x=>x.ActivityRegistered).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _dbContext.Teams.Include(x => x.Members).Include(x => x.ActivityRegistered).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByActivityId(Guid activityId)
        {
            return await _dbContext.Teams.Include(x => x.Members).Include(x=>x.ActivityRegistered).Where(t => t.ActivityRegistered.Id == activityId).ToListAsync();
        }

        public async Task<Team?> EditTeam(Team team)
        {
            var existingTeam = await _dbContext.Teams.Include(x => x.Members).FirstOrDefaultAsync(t => t.Id == team.Id);

            if (existingTeam != null)
            {
                existingTeam.Members = team.Members;
                _dbContext.Entry(existingTeam).CurrentValues.SetValues(team);
                await _dbContext.SaveChangesAsync();
                return team;
            }

            return null;
        }

        public async Task<Team?> DeleteTeam(Guid id)
        {
            var existingTeam = await _dbContext.Teams.Include(x => x.Members).FirstOrDefaultAsync(t => t.Id == id);

            if (existingTeam == null)
            {
                return null;
            }

            _dbContext.Teams.Remove(existingTeam);
            await _dbContext.SaveChangesAsync();
            return existingTeam;
        }
    }
}
