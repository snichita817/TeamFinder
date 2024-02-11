using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatesController : Controller
    {
        private readonly IUpdateRepository _updateRepository;
        public UpdatesController(IUpdateRepository updateRepository)
        {
            this._updateRepository = updateRepository;
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
            };

            update = await _updateRepository.CreateAsync(update);

            // from domain model to dto
            var response = new UpdateDto
            {
                Id = update.Id,
                Title = update.Title,
                Text = update.Text,
                Date = update.Date,
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
    }
}
