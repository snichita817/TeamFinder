using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Repositories.Interface;
using TeamFinder.Models;
using TeamFinder.Models.DTO.Updates;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TeamFinder.Repositories.Implementation;

namespace TeamFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatesController : Controller
    {
        private readonly IUpdateRepository _updateRepository;
        private readonly IActivityRepository _activityRepository;

        public UpdatesController(IUpdateRepository updateRepository, IActivityRepository activityRepository)
        {
            this._updateRepository = updateRepository;
            this._activityRepository = activityRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Organizer")]
        public async Task<IActionResult> CreateUpdate([FromBody] CreateUpdateRequestDto request)
        {
            if(await IsCurrentUserActivityCreatorOrAdmin(request.ActivityId) == false)
            {
                return Unauthorized("You are not authorized!");
            }

            // mapping dto to domain model
            var update = new Update
            {
                Title = request.Title,
                Text = request.Text,
                Date = request.Date,
                Activity = await _activityRepository.GetActivityAsync(request.ActivityId)
            };

            update = await _updateRepository.CreateAsync(update);

            // from domain model to dto
            var response = new UpdateDto
            {
                Id = update.Id,
                Title = update.Title,
                Text = update.Text,
                Date = update.Date,
                ActivityId = update.Activity.Id
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUpdates()
        {
            var updates = await _updateRepository.GetAllAsync();

            // converting domain model to dto
            var response = new List<UpdateDto>();

            foreach (var update in updates)
            {
                response.Add(new UpdateDto
                {
                    Id = update.Id,
                    Title = update.Title,
                    Text = update.Text,
                    Date = update.Date,
                    ActivityId = update.Activity.Id
                });
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin, Organizer")]
        public async Task<IActionResult> DeleteUpdate([FromRoute] Guid id)
        {
            if (await IsCurrentUserActivityCreatorOrAdmin(id) == false)
            {
                return Unauthorized("You are not authorized!");
            }
            var updateToDelete = await _updateRepository.DeleteAsync(id);

            if(updateToDelete == null)
            {
                return NotFound();
            }

            // map domain model to dto
            var updateDto = new UpdateDto
            {
                Id = updateToDelete.Id,
                Title = updateToDelete.Title,
                Text = updateToDelete.Text,
                Date = updateToDelete.Date,
            };

            return Ok(updateDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUpdate([FromRoute] Guid id)
        {
            var update = await _updateRepository.GetAsync(id);

            if (update is null)
            {
                return NotFound();
            }

            // domain model to dto
            var response = new UpdateDto
            {
                Id = update.Id,
                Title = update.Title,
                Text = update.Text,
                Date = update.Date,
                ActivityId = update.Activity.Id
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin, Organizer")]
        public async Task<IActionResult> EditUpdate([FromRoute] Guid id, [FromBody] EditUpdateRequestDto request)
        {
            if(await IsCurrentUserActivityCreatorOrAdmin(id) == false)
            {
                return Unauthorized("You are not authorized!");
            }

            var update = new Update
            {
                Id = id,
                Title = request.Title,
                Text = request.Text,
                Date = DateTime.Now,
                Activity = await _activityRepository.GetActivityAsync(request.ActivityId)
            };

            update = await _updateRepository.EditUpdate(update);

            if(update is null)
            {
                return NotFound();
            }

            var response = new UpdateDto
            {
                Id = update.Id,
                Title = update.Title,
                Text = update.Text,
                Date = update.Date,
                ActivityId = update.Activity.Id
            };

            return Ok(response);
        }

        #region Helper Methods
        private async Task<bool> IsCurrentUserActivityCreatorOrAdmin(Guid activityId)
        {
            // Get the current user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return false;
            }
            var isAdmin = User.IsInRole("Admin");

            Activity? activity = null;

            var update = await _updateRepository.GetAsync(activityId);
            if(update == null)
            {
                activity = await _activityRepository.GetActivityAsync(activityId);
            }
            else
            {
                activity = await _activityRepository.GetActivityAsync(update.Activity.Id);
            }
            if (activity == null)
            {
                return false;
            }

            return activity != null && (activity.CreatedBy.Id == userId || isAdmin);
        }


        #endregion
    }
}
