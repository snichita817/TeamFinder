using TeamFinder.Models.DTO.Categories;

namespace TeamFinder.Models.DTO.Auth
{
    public class UserProfileResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? University { get; set; }
        public string? CourseOfStudy { get; set; }
        public int? GraduationYear { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? Skills { get; set; }
        public List<CategoryDto>? Categories { get; set; } = new List<CategoryDto>();
        public string? PortfolioUrl { get; set; }
        public float? Rating { get; set; }
    }
}
