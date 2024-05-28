namespace TeamFinder.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Update Update { get; set; }
        public ApplicationUser User { get; set; }
    }
}
