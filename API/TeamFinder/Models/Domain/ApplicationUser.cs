using Microsoft.AspNetCore.Identity;

namespace TeamFinder.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        // Personal Info
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? University { get; set; } // Or Institution
        public string? CourseOfStudy { get; set; } // e.g., Computer Science
        public int? GraduationYear { get; set; }

        // Professional Info
        public string? Bio { get; set; } // A short bio about the student
        public string? LinkedInUrl { get; set; } // Optional
        public string? GitHubUrl { get; set; } // Optional, for showcasing projects

        // Skills & Interests
        public string? Skills { get; set; } // Could be further normalized into a separate table if needed
        public string? Interests { get; set; } // E.g., Web Development, AI, etc.

        // Portfolio
        // Consider a separate table for project links if they are numerous
        public string? PortfolioUrl { get; set; } // Optional
    }
}
