using Application.Mapping;
using Domain.Entities;

namespace Application.Models
{
    public class UsuarioModel : Model<Usuario>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        private string RememberPassword { get; set; }
        public string Email { get; set; }
        public PersonaModel Persona { get; set; }
        public RolModel Rol { get; set; }
        public TipoUsuarioModel Tipo { get; set; }
        public bool FirstPassword { get => (RememberPassword == null || RememberPassword == string.Empty); }
        public UsuarioModel() { }
        public UsuarioModel(Usuario entity) : base(entity.Id)
        {
            Username = entity.Username;
            Password = entity.Password;
            RememberPassword = entity.RememberPassword;
            Email = entity.Email;
            Tipo = (TipoUsuarioModel)entity.Tipo;
        }
        public override Usuario ReverseMap()
        {
            return new Usuario
            {
                Id = GetId(Key),
                Username = Username,
                Password = Password,
                Email = Email,
                Rol = Rol != null ? Rol.ReverseMap() : null,
            };
        }
        public UsuarioModel Include(Rol rol)
        {
            if (rol != null)
            {
                Rol = new RolModel(rol);
            }
            return this;
        }
        public UsuarioModel Include(Persona persona)
        {
            if (persona != null)
            {
                Persona = new PersonaModel(persona);
            }
            return this;
        }
    }
    public enum TipoUsuarioModel
    {
        Admin = 0, Directivo = 1, Docente = 2, Estudiante = 3
    }
    public class RolModel : Model<Rol>
    {
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public RolModel() { }
        public RolModel(Rol entity) : base(entity.Id)
        {
            Nombre = entity.Nombre;
            Activo = entity.Estado;
        }
        public override Rol ReverseMap()
        {
            return new Rol
            {
                Id = GetId(Key),
                Nombre = Nombre,
                Estado = Activo,
            };
        }
    }
}