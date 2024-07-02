using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var activity = await _dbContext.Activities.Include(x => x.WinnerResult).FirstOrDefaultAsync(x => x.Id == winnerResultDto.ActivityId);
            if (activity == null)
            {
                return NotFound("Activity not found");
            }
            if (activity.EndDate > DateTime.Now)
            {
                return BadRequest("Activity has not ended yet");
            }
            if (activity.WinnerResult != null)
            {
                return BadRequest("Winner result already exists for this activity");
            }

            var teamIds = winnerResultDto.Teams.Select(t => t.Id).ToList();
            var teams = await _dbContext.Teams.Where(t => teamIds.Contains(t.Id)).ToListAsync();
            if (teams.Count != winnerResultDto.Teams.Count)
            {
                return BadRequest("One or more teams not found");
            }

            var orderedTeams = winnerResultDto.Teams.Select(t => new OrderedTeam
            {
                TeamId = t.Id,
                Order = t.Order
            }).ToList();

            var winnerResult = new WinnerResult
            {
                ActivityId = winnerResultDto.ActivityId,
                Teams = teams,
                OrderedTeams = orderedTeams
            };

            _dbContext.WinnerResults.Add(winnerResult);
            await _dbContext.SaveChangesAsync();

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

            var orderedTeams = winnerResult.OrderedTeams.OrderBy(ot => ot.Order)
                .Select(ot => winnerResult.Teams.First(t => t.Id == ot.TeamId)).ToList();

            return Ok(new
            {
                winnerResult.Id,
                winnerResult.ActivityId,
                Teams = orderedTeams
            });
        }
    }
}
