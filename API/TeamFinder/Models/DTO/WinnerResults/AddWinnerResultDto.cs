namespace TeamFinder.Models.DTO.WinnerResults
{
    public class AddWinnerResultDto
    {
        public Guid ActivityId { get; set; }
        public List<TeamOrderDto> Teams { get; set; }
    }

    public class TeamOrderDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
    }
}
