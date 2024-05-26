using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamFinder.Data;
using TeamFinder.Data.Repositories;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.OrganizerApplications;

[ApiController]
[Route("api/[controller]")]
public class OrganizerApplicationsController : ControllerBase
{
    private readonly IOrganizerApplicationRepository _repository;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrganizerApplicationsController(IOrganizerApplicationRepository repository,
        UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ApplyForOrganizer([FromBody] CreateOrganizerApplicationDto request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return BadRequest("No user found!");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Contains("Organizer"))
        {
            return BadRequest("User is already an organizer!");
        }

        var application = new OrganizerApplication
        {
            User = user,
            Reason = request.Reason,
            ApplicationDate = DateTime.UtcNow,
            Status = RequestStatus.Pending
        };

        await _repository.AddApplicationAsync(application);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetApplications()
    {
        var applications = await _repository.GetAllApplicationsAsync();

        var applicationDtos = applications.Select(app => new OrganizerApplicationDto
        {
            Id = app.Id,
            User = new UserResponseDto
            {
                Id = app.User.Id,
                UserName = app.User.UserName,
                Email = app.User.Email
            },
            Reason = app.Reason,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status.ToString()
        }).ToList();

        return Ok(applicationDtos);
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetApplication(Guid id)
    {
        var application = await _repository.GetApplicationByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }

        var applicationDto = new OrganizerApplicationDto
        {
            Id = application.Id,
            User = new UserResponseDto
            {
                Id = application.User.Id,
                UserName = application.User.UserName,
                Email = application.User.Email
            },
            Reason = application.Reason,
            ApplicationDate = application.ApplicationDate,
            Status = application.Status.ToString()
        };

        return Ok(applicationDto);
    }

    [HttpPut("approve/{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveApplication(Guid id)
    {
        var application = await _repository.GetApplicationByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(application.User.Id);
        if (user == null)
        {
            return BadRequest("No user found!");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Contains("Organizer"))
        {
            return BadRequest("User is already an organizer!");
        }

        var result = await _userManager.AddToRoleAsync(user, "Organizer");
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return ValidationProblem(ModelState);
        }

        application.Status = RequestStatus.Accepted;
        await _repository.UpdateApplicationAsync(application);
        return Ok();
    }

    [HttpPut("reject/{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RejectApplication(Guid id)
    {
        var application = await _repository.GetApplicationByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }
        application.Status = RequestStatus.Rejected;
        await _repository.UpdateApplicationAsync(application);
        return Ok();
    }
}
