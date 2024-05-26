namespace TeamFinder.Models.DTO.Auth
{
    public class EditUserRequestDto
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
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
        public string? PortfolioUrl { get; set; }

        public List<Guid>? Categories { get; set; }
    }
}
