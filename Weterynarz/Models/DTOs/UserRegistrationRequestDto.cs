using System.ComponentModel.DataAnnotations;

namespace Weterynarz.Models.DTOs
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
