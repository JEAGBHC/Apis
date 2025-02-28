using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace webApi.DTOs
{
    public class CredencialesUsuarioDtO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
