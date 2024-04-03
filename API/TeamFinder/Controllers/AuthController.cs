using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<ApplicationUser> userManager,
            ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();

            var response = new List<UserResponseDto>();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Add(
                    new UserResponseDto 
                    { 
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Roles = roles.ToList()
                    });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("users/{id:Guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if(user is null)
            {
                return NotFound();
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                var response = new UserProfileResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    University = user.University,
                    CourseOfStudy = user.CourseOfStudy,
                    GraduationYear = user.GraduationYear,
                    Bio = user.Bio,
                    LinkedInUrl = user.LinkedInUrl,
                    GitHubUrl = user.GitHubUrl,
                    Skills = user.Skills,
                    Interests = user.Interests,
                    PortfolioUrl = user.PortfolioUrl
                };

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("users/{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var userToDelete = await _userManager.FindByIdAsync(id.ToString());
            var deletedUser = await _userManager.DeleteAsync(userToDelete);

            if(deletedUser == null)
            {
                return NotFound();
            }

            return Ok();    
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create IdentityUser object
            var user = new ApplicationUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if(identityResult.Succeeded)
            {
                // Add Role to user (user)
                identityResult = await _userManager.AddToRoleAsync(user, "User");

                if (identityResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var token = _tokenRepository.CreateJwtToken(user, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Roles = roles.ToList(),
                        Token = token
                    };
                    return Ok(response);
                }
                else
                {
                    if(identityResult.Errors.Any())
                    {
                        foreach(var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if(identityUser is not null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, request.Password);
                if(checkPasswordResult) 
                {
                    var roles = await _userManager.GetRolesAsync(identityUser);

                    // Create a token and response
                    var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        Id = identityUser.Id,
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("addrole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return NotFound();
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if(userRoles.Contains(request.RoleToAdd))
                {
                    ModelState.AddModelError("", "Role already exists");
                    return ValidationProblem(ModelState);
                }

                var identityResult = await _userManager.AddToRoleAsync(user, request.RoleToAdd);
                if(identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if(identityResult.Errors.Any())
                    {
                        foreach(var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    return ValidationProblem(ModelState);
                }
            }
        }

        [HttpPost]
        [Route("users/edit/{id:Guid}")]
        public async Task<IActionResult> EditUser([FromRoute] Guid id, [FromBody] EditUserRequestDto request)
        {
            // Find the user by ID.
            var userToUpdate = await _userManager.FindByIdAsync(id.ToString());
            if (userToUpdate == null)
            {
                return NotFound("User not found.");
            }

            // Check if the new email is already taken by another user.
            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != userToUpdate.Id)
            {
                return Conflict("Email already exists.");
            }

            // Update user properties.
            UpdateUserProperties(userToUpdate, request);

            // Try updating the user.
            var updateResult = await _userManager.UpdateAsync(userToUpdate);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return ValidationProblem(ModelState);
            }

            return Ok("User updated successfully.");
        }

        private void UpdateUserProperties(ApplicationUser user, EditUserRequestDto request)
        {
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.University = request.University;
            user.CourseOfStudy = request.CourseOfStudy;
            user.GraduationYear = request.GraduationYear;
            user.Bio = request.Bio;
            user.LinkedInUrl = request.LinkedInUrl;
            user.GitHubUrl = request.GitHubUrl;
            user.Skills = request.Skills;
            user.Interests = request.Interests;
            user.PortfolioUrl = request.PortfolioUrl;
        }
    }
}
