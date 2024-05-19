using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.Categories;
using TeamFinder.Models.DTO.Teams;
using TeamFinder.Models.DTO.Updates;

namespace TeamFinder.Models.DTO.Activities;

public class ActivityDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? OpenRegistration { get; set; }
    public int? MaxParticipant { get; set; }
    public string? UrlHandle { get; set; }
    public UserResponseDto? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<UpdateDto>? Updates { get; set; }
    public List<CategoryDto>? Categories { get; set; } = new List<CategoryDto>();
    public List<TeamDto>? Teams { get; set; } = new List<TeamDto>();
}