using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO;
using TeamFinder.Repositories.Interface;
using TeamFinder.Models;

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
        public async Task<IActionResult> CreateUpdate([FromBody] CreateUpdateRequestDto request)
        {
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
        public async Task<IActionResult> DeleteUpdate([FromRoute] Guid id)
        {
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
        public async Task<IActionResult> EditUpdate([FromRoute] Guid id, [FromBody] EditUpdateRequestDto request)
        {
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
    }
}
