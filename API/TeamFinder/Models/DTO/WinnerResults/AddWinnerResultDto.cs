namespace TeamFinder.Models.DTO.WinnerResults
{
    public class AddWinnerResultDto
    {
        public Guid ActivityId { get; set; }
        public List<Guid> TeamIds { get; set; }
    }
}
