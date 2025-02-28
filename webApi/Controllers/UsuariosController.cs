using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Azure.Identity;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webApi.Datos;
using webApi.DTOs;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<Usuario> signInManager;
        private readonly IServiciosUsuarios serviciosUsuarios;

        public UsuariosController(UserManager<Usuario> userManager, IConfiguration configuration,
            SignInManager<Usuario> signInManager, IServiciosUsuarios serviciosUsuarios )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.serviciosUsuarios = serviciosUsuarios;
        }




        [HttpPost("registro")]
        

        public async Task<ActionResult<RespuestaAutenticacionDtO>> Registrar
            (CredencialesUsuarioDtO credencialesUsuarioDtO)
        {
            var usuario = new Usuario
            {
                UserName = credencialesUsuarioDtO.Email,
                Email = credencialesUsuarioDtO.Email
            };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioDtO.Password!);

            if (resultado.Succeeded)
            {

                var respuestaAutenticacion = await ConstruirToken(credencialesUsuarioDtO);
                return respuestaAutenticacion;

            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
                return ValidationProblem();
            }
        }





        [HttpPost("login")]
 
        public async Task<ActionResult<RespuestaAutenticacionDtO>> Login(
            CredencialesUsuarioDtO credencialesUsuarioDtO)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDtO.Email);


            if (usuario is null)
            {
                return RetornarLoginIncorrecto();

            }
            var resultado = await signInManager.CheckPasswordSignInAsync(usuario,
                
                //lockoutOnfailure =  bloquear la cuenta si el usuario se equivoca varias veces 
                credencialesUsuarioDtO.Password!, lockoutOnFailure: false);
            if (resultado.Succeeded) {

                return await ConstruirToken(credencialesUsuarioDtO);

            }
            else
            {
                return RetornarLoginIncorrecto();
            }
        }

        [HttpGet("renovar-token")]
        [Authorize(Policy ="esadmin")]
        public async Task<ActionResult<RespuestaAutenticacionDtO>> RenovarToken()
        {
            var usuario = await serviciosUsuarios.ObtenerUsuario();

            if (usuario is null)
            {
                return NotFound();
            }

            var credencialesUsuarioDTO = new CredencialesUsuarioDtO { Email = usuario.Email! };

            var respuestaAutenticacion = await ConstruirToken(credencialesUsuarioDTO);
            return respuestaAutenticacion;
        }

        [HttpPost("hacer-admin")]
        //[Authorize(Policy = "esadmin")]
        public async Task<ActionResult> HacerAdmin(EditarClaimDto editarClaimDto)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDto.Email);

            if (usuario is null)
            {
                return NotFound();
            }

            await userManager.AddClaimAsync(usuario, new Claim("esadmin", "true"));
            return NoContent();
        }


        [HttpPost("remover-admin")]
        [Authorize(Policy = "esadmin")]
        public async Task<ActionResult> RemoverAdmin(EditarClaimDto editarClaimDto)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDto.Email);

            if (usuario is null)
            {
                return NotFound();
            }

            await userManager.RemoveClaimAsync(usuario, new Claim("esadmin", "true"));
            return NoContent();
        }





        private ActionResult RetornarLoginIncorrecto(){
            ModelState.AddModelError(string.Empty, "Login incorrecto");
            return ValidationProblem();
        


}



        //metodo constructor del token 
        public async Task<RespuestaAutenticacionDtO> ConstruirToken(
            CredencialesUsuarioDtO credencialesUsuarioDtO)
        {
            var claims = new List<Claim>
            {
                new Claim("email", credencialesUsuarioDtO.Email),
                new Claim("maximo decimo", "meridio")
            };
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDtO.Email);
            var claimsDB =await userManager.GetClaimsAsync(usuario!);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            var credenciales  = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.Now.AddMonths(1);
            var tokenDeSeguridad= new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: credenciales);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDtO
            {

                Token = token,
                Expiracion = expiracion

            };
            

         }
    
    
    }
}
