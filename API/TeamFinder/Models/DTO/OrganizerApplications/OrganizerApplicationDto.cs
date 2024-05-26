using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Auth;

namespace TeamFinder.Models.DTO.OrganizerApplications
{
    public class OrganizerApplicationDto
    {
        public Guid Id { get; set; }
        public UserResponseDto User { get; set; }
        public string Reason { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
    }
}
