using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TeamFinder.Models.Domain
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public RequestStatus AcceptedToActivity { get; set; }
        public bool IsPrivate { get; set; }
        public Guid TeamCaptainId { get; set; }
        public string? SubmissionUrl { get; set; }

        public Activity ActivityRegistered { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        public ICollection<TeamMembershipRequest>? TeamMembershipRequests { get; set; } = new List<TeamMembershipRequest>();
    }
}
