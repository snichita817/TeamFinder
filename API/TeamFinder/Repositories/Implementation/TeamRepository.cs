﻿using iText.Commons.Actions.Contexts;
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

        public async Task<IEnumerable<Team>> GetAllTeamsAsync(string? query)
        {
            var teams = _dbContext.Teams.Include(x => x.ActivityRegistered).AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                teams = teams.Where(t => t.Name.Contains(query));
            }
            teams = teams.Where(t => t.AcceptedToActivity != RequestStatus.Rejected);

            return await teams.Include(x => x.Members).Include(x => x.ActivityRegistered).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByActivityId(Guid activityId, string? query)
        {
            var teams = _dbContext.Teams.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                teams = teams.Where(t => t.Name.Contains(query));
            }

            // Pagination
            return await teams.Include(x => x.Members).Include(x => x.ActivityRegistered).Where(t => t.ActivityRegistered.Id == activityId).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetActivityUreviewedTeams(Guid activityId, string? query)
        {
            var teams = _dbContext.Teams.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                teams = teams.Where(t => t.Name.Contains(query));
            }

            return await teams
                .Include(x => x.Members).Include(x => x.ActivityRegistered).Where(t => t.ActivityRegistered.Id == activityId && t.AcceptedToActivity == RequestStatus.Pending).ToListAsync();

            //return await _dbContext.Teams.Include(x => x.Members).Include(x => x.ActivityRegistered).Where(t => t.ActivityRegistered.Id == activityId && t.AcceptedToActivity == RequestStatus.Pending).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByUserId(string userId)
        {
            return await _dbContext.Teams
                .Include(t => t.Members)
                .Where(t => t.Members.Any(m => m.Id == userId))
                .ToListAsync();
        }

        public async Task<Team> AcceptTeam(Guid teamId)
        {
            var team = await _dbContext.Teams.Include(x => x.Members).Include(x => x.ActivityRegistered).FirstOrDefaultAsync(t => t.Id == teamId);
            
            if (team == null)
            {
                throw new Exception("Team not found");
            }

            var acceptedTeams = team.ActivityRegistered.Teams.Where(t => t.AcceptedToActivity == RequestStatus.Accepted).ToList();
            if(acceptedTeams.Count >= team.ActivityRegistered.MaxTeams)
            {
                throw new Exception("Activity has reached maximum number of accepted Teams!");
            }
            
            team.AcceptedToActivity = RequestStatus.Accepted;
            await _dbContext.SaveChangesAsync();
            return team;
        }

        public async Task<Team> RejectTeam(Guid teamId)
        {
            var team = await _dbContext.Teams.Include(x => x.Members).Include(x => x.ActivityRegistered).FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
            {
                throw new Exception("Team not found");
            }

            team.AcceptedToActivity = RequestStatus.Rejected;
            await _dbContext.SaveChangesAsync();
            return team;
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
            var existingTeam = await _dbContext.Teams.Include(x => x.Members).Include(x=>x.ActivityRegistered).FirstOrDefaultAsync(t => t.Id == id);

            if (existingTeam == null)
            {
                return null;
            }

            _dbContext.Teams.Remove(existingTeam);
            await _dbContext.SaveChangesAsync();
            return existingTeam;
        }

        public async Task<Team?> GetUserTeam(Guid activityId, string userId)
        {
            return await _dbContext.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Members.Any(m => m.Id == userId) && t.ActivityRegistered.Id == activityId);
        }
    }
}
