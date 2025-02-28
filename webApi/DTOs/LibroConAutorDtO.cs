using System.Reflection.Metadata.Ecma335;

namespace webApi.DTOs
{
    public class LibroConAutorDtO: LibroDtO
    {
        public int AutorId { get; set; }
        public required string AutorNombre { get; set; }
    }
}
