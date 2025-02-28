using System.ComponentModel.DataAnnotations;
using webApi.validaciones;

namespace webApi.DTOs
{
    public class AutorPatchDtO
    {
        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} solo admite {1} caracteres")]
        [PrimeraMayuscula]
        public required string Nombres { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} solo admite {1} caracteres")]
        [PrimeraMayuscula]
        public required string Apellidos { get; set; }
        [StringLength(20, ErrorMessage = "El campo {0} solo admite {1} caracteres")]


        public string? Identificacion { get; set; }
        //  referencia nula para que no tengamos error    new List<Libro>();
    }
}
