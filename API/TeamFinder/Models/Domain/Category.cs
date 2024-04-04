namespace TeamFinder.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Activity> Activities { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
