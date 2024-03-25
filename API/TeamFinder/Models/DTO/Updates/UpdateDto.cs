namespace TeamFinder.Models.DTO.Updates
{
    public class UpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ActivityId { get; set; }
    }
}
