using TeamFinder.Models.DTO.Auth;

namespace TeamFinder.Models.DTO.Reviews
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public UserResponseDto Organizer { get; set; }
    }
}
