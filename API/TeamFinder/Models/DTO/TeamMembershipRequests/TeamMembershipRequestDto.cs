using System.Text.Json.Serialization;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.Teams;

namespace TeamFinder.Models.DTO.TeamMembershipRequests
{
    public class TeamMembershipRequestDto
    {
        public Guid Id { get; set; }
        public UserResponseDto User { get; set; }
        public TeamDto Team { get; set; }
        public DateTime RequestDate { get; set; }
        public enum RequestStatus
        {
            Pending,
            Accepted,
            Rejected
        }
        public string Status { get; set; }
    }
}
