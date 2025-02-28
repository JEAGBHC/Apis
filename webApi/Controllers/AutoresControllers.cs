using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Datos;
using webApi.DTOs;
using webApi.Entidades;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/Autores")]
    //valor para usuar el autorizado, solo personas logueadas podran acceder a este controlador 
    [Authorize (Policy ="esadmin")]
    public class AutoresControllers : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresControllers> logger;
        private readonly IMapper mapper;

        public AutoresControllers(ApplicationDbContext context, ILogger<AutoresControllers> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }



        //[HttpGet]
        //public async Task<IEnumerable<Autor>> Get(){
        //logger.LogInformation("Obteniendo el listado de autores");
        //return await context.Autores.ToListAsync(); }

        //aqui agregaremos el DTO concaatenar nombre y apellido
        //[HttpGet]

        //public async Task<IEnumerable<AutorDtO>> Get()
        //{ 
        //    var autores =await context.Autores.ToListAsync(); 
        //    var autoresDTO = autores.Select(autor => new 
        //    AutorDtO { Id = autor.Id, NombreCompleto = $"{autor.Nombres}{autor.Apellidos}" }); 
        //    return autoresDTO;

        //}



        //Aqui agregaremos un get con automaper
        [HttpGet]
        public async Task<IEnumerable<AutorDtO>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDTO = mapper.Map<IEnumerable<AutorDtO>>(autores);

            return autoresDTO;
        }


        ///optener varios Autores 
        [HttpGet("{ids}", Name = "Varios")]
        public async Task<ActionResult<List<AutorDtOConLibros>>> Get(String ids)
        {
            var idsColeccion = new List<int>();
            foreach (var id in ids.Split(","))
            {
                if (int.TryParse(id, out int idInt))
                {
                    idsColeccion.Add(idInt);

                }
            }
            if (!idsColeccion.Any())
            {
                ModelState.AddModelError(nameof(ids), "Ningun Id fue encontrado");
                return ValidationProblem();

            }

            var autor = await context.Autores.Include(x => x.Libros).Include(x=>x.Libros)
                .Where(x=>idsColeccion.Contains(x.Id)).ToListAsync();

            if (autor.Count!= idsColeccion.Count)
            {
                return NotFound();
            }
            var autorDto = mapper.Map<List<AutorDtOConLibros>>(autor);
            return autorDto;
        }


        ///metodo get no trae datos de la tabla, esta harcodeado 
        //public IEnumerable<Autor> Get()
        //{
        //    return new List<Autor>
        //    {
        //        new Autor{Id = 1, Nombre="Maximo"},
        //          new Autor{Id = 1, Nombre="Felix"},
        //            new Autor{Id = 1, Nombre="Decimo"}


        //    };
        // }
        ///primero es una plantilla que diferencia este del primer get
        [HttpGet("Primero")]  //api/autores/primero 
        public async Task<Autor> GetPrimerAutor()
        {
            return await context.Autores.FirstAsync();
        }


        //lo que esta dentro de la ruta se llama parametro de ruta

        [HttpGet("{id:int}", Name = "ObtenerAutor")] //appiautores/id}
                                                     //ActionResult significa el resultado de una acción
        public async Task<ActionResult<AutorDtOConLibros>> Get(int id)
        {
            var autor = await context.Autores.Include(x => x.Libros)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorDtO = mapper.Map<AutorDtOConLibros>(autor); 
            return autorDtO;
        }

       

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDtO autorCreacionDtO)
        {
            var autor = mapper.Map<Autor>(autorCreacionDtO);
            context.Add(autor);
            //guardar Cambios de manera asincrona => SaveChangesAsyncs();
            await context.SaveChangesAsync();
            var autorDto = mapper.Map<AutorDtO>(autor);
            //retornamos el nombre de la peticion 
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDto);
        }


        [HttpPut("{id:int}")]  //API//usuario/id 
        public async Task<ActionResult> put(int id, AutorCreacionDtO autorCreacionDtO)
        {
            var autor = mapper.Map<Autor>(autorCreacionDtO);
            autor.Id = id;    
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")] //api/usuario/id
        public async Task<ActionResult> Delete(int id)
        {
            //  ExecuteDeleteAsync  == ejecutar borrado de manera asincrona 
            var registroBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (registroBorrados == 0)
            {
                return NotFound();
            }
            return Ok();
        }



    }
}
