namespace TeamFinder.Models.Domain
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool AcceptedToActivity { get; set; }
        public bool IsPrivate { get; set; }

        public ApplicationUser CreatedBy { get; set; }
        public Activity ActivityRegistered { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
    }
}
