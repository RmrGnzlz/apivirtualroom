using System.Collections.Generic;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class AsignaturaRequest : Request<AsignaturaModel>
    {
        public string Institucion { get; set; }
        public string Nombre { get; set; }
        public string Grado { get; set;  }
        public string Grupo { get; set; }
        public override AsignaturaModel ToEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}