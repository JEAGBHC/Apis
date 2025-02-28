using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs
{
    public class AutorDtO
    {
        public int Id { get; set; }
        [Required]
        public required string NombreCompleto { get; set; }

       
    }
}
