using System.Net.WebSockets;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Datos;
using webApi.DTOs;
using webApi.Entidades;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/Libros")]
    //valor para usuar el autorizado, solo personas logueadas podran acceder a este controlador 
    [Authorize]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //esto permite ser anonimo, lo que quiere decir que desactiva el Authorize
        [AllowAnonymous]
        public async Task<IEnumerable<LibroDtO>> Get()
        {
            var  libros= await context.Libros.ToListAsync();
            var LibrosDtO = mapper.Map<IEnumerable<LibroDtO>>(libros);
            return LibrosDtO;


        }
        //Libro por id 

        [HttpGet("{id:int}", Name ="ObtenerLibros")] //appiautores/id}
                              //ActionResult significa el resultado de una acción
        public async Task<ActionResult<LibroConAutorDtO>> Get(int id)
        {
            var libro = await context.Libros.Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }


            var libroDtO = mapper.Map<LibroConAutorDtO>(libro);
            return libroDtO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDtO libroCreacionDtO)
        {
            var libro = mapper.Map<Libro>(libroCreacionDtO);
            context.Add(libro);
            //AnyAsync = retorna verdadero o falso si existe o 
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor)
            {
                //forma tradicional de mostrar un error 
                //return BadRequest($"El autor de id {libro.AutorId} no existe ");

                ModelState.AddModelError(nameof(libro.AutorId), 
                    $"El autor de id {libro.AutorId} no existe ");
                return ValidationProblem();

            }
            context.Add(libro);
            await context.SaveChangesAsync();
            var libroDtO = mapper.Map<LibroDtO>(libro);
            //retornamos el nombre de la peticion 
            return CreatedAtRoute("ObtenerLibros",new {id =libro.Id },libroDtO);

        }

        [HttpPut("{id:int}")]  //API//usuario/id 
        public async Task<ActionResult> put(int id, LibroCreacionDtO libroCreacionDtO)
        {
            //if (id != libro.Id)
            //{
            //    return BadRequest("Los ids deben de coincidir");

            //}
            var libro = mapper.Map<Libro>(libroCreacionDtO);
            libro.Id = id;

            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            

            if (!existeAutor)
            {
                return BadRequest($"El autor del libro no existe ");


            }

          
            context.Update(libro);
            await context.SaveChangesAsync();
            
            return NoContent();
        }


        [HttpDelete("{id:int}")] //api/usuario/id
        public async Task<ActionResult> Delete(int id)
        {
            //  ExecuteDeleteAsync  == ejecutar borrado de manera asincrona 
            var registroBorrados = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (registroBorrados == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
