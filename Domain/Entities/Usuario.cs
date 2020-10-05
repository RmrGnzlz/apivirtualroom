using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class Usuario : Entity<int>
    {
        [Required] public Rol Rol { get; set; }
        public int PersonaId { get; set; }
        [Required] public Persona Persona { get; set; }
        [MinLength(4), MaxLength(20), Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        public string RememberPassword { get; set; }
        public string Email { get; set; }
        public TipoUsuario Tipo { get; set; }
        public string CodigoRecuperacion { get; set; }
        public DateTime ExpiracionCodigo { get; set; }
    }
    public class Rol : Entity<int>
    {
        [MinLength(3), MaxLength(40), Required] public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
    public enum TipoUsuario
    {
        Admin = 0, Directivo = 1, Docente = 2, Estudiante = 3
    }
}