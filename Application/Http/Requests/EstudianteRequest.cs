using System.Collections.Generic;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class EstudianteRequest : Request<EstudianteModel>
    {
        public string Sede { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Grado { get; set; }
        public string Grupo { get; set; }
        public string Email { get; set; }
        public override EstudianteModel ToEntity()
        {
            return new EstudianteModel
            {
                DatosPersonales = new PersonaModel
                {
                    Apellidos = Apellidos,
                    Nombres = Nombres,
                    Documento = new DocumentoModel
                    {
                        Numero = NumeroDocumento,
                        Tipo = TipoDocumento
                    }
                },
                Grado = Grado
            };
        }
    }
    public class RegistroEstudiantesRequest : BaseRequest
    {
        public string NIT { get; set; }
        public List<EstudianteRequest> Estudiantes { get; set; }
    }
}