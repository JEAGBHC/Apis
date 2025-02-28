using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs
{
    public class EditarClaimDto
    {
        [EmailAddress]
        [Required]
        public required string  Email{ get; set; }
    }
}
