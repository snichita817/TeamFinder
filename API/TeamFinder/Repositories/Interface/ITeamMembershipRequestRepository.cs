using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface ITeamMembershipRequestRepository
    {
        Task<TeamMembershipRequest> CreateTeamMembershipRequestAsync(TeamMembershipRequest request);
        Task<IEnumerable<TeamMembershipRequest>> GetTeamMembershipRequestAsync(Guid teamId, TeamMembershipRequest.RequestStatus requestStatus);
        Task<bool> AcceptTeamMembershipRequestAsync(Guid requestId);
        Task<bool> RejectTeamMembershipRequestAsync(Guid requestId);
    }
}
