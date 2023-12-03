﻿using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequestDto request)
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
            UrlHandle = "randomUrlHandle",
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.Now
        };

        activity = await _activityRepository.CreateAsync(activity);
        
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

    [HttpGet]
    public async Task<IActionResult> GetAllActivities()
    {
        var activities = await _activityRepository.GetAllActivities();
        
        // Mapping Domain Model to DTO
        var response = new List<ActivityDto>();

        foreach (var activity in activities)
        {
            response.Add(
            new ActivityDto
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
            });
        }

        return Ok(response);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> EditActivity([FromRoute] Guid id, EditActivityRequestDto request)
    {
        // From DTO to Domain Model
        var activity = new Activity
        {
            Id = id,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OpenRegistration = request.OpenRegistration,
            MaxParticipant = request.MaxParticipant,
            UrlHandle = "exampleHandle",
            CreatedBy = request.CreatedBy,
            CreatedDate = new DateTime()
        };

        activity = await _activityRepository.EditActivity(activity);

        if (activity == null)
        {
            return NotFound();
        }
        
        // Convert Domain Model to Dto
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

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteActivity([FromRoute] Guid id)
    {
        var deletedActivity = await _activityRepository.DeleteActivity(id);

        if (deletedActivity == null)
        {
            return NotFound();
        }
        
        // Map from Domain Model to Dto
        var activityDto = new ActivityDto
        {
            Id = deletedActivity.Id,
            Title = deletedActivity.Title,
            ShortDescription = deletedActivity.ShortDescription,
            LongDescription = deletedActivity.LongDescription,
            StartDate = deletedActivity.StartDate,
            EndDate = deletedActivity.EndDate,
            OpenRegistration = deletedActivity.OpenRegistration,
            MaxParticipant = deletedActivity.MaxParticipant,
            UrlHandle = deletedActivity.UrlHandle,
            CreatedBy = deletedActivity.CreatedBy,
            CreatedDate = deletedActivity.CreatedDate
        };

        return Ok(activityDto);
    }
}