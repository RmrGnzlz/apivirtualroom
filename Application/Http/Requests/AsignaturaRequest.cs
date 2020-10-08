using System.Collections.Generic;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class AsignaturaRequest : Request<AsignaturaModel>
    {
        public string Institucion { get; set; }
        public string Nombre { get; set; }
        public List<string> Grados { get; set;  }
        public override AsignaturaModel ToEntity()
        {
            return new AsignaturaModel(
                new Domain.Entities.Asignatura {
                    Nombre = Nombre
                }
            );
        }
    }
    public class RegistroAsignaturasRequest : BaseRequest
    {
        public string NIT { get; set; }
        public List<AsignaturaRequest> Asignaturas { get; set; }
    }
}