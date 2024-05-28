namespace TeamFinder.Models.DTO.Comments
{
    public class AddCommentDto
    {
        public string Text { get; set; }
        public Guid UpdateId { get; set; }
        public string UserId { get; set; }
    }
}
