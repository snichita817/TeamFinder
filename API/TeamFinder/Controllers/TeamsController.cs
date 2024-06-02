using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamFinder.Models;
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
            Activity? activityRegistered = await _activityRepository.GetActivityAsync(Guid.Parse(request.ActivityRegistered));
            if(activityRegistered == null)
            {
                return BadRequest("Activity not found.");
            }
            if(activityRegistered.StartDate < DateTime.Now)
            {
                return BadRequest("Activity has already started.");
            }

            var acceptedTeams = activityRegistered.Teams.Where(t => t.AcceptedToActivity == RequestStatus.Accepted).ToList();
            if (acceptedTeams.Count >= activityRegistered.MaxTeams)
            {
                return BadRequest("Activity has reached the maximum number of accepted teams.");
            }

            // Map request to domain model
            var team = new Team
            {
                Name = request.Name,
                Description = request.Description,
                IsPrivate = request.IsPrivate,
                TeamCaptainId = Guid.Parse(request.TeamCaptainId),
                ActivityRegistered = activityRegistered,
                MinParticipant = activityRegistered.MinParticipant,
                MaxParticipant = activityRegistered.MaxParticipant,
                Members = new List<ApplicationUser>(),
            };
            var user = await _userManager.FindByIdAsync(request.TeamCaptainId);
            team.Members.Add(user);

            var activityTeams = await _teamRepository.GetTeamsByActivityId(activityRegistered.Id);
            foreach (var teams in activityTeams)
            {
                if (teams.Members.Contains(user))
                {
                    return BadRequest("User is already a member of a team in this activity.");
                }
            }

            if (activityRegistered.OpenRegistration)
            {
                team.AcceptedToActivity = RequestStatus.Accepted;
            } else
            {
                team.AcceptedToActivity = RequestStatus.Pending;
            }

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
        public async Task<IActionResult> GetAllTeams([FromQuery] string? query)
        {
            var teams = await _teamRepository.GetAllTeamsAsync(query);

            List<TeamDto> response = new List<TeamDto>();
            foreach (var team in teams)
            {
                response.Add(await BuildTeamDto(team));
            }

            return Ok(response);
        }

        [HttpGet("activity/{activityId}")]
        public async Task<IActionResult> GetTeamsByActivityId(Guid activityId, [FromQuery] string? query)
        {
            var teams = await _teamRepository.GetTeamsByActivityId(activityId, query);

            List<TeamDto> response = new List<TeamDto>();
            foreach (var team in teams)
            {
                response.Add(await BuildTeamDto(team));
            }

            return Ok(response);
        }

        [HttpGet("myteams")]
        public async Task<IActionResult> GetTeamsForCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var teams = await _teamRepository.GetTeamsByUserId(userId);

            List<TeamDto> response = new List<TeamDto>();
            foreach (var team in teams)
            {
                response.Add(await BuildTeamDto(team));
            }

            return Ok(response);
        }

        [HttpGet("review/activity/{activityId:Guid}")]
        public async Task<IActionResult> ReviewTeams(Guid activityId, [FromQuery] string? query)
        {
            var teams = await _teamRepository.GetActivityUreviewedTeams(activityId, query);

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
            if (!await IsCurrentUserActivityCreator(teamId))
            {
                return Unauthorized("You are not authorized!");
            }
            Team? team;
            try
            {
                team = await _teamRepository.AcceptTeam(teamId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
            if (!await IsCurrentUserActivityCreator(teamId))
            {
                return Unauthorized("You are not authorized!");
            }

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
            if(await IsCurrentUserTeamLeader(id) == false)
            {
                return Unauthorized("You are not authorized!");
            }
            var existingTeam = await _teamRepository.GetTeamByIdAsync(id);
            if(existingTeam == null) return NotFound("Team not found.");

            var team = new Team
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                AcceptedToActivity = existingTeam.AcceptedToActivity,
                IsPrivate = request.IsPrivate,
                TeamCaptainId = Guid.Parse(request.TeamCaptainId),
                SubmissionUrl = existingTeam.SubmissionUrl,
                MinParticipant = existingTeam.MinParticipant,
                MaxParticipant = existingTeam.MaxParticipant,
                ActivityRegistered = existingTeam.ActivityRegistered,
                Members = new List<ApplicationUser>(),
                TeamMembershipRequests = existingTeam.TeamMembershipRequests,
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
            if (await IsCurrentUserTeamLeader(id) == false)
            {
                return Unauthorized("You are not authorized!");
            }

            var team = await _teamRepository.DeleteTeam(id);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpGet("user-team/{activityId:Guid}")]
        public async Task<IActionResult> GetUserTeamForActivity(Guid activityId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Please login before accessing this page!");
            }

            var team = await _teamRepository.GetUserTeam(activityId, userId);

            if (team == null)
            {
                return NotFound();
            }

            var response = await BuildTeamDto(team);

            return Ok(response);
        }

        [HttpPut("remove-member/{teamId:Guid}/{userId}")]
        public async Task<IActionResult> RemoveMember([FromRoute] Guid teamId, [FromRoute] string userId)
        {
            var callerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (callerId != userId && await IsCurrentUserTeamLeader(teamId) == false)
            {
                return Unauthorized("You are not authorized!");
            }

            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                return NotFound("Team not found.");
            }

            var user = team.Members.FirstOrDefault(m => m.Id == userId);
            if (user == null)
            {
                return NotFound("Member not found in the team.");
            }

            if (team.TeamCaptainId.ToString() == userId)
            {
                team.Members.Remove(user);

                if (team.Members.Any())
                {
                    var random = new Random();
                    int index = random.Next(team.Members.Count);
                    team.TeamCaptainId = Guid.Parse(team.Members.ElementAt(index).Id);
                }
                else
                {
                    team.TeamCaptainId = Guid.Empty;
                }
            }
            else
            {
                team.Members.Remove(user);
            }
            if(team.Members.Count <= 0)
            {
                await _teamRepository.DeleteTeam(teamId);
                return NotFound("Team has been deleted!");
            }

            // Update the team in the repository
            await _teamRepository.EditTeam(team);

            return Ok();
        }
    
        #region File Uploads
        [HttpPut("{teamId:Guid}/upload/{submissionUrl}")]
        public async Task<IActionResult> ChangeSubmissionLink([FromRoute] Guid teamId, [FromRoute] string submissionUrl)
        {
            if(await IsCurrentUserTeamLeader(teamId) == false)
            {
                return Unauthorized("You are not authorized!");
            }

            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            if (team == null) return BadRequest("Team not found.");
            if (team.AcceptedToActivity != RequestStatus.Accepted) return BadRequest("Team has not been accepted to the activity.");
            if (team.ActivityRegistered.StartDate > DateTime.Now) return BadRequest("Activity has not started yet.");
            if (team.ActivityRegistered.EndDate < DateTime.Now) return BadRequest("Activity has ended.");

            team.SubmissionUrl = submissionUrl;

            var result = await _teamRepository.EditTeam(team);
            if (result == null) return BadRequest("Unable to change submission link.");

            return Ok();
        }
        #endregion

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

                // Verify if team has reached the maximum number of participants
                if (team.Members.Count >= team.MaxParticipant) 
                    return BadRequest("Team has reached the maximum number of participants.");

                // Verify if the activity has already started or ended
                var activity = team.ActivityRegistered;
                if(activity.StartDate < DateTime.Now) return BadRequest("Activity has already started.");
                if (activity.EndDate < DateTime.Now) return BadRequest("Activity has ended.");

                // Verify if the user is already a member of a team in the activity
                activity = await _activityRepository.GetActivityAsync(activity.Id);
                if(activity == null)
                {
                    return BadRequest("Activity not found.");
                }

                var activityTeams = activity.Teams;
                foreach(var t in activityTeams)
                {
                    var activityTeam = await _teamRepository.GetTeamByIdAsync(t.Id);
                    
                    if(activityTeam == null) continue;
                    if (activityTeam.Members.Contains(user))
                    {
                        return BadRequest("User is already a member of a team in this activity.");
                    }
                }

                // Create Request
                var teamMembershipRequest = new TeamMembershipRequest
                {
                    Team = team,
                    User = user,
                };

                var result = await _teamMembershipRequestService.CreateTeamMembershipRequestAsync(teamMembershipRequest);
                if (result == null) return BadRequest("Unable to create membership request.");

                if(!team.IsPrivate) await _teamMembershipRequestService.AcceptTeamMembershipRequestAsync(result.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{teamId}/team-membership-requests")]
        public async Task<IActionResult> GetMembershipRequests(Guid teamId, [FromQuery] string? query)
        {
            if(await IsCurrentUserTeamLeader(teamId) == false)
            {
                return Unauthorized("You are not authorized!");
            }
            var requests = await _teamMembershipRequestService.GetTeamMembershipRequestAsync(teamId, RequestStatus.Pending, query);
            
            var response = new List<TeamMembershipRequestDto>();
            foreach (var request in requests)
            {
                response.Add(await BuildTeamMembershipRequestDto(request));
            }
            
            return Ok(response);
        }

        [HttpGet("my-team-membership-requests")]
        public async Task<IActionResult> GetUserMembershipRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var requests = await _teamMembershipRequestService.GetUserTeamMembershipRequests(userId);

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
            if (!await IsCurrentUserMembershipRequestTeamLeader(requestId))
            {
                return Unauthorized("You are not authorized!");
            }

            var request = await _teamMembershipRequestService.GetTeamMembershipRequestAsync(requestId);
            var team = await _teamRepository.GetTeamByIdAsync(request.Team.Id);
            if(team == null) return BadRequest("Team not found.");

            if(team.Members.Count >= team.MaxParticipant)
            {
                return BadRequest("Team has reached the maximum number of participants.");
            }
            var activity = await _activityRepository.GetActivityAsync(team.ActivityRegistered.Id);
            if(activity == null)
            {
                return BadRequest("Activity not found.");
            }
            if(activity.StartDate < DateTime.Now) return BadRequest("Activity has already started.");

            bool result = false;
            try
            {
                result = await _teamMembershipRequestService.AcceptTeamMembershipRequestAsync(requestId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (!result) return BadRequest("Unable to accept membership request. Verify if the user is already in a different team.");

            return Ok();
        }

        [HttpPut("team-membership-requests/{requestId}/reject")]
        public async Task<IActionResult> RejectMembershipRequest(Guid requestId)
        {
            if (!await IsCurrentUserMembershipRequestTeamLeader(requestId))
            {
                return Unauthorized("You are not authorized!");
            }

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
                //SubmissionUrl = team.SubmissionUrl,
                MinParticipant = team.MinParticipant,
                MaxParticipant = team.MaxParticipant,
                ActivityRegistered = team.ActivityRegistered == null ? null : new ActivityDto
                {
                    Id = team.ActivityRegistered.Id,
                    Title = team.ActivityRegistered.Title,
                },
                Members = users,
            };

            if(await IsCurrentUserTeamLeader(team.Id) || await IsCurrentUserActivityCreator(team.Id))
            {
                response.SubmissionUrl = team.SubmissionUrl;
            }

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

        private async Task<bool> IsCurrentUserActivityCreator(Guid teamId)
        {
            // Get the current user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return false;
            }
            var isAdmin = User.IsInRole("Admin");

            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                return false;
            }

            var activity = await _activityRepository.GetActivityAsync(team.ActivityRegistered.Id);

            return activity != null && (activity.CreatedBy.Id == userId || isAdmin);
        }
        
        private async Task<bool> IsCurrentUserTeamLeader(Guid teamId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return false;
            }
            var isAdmin = User.IsInRole("Admin");

            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                return false;
            }

            return team.TeamCaptainId.ToString() == userId || isAdmin;
        }
        
        private async Task<bool> IsCurrentUserMembershipRequestTeamLeader(Guid requestId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return false;
            }
            var isAdmin = User.IsInRole("Admin");
            var request = await _teamMembershipRequestService.GetTeamMembershipRequestAsync(requestId);
            if (request == null)
            {
                return false;
            }

            return request.Team.TeamCaptainId.ToString() == userId || isAdmin;
        }
        
        
        #endregion
    }
}
