﻿namespace BibliotecaAPI.DTO
{
    public class UsuarioDto
    {
        public required string Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}