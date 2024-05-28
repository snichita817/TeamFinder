using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsByOrganizerIdAsync(string organizerId);
        Task<Review> AddReviewAsync(Review review);
    }
}
