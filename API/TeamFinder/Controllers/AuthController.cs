using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager,
            ITokenRepository tokenRepository,
            ICategoryRepository categoryRepository,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _categoryRepository = categoryRepository;
            _signInManager = signInManager;
        }

        private async Task<bool> CheckEmailTakenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
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
            var user = await _userManager.Users.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id.ToString());
            
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
                    Categories = user.Categories.Select(x => new Models.DTO.Categories.CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList(),
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
            if (await CheckEmailTakenAsync(request.Email))
            {
                return BadRequest($"An existing account is already using {request.Email}. Try using another email address.");
            }

            // Create IdentityUser object
            var user = new ApplicationUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if(!identityResult.Succeeded) return BadRequest(identityResult.Errors);

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
            if(identityUser is null)
            {
                return Unauthorized("Invalid username or password");
            }

            if (identityUser.EmailConfirmed == false) return Unauthorized("Please check your email to confirm your account.");

            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, false);
            if(!result.Succeeded)
            {
                return Unauthorized("Invalid username or password");
            }

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

            /*if (identityUser is not null)
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
                return Unauthorized("Invalid username or password");
            }
            ModelState.AddModelError("", "Email or Password Incorrect");

            return ValidationProblem(ModelState);*/
        }

        [HttpPut]
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

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<IActionResult> RefreshUserToken()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
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

        [HttpPut]
        [Route("users/edit/{id:Guid}")]
        public async Task<IActionResult> EditUser([FromRoute] Guid id, [FromBody] EditUserRequestDto request)
        {
            var userToUpdate = await _userManager.Users.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id.ToString());
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
            
            // Check if the new username is already taken by another user.
            var existingUserByUsername = await _userManager.FindByNameAsync(request.UserName);
            if (existingUserByUsername != null && existingUserByUsername.Id != userToUpdate.Id)
            {
                return Conflict("Username already exists.");
            }

            // Update user properties.
            userToUpdate.Email = request.Email;
            userToUpdate.UserName = request.UserName;
            userToUpdate.FirstName = request.FirstName;
            userToUpdate.LastName = request.LastName;
            userToUpdate.University = request.University;
            userToUpdate.CourseOfStudy = request.CourseOfStudy;
            userToUpdate.GraduationYear = request.GraduationYear;
            userToUpdate.Bio = request.Bio;
            userToUpdate.LinkedInUrl = request.LinkedInUrl;
            userToUpdate.GitHubUrl = request.GitHubUrl;
            userToUpdate.Skills = request.Skills;
            userToUpdate.PortfolioUrl = request.PortfolioUrl;
            userToUpdate.Categories = new List<Category>();

            var categoriesToAdd = new List<Category>();
            foreach(var categoryId in request.Categories)
            {
                var category = await _categoryRepository.GetAsync(categoryId);
                if(category != null)
                {
                    categoriesToAdd.Add(category);
                }
            }
            userToUpdate.Categories = categoriesToAdd;
            
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

            return Ok(new { message = "User updated successfully." });
        }
    }
}
