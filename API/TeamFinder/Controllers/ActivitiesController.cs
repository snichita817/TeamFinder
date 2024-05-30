using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Activities;
using TeamFinder.Models.DTO.Updates;
using TeamFinder.Models.DTO.Categories;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.Teams;
using TeamFinder.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TeamFinder.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ActivitiesController : Controller
{
    private readonly IActivityRepository _activityRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ActivitiesController(IActivityRepository activityRepository,
        ICategoryRepository categoryRepository,
        UserManager<ApplicationUser> userManager)
    {
        this._activityRepository = activityRepository;
        this._categoryRepository = categoryRepository;
        this._userManager = userManager;
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Organizer")]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequestDto request)
    {
        // Map DTO to Domain Model
        var activity = new Models.Activity
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OpenRegistration = request.OpenRegistration,
            MaxTeams = request.MaxTeams,
            MinParticipant = request.MinParticipant,
            MaxParticipant = request.MaxParticipant,
            UrlHandle = "randomUrlHandle",
            CreatedBy = await _userManager.FindByIdAsync(request.CreatedBy),
            CreatedDate = DateTime.Now,
            Updates = new List<Update>(),
            Categories = new List<Category>(),
            Teams = new List<Team>()
        };

        foreach (var categoryId in request.Categories) 
        {
            var existingCategory = await _categoryRepository.GetAsync(categoryId);

            if(existingCategory is not null)
            {
                activity.Categories.Add(existingCategory);
            }
        }

        activity = await _activityRepository.CreateAsync(activity);

        // From Domain Model to DTO
        var response = await BuildActivityDto(activity);

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
            return NotFound("Activity no more exists!");
        }
        
        var response = await BuildActivityDto(activity);

        return Ok(response);
    }

    [HttpGet]
    // GET: api/Activities?query=MateInfoUB
    public async Task<IActionResult> GetAllActivities([FromQuery] string? query, [FromQuery] string? filter)
    {
        var activities = await _activityRepository.GetAllActivities(query, filter);
        
        // Mapping Domain Model to DTO
        var response = new List<ActivityDto>();

        foreach (var activity in activities)
        {
            response.Add(await BuildActivityDto(activity));
        }

        return Ok(response);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Admin, Organizer")]
    public async Task<IActionResult> EditActivity([FromRoute] Guid id, EditActivityRequestDto request)
    {
        if(await IsCurrentUserActivityCreatorOrAdmin(id) == false)
        {
            return Unauthorized("You are not authorized!");
        }

        // From DTO to Domain Model
        var activity = new Models.Activity
        {
            Id = id,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OpenRegistration = request.OpenRegistration,
            MaxTeams = request.MaxTeams,
            MinParticipant = request.MinParticipant,
            MaxParticipant = request.MaxParticipant,
            UrlHandle = "exampleHandle",
            CreatedBy = await _userManager.FindByIdAsync(request.CreatedBy),
            CreatedDate = DateTime.UtcNow,
            Categories = new List<Category>()
        };

        foreach(var categoryId in request.Categories)
        {
            var existingCategory = await _categoryRepository.GetAsync(categoryId);

            if (existingCategory != null)
            {
                activity.Categories.Add(existingCategory);
            }
        }

        activity = await _activityRepository.EditActivity(activity);

        if (activity == null)
        {
            return NotFound();
        }
        
        // Convert Domain Model to Dto
        var response = await BuildActivityDto(activity);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Admin, Organizer")]
    public async Task<IActionResult> DeleteActivity([FromRoute] Guid id)
    {
        if (await IsCurrentUserActivityCreatorOrAdmin(id) == false)
        {
            return Unauthorized("You are not authorized!");
        }

        var deletedActivity = await _activityRepository.DeleteActivity(id);

        if (deletedActivity == null)
        {
            return NotFound();
        }
        
        // Map from Domain Model to Dto
        var activityDto = await BuildActivityDto(deletedActivity);

        return Ok(activityDto);
    }

    #region Helper Methods
    private async Task<ActivityDto> BuildActivityDto(Models.Activity activity)
    {
        var roles = (List<string>) await _userManager.GetRolesAsync(activity.CreatedBy);
        try
        {
            var response = new ActivityDto
            {
                Id = activity.Id,
                Title = activity.Title,
                ShortDescription = activity.ShortDescription,
                LongDescription = activity.LongDescription,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                OpenRegistration = activity.OpenRegistration,
                MaxTeams = activity.MaxTeams,
                MinParticipant = activity.MinParticipant,
                MaxParticipant = activity.MaxParticipant,
                UrlHandle = activity.UrlHandle,
                CreatedBy = new UserResponseDto
                {
                    Id = activity.CreatedBy.Id,
                    UserName = activity.CreatedBy.UserName,
                    Email = activity.CreatedBy.Email,
                    Roles = roles,
                },
                CreatedDate = activity.CreatedDate,
                Updates = activity.Updates?.Select(x => new UpdateDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Date = x.Date
                }).ToList(),
                Categories = activity.Categories?.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                Teams = activity.Teams?.Select(c => new TeamDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedDate = c.CreatedDate,
                    AcceptedToActivity = c.AcceptedToActivity.ToString(),
                    IsPrivate = c.IsPrivate,
                }).ToList(),
            };
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
    
    private async Task<bool> IsCurrentUserActivityCreatorOrAdmin(Guid activityId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return false;
        }

        var isAdmin = User.IsInRole("Admin");

        var activity = await _activityRepository.GetActivityAsync(activityId);
        if (activity == null)
        {
            return false;
        }

        return activity != null && (activity.CreatedBy.Id == userId || isAdmin);

    }
    #endregion
}