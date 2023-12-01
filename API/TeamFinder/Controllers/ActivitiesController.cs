using Microsoft.AspNetCore.Mvc;
using TeamFinder.Data;
using TeamFinder.Models;
using TeamFinder.Models.DTO;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ActivitiesController : Controller
{
    private readonly IActivityRepository _activityRepository;

    public ActivitiesController(IActivityRepository activityRepository)
    {
        this._activityRepository = activityRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateActivity(CreateActivityRequestDto request)
    {
        // Map DTO to Domain Model
        var activity = new Activity
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OpenRegistration = request.OpenRegistration,
            MaxParticipant = request.MaxParticipant,
            UrlHandle = request.UrlHandle,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.Now
        };

        await _activityRepository.CreateAsync(activity);
        
        // From Domain Model to DTO
        var response = new ActivityDto
        {
            Id = activity.Id,
            Title = activity.Title,
            ShortDescription = activity.ShortDescription,
            LongDescription = activity.LongDescription,
            StartDate = activity.StartDate,
            EndDate = activity.EndDate,
            OpenRegistration = activity.OpenRegistration,
            MaxParticipant = activity.MaxParticipant,
            UrlHandle = activity.UrlHandle,
            CreatedBy = activity.CreatedBy,
            CreatedDate = activity.CreatedDate
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetActivity([FromRoute] Guid id)
    {
        // Get activity from database
        var activity = await _activityRepository.GetActivityAsync(id);

        if (activity is null)
        {
            return NotFound();
        }
        
        // Convert Domain Model to DTO
        var response = new ActivityDto
        {
            Id = activity.Id,
            Title = activity.Title,
            ShortDescription = activity.ShortDescription,
            LongDescription = activity.LongDescription,
            StartDate = activity.StartDate,
            EndDate = activity.EndDate,
            OpenRegistration = activity.OpenRegistration,
            MaxParticipant = activity.MaxParticipant,
            UrlHandle = activity.UrlHandle,
            CreatedBy = activity.CreatedBy,
            CreatedDate = activity.CreatedDate
        };

        return Ok(response);
    }
}