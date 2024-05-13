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
        public bool AcceptedToActivity { get; set; }
        public bool IsPrivate { get; set; }

        public List<UserResponseDto>? Members { get; set; }
    }
}
