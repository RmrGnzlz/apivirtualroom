using Application.Mapping;
using Domain.Entities;

namespace Application.Models
{
    public class PersonaModel : Model<Persona>
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DocumentoModel Documento { get; set; }
        public InstitucionModel Institucion { get; set; }
        public UsuarioModel Usuario { get; set; }
        public PersonaModel() { }
        public PersonaModel(Persona entity) : base(entity.Id)
        {
            Nombres = entity.Nombres;
            Apellidos = entity.Apellidos;
            if (entity.Documento != null)
            {
                Documento = new DocumentoModel(entity.Documento);
            }
        }
        public override Persona ReverseMap()
        {
            return new Persona
            {
                Id = GetId(Key),
                Nombres = Nombres,
                Apellidos = Apellidos,
                Documento = Documento != null ? Documento.ReverseMap() : null
            };
        }
        public PersonaModel Include(Institucion institucion)
        {
            if (institucion != null)
            {
                Institucion = new InstitucionModel(institucion);
            }
            return this;
        }

        public PersonaModel Include(Usuario usuario)
        {
            if (usuario != null)
            {
                Usuario = new UsuarioModel(usuario);
            }
            return this;
        }
    }
}