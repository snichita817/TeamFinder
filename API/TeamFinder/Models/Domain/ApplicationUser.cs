using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace TeamFinder.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        // Personal Info
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? University { get; set; } // Or Institution
        public string? CourseOfStudy { get; set; } // e.g., Computer Science
        public int? GraduationYear { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // Professional Info
        public string? Bio { get; set; } // A short bio about the student
        public string? LinkedInUrl { get; set; } // Optional
        public string? GitHubUrl { get; set; } // Optional, for showcasing projects

        // Skills & Interests
        public string? Skills { get; set; } // Could be further normalized into a separate table if needed
        // Portfolio
        // Consider a separate table for project links if they are numerous
        public string? PortfolioUrl { get; set; } // Optional

        public ICollection<Category> Categories { get; set; }
        public ICollection<ApplicationUser>? CreatedActivities { get; set; }
        public ICollection<Team>? Teams { get; set; }
        public ICollection<OrganizerApplication>? OrganizerApplications { get; set; }
    }
}
