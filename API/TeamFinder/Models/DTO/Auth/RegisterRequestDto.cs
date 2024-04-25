using System.ComponentModel.DataAnnotations;

namespace TeamFinder.Models.DTO.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email is required!")]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least {2}, and maximum {1} characters")]
        public string Password { get; set; }
    }
}
