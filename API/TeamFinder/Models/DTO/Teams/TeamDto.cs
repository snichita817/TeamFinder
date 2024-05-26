using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Activities;
using TeamFinder.Models.DTO.Auth;

namespace TeamFinder.Models.DTO.Teams
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AcceptedToActivity { get; set; }
        public bool IsPrivate { get; set; }
        public string TeamCaptainId { get; set; }
        public string? SubmissionUrl { get; set; }

        public ActivityDto? ActivityRegistered { get; set; }
        public List<UserResponseDto>? Members { get; set; }
    }
}
