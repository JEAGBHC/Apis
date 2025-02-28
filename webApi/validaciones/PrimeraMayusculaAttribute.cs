using System.ComponentModel.DataAnnotations;

namespace webApi.validaciones
{
    public class PrimeraMayusculaAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) { 
            
                return ValidationResult.Success;
            }
            var valueString = value.ToString()!;
            var primeraLetra = valueString[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La Primera Letra debe de ser Mayúscula");
            }
            return ValidationResult.Success;
        }

    }
}
