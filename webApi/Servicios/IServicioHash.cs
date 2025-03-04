using webApi.DTOs;

namespace webApi.Servicios
{
    public interface IServicioHash
    {
        ResultadoHashDto Hash(string input);
        ResultadoHashDto Hash(string input, byte[] sal);
     
    }
}