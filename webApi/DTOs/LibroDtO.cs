using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs
{
    public class LibroDtO
    {
        public int Id { get; set; }
        [Required]
        public required string Titulo { get; set; }
    }
}
