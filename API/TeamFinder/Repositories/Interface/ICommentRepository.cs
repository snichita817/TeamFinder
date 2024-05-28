using TeamFinder.Models.Domain;

namespace TeamFinder.Repositories.Interface
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByUpdateIdAsync(Guid updateId);
        Task<Comment> AddCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Guid commentId);
    }
}
