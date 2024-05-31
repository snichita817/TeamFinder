using TeamFinder.Models.DTO.Teams;

namespace TeamFinder.Models.DTO.WinnerResults
{
    public class WinnerResultDto
    {
        public Guid Id { get; set; }
        public List<TeamDto> Teams { get; set; }
    }
}
