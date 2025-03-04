namespace webApi.DTOs
{
    public class ResultadoHashDto
    {
        public required string Hash { get; set; }
        public required byte[] Sal { get; set; }
    }
}
