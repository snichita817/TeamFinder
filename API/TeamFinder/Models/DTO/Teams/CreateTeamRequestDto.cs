namespace TeamFinder.Models.DTO.Teams
{
    public class CreateTeamRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AcceptedToActivity { get; set; }
        public bool IsPrivate { get; set; }

        public string CreatedBy { get; set; }
        public string ActivityRegistered { get; set; }
        public string[] Members { get; set; }

    }
}
