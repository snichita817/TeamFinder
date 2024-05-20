namespace TeamFinder.Models.Domain
{
    public class TeamMembershipRequest
    {
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }
        public Team Team { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
