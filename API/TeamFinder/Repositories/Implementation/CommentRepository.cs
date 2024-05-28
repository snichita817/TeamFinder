using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUpdateIdAsync(Guid updateId)
    {
        return await _context.Comments
            .Where(c => c.Update.Id == updateId)
            .Include(c => c.User)
            .ToListAsync();
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return false;
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
}
