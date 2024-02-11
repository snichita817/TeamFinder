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
            };

            update = await _updateRepository.CreateAsync(update);

            // from domain model to dto
            var response = new UpdateDto
            {
                Id = update.Id,
                Title = update.Title,
                Text = update.Text,
            };

            return Ok(response);
        }
    }
}
