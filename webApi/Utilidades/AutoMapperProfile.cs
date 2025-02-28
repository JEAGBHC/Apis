using AutoMapper;
using webApi.DTOs;
using webApi.Entidades;

namespace webApi.Utilidades
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() {
            CreateMap<Autor, AutorDtO>()
                    .ForMember(dto => dto.NombreCompleto,
                    config => config.MapFrom(autor => $"{autor.Nombres} {autor.Apellidos}"));

            // autor con libros 
            CreateMap<Autor, AutorDtOConLibros>()
                    .ForMember(dto => dto.NombreCompleto,
                    config => config.MapFrom(autor => $"{autor.Nombres} {autor.Apellidos}"));

            CreateMap<AutorCreacionDtO, Autor>();


            CreateMap<Libro, LibroDtO>();
            CreateMap<LibroCreacionDtO,  Libro>();

            CreateMap<Libro, LibroConAutorDtO>()
                    .ForMember(dto => dto.AutorNombre,
                    config => config.MapFrom(ent => $"{ent.Autor!.Nombres} {ent.Autor.Apellidos}"));


        }


    }
}
