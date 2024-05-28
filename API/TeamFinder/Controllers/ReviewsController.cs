using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.Reviews;
using TeamFinder.Repositories.Interface;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(IReviewRepository reviewRepository, UserManager<ApplicationUser> userManager)
    {
        _reviewRepository = reviewRepository;
        _userManager = userManager;
    }

    [HttpGet("{organizerId}")]
    public async Task<IActionResult> GetReviews(string organizerId)
    {
        var reviews = await _reviewRepository.GetReviewsByOrganizerIdAsync(organizerId);

        List<ReviewDto> result = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            result.Add(await BuildReviewDto(review));
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostReview([FromBody] AddReviewDto reviewDto)
    {
        var organizer = await _userManager.FindByIdAsync(reviewDto.OrganizerId);
        var review = new Review
        {
            Content = reviewDto.Content,
            Rating = reviewDto.Rating,
            Date = DateTime.UtcNow,
            Organizer = organizer
        };

        var addedReview = await _reviewRepository.AddReviewAsync(review);

        if (addedReview == null)
            return BadRequest("Failed to add review");

        var response = await BuildReviewDto(addedReview);

        return Ok(response);
    }

    #region Helper Methods
    private async Task<ReviewDto> BuildReviewDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            Content = review.Content,
            Rating = review.Rating,
            Date = review.Date,
            Organizer = new UserResponseDto
            {
                Id = review.Organizer.Id,
                Email = review.Organizer.Email,
                UserName = review.Organizer.UserName,
                Roles = (List <string>) await _userManager.GetRolesAsync(review.Organizer)
            }
        };
    }
    #endregion
}
