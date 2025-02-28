using System.ComponentModel.DataAnnotations;
using webApi.validaciones;

namespace webApi.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [PrimeraMayuscula]
        public required string? Titulo { get; set; }

        public int AutorId { get; set; }

        public Autor? Autor { get; set; }
    }
}
