using System.ComponentModel.DataAnnotations;
using webApi.Entidades;
using webApi.validaciones;

namespace webApi.DTOs
{
    public class LibroCreacionDtO
    {
        [Required]
        [StringLength(100, ErrorMessage ="El campo {0} debe de tener {1} caracteres o menos")]
        public required string? Titulo { get; set; }
        public int AutorId { get; set; }

    }
}
