namespace TeamFinder.Models.Domain
{
    public class OrganizerApplication
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Reason { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
