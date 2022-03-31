using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos{
    public class CreateUserDto{
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Not a valid email")]
        public string Email { get; set; }
    }
}