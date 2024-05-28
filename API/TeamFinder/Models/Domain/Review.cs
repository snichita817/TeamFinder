namespace TeamFinder.Models.Domain
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public ApplicationUser Organizer { get; set; }
    }
}