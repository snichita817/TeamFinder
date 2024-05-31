using System.ComponentModel.DataAnnotations.Schema;

namespace TeamFinder.Models.Domain
{
    public class WinnerResult
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
