using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using webApi.DTOs;

namespace webApi.Servicios
{
    public class ServicioHash : IServicioHash
    {

        //mismo metodo 
        public ResultadoHashDto Hash(string input)
        {

            var sal = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);

            }

            return Hash(input, sal);
        }




        public ResultadoHashDto Hash(string input, byte[] sal)
        {

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(

                password: input,
                salt: sal,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8
                ));
            return new ResultadoHashDto
            {
                Hash = hashed,
                Sal = sal
            };


        }


    }
}
