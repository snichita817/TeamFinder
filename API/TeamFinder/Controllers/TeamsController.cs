using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO.Activities;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Models.DTO.TeamMembershipRequests;
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
        private readonly ITeamMembershipRequestRepository _teamMembershipRequestService;

        public TeamsController(ITeamRepository teamRepository,
            UserManager<ApplicationUser> userManager,
            IActivityRepository activityRepository,
            ITeamMembershipRequestRepository teamMembershipRequestService)
        {
            _teamRepository = teamRepository;
            _activityRepository = activityRepository;
            _userManager = userManager;
            _teamMembershipRequestService = teamMembershipRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequestDto request)
        {
            // Map request to domain model
            var team = new Team
            {
                Name = request.Name,
                Description = request.Description,
                IsPrivate = request.IsPrivate,
                TeamCaptainId = Guid.Parse(request.TeamCaptainId),
                ActivityRegistered = await _activityRepository.GetActivityAsync(Guid.Parse(request.ActivityRegistered)),
                Members = new List<ApplicationUser>(),
            };
            team.Members.Add(await _userManager.FindByIdAsync(request.TeamCaptainId));

            team = await _teamRepository.CreateAsync(team);

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(Guid id)
        {
            var team = await _teamRepository.GetTeamByIdAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();

            List<TeamDto> response = new List<TeamDto>();
            foreach (var team in teams)
            {
                response.Add(await BuildTeamDto(team));
            }

            return Ok(response);
        }

        [HttpGet("activity/{activityId}")]
        public async Task<IActionResult> GetTeamsByActivityId(Guid activityId)
        {
            var teams = await _teamRepository.GetTeamsByActivityId(activityId);

            List<TeamDto> response = new List<TeamDto>();
            foreach (var team in teams)
            {
                response.Add(await BuildTeamDto(team));
            }

            return Ok(response);
        }

        [HttpGet("review/activity/{activityId:Guid}")]
        public async Task<IActionResult> ReviewTeams(Guid activityId)
        {
            var teams = await _teamRepository.GetActivityUreviewedTeams(activityId);

            List<TeamDto> response = new List<TeamDto>();
            foreach (var team in teams)
            {
                response.Add(await BuildTeamDto(team));
            }

            return Ok(response);
        }

        [HttpPut("review/{teamId:Guid}/accept")]
        public async Task<IActionResult> AcceptTeam(Guid teamId)
        {
            var team = await _teamRepository.AcceptTeam(teamId);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpPut("review/{teamId:Guid}/reject")]
        public async Task<IActionResult> RejectTeam(Guid teamId)
        {
            var team = await _teamRepository.RejectTeam(teamId);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> EditTeam([FromRoute] Guid id, [FromBody] EditTeamRequestDto request)
        {
            var team = new Team
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                IsPrivate = request.IsPrivate,
                TeamCaptainId = Guid.Parse(request.TeamCaptainId),
                Members = new List<ApplicationUser>(),
            };

            foreach (var member in request.Members)
            {
                team.Members.Add(await _userManager.FindByIdAsync(member));
            }

            team = await _teamRepository.EditTeam(team);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var team = await _teamRepository.DeleteTeam(id);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        #region Team Membership Requests
        [HttpPost("team-membership-requests")]
        public async Task<IActionResult> CreateMembershipRequest([FromBody] AddTeamMembershipRequestDto request)
        {
            try
            {
                var team = await _teamRepository.GetTeamByIdAsync(Guid.Parse(request.TeamId));
                if (team == null) return BadRequest("Team not found.");

                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null) return BadRequest("User not found.");

                var teamMembershipRequest = new TeamMembershipRequest
                {
                    Team = team,
                    User = user,
                };

                var result = await _teamMembershipRequestService.CreateTeamMembershipRequestAsync(teamMembershipRequest);
                if (result == null) return BadRequest("Unable to create membership request.");

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{teamId}/team-membership-requests")]
        public async Task<IActionResult> GetMembershipRequests(Guid teamId)
        {
            var requests = await _teamMembershipRequestService.GetTeamMembershipRequestAsync(teamId, RequestStatus.Pending);
            
            var response = new List<TeamMembershipRequestDto>();
            foreach (var request in requests)
            {
                response.Add(await BuildTeamMembershipRequestDto(request));
            }
            
            return Ok(response);
        }

        [HttpPut("team-membership-requests/{requestId}/accept")]
        public async Task<IActionResult> AcceptMembershipRequest(Guid requestId)
        {
            var result = await _teamMembershipRequestService.AcceptTeamMembershipRequestAsync(requestId);
            if (!result) return BadRequest("Unable to accept membership request.");

            return Ok();
        }

        [HttpPut("team-membership-requests/{requestId}/reject")]
        public async Task<IActionResult> RejectMembershipRequest(Guid requestId)
        {
            var result = await _teamMembershipRequestService.RejectTeamMembershipRequestAsync(requestId);
            if (!result) return BadRequest("Unable to reject membership request.");

            return Ok();
        }
        #endregion

        #region Helper Methods
        private async Task<TeamDto> BuildTeamDto(Team team)
        {
            // Populate the users that are in the team
            List<UserResponseDto> users = new List<UserResponseDto>();

            if(team.Members != null)
            {
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
            }

            var response = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                CreatedDate = team.CreatedDate,
                AcceptedToActivity = team.AcceptedToActivity.ToString(),
                IsPrivate = team.IsPrivate,
                TeamCaptainId = team.TeamCaptainId.ToString(),
                ActivityRegistered = team.ActivityRegistered == null ? null : new ActivityDto
                {
                    Id = team.ActivityRegistered.Id,
                    Title = team.ActivityRegistered.Title,
                },
               // ActivityId = team.ActivityRegistered == null ? null : team.ActivityRegistered.Id.ToString(),
                Members = users,
            };

            return response;
        }
        
        private async Task<TeamMembershipRequestDto> BuildTeamMembershipRequestDto(TeamMembershipRequest request)
        {
            var roles = await _userManager.GetRolesAsync(request.User);
            var response = new TeamMembershipRequestDto
            {
                Id = request.Id,
                User = new UserResponseDto
                {
                    Id = request.User.Id,
                    UserName = request.User.UserName,
                    Email = request.User.Email,
                    Roles = roles.ToList()
                },
                Team = await BuildTeamDto(request.Team),
                RequestDate = request.RequestDate,
                Status = request.Status.ToString(),
            };

            return response;
        }
        #endregion
    }
}
