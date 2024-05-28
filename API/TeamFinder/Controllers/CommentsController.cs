using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Comments;
using TeamFinder.Repositories.Interface;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CommentsController(ICommentRepository commentRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _commentRepository = commentRepository;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("{updateId}")]
    public async Task<IActionResult> GetComments(Guid updateId)
    {
        var comments = await _commentRepository.GetCommentsByUpdateIdAsync(updateId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> PostComment([FromBody] AddCommentDto commentDto)
    {
        var user = await _userManager.FindByIdAsync(commentDto.UserId);
        if (user == null)
        {
            return Unauthorized();
        }

        var update = await _context.Updates.FindAsync(commentDto.UpdateId);
        if (update == null)
        {
            return NotFound();
        }

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Text = commentDto.Text,
            Update = update,
            User = user
        };

        var addedComment = await _commentRepository.AddCommentAsync(comment);
        return Ok(addedComment);
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var result = await _commentRepository.DeleteCommentAsync(commentId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}