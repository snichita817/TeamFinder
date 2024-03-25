namespace TeamFinder.Models.DTO.Updates
{
    public class EditUpdateRequestDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ActivityId { get; set; }
    }
}
