﻿using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Repositories.Implementation
{
    public class TeamMembershipRequestRepository : ITeamMembershipRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamMembershipRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TeamMembershipRequest> GetTeamMembershipRequestAsync(Guid requestId)
        {
            var request = await _context.TeamMembershipRequests
                .Include(mr => mr.User)
                .Include(mr => mr.Team)
                .FirstOrDefaultAsync(mr => mr.Id == requestId);
            if(request == null)
            {
                throw new Exception("Request not found");
            }

            return request;
        }

        public async Task<bool> AcceptTeamMembershipRequestAsync(Guid requestId)
        {
            var request = await _context.TeamMembershipRequests.Include(x=>x.Team).Include(x=>x.User).FirstOrDefaultAsync(mem => mem.Id == requestId);
            if (request == null) return false;

            request.Status = RequestStatus.Accepted;
            _context.TeamMembershipRequests.Update(request);

            // Add the user to the team
            var team = await _context.Teams.Include(x=>x.Members).Include(x => x.ActivityRegistered).FirstOrDefaultAsync(t => t.Id == request.Team.Id);
            if(team!= null)
            {
                var activityTeams = _context.Teams.Include(x=>x.Members).Where(t => t.ActivityRegistered.Id == team.ActivityRegistered.Id).ToList();
                foreach(var activityTeam in activityTeams)
                {
                    if (activityTeam.Members.Contains(request.User))
                    {
                        throw new Exception("User already in a team for this activity");
                    }
                }
            }

            var user = await _context.Users.FindAsync(request.User.Id);
            if (team != null && user != null)
            {
                team.Members.Add(user);
                _context.Teams.Update(team);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TeamMembershipRequest> CreateTeamMembershipRequestAsync(TeamMembershipRequest request)
        {
            // Fetch the team and include its TeamMembershipRequests collection
            var team = await _context.Teams
                                     .Include(t => t.TeamMembershipRequests)
                                     .FirstOrDefaultAsync(t => t.Id == request.Team.Id);

            if (team == null)
            {
                throw new Exception("Team not found");
            }

            // Ensure the user entity is attached correctly
            if (_context.Entry(request.User).State == EntityState.Detached)
            {
                _context.Attach(request.User);
            }

            // Add the membership request to the team's collection
            team.TeamMembershipRequests.Add(request);

            // Add the membership request to the context
            _context.TeamMembershipRequests.Add(request);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<IEnumerable<TeamMembershipRequest>> GetUserTeamMembershipRequests(string userId)
        {
            return await _context.TeamMembershipRequests
                .Where(mr => mr.User.Id == userId)
                .Include(mr => mr.User)
                .Include(mr => mr.Team)
                .ToListAsync();
        }

        public async Task<IEnumerable<TeamMembershipRequest>> GetTeamMembershipRequestAsync(Guid teamId, RequestStatus requestStatus, string? query)
        {
            var teams = _context.TeamMembershipRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                teams = teams.Where(t => t.User.UserName.Contains(query));
            }

            return await teams
                .Where(mr => mr.Team.Id == teamId && mr.Status == requestStatus)
                .Include(mr => mr.User)
                .Include(mr => mr.Team)
                .ToListAsync();
        }

        public async Task<bool>RejectTeamMembershipRequestAsync(Guid requestId)
        {
            var request = await _context.TeamMembershipRequests.FindAsync(requestId);
            if (request == null) return false;

            request.Status = RequestStatus.Rejected;
            _context.TeamMembershipRequests.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
