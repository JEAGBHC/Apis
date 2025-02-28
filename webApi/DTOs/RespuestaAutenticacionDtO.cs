namespace webApi.DTOs
{
    public class RespuestaAutenticacionDtO
    {
        public required string Token  { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
