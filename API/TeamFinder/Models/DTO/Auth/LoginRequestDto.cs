using System.ComponentModel.DataAnnotations;

namespace TeamFinder.Models.DTO.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
