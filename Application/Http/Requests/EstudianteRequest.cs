using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class EstudianteRequest : Request<EstudianteModel>
    {
        public string InstitucionNIT { get; set; }
        public string Sede { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string TipoDocumento { get; set; }
        public string Identificacion { get; set; }
        public string Grado { get; set; }
        public string Grupo { get; set; }
        public override EstudianteModel ToEntity()
        {
            return new EstudianteModel
            {
                DatosPersonales = new PersonaModel
                {
                    Institucion = new InstitucionModel { Nit = InstitucionNIT },
                    Apellidos = Apellidos,
                    Nombres = Nombres,
                    Documento = new DocumentoModel
                    {
                        Numero = Identificacion,
                        Tipo = TipoDocumento
                    }
                },
                Grado = Grado
            };
        }
    }
}