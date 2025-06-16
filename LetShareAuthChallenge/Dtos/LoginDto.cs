using System.ComponentModel.DataAnnotations;

namespace LetShareAuthChallenge.Dtos
{
    public class LoginDto
    {
        [Required]
        public string GrantType { get; set; } = string.Empty;  // should be "password"
        [Required]
        public string ClientId { get; set; } = string.Empty;   // e.g., "web"
        [Required]
        public string ClientSecret { get; set; } = string.Empty; // e.g., "webpass1"
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
