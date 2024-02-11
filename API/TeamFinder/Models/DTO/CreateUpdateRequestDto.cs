namespace TeamFinder.Models.DTO
{
    public class CreateUpdateRequestDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ActivityId { get; set; }
    }
}
