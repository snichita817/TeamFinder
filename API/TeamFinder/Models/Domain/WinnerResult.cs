using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TeamFinder.Models.Domain
{
    public class WinnerResult
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }

        public ICollection<Team> Teams { get; set; }

        public List<OrderedTeam> OrderedTeams { get; set; }
    }

    public class OrderedTeam
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public int Order { get; set; }
    }
}
