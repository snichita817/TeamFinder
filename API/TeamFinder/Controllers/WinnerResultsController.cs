using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamFinder.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.WinnerResults;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WinnerResultsController : ControllerBase
    {
        private readonly IWinnerResultRepository _winnerResultRepository;
        private readonly ApplicationDbContext _dbContext;

        public WinnerResultsController(IWinnerResultRepository winnerResultRepository, ApplicationDbContext context)
        {
            _winnerResultRepository = winnerResultRepository;
            _dbContext = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Organizer")]
        public async Task<IActionResult> CreateWinnerResult([FromBody] AddWinnerResultDto winnerResultDto)
        {
            var activity = await _dbContext.Activities.Include(x=>x.WinnerResult).FirstOrDefaultAsync(x => x.Id == winnerResultDto.ActivityId);
            if (activity == null)
            {
                return NotFound("Activity not found");
            }
            /*if (activity.EndDate > DateTime.Now)
            {
                return BadRequest("Activity has not ended yet");
            }*/
            if (activity.WinnerResult != null)
            {
                return BadRequest("Winner result already exists for this activity");
            }

            var teams = await _dbContext.Teams.Where(t => winnerResultDto.TeamIds.Contains(t.Id)).ToListAsync();
            if (teams.Count != winnerResultDto.TeamIds.Count)
            {
                return BadRequest("One or more teams not found");
            }

            var winnerResult = new WinnerResult
            {
                ActivityId = winnerResultDto.ActivityId,
                Teams = teams
            };

            var createdWinnerResult = await _winnerResultRepository.CreateAsync(winnerResult);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWinnerResultById(Guid id)
        {
            var winnerResult = await _dbContext.WinnerResults
                .Include(wr => wr.Teams)
                .FirstOrDefaultAsync(wr => wr.Id == id);

            if (winnerResult == null)
            {
                return NotFound();
            }

            return Ok(winnerResult);
        }
    }
}
