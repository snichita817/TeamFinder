using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Activities;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.Teams;
using TeamFinder.Repositories.Implementation;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public TeamsController(ITeamRepository teamRepository,
            UserManager<ApplicationUser> userManager,
            IActivityRepository activityRepository)
        {
            _teamRepository = teamRepository;
            _activityRepository = activityRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequestDto request)
        {
            // Map request to domain model
            var team = new Team
            {
                Name = request.Name,
                AcceptedToActivity = request.AcceptedToActivity,
                IsPrivate = request.IsPrivate,
                CreatedBy = await _userManager.FindByIdAsync(request.CreatedBy),
                ActivityRegistered = await _activityRepository.GetActivityAsync(request.ActivityRegistered),
                Members = new List<ApplicationUser>(),
            };
            team.Members.Add(team.CreatedBy);

            team = await _teamRepository.CreateAsync(team);

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        #region Helper Methods
        private async Task<TeamDto> BuildTeamDto(Team team)
        {
            // Populate the users that are in the team
            List<UserResponseDto> users = new List<UserResponseDto>();
            foreach (var m in team.Members)
            {
                var roles = await _userManager.GetRolesAsync(m);
                users.Add(new UserResponseDto
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    Email = m.Email,
                    Roles = roles.ToList()
                });
            }

            var response = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                CreatedDate = team.CreatedDate,
                AcceptedToActivity = team.AcceptedToActivity,
                IsPrivate = team.IsPrivate,
                CreatedBy = new UserResponseDto
                {
                    Id = team.CreatedBy.Id,
                    UserName = team.CreatedBy.UserName,
                    Email = team.CreatedBy.Email,
                    Roles = (List<string>)await _userManager.GetRolesAsync(team.CreatedBy),
                },
                Members = users,
            };

            return response;
        }
        #endregion
    }
}
