using System.ComponentModel.DataAnnotations;
using webApi.validaciones;

namespace webApi.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} solo admite {1} caracteres")]
        [PrimeraMayuscula]
        public required string Nombres { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [StringLength(150, ErrorMessage = "El campo {0} solo admite {1} caracteres")]
        [PrimeraMayuscula]
        public required string Apellidos { get; set; }
        [StringLength(20, ErrorMessage = "El campo {0} solo admite {1} caracteres")]


        public string? Identificacion{ get; set; }
        //  referencia nula para que no tengamos error    new List<Libro>();
        public List<Libro> Libros { get; set; } = new List<Libro>();








        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!string.IsNullOrEmpty(Nombre))
        //    {
        //        var primeraLetra = Nombre[0].ToString();
        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe de ser mayuscula por modelo",
        //                new string[] { nameof(Nombre) });
        //        }
        //    }
        //}
    
    }
}
