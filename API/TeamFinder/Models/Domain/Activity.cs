using System.ComponentModel.DataAnnotations;
using TeamFinder.Models.Domain;

namespace TeamFinder.Models;

public class Activity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool OpenRegistration { get; set; }
    public int MaxTeams { get; set; }
    public int MinParticipant { get; set; }
    public int MaxParticipant { get; set; }
    public string UrlHandle { get; set; }
    // Here will be the user that created the Activity
    public ApplicationUser CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<Update> Updates { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<Team> Teams { get; set; }
}